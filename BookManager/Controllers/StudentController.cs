using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BookManager.Models;

namespace BookManager.Controllers
{
    public class StudentController : Controller
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
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logining(Sigin sigin)
        {
            var mod = EF.Sigin.FirstOrDefault(x => x.Username == sigin.Username && x.Password == sigin.Password);
            if (mod == null)
            {
                return Content("用户名或密码错误");
            }
            else
            {
                return Content("success");
            }
        }

    }
}