using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS_School.Controllers
{
    public class Reactions : Controller
    {
        // GET: Reactions
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reactions/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reactions/Create
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

        // GET: Reactions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reactions/Edit/5
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

        // GET: Reactions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reactions/Delete/5
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
