using System;

namespace DND.Middleware.System
{
    public class Result
    {
        public bool IsSucceeded { get; set; }
        public string ErrorMessage { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public InputModelStateDictionary InputModelStateDictionary { get; set; }

        public Result Error(string errorMessage)
        {
            IsSucceeded = false;
            ErrorMessage = errorMessage;
            return this;
        }

        public Result Error(Exception exception)
        {
            IsSucceeded = false;
            ExceptionMessage = exception.Message;
            ExceptionType = exception.GetType().Name;
            return this;
        }

        public Result Error(InputModelStateDictionary inputModelStateDictionary)
        {
            IsSucceeded = false;
            InputModelStateDictionary = inputModelStateDictionary;
            return this;
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
        public Pagination Pagination { get; set; }

        public void Success(T data)
        {
            IsSucceeded = true;
            Data = data;
        }

        public void Success(T data, Pagination paginationInfo)
        {
            IsSucceeded = true;
            Data = data;
            Pagination = paginationInfo;
        }
    }
}
