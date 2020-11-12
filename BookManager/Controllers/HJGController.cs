using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class HJGController : Controller
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
        // GET: HJG
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult User()
        {
            return View();
        }
        public ActionResult UserAdd(Models.User user)
        {
            EF.User.Add(user);
            EF.SaveChanges();
            return Content("注册成功");
        }
    }
}