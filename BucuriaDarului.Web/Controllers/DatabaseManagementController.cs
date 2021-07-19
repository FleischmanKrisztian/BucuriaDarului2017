using BucuriaDarului.Contexts.SynchronizationContexts;
using BucuriaDarului.Gateway.DatabaseManagementGateways;
using Microsoft.AspNetCore.Mvc;

namespace BucuriaDarului.Web.Controllers
{
    public class DatabaseManagementController : Controller
    {
        public IActionResult ServerManagement()
        {
            return View();
        }

        public ActionResult SynchronizationResults(int numberOfModifications=0, int numberOfDeletions=0, string outOfSyncDocuments = "")
        {
            ViewBag.outOfSyncDocuments = outOfSyncDocuments;
            ViewBag.numberOfModifications = numberOfModifications.ToString();
            ViewBag.numberOfDeletions = numberOfDeletions.ToString();
            return View();
        }

        public ActionResult Synchronize()
        {
            return View();
        }

        public ActionResult BackupManagerApp()
        {
            return Redirect("BackupManagerApp:argument");
        }

        public ActionResult SynchronizeData()
        {
            var databaseSynchronizationContext = new DatabaseSynchronizationContext(new SynchronizationGateway());
            var response = databaseSynchronizationContext.Execute();
            return RedirectToAction("SynchronizationResults", "DatabaseManagement", new { response.NumberOfModifications, response.NumberOfDeletions, response.OutOfSyncDocuments });
        }
    }
}
