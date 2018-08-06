namespace CongDoanCoreApp.Utilities.Dtos
{
    public class GenericResult
    {
        public GenericResult()
        {
        }

        public GenericResult(bool success)
        {
            this.Success = success;
        }

        public GenericResult(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public GenericResult(bool success, object data)
        {
            Success = success;
            Data = data;
        }

        public string Message { get; set; }

        public bool Success { get; set; }

        public object Data { get; set; }

        public object Error { get; set; }
    }
}