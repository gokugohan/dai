using DaiWebRole.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaiWebRole.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {        
        public ActionResult Index()
        {
            List<Log> logs = new AzureTableStorage().GetTableContents();
            return View(logs);
        }
    }
}