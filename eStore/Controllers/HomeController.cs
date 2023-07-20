using BusinessObject;
using DataAccess.Repository;
using eStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace eStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _Accessor;
        private readonly IMemberRepository _member;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor Accessor, IMemberRepository member)
        {
            _logger = logger;
            _Accessor = Accessor;
            _member = member;
        }

        public IActionResult Index()
        {
            ViewBag.Username = _Accessor.HttpContext.Session.GetString("Username");
            return View();
        }

        public IActionResult Profile()
        {
            var username= _Accessor.HttpContext.Session.GetString("Username");
            Member member = _member.GetMember(username);
            return View(member);
        }
        [HttpPost]
        public IActionResult Profile(int id, [Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            TempData["Error"] = "";
            if (ModelState.IsValid)
            {
                try
                { 
                    _member.Update(member);
                    TempData["Error"] = "Update successful!";
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }
            else
            {
                TempData["Error"] = "Wrong: please try again!";
            }
            return View(member);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}