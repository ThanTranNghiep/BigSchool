using BigSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BigSchool.ViewModels;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var UpcommingCourses = _context.Courses
                .Include(x => x.Lecturer)
                .Include(x => x.Category)
                .Where(x => x.DateTime > DateTime.Now);
            // return View(UpcommingCourses);

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = UpcommingCourses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}