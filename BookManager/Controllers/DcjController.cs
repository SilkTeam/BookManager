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
        public ActionResult Sigin()
        {
            return View();
        }
        public ActionResult SiginAdd(Models.Sigin sigin)
        {
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
            return Content("管理员添加成功！");
        }
        public ActionResult User()
        {
            return View();
        }
        public ActionResult UserAdd(Models.User user)
        {
            EF.User.Add(user);
            EF.SaveChanges();
            return Content("学生信息添加成功！");
        }
    }
}