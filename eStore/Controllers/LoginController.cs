using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly MemberRepository _memberRepository=new MemberRepository();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string name, string pass)
        {
            if (name.Trim().Equals("admin@estore.com") && pass.Trim().Equals("admin@@"))
            {
                HttpContext.Session.SetString("Username", name.Trim());
                return RedirectToAction("Index", "Home");
            }
            var member = _memberRepository.GetMembers()
                .Where(x => x.Email == name.Trim() && x.Password == pass.Trim())
                .FirstOrDefault();
            if(member != null) {
                HttpContext.Session.SetString("Username", name.Trim());
                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = "Wrong: please try again!";
            return RedirectToAction("Index");
        }
    }
}
