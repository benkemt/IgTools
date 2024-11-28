using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ig.Api.Client.Model;

public class BaseResult
{
    protected BaseResult(string errorCode)
    {
        IsSuccess = string.IsNullOrEmpty(errorCode);
        ErrorCode = errorCode;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public string ErrorCode { get; }
}


public class Result(string error) : BaseResult(error)
{
    public static Result Success() => new Result("");

    public static Result Failure(string error) => new Result(error);
}


public class Result<T> : BaseResult
{
    public Result(T data, string errorCode) : base(errorCode)
    {
        Data = data;
    }

    public Result(string errorCode) : base(errorCode)
    {
    }

    public static Result<T> Success(T value) => new Result<T>(value, "" );

    public static Result<T> Failure(string error) => new Result<T>(error);

    public T? Data { get;  } 
}