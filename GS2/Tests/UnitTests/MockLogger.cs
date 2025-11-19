using Microsoft.Extensions.Logging;

namespace Tests.UnitTest
{
    /// <summary>
    /// Mock implementation of ILogger for testing purposes
    /// </summary>
    public class MockLogger : ILogger<object>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Mock logger - does nothing
        }
    }

    /// <summary>
    /// Generic mock implementation of ILogger for testing purposes
    /// </summary>
    public class MockLogger<T> : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Mock logger - does nothing
        }
    }
}
