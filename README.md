![](https://travis-ci.org/peterrexj/Selenium.Essentials.svg?branch=master)

# Selenium.Essentials 

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


# Documentation
https://github.com/peterrexj/Selenium.Essentials/wiki

# Prerequisites

Visual Studio 2017 or higher

# Install

`nuget install Selenium.Essentials`

# Usage


This is the conventional way to declare IWebElement (OLD WAY)
```c#
private IWebElement _headerContent = _driver.FindElement(By.Id("chkAreYouRobot"));
```

The below code show how to declare custom controls of type Checkbox. This clearly defines what control in the UI resemble and also contains its custom properties and methods.

```c#
private CheckboxControl _userTypeCheck => new CheckboxControl(_driver, By.CssSelector("div.user h2"));
```

To make an operation on this control for Check (tick the checkbox)

```c#
public void CheckUserType() {
  _userTypeCheck.WaitForElementVisible(errorMessage: "The User Type checkbox was not visible in the UI");
  _userTypeCheck.Check();
}
```

Conditionally check if the element exist by waiting for 2 seconds and make a check operation if available

```c#
public void CheckUserType() {
  if (_userTypeCheck.WaitForElementVisible(waitTimeSec: 2, throwExceptionWhenNotFound: false)) {
    _userTypeCheck.Check();
  }
}
```

Shows how the page object element looks like

```c#
private WebControl _headerContent => new WebControl(_driver, By.CssSelector("div.user h2"));
private UnorderedListControl _tabNavigation => new UnorderedListControl(_driver, By.XPath("//div[@id='p-namespaces']/ul"));
private TableControl _tableMainContent => new TableControl(_driver, By.Id("mp-upper"));
private TextboxControl _usernameTxt => new TextboxControl(_driver, By.Id("txtUserName"));
private TextboxControl _passwordTxt => new TextboxControl(_driver, By.Id("txtPassword"));
private ButtonControl _loginBtn => new ButtonControl(_driver, By.Name("loginUser"));
```




# Useful resource
