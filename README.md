![](https://travis-ci.org/peterrexj/Selenium.Essentials.svg?branch=master)
[![Sauce Test Status](https://saucelabs.com/buildstatus/peterrexj)](https://app.saucelabs.com/u/peterrexj)
[![Sauce Test Status](https://saucelabs.com/browser-matrix/peterrexj.svg)](https://saucelabs.com/u/peterrexj)

# Selenium.Essentials (work in progress)

![](https://github.com/peterrexj/Selenium.Essentials/blob/master/docs/resources/images/Icon.png)



Build Selenium test in C# using advanced controls and wrappers which fastens the development time and help to pass the test consistently with plenty of extensions and helpers methods on Element, Driver and C# types.

# Overview

Instead of using IWebElement, the new custom controls give meaning to your page objects and make it more readable. The controls are defined from a BaseControl which has definitions for operations that are common to all the controls. Each custom control contains all the common properties together with the unqiue properties and methods which are applicable only to the control. 
For example, CheckboxControl will have all properties of the base control and also defines Check() which ticks the checkbox, UnCheck() which unchecks the checkbox, IsChecked returns a bool value based on the control is Checked or Unchecked reading from the UI 

The Custom control also provides the IWebElement as a property which is used by Selenium, in case you need to do any operations on top of this. 

There are plenty of Wait operation defined on the base control which flows through all the custom controls. There are different overrides to the wait operation where you can control the time to wait, whether to throw exception if fails, message for assertions when the waits are used for assert operations. Some custom control overrides the default wait to give a better meaning.

WebDriver comes with some useful extensions which helps during the automation. For example, executing javascript, scroll operations, taking screenshot, getting driver capabilities.

There is a simple Api framework, which can help in writing Integration tests. 

Also comes with lots of extensions and helpers on different types which will help increase productivity. Example, 
- Loading excel and converting to C#
- Converting Json to Dictionary
- Serialization and Deserialization
- Regular Expression, DateTime, String, Enumerable, Async

# Benefits
- Readable page objects which clearly defines what each control resemble in the browser
- Custom controls with wrapped operations 
 -- Checkbox - Check, UnCheck, IsChecked, more
 -- Textbox - Custom clear and set operations (extented clear which will make sure the content is cleared by doing Ctrl+a and BackSpace) and Set operation to overcome some responsive issues
 -- UnorderedList - Total, Items
 -- Select - operations on SelectElement
 -- Table - TotalColumns, TotalRows, ColumnNames, GeCellContent, GetRowPosition, GetColumnPosition, more
 -- Collection - working with Driver.FindElements(...)
 -- FileUpload - UploadFile
 -- Button
 -- WebControl - for all generic html control
- WebElement and WebDriver extension methods for most of the 
- Api framework to write integration tests
-- Supports fluent 
 -- Support most of the operations
 -- Simple and easy to manage the tests
- Extensions which provide many methods for automation engineering works
-- String, RegEx, Enumerable, DatTime, Async, more
- Helpers to load excel, serialization, Json to Dictonary, more
- Attributes for test to load json and xml data

# Documentation
https://github.com/peterrexj/Selenium.Essentials/wiki

# Prerequisites

Visual Studio 2017 or higher

You need to know 
- about the basics of writing test using Selenium (concept of IWebDriver, IWebElement and starting the driver)
- know how to inspect and find elements (creating selectors)
- know about the page object model

# Install

`nuget install Selenium.Essentials`

# Usage


This is the conventional way to declare IWebElement (OLD WAY)
```c#
private IWebElement _headerContent = _driver.FindElement(By.Id("chkAreYouRobot"));
```

The below code show how to declare custom controls of type Checkbox. This clearly defines what control in the UI corresponds to and also contains its custom properties and methods.

Add the `using` statement for this package

```c#
using Selenium.Essentials
```

Initialize the custom controls in your page objects. If you have existing page objects with IWebElement, then you can easily change by using the same selector and passing it to the new custom control.
Remember, the driver is explicitly passed to the control, in order to have better control over the page objects if you intend to run the scripts in parallel, on the same machine.

```c#
private CheckboxControl _userTypeCheck => new CheckboxControl(_driver, By.CssSelector("div.user h2"));
```

To make an operation on this control, for example Check (tick the checkbox)

```c#
public void CheckUserType() {
  _userTypeCheck.WaitForElementVisible(errorMessage: "The User Type checkbox was not visible in the UI"); //This can be used as an assertions, and when not found, it will throw with an exception with "errorMessage" passed
  _userTypeCheck.Check();
}
```

Conditionally check if the element exist by waiting for maximum of 2 seconds and make a check operation if available. The below sample will not throw an exception if the control was not found in the browser, instead the `WaitForElementVisible` will return false after 2 seconds and will not go inside the `if` condition

```c#
public void CheckUserType() {
  if (_userTypeCheck.WaitForElementVisible(waitTimeSec: 2, throwExceptionWhenNotFound: false)) {
    _userTypeCheck.Check();
  }
}
```

Benefits


Shows how the page object element looks like

```c#
private WebControl _headerContent => new WebControl(_driver, By.CssSelector("div.user h2"));
private UnorderedListControl _tabNavigation => new UnorderedListControl(_driver, By.XPath("//div[@id='p-namespaces']/ul"));
private TableControl _tableMainContent => new TableControl(_driver, By.Id("mp-upper"));
private TextboxControl _usernameTxt => new TextboxControl(_driver, By.Id("txtUserName"));
private TextboxControl _passwordTxt => new TextboxControl(_driver, By.Id("txtPassword"));
private ButtonControl _loginBtn => new ButtonControl(_driver, By.Name("loginUser"));
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

Most of the above operations are also added to IWebElement as extension methods

# Useful resource
