using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                default:
                    break;
            }

            return iThirdApp;
        }
    }
}
