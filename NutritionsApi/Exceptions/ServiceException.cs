namespace NutritionsApi.Exceptions;

public class ServiceException(string message, int statusCode, string errorCode = "") : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public string ErrorCode { get; } = errorCode;
}