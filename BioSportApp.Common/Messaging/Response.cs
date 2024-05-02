namespace BioSportApp.Common.Messaging
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
