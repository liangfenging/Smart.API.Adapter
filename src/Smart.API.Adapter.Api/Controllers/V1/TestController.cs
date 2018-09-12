
using Smart.API.Adapter.Web.Api;
using System.Net.Http;
using System.Web.Http;
namespace Smart.API.Adapter.Api.Controllers.V1
{

    /// <summary>
    /// Smart.API.Adapter Open Api
    /// </summary>

    public class TestController : ApiControllerBase
    {
        [HttpPost, WriteLog, ActionName("testtest")]
        public HttpResponseMessage testtest()
        {
            return Request.CreateResponse();
        }
    }
}
