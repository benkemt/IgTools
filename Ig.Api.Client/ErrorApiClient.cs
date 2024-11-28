namespace Ig.Api.Client;

public class ErrorApiClient
{
    public string Code { get; set; } = null!;

    public static ErrorApiClient None()
    {
        return new ErrorApiClient()
        {
            Code = "None"
        };
    }

    public static ErrorApiClient Unknown()
    {
        return new ErrorApiClient()
        {
            Code = "Unknown"
        };
    }

    public static ErrorApiClient ErrorCode(string errorCode)
    {
        return new ErrorApiClient()
        {
            Code = errorCode
        };
    }
}