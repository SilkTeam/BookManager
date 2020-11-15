using BookManager.Filter;
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

        public ActionResult Index()
        {
            ViewBag.list = EF.Book.ToList();
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
                //sigin.Password = "123456";
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
        public ActionResult Send()
        {
            var user = Session["User"] as User;
            var mod = EF.Log.Where(x => x.Uid == user.ID && x.Info == "Send");
            if (mod.Count() > 0)
            {
                string bookName = "";
                foreach (var item in mod)
                {
                    bookName = item.Bid + "\r\n";
                }
                return Content(bookName);
            }
            else
            {
                return Content("你没有正在申请借阅的书籍");
            }
        }
    }
}