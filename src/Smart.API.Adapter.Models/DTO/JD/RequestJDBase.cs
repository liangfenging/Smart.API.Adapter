using System.Configuration;

namespace Smart.API.Adapter.Models.DTO.JD
{
    public class RequestJDBase
    {
        public RequestJDBase()
        { }
        public string parkLotCode
        {
            get
            {
                string parkLotCode = ConfigurationManager.AppSettings["ParkLotCode"];
                if (string.IsNullOrWhiteSpace(parkLotCode))
                {
                    parkLotCode = "";
                }
                return parkLotCode;
            }
        }

        public string token
        {
            get
            {
                string token = ConfigurationManager.AppSettings["Token"];
                if (string.IsNullOrWhiteSpace(token))
                {
                    token = "";
                }
                return token;
            }
        }
    }
}
