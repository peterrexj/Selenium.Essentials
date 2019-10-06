
# Selenium.Essentials
![](https://github.com/peterrexj/Selenium.Essentials/blob/master/docs/resources/images/Icon.png) </br>

![](https://travis-ci.org/peterrexj/Selenium.Essentials.svg?branch=master) 
[![Sauce Test Status](https://saucelabs.com/buildstatus/peterrexj)](https://app.saucelabs.com/u/peterrexj)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/02286e55e59c476a9c0d4fd2c4dae87e)](https://www.codacy.com/app/peterrexj/Selenium.Essentials?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=peterrexj/Selenium.Essentials&amp;utm_campaign=Badge_Grade) </br>
[![Sauce Test Status](https://saucelabs.com/browser-matrix/peterrexj.svg)](https://saucelabs.com/u/peterrexj) 
</br>
[![Powered By Sauce Labs](https://github.com/peterrexj/Selenium.Essentials/blob/master/docs/resources/images/PoweredBySauce LabsBadgesGray.svg)](https://saucelabs.com) 


Build Selenium web automation test using advanced web controls with wrappers and plenty of extensions to fasten your automation development time. Focus more on script logic with better consistent script execution, less maintenance, no hardwaits, with improved script execution performance and integrated Api testing framework.

# Overview

Selenium provides only option to create only a single generic control which is called the IWebElement. Imagine if you have option to declare controls which resemble the html elements and provide its functionality, for example, Checkbox, Textbox, Button.

Selenium Essentials provide new custom controls giving meaning to your page objects and making it more readable. Every control is defined from a BaseControl which has a set of definitions applicable to all controls as well as its custom actions. For example, Checkbox control will have all properties of the BaseControl and also defines Check() which ticks the checkbox in the UI, UnCheck() which unticks the checkbox, IsChecked returns a bool value based on the control is Checked or Unchecked reading from the UI.

The Custom control also expose the underlying IWebElement as a property used by Selenium, in case you need to do any operations on top of this. 

There are plenty of Wait operation defined on the base control which flows through all the custom controls. There are different overrides to the wait operation where you can control the time to wait, whether to throw exception if fails, message for assertions when the waits are used for assert operations. Some custom control overrides the default wait to give a better meaning.

[Read more about controls here](https://github.com/peterrexj/Selenium.Essentials/wiki/PageObject-with-new-controls)


WebDriver and WebElement comes with some useful extensions which helps during the automation. For example, executing javascript, scroll operations, taking screenshot, getting driver capabilities.

There is a simple Api framework, which can help in writing Integration tests using a fluent approach.

Package contains lots of extensions and helpers over different types which will help increase productivity. Example, 
- Loading excel and converting to C#
- Converting Json to Dictionary
- Serialization and Deserialization
- Regular Expression, DateTime, String, Enumerable, Async

# Benefits
* Readable page objects which clearly defines what each control resemble in the browser
* Custom controls with wrapped operations 
  * Checkbox - Check, UnCheck, IsChecked, more
  * Textbox - Custom clear and set operations (extented clear which will make sure the content is cleared by doing Ctrl+a and BackSpace) and Set operation to overcome some responsive issues
  * UnorderedList - Total, Items
  * Select - operations on SelectElement
  * Table - TotalColumns, TotalRows, ColumnNames, GeCellContent, GetRowPosition, GetColumnPosition, more
  * Collection - working with Driver.FindElements(...)
  * FileUpload - UploadFile
  * Button
  * WebControl - for all generic html control
* WebElement and WebDriver extension methods for most of the 
* Api framework to write integration tests
  * Supports fluent 
  * Support most of the operations
  * Simple and easy to manage the tests
* Extensions which provide many methods for automation engineering works
  * String, RegEx, Enumerable, DatTime, Async, more
* Helpers to load excel, serialization, Json to Dictonary, more
* Attributes for test to load json and xml data

# Prerequisites

[Visual Studio](https://visualstudio.microsoft.com/downloads/) 2017 or higher

You need to know
* Minimum
  * about the basics of writing test using [Selenium](https://www.seleniumhq.org/) (concept of IWebDriver, IWebElement and starting the driver)
  * know how to inspect and find elements (creating better selectors)
  * know about the page object model
  * any test framework, for example, [nUnit](https://nunit.org/)
  * c# beginner
* Good to know
  * CI CD pipelines
  * Multibrowser automated test configurations (any)
    * Selenium Grid
    * Sauce Labs
    * Browser Stack
    * Inhouse setup with physical servers and Vm
    * Cloud configured machines to run tests
    * Any custom setup to run multibrowser tests
  * Mobile test with Appium
  * Advanced Logging
  * Test Reporting
  * Programming knowledge
    * c# Generics
    * OOPS concepts
    * Linq and Lamda (basic)
    * Multithread programming 

# Install

`nuget install Selenium.Essentials`

# Setup

Once you have setup your project or added the nuget package to your existing project, follow the links below to speed up your development.

1. [Onboarding](https://github.com/peterrexj/Selenium.Essentials/wiki/Onboarding)
1. [Initialize Web Driver](https://github.com/peterrexj/Selenium.Essentials/wiki/Initialize-Web-Driver)
1. [PageObject with new controls](https://github.com/peterrexj/Selenium.Essentials/wiki/PageObject-with-new-controls)
1. [Some Tips](https://github.com/peterrexj/Selenium.Essentials/wiki/Tips-and-Tricks)

# Usage

Conventional way to declare elements (IWebElement)
```c#
private IWebElement _headerContent = driver.FindElement(By.Id("chkAreYouRobot"));
```
The below section show how to use new `Checkbox` control. This definition is clear on what element in the UI corresponds to and also contains its custom properties and methods.

Add the `using` statement for this package

```c#
using Selenium.Essentials
```

Define the new control in your page object. If you have existing page objects with `IWebElement`, then you can easily change with same selector and passing it to the new custom control.
Remember, the driver is explicitly passed to the control, in order to have better control over the page object if you intend to run the scripts in parallel, on the same machine.

```c#
private CheckboxControl _userTypeCheck => new CheckboxControl(driver, By.Id("chkAreYouRobot"));
```

_Notice the declaration is by an expression (=>) which does not store the value but fetch everytime when accessed. Following this pattern reduces StaleElementException._

To tick the checkbox

```c#
public void CheckUserType() {
  _userTypeCheck.WaitForElementVisible(errorMessage: "The User Type checkbox was not visible in the UI"); //This can be used as an assertions, and when not found, it will throw with an exception with "errorMessage" passed
  _userTypeCheck.Check();
}
```

**_Conditionally check_** if the element exist by waiting for maximum of 2 seconds and make a check operation if available. The below sample will not throw an exception if the control was not found in the browser, instead the `WaitForElementVisible` will return false after 2 seconds and will not go inside the `if` condition

```c#
public void CheckUserType() {
  if (_userTypeCheck.WaitForElementVisible(waitTimeSec: 2, throwExceptionWhenNotFound: false)) {
    _userTypeCheck.Check();
  }
}
```

This is how it looks like when you have new controls in your page object

```c#
private ButtonControl _loginBtn => new ButtonControl(driver, By.Name("loginUser"));
private TextboxControl _usernameTxt => new TextboxControl(driver, By.Id("txtUserName"));
private TextboxControl _passwordTxt => new TextboxControl(driver, By.Id("txtPassword"));
private WebControl _headerContent => new WebControl(driver, By.CssSelector("div.user h2"));
private UnorderedListControl _tabNavigation => new UnorderedListControl(driver, By.XPath("//div[@id='p-namespaces']/ul"));
private TableControl _tableMainContent => new TableControl(driver, By.Id("mp-upper"));
```

Custom control default properties and methods (few listed)

| Properties      | Properties             | Methods                    | Methods       |  
| -------------   | -----------------------|--------------------------- |---------------|
| `IsReadonly`    | `Value`                | `GetAttribute(string)`     | `ScrollTo()`  | 
| `IsDisabled`    | `Text`        				 | `Click()`                  | `Clear()`     |
| `IsEnabled`     | `ElementId`            | `WaitAndClick(int)`        | `Highlight()` |
| `IsVisible`     | `Class`                | `ScrollAndClick()`         |               |
| `CssDisplayed`  | `Classes`              | `ClickByJsScript()`        |               |
| `IsDisplayed`   | `Driver`               | `WaitClickAndIgnoreError()`|               |
| `IsHidden`      | `RawElement`           | `SendKeys()`               |               |
| `Exists`        | `ParentControl`        | `SendEnter()`              |               |
| `XpathSelector` | `ParentRawElement`     | `SendTab()`                |               |
| `By`            |                        | `SetFocusByJavascript()`   |               |

Wait operations (few listed)

| Wait On | Description |
|---------|-------------|
|`WaitUntilElementVisible`|Wait until the element is visible|
|`WaitUntilElementInvisible` |Wait until the element goes invisible|
|`WaitUntilElementEnabled`|Wait until the element is enabled|
|`WaitUntilElementExists`|Wait until the element exists|
|`WaitUntilElementCssDisplayed`|Wait until the element is Css Displayed (display: none not applied)|
|`WaitUntilElementClickable`|Wait until the element is clickable, which is element Exists, Displayed, CssDisplayed and Enabled|
|`WaitUntilElementTextTrimEquals`|Wait until the text on the element after trim is equal to the text passed for match|
|`WaitUntilElementTextStartsWith`|Wait until the text on the element after trim is starts with the text passed for match|
|`WaitUntilElementTextContains`|Wait until the text on the element contains the text passed for match|
|`WaitUntilElementHasSomeText`|Wait until the element has any text on it|


Most of the above operations are also added to IWebElement as extension methods

# Documentation
https://github.com/peterrexj/Selenium.Essentials/wiki