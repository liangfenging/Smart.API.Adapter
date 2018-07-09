
namespace Smart.API.Adapter.ThirdApp
{
    public class ThirdAppFactory
    {
        public static IThirdApp Create(int ThirdApp)
        {
            IThirdApp iThirdApp = null;
            switch (ThirdApp)
            { 
                case (int)enumAppType.JDPark:
                    iThirdApp = new JDParkThird();
                    break;
                case (int)enumAppType.NanFangUnion:
                    iThirdApp = new NanFangUnion();
                    break;
                default:
                    break;
            }

            return iThirdApp;
        }
    }
}
