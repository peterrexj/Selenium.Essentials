using OpenQA.Selenium;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pj.Library.PjUtility;

namespace Selenium.Essentials
{
    public class TextboxControl : BaseControl, IEditableControl
    {
        private bool _useExtendedClear;
        public TextboxControl(IWebDriver driver, By by, BaseControl parentControl = null, string description = null, bool firstAvailable = false, bool useExtendedClear = false)
            : base(driver, by, parentControl, description, firstAvailable)
        {
            _useExtendedClear = useExtendedClear;
        }

        public void Set(string val)
        {
            Runtime.Logger.Log($"Trying to set value [{val}] in the textbox [{By}]");
            WaitUntilElementVisible();
            Clear();
            SendKeys(val);

            if (Get().EqualsIgnoreCase(val)) return;

            Runtime.Logger.Log($"Re-trying to set value [{val}] in the textbox [{By}]");
            WaitUntilElementInvisible(1, throwExceptionWhenNotFound: false); //Wait for a period before apply. There are some textbox which have UI alterations like $ signs and datetimes
            Clear();
            SendKeys(val);
        }

        public string Get()
        {
            return Value;
        }

        public void Append(string val)
        {
            WaitUntilElementVisible();
            SendKeys(val);
        }

        public override void Clear()
        {
            try
            {
                if (_useExtendedClear)
                {
                    RawElement.SendKeys(Keys.Control + "a");
                    RawElement.SendKeys(Keys.Delete);

                    if (Value.HasValue())
                    {
                        base.Clear();
                    }

                    if (Value.HasValue())
                        throw new Exception("Control was not cleared");
                }
                else
                {
                    base.Clear();
                }
            }
            catch (Exception ex)
            {
                Highlight();
                throw new WebControlException(Driver, ex, "Control could not be cleared.", this);
            }
        }
    }
}