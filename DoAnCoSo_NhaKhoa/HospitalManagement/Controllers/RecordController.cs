using BELibrary.Core.Entity;
using BELibrary.DbContext;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HospitalManagement.Controllers
{
    public class RecordController : BaseController
    {
        // GET: Record
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Account");
        }
    }
}