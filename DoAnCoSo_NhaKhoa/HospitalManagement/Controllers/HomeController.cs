using BELibrary.Core.Entity;
using BELibrary.DbContext;
using System.Linq;
using System.Web.Mvc;

namespace HospitalManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var test = dbContext.News.FirstOrDefault();
            //var testview = Mapper.Map<NewsViewModel>(test);

            using (var workScope = new UnitOfWork(new HospitalManagementDbContext()))
            {
                ViewBag.Doctors = workScope.Doctors.Include(x => x.Faculty).Take(8).ToList();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Team()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Service()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Price()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Activities()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Dental()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page. ";

            return View();
        }

        public ActionResult BocRangSu()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Implant()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult NiengRang()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Veneer()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult TayTrang()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult NhoRang()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult NhaChu()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Tuy()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult TramRang()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult ThaiPhu()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult E404()
        {
            ViewBag.Message = "Your contact page. ";

            return View();
        }
    }
}