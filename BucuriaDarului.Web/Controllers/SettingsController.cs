using BucuriaDarului.Contexts.SettingsContexts;
using BucuriaDarului.Core;
using BucuriaDarului.Gateway.SettingsGateways;
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
            return View(SingleSettingReturnerGateway.GetSettingItem());
        }

        [HttpPost]
        public ActionResult Settings(string lang, int quantity, int numberOfDaysBeforBirthday, int numberOfDaysBeforeExpiration)
        {
            var settingContext = new SettingsUpdateContext(new SettingsUpdateGateway());
            settingContext.Execute(lang, quantity, numberOfDaysBeforBirthday, numberOfDaysBeforeExpiration);

            TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = quantity;
            TempData[Constants.CONNECTION_LANGUAGE] = lang;
            TempData[Constants.ALARM_NUMBER_OF_DAYS_BEFORE_BIRTHDAY] = numberOfDaysBeforBirthday;
            TempData[Constants.NUMBER_OF_DAYS_BEFORE_EXPIRATION] = numberOfDaysBeforeExpiration;

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
            TempData[Constants.ALARM_NUMBER_OF_DAYS_BEFORE_BIRTHDAY] = response.NumberOfDaysBeforBirthday;
            TempData[Constants.NUMBER_OF_DAYS_BEFORE_EXPIRATION] = response.NumberOfDaysBeforeExpiration;
            return RedirectToAction("Index", "Home");
        }
    }
}