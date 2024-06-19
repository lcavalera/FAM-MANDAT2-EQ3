namespace Acef.Reasons.ApplicationCore.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; }
        public object? Errors { get; set; }
    }
}
