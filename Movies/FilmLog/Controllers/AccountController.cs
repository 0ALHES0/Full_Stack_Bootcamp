using Microsoft.AspNetCore.Mvc;
using FilmLog.Models;

namespace FilmLog.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult LoginOrRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginOrRegister(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                // Basit bir kontrol (Gerçek sistemde veritabanı kontrolü yapılmalı)
                if (model.Email == "test@test.com" && model.Password == "123456")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz email veya şifre.");
                }
            }
            return View(model);
        }
    }
}
