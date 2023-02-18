using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS_School.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    public class IdeasController : Controller
    {
        // GET: ContributionsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ContributionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContributionsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContributionsController/Create
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

        // GET: ContributionsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContributionsController/Edit/5
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

        // GET: ContributionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContributionsController/Delete/5
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
