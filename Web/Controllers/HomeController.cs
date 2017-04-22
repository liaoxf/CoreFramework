using AutofacFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private IAuthService _authService;
        public HomeController(IAuthService authService) {
            this._authService = authService;
        }
        // GET: Index
        public ActionResult Index()
        {
            _authService.Auth();
            ViewBag.result = _authService.GetAuthItems().First().MenuName + DateTime.Now.ToString("hh:mm:ss");
            return View();
        }


        // GET: Index/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Index/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Index/Create
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

        // GET: Index/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Index/Edit/5
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

        // GET: Index/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Index/Delete/5
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
