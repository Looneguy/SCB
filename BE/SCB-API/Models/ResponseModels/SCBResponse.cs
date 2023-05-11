using Newtonsoft.Json.Linq;

namespace SCB_API.Models.ResponseModels
{
    public class SCBResponse<T>
    {
        public SCBResponse(T? value)
        {
            Value = value;
            Success = value != null;
        }

        public SCBResponse(string message)
        {
            Success = false;
            ErrorMessage = message;
        }

        public bool Success { get; set; }
        public T? Value {get;set; }
        public string? ErrorMessage { get; set; }
    }
}
