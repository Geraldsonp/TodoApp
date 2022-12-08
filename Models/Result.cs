using Microsoft.AspNetCore.Mvc;
using TodoApp.Models.Entity;

namespace TodoApp.Models;

public struct Result<T>
{
    public bool Succeded { get; }
    public string ErrorMessage => GetErrorMessage();
    public ErrorTypes ErrorType { get; } = ErrorTypes.None;
    public T? Data { get; }

    public Result(ErrorTypes errorType)
    {
        Succeded = false;
        ErrorType = errorType;
        Data = default;
    }

    public Result(T data)
    {
        Data = data;
        Succeded = true;
    }

    public Result(bool succeded = true)
    {
        Succeded = succeded;
        Data = default;
    }

    private string GetErrorMessage()
    {
        switch (ErrorType)
        {
            case ErrorTypes.None:
                return "No Error";
            case ErrorTypes.ListDoesNotHasSpace:
                return "List Does not has enough Space";
            case ErrorTypes.IncorrectCredentials:
                return "UserName or Password Error";
            case ErrorTypes.EntityNotFound:
                return "Entity with given Id does not exist";
        }

        return "";
    }
}