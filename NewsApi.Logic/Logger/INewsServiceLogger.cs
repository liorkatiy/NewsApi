using System;

namespace NewsApi.Logic.Logger
{
    public interface INewsServiceLogger
    {
        void LogError(string message);
        void LogError(Exception e,string message);
    }
}
