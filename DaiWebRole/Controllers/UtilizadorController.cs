using DaiWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DaiWebRole.Controllers
{
    public class UtilizadorController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UtilizadorViewModel utilizador)
        {
            if (ModelState.IsValid)
            {
                using (EntityDai db = new EntityDai())
                {
                    var count = db.Utilizadores.Where(u=>u.UserName == utilizador.UserName && u.Password == utilizador.Password).Count();
                    if (count == 1)
                    {
                        FormsAuthentication.SetAuthCookie(utilizador.UserName, false);
                        ViewBag.mensagem = "Login sucesso";
                        return RedirectToAction("Index", "Livro");                        
                    }
                    else
                    {
                        ViewBag.mensagem = "Utilizador inválido.";
                        return View();
                    }
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult Logout()
        {
            
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Registar()
        {
            return View(new UtilizadorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Registar(UtilizadorViewModel utilizador)
        {
            if (ModelState.IsValid)
            {
                using (var db = new EntityDai())
                {
                    Utilizador u = new Utilizador
                    {
                        UserName = utilizador.UserName,
                        Password = utilizador.Password
                    };
                    db.Utilizadores.Add(u);
                    db.SaveChanges();
                }
            }
            else
            {
                ViewBag.mensagem = "Something is wrong bro.";
                return View();
            }
            return RedirectToAction("Login");
        }
    }
}