using Finalaplication.Common;
using Finalaplication.DatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Finalaplication.Controllers
{
    public class SettingsController : Controller
    {
        private SettingsManager settingsManager = new SettingsManager();

        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Settings(string lang, int quantity)
        {
            Settings set = settingsManager.GetSettingsItem();
            set.Quantity = quantity;
            set.Lang = lang;
            ViewBag.Lang = lang;
            settingsManager.UpdateSettings(set);
            TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = set.Quantity;
            TempData[Constants.CONNECTION_LANGUAGE] = set.Lang;
            Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            }
      );
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Firststartup()
        {
            try
            {
                Settings settings = settingsManager.GetSettingsItem();
                if (settings != null)
                {
                    TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = settings.Quantity;
                    TempData[Constants.CONNECTION_LANGUAGE] = settings.Lang;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    settings = new Settings
                    {
                        Lang = "en",
                        Quantity = 15
                    };
                    settingsManager.AddSettingsToDB(settings);
                    TempData[Constants.NUMBER_OF_ITEMS_PER_PAGE] = settings.Quantity;
                    TempData[Constants.CONNECTION_LANGUAGE] = settings.Lang;

                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }
    }
}