using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public class FileUploadControl : BaseControl, IEditableControl
    {
        public FileUploadControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null)
          : base(driver, by, parentControl, description)
        {
        }

        public void Set(string filePath)
        {
            SendKeys(filePath);
        }

        public string Get()
        {
            return Value;
        }

        public void UploadFile(string filepath)
        {
            Set(filepath);
        }
    }
}
