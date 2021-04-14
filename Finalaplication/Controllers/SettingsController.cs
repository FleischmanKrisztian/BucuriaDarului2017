using Finalaplication.Common;
using Finalaplication.DatabaseLocalManager;
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

        public SettingsController()
        {
        }

        public IActionResult Settings()
        {
            try
            {
                ViewBag.env = TempData.Peek(VolMongoConstants.CONNECTION_ENVIRONMENT);
                return View();
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        [HttpPost]
        public ActionResult Settings(string lang, int quantity)
        {
            Settings set = settingsManager.GetSettingsItem();
            set.Quantity = quantity;
            set.Lang = lang;
            ViewBag.Lang = lang;
            TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = set.Quantity;
            TempData[VolMongoConstants.CONNECTION_LANGUAGE] = set.Lang;
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
                Settings set = settingsManager.GetSettingsItem();
                TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = set.Quantity;
                TempData[VolMongoConstants.CONNECTION_LANGUAGE] = set.Lang;

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                Settings set = new Settings
                {
                    Lang = "English",
                    Quantity = 15,
                };
                TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = set.Quantity;
                TempData[VolMongoConstants.CONNECTION_LANGUAGE] = set.Lang;

                return RedirectToAction("Index", "Home");
            }
        }
    }
}