using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class AdminController : Controller
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
            ViewBag.list = EF.User.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Sigin sigin, User user)
        {
            if (EF.Sigin.FirstOrDefault(x => x.Username == user.Name) != null)
            {
                return Content("当前用户名已被占用！");
            }
            else
            {
                sigin.Username = user.Name;
                EF.Sigin.Add(sigin);
                EF.SaveChanges();
                user.Uid = EF.Sigin.FirstOrDefault(x => x.Username == user.Name).ID;
                EF.User.Add(user);
                EF.SaveChanges();
                return Content("success");
            }
        }
    }
}