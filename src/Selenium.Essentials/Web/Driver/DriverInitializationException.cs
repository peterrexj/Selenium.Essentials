using System;

namespace Selenium.Essentials;

// Inherits from the Exception class to create a custom exception type.
public class DriverInitializationException : Exception
{
    // Constructor without parameters
    public DriverInitializationException()
    {
    }

    // Constructor that accepts a single string message
    public DriverInitializationException(string message)
        : base(message)
    {
    }

    // Constructor that accepts a string message and an inner exception 
    // which is passed to the base Exception class. This is useful for wrapping 
    // exceptions without losing the original stack trace.
    public DriverInitializationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
