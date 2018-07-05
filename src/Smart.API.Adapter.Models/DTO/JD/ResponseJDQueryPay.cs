
namespace Smart.API.Adapter.Models.DTO.JD
{
    public class ResponseJDQueryPay : BaseJdRes
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
