using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Finalaplication.Common
{
    public class ControllerHelper
    {
        public static void setViewBagEnvironment(ITempDataDictionary tempDataDic, dynamic viewBag)
        {
            string message = tempDataDic.Peek("environment").ToString();
            viewBag.env = message;
        }
    }
}
