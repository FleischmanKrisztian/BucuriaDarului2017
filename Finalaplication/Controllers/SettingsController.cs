using Finalaplication.App_Start;
using Finalaplication.Common;
using Finalaplication.DatabaseManager;
using Finalaplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Linq;

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
        public ActionResult Settings(string lang, string env, int quantity)
        {
            Settings set = settingsManager.GetSettingsItem();
            set.Env = env;
            set.Quantity = quantity;
            set.Lang = lang;
            ViewBag.Lang = lang;
            TempData[VolMongoConstants.CONNECTION_ENVIRONMENT] = set.Env;
            TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = set.Quantity;
            TempData[VolMongoConstants.CONNECTION_LANGUAGE] = set.Lang;
            Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
            new CookieOptions { 
              Expires = DateTimeOffset.UtcNow.AddYears(1),
              IsEssential = true,
              SameSite = SameSiteMode.Lax }
      );

            string condition = "i";
            settingsManager.UpdateSettingsItem_Env(condition,set);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Changeenvironment()
        {
            try
            {
                Settings set = settingsManager.GetSettingsItem();
                set.Env = VolMongoConstants.CONNECTION_MODE_OFFLINE;
                string condition = "i";
                settingsManager.UpdateSettingsItem_Env(condition, set);
                TempData[VolMongoConstants.CONNECTION_ENVIRONMENT] = set.Env;
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Localserver", "Home");
            }
        }

        public ActionResult Firststartup()
        {
            try
            {
                Settings set = settingsManager.GetSettingsItem();
                TempData[VolMongoConstants.CONNECTION_ENVIRONMENT] = set.Env;
                TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = set.Quantity;
                TempData[VolMongoConstants.CONNECTION_LANGUAGE] = set.Lang;

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                Settings set = new Settings
                {
                    Lang = "en",
                    Quantity = 10,
                    Env = "offline"
                };
                TempData[VolMongoConstants.CONNECTION_ENVIRONMENT] = set.Env;
                TempData[VolMongoConstants.NUMBER_OF_ITEMS_PER_PAGE] = set.Quantity;
                TempData[VolMongoConstants.CONNECTION_LANGUAGE] = set.Lang;

                return RedirectToAction("Index", "Home");
            }
        }
    }
}