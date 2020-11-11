using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        private Models.BookManagerEntities1 _ef;
        private Models.BookManagerEntities1 EF
        {
            get
            {

                if (_ef == null)
                {
                    _ef = new Models.BookManagerEntities1();

                }
                return _ef;
            }


        }
        public ActionResult Index()
        {
            var xians = EF.User.ToList();
            return View(xians);
        }
        public ActionResult Add()
        {
            var xinz = EF.User.ToList();
            ViewBag.xinzs = xinz;
            return View();
        }
        public ActionResult AddSave(Models.User user)
        {
            EF.User.Add(user);
            EF.SaveChanges();
            return Redirect("/default/index");
        }
        public ActionResult Edit()
        {
            var deid = Convert.ToInt32(Request["deid"]);
            var mod = EF.User.FirstOrDefault(x => x.ID == deid);
            return View(mod);
        }
        public ActionResult EditSave(Models.User newuser)
        {
            var old = EF.User.FirstOrDefault(x => x.ID == newuser.ID);
            old.Name = newuser.Name;
            old.Sex = newuser.Sex;
            old.Age = newuser.Age;
            old.Uid = newuser.Uid;
            old.EntryTime = newuser.EntryTime;
            EF.SaveChanges();
            return Redirect("/default/index");
        }
        public ActionResult Delete(int deid)
        {
            var mod = EF.User.FirstOrDefault(x => x.ID == deid);
            EF.User.Remove(mod);
            EF.SaveChanges();
            return Redirect("/default/index");
        }
    }
}