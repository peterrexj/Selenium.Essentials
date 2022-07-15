using NUnit.Framework;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.SampleTest.Core
{
    public static class TestContextExtensions
    {
        /// <summary>
        /// Gives the Test Name from the nUnit test context by removing special chars and adding whitespace
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string TestName(this TestContext context)
            => context.Test.Name.RemoveSpecialChars().ApplyWhitespaceOverCaps().ReplaceMultiple(" ", "_", "  ");
    }
}
