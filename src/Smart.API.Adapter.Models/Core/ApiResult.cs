using System.Net.Http.Headers;

namespace Smart.API.Adapter.Models
{
    public class ApiResult
    {
        public ApiResult() { }

        public bool successed { get; set; }

        public string code { get; set; }

        public string message { get; set; }

        public string stackTrace { get; set; }

        public HttpResponseHeaders ResponseHeaders { get; set; }
    }

    public class ApiResult<T> : ApiResult
    {
        public ApiResult() : base() { }

        public T data { get; set; }
    }
}
