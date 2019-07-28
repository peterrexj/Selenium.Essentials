using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Selenium.Essentials
{
    public static class AsyncExtensions
    {
        /// <summary>
        /// This logic will restrict only specific number of threads to run concurrently. As there will be many number of tasks to run, we should restrict the concurrency
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="parallelExecutionCount"></param>
        internal static void ExecuteAllTasks(this IEnumerable<Task> tasks, int parallelExecutionCount = 1, int waitTimeInMilliSecondsBetweenEachTask = 0)
        {
            if (!tasks.Any())
            {
                return;
            }

            tasks.ExecuteAllTasksInternal(parallelExecutionCount, waitTimeInMilliSecondsBetweenEachTask);

            Task.WaitAll(tasks.ToArray());
        }

        internal static T[] ExecuteAllTasks<T>(this List<Task<T>> tasks, int parallelExecutionCount = 1, int waitTimeInMilliSecondsBetweenEachTask = 0)
        {
            if (tasks.Count() == 0)
            {
                return new T[0];
            }

            tasks.ExecuteAllTasksInternal(parallelExecutionCount, waitTimeInMilliSecondsBetweenEachTask);

            // Wait till all the task are complete and return each of the task's results
            return Task.WhenAll(tasks.ToArray()).GetAwaiter().GetResult();
        }

        private static IEnumerable<Task> ExecuteAllTasksInternal(this IEnumerable<Task> tasks, int parallelExecutionCount = 1, int waitTimeInMilliSecondsBetweenEachTask = 0)
        {
            if (tasks.Count() == 0)
            {
                return Enumerable.Empty<Task>();
            }

            var maxThread = new SemaphoreSlim(parallelExecutionCount);
            var continuationTasks = new List<Task>();
            foreach (var task in tasks)
            {
                maxThread.Wait();
                if (waitTimeInMilliSecondsBetweenEachTask > 0) { Thread.Sleep(waitTimeInMilliSecondsBetweenEachTask); }
                task.Start();
                continuationTasks.Add(task.ContinueWith(t => maxThread.Release()));
            }

            Task.WaitAll(continuationTasks.ToArray());

            return tasks;
        }

        private static Dictionary<string, List<Task>> _tasks = new Dictionary<string, List<Task>>();

        [Obsolete("Use the TaskGroup class instead")]
        public static void ExecuteAsync(this Task task, string id)
        {
            if (!_tasks.ContainsKey(id))
            {
                _tasks.Add(id, new List<Task>());
            }

            _tasks[id].Add(task);
            task.Start();
            task.ContinueWith(t => RemoveTask(id, t));
        }

        private static void RemoveTask(string id, Task t)
        {
            if (_tasks.ContainsKey(id))
            {
                try
                {
                    _tasks[id].Remove(t);
                }
                catch (KeyNotFoundException) { }
                catch (Exception) { throw; }
            }
        }

        private static void RemoveAllTasks(string id)
        {
            if (_tasks.ContainsKey(id))
            {
                try
                {
                    _tasks[id].Where(t => t.Status != TaskStatus.Running).Iter(t => RemoveTask(id, t));
                }
                catch (Exception) { }
                finally
                {
                    if (_tasks[id].Any(t => t.Status == TaskStatus.Running))
                    {
                        throw new Exception($"There is still some threads running in the background, need to wait for this task: {id}");
                    }
                    else
                    {
                        _tasks.Remove(id);
                    }
                }
            }
        }

        [Obsolete("Use the TaskGroup class instead")]
        public static void WaitAllAysncTaskToComplete(string id)
        {
            Task.WaitAll(_tasks[id].ToArray());
            RemoveAllTasks(id);
        }
    }

    public class TaskGroup<T> : TaskGroup
    {
        public Task<T> Run(Func<T> func)
        {
            var t = new Task<T>(() => func());
            Add(new TaskWrapper(t, isLazy: false));
            t.Start();
            return t;
        }
        public Task<T> Add(Func<T> func)
        {
            var t = new Task<T>(() => func());
            Add(new TaskWrapper(t, isLazy: true));
            return t;
        }

        public IEnumerable<Task<T>> Add(IEnumerable<Func<T>> actions)
        {
            var list = new List<Task<T>>();
            foreach (var action in actions)
            {
                list.Add(Add(action));
            }
            return list;
        }

        public new T[] WaitAll(int parallelExecutionCount = 1, int waitTimeInMilliSecondsBetweenEachTask = 0)
        {
            lock (_tasks)
            {
                var lazyTasks = _tasks.Where(t => t.IsLazy).Select(t => (Task<T>)t.Task).ToList();
                parallelExecutionCount = parallelExecutionCount <= 0 ? lazyTasks.Count : parallelExecutionCount;
                var taskResults = lazyTasks.ExecuteAllTasks(parallelExecutionCount, waitTimeInMilliSecondsBetweenEachTask);

                var immediateTasks = _tasks.Where(t => !t.IsLazy).Select(t => (Task<T>)t.Task).ToArray();
                var results = Task.WhenAll(immediateTasks).GetAwaiter().GetResult();

                _tasks.Clear();

                return taskResults.Concat(results).ToArray();
            }
        }
    }

    public class TaskGroup
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public int Count { get { return _tasks.Count(); } }

        protected IList<TaskWrapper> _tasks = new List<TaskWrapper>();

        protected IList<Task> _continuationTasks = new List<Task>();

        /// <summary>
        /// Starts a new task immediately with the action that is passed in.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task Run(Action action)
        {
            var t = new Task(() => action());
            Add(new TaskWrapper(t, isLazy: false));
            t.Start();
            return t;
        }

        /// <summary>
        /// Starts tasks in batches based on the MAX Thread count
        /// After adding all the task use WaitAll() to wait for the remaining task to complete
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxThreads"></param>
        /// <returns></returns>
        public Task Batch(Action action, int maxThreads)
        {
            var tk = new Task(() => action());
            Add(new TaskWrapper(tk));

            if (TaskRunningCount < maxThreads)
            {
                try
                {
                    tk.Start();
                }
                catch (Exception ex)
                {
                }

                var completionTask = tk.ContinueWith(t => NotifyTaskComplete(maxThreads));
                _continuationTasks.Add(completionTask);
            }

            return tk;
        }

        private int TaskRunningCount => _tasks.Where(t => t.Task.Status == TaskStatus.Running).Count();

        private void NotifyTaskComplete(int maxThreads)
        {
            lock (_tasks)
            {
                var tasksToRun = maxThreads - TaskRunningCount;
                var nonRunningTasksTemp = _tasks.Where(t => t.Task.Status == TaskStatus.Created).Take(tasksToRun).ToList();
                foreach (var t in nonRunningTasksTemp)
                {
                    t.Task.Start();
                    var completionTask = t.Task.ContinueWith(continuationTask => NotifyTaskComplete(maxThreads));
                    _continuationTasks.Add(completionTask);
                }
            }

        }

        public TaskGroup AddAndContinue(Action action)
        {
            var t = new Task(() => action());
            Add(new TaskWrapper(t, isLazy: true));
            return this;
        }
        /// <summary>
        /// Create a new task but don't start executing it yet.
        /// Call the WaitAll() method to start the tasks.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task Add(Action action)
        {
            var t = new Task(() => action());
            Add(new TaskWrapper(t, isLazy: true));
            return t;
        }

        public IEnumerable<Task> Add(IEnumerable<Action> actions)
        {
            var list = new List<Task>();
            foreach (var action in actions)
            {
                list.Add(Add(action));
            }
            return list;
        }

        protected TaskWrapper Add(TaskWrapper taskWrapper)
        {
            lock (_tasks)
            {
                _tasks.Add(taskWrapper);
                return taskWrapper;
            }
        }

        public void WaitAll(int parallelExecutionCount = 0, int waitTimeInMilliSecondsBetweenEachTask = 0)
        {
            Task[] immediateTasks;

            lock (_tasks)
            {
                var lazyTasks = _tasks.Where(t => t.IsLazy).Select(t => t.Task).ToList();
                parallelExecutionCount = parallelExecutionCount <= 0 ? lazyTasks.Count : parallelExecutionCount;
                lazyTasks.ExecuteAllTasks(parallelExecutionCount, waitTimeInMilliSecondsBetweenEachTask);

                immediateTasks = _tasks.Where(t => !t.IsLazy).Select(t => t.Task).ToArray();
            }

            Task.WaitAll(immediateTasks);
            Task.WaitAll(_continuationTasks.ToArray());

            lock (_tasks)
            {
                _tasks.Clear();
            }
        }

        public void RemoveAllTasks()
        {
            lock (_tasks)
            {
                _tasks.Clear();
            }
        }
    }

    public class TaskWrapper
    {
        public int Id => Task.Id;
        public Task Task { get; private set; }
        public bool IsLazy { get; private set; }

        public TaskWrapper(Task task, bool isLazy = false)
        {
            Task = task;
            IsLazy = isLazy;
        }
    }
}
