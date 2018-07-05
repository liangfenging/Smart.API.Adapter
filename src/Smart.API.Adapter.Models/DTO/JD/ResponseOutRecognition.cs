
namespace Smart.API.Adapter.Models.DTO.JD
{
    public class ResponseOutRecognition : BaseJdRes
    {
        public string resultCode
        {
            get;
            set;
        }

        public string qrCode
        {
            get;
            set;
        }

        public string cost
        {
            get;
            set;
        }
    }
}
