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

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Sigin sigin, User user)
        {
            if (EF.Sigin.FirstOrDefault(x=>x.Username ==  user.Name) != null)
            {
                return Content("当前用户名已被占用！");
            }
            else
            {
                sigin.Username = user.Name;
                sigin.Password = "123456";
                sigin.Identity = 0;
                EF.Sigin.Add(sigin);
                EF.SaveChanges();
                user.Uid = EF.Sigin.FirstOrDefault(x => x.Username == user.Name).ID;
                EF.User.Add(user);
                EF.SaveChanges();
                return Content("success");
            }
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            var mod = EF.User.FirstOrDefault(x => x.ID == ID);
            return View(mod);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            var mod = EF.User.FirstOrDefault(x => x.ID == user.ID);
            if (mod != null)
            {
                mod = user;
                EF.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("非法访问！");// 用户不存在
            }
        }

        public ActionResult Delete(int ID)
        {
            var user = EF.User.FirstOrDefault(x => x.ID == ID);
            var sigin = EF.Sigin.FirstOrDefault(x => x.ID == user.Uid);
            if (user != null && sigin != null)
            {
                EF.User.Remove(user);
                EF.Sigin.Remove(sigin);
                EF.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("用户不存在");
            }
        }
    }
}