﻿using BookManager.Filter;
using BookManager.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class HomeController : Controller
    {

        private BookManagerEntities _ef;
        private BookManagerEntities EF
        {
            get
            {
                if (_ef == null)
                    _ef = new BookManagerEntities();
                return _ef;
            }
        }

        public ActionResult Index(int siz=10)
        {
            ViewBag.list = EF.Book.Take(siz).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["Identity"] != null)
                return Redirect("/Manager/Index");

            return View();
        }

        [HttpPost]
        public ActionResult Register(Sigin sigin, User user)
        {
            if (EF.Sigin.FirstOrDefault(x => x.Username == user.Name) != null)
            {
                return Content("当前用户名已被占用！");
            }
            else
            {
                sigin.Username = user.Name;
                //sigin.Password = sigin.Password;
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

        [Auth]
        [HttpPost]
        public ActionResult Send()
        {
            var ID = Convert.ToInt32(Session["BorrowID"]);
            //var data = "Send";
            var mod = EF.Log.Where(x => x.Uid == ID && x.Info.IndexOf("Send1") > -1);
            if (mod.Count() > 0)
            {
                string bookName = "";
                foreach (var item in mod)
                {
                   bookName = item.Book.Name + "\r\n";
                }
                return Content(bookName);
            }
            else
            {
                return Content("你没有正在申请借阅的书籍");
            }
        }

        [HttpGet]
        public ActionResult Forget() {
            return View(); 
        }

        [HttpPost]
        public ActionResult Forget(string Name, string Phone, string Password)
        {
            var user = EF.User.FirstOrDefault(x => x.Name == Name && x.Phone == Phone);
            var sigin = EF.Sigin.FirstOrDefault(x => x.Username == Name);
            if (user != null)
            {
                sigin.Password = Password;
                EF.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("手机号验证失败");
            }
        }

        public ActionResult List()
        {
            ViewBag.list = EF.Book.ToList();
            return View();
        }
    }
}