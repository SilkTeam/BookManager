using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BookManager.Models;

namespace BookManager.Controllers
{
    public class ManagerController : Controller
    {
        private BookManagerEntities _ef;
        public BookManagerEntities EF
        {
            get
            {
                if (_ef == null)
                    _ef = new BookManagerEntities();
                return _ef;
            }
        }

        public ActionResult Index()
        {
            return View(EF.User.ToList());
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult AddSave(Sigin sigin, User user)
        {
            EF.User.Add(user);
            EF.SaveChanges();
            return Redirect("/default/index");
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var deid = Convert.ToInt32(Request["deid"]);
            var mod = EF.User.FirstOrDefault(x => x.ID == deid);
            return View(mod);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            var mod = EF.User.FirstOrDefault(x => x.ID == user.ID);
            mod = user;
            //mod.Name = user.Name;
            //mod.Sex = user.Sex;
            //mod.Age = user.Age;
            //mod.Uid = user.Uid;
            //mod.EntryTime = user.EntryTime;
            EF.SaveChanges();
            return Redirect("/default/index");
        }

        public ActionResult Delete(int ID)
        {
            var user = EF.User.FirstOrDefault(x => x.ID == ID);
            var sigin = EF.Sigin.FirstOrDefault(x => x.ID == user.Uid);
            EF.User.Remove(user);
            EF.Sigin.Remove(sigin);
            EF.SaveChanges();
            return Content("success");
        }
    }
}