using DaiWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaiWebRole.Controllers
{
    [Authorize(Users="helder")]
    public class EmprestimoController : Controller
    {
        private EntityDai db = new EntityDai();
        // GET: Emprestimo
        public ActionResult Index(int id)
        {               
            return View();
        }

        // GET: Emprestimo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Emprestimo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emprestimo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Emprestimo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Emprestimo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Emprestimo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Emprestimo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
