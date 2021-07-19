using BucuriaDarului.Contexts.ExcelPrinterControllerContext;
using BucuriaDarului.Gateway.ExcelPrinterGateways;
using BucuriaDarului.Web.Common;
using Microsoft.AspNetCore.Mvc;

namespace BucuriaDarului.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelprinterController : ControllerBase
    {
        [HttpGet("{keys}", Name = "Get")]
        public string Get(string keys)
        {
            var ids_ = string.Empty;
            var header = string.Empty;
            var key1 = string.Empty;
            var key2 = string.Empty;
            if (keys.Contains(";") == true)
            {
                var splited = keys.Split(";");
                key1 = splited[0];
                key2 = splited[1];
            }

            DictionaryHelper.d.TryGetValue(key1, out ids_);
            DictionaryHelper.d.TryGetValue(key2, out header);

            var context = new ExcelPrinterContext(new ExcelPrinterGateway());
            var jsonstring = context.Execute(ids_, header);

            return jsonstring;
        }
    }
}