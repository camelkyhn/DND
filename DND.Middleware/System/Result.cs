using System;
using System.Linq;
using DND.Middleware.Base.Filter;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DND.Middleware.System
{
    public class Result
    {
        public bool IsSucceeded { get; set; }
        public string ErrorMessage { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public ObjectModelStateDictionary RequestModelState { get; set; } = new();

        public void Success()
        {
            IsSucceeded = true;
        }

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

        public Result Error(ModelStateDictionary modelState)
        {
            IsSucceeded = false;
            foreach (var state in modelState)
            {
                RequestModelState[state.Key] = state.Value.Errors.Select(e => e.ErrorMessage).ToList();
            }

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

        public void Success(T data, FilterDto filterDto)
        {
            IsSucceeded = true;
            Data = data;
            Pagination = new Pagination { TotalCount = filterDto.TotalCount, PageSize = filterDto.PageSize, PageNumber = filterDto.PageNumber };
        }

        public new Result<T> Error(string errorMessage)
        {
            IsSucceeded = false;
            ErrorMessage = errorMessage;
            return this;
        }

        public new Result<T> Error(Exception exception)
        {
            IsSucceeded = false;
            ExceptionMessage = exception.Message;
            ExceptionType = exception.GetType().Name;
            return this;
        }

        public new Result<T> Error(ModelStateDictionary modelState)
        {
            IsSucceeded = false;
            foreach (var state in modelState)
            {
                RequestModelState[state.Key] = state.Value.Errors.Select(e => e.ErrorMessage).ToList();
            }

            return this;
        }
    }
}
