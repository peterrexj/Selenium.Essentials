using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public static class ControlExtensions
    {
        public static IList<IBaseControl> FindVisibleElements(this IBaseControl[] availableControls)
        {
            var visibleControls = new List<IBaseControl>();
            foreach (IBaseControl ctrl in availableControls)
            {
                try
                {
                    // This will try to do FindElement. If it throws an exception we assume it doesn't exist.
                    var el = ctrl.RawElement;
                    if (ctrl.Exists && ctrl.CssDisplayed) visibleControls.Add(ctrl);
                }
                catch
                {
                    // Continue looping through all the locators
                }
            }

            return visibleControls;
        }

    }
}
