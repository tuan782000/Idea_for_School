using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS_School.Controllers
{
    public class Views : Controller
    {
        // GET: Views
        public ActionResult Index()
        {
            return View();
        }

        // GET: Views/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Views/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Views/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Views/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Views/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Views/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Views/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
