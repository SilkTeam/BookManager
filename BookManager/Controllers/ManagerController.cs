using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookManager.Filter;
using BookManager.Models;

namespace BookManager.Controllers
{
    [Auth]
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Sigin()
        {
            if (Session["Identity"] != null)
                return Redirect("/Manager/Index");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Sigin(Sigin sigin)
        {
            var mod = EF.Sigin.FirstOrDefault(x => x.Username == sigin.Username && x.Password == sigin.Password);
            if (mod != null)
            {
                Session["User"] = mod;
                Session["Identity"] = mod.Identity;
                return Content("success");
            }
            else
            {
                return Content("用户名或密码错误");
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
            if (EF.Sigin.FirstOrDefault(x => x.Username ==  user.Name) != null)
            {
                return Content("当前用户名已被占用！");
            }
            else
            {
                sigin.Username = user.Name;
                //sigin.Password = "123456";
                if (Convert.ToInt32(Session["Identity"]) == 1)
                    sigin.Identity = 0;

                EF.Sigin.Add(sigin);
                EF.SaveChanges();
                user.Uid = EF.Sigin.FirstOrDefault(x => x.Username == user.Name).ID;
                user.EntryTime = DateTime.Now;
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
            if (sigin.Identity >= Convert.ToInt32(Session["Identity"]))
                return Content("权限不足");

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

        public ActionResult Logout()
        {
            Session["User"] = Session["Identity"] = null;
            return Content("success");
;        }
    }
}