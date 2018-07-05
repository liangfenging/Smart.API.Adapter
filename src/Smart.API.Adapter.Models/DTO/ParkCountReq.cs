using System.Configuration;

namespace Smart.API.Adapter.Models
{
    public  class ParkCountReq
    {
        public string Param { get; set; }
        public string Token
        {
            get
            {
                string token = ConfigurationManager.AppSettings["Token"];
                if (string.IsNullOrWhiteSpace(token))
                {
                    token = "1";
                }
                return token;
 
            }
        }
    }
}
