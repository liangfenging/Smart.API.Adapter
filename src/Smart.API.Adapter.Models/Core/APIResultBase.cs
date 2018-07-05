
namespace Smart.API.Adapter.Models
{
    public class APIResultBase
    {
        private string resultcode = "0";

        public string code
        {
            get { return resultcode; }
            set { resultcode = value; }
        }

        private string message = "";

        public string msg
        {
            get { return message; }
            set { message = value; }
        }
    }

    public class APIResultBase<T> : APIResultBase
    {
        public T data { get; set; }
    }
}
