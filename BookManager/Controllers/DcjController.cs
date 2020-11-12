using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class DcjController : Controller
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
        public ActionResult indexS()
        {
            var list= EF.Sigin.ToList();
            return View(list);
        }
        public ActionResult indexU()
        {
            var list= EF.User.ToList();
            return View(list);
        }
        //添加管理员
        public ActionResult Sigin()
        {
            return View();
        }
        public ActionResult SiginAdd(Models.Sigin sigin)
        {
            sigin.Identity = 1;
            var pwd2 = Request["pwd"];
            if (sigin.Password == "")
            {
                return Content("密码不能为空，请重新输入！");
            }
            if (sigin.Password != pwd2)
            {
                return Content("两次密码不一致，请重新输入！");
            }
            EF.Sigin.Add(sigin);
            EF.SaveChanges();
            return Redirect("/Dcj/indexS");
        }
        //添加学生信息
        public ActionResult User()
        {
            return View();
        }
        public ActionResult UserAdd(Models.User user)
        {
            if (user.Name != null)
            {
                user.EntryTime = DateTime.Now;
                user.Uid = 1;
                EF.User.Add(user);
                EF.SaveChanges();
                return Redirect("/Dcj/indexU");
            }
            return Content("请输入学生信息。");
        }
    }
}