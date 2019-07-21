using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Selenium.Essentials.Utilities.Helpers
{
    public static class RandomHelper
    {

        public static string RandomStringGenerator(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                var ch = (char)random.Next('A', 'Z' + 1);
                builder.Append(ch);
            }
            return builder.ToString().ToLowerInvariant();
        }

        private static string GenerateRandom(string chars, int size)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(chars, size).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomAlphanumeric(int size) => GenerateRandom("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", size);

        public static string RandomAlpha(int size) => GenerateRandom("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", size);


        public static string RandomNumberGenerator(int digitLength, string startNumber = "")
        {
            var rand = new Random();
            var result = startNumber.ToCharArray().Select(s => s.ToString()).ToList();
            result.AddRange(Enumerable.Range(1, digitLength - startNumber.Length)
                    .Select(i => rand.Next(9).ToString()).ToList());
            return string.Join("", result);
        }

        public static string RandomDobGenerator(int minAge, int maxAge)
        {
            DateTime dt = DateTime.Now;
            DateTime dtStart = dt.AddYears(-maxAge);
            DateTime dtEnd = dt.AddYears(-minAge);

            int maxDays = (dtEnd.Year - dtStart.Year) * 355;
            Random randDays = new Random();
            DateTime dob = dtStart.AddDays(randDays.Next(1, maxDays));
            return dob.ToString("dd/MM/yyyy");
        }
    }
}
