using BucuriaDarului.Contexts.SettingsContexts;
using BucuriaDarului.Gateway.SettingsGateways;
using BucuriaDarului.Web.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BucuriaDarului.Web.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Settings(string lang, int quantity)
        {
            var settingContext = new SettingsUpdateContext(new SettingsUpdateGateway());
            settingContext.Execute(lang, quantity);

            TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = quantity;
            TempData[Constants.CONNECTION_LANGUAGE] = lang;
            SetCookie(lang);
            return RedirectToAction("Index", "Home");
        }

        public void SetCookie(string language)
        {
            Response.Cookies.Append(
             CookieRequestCultureProvider.DefaultCookieName,
             CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
             new CookieOptions
             {
                 Expires = DateTimeOffset.UtcNow.AddYears(1),
                 IsEssential = true,
                 SameSite = SameSiteMode.Lax
             }
       );
        }

        public ActionResult FirstStartup()
        {
            var firstStartupContext = new FirstStartupContext();
            var response = firstStartupContext.Execute();
            TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = response.NumberOfItemsPerPage;
            TempData[Constants.CONNECTION_LANGUAGE] = response.Language;
            return RedirectToAction("Index", "Home");
        }
    }
}