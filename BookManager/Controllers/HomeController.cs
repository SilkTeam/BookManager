using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BookManager.Models;

namespace BookManager.Controllers
{
    
    public class HomeController : Controller
    {

        private BookManagerEntities _ef;
        private BookManagerEntities EF
        {
            get {
                if (_ef == null)
                    _ef = new BookManagerEntities();
                return _ef;
            } 
        }

        public ActionResult Index()
        {
            if (Session["login-tea"] == null)
            {
                return Redirect("/home/login");
            }
            var list = EF.Book.ToList();
            return View(list);
        }


        public ActionResult BIndex()
        {
            var list = EF.Book.ToList();
            return View(list);
        }


        [HttpGet]
        public ActionResult BAdd()
        {
            if (Session["login-tea"] == null)
            {
                return Redirect("/home/login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult BAdd(Book book)
        {
            if (Session["login-tea"] == null)
            {
                return Redirect("/home/login");
            }
            EF.Book.Add(book);
            EF.SaveChanges();
            return Redirect("/Home/BIndex");
        }


        public ActionResult Bdelete(Borrow borrow)
        {
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Book.FirstOrDefault(x=>x.ID==ID);
            if (!borrow.Use)
            {
                EF.Book.Remove(ido);
                return Redirect("/Home/BIndex");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult  Eidt()
        {
            if (Session["login-tea"] == null)
            {
                return Redirect("/home/login");
            }
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Book.FirstOrDefault(x => x.ID == ID);
            return View(ido);
        }

        [HttpPost]
        public ActionResult Eidt(Book book)
        {
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Book.FirstOrDefault(x => x.ID == ID);
            ido.Name = book.Name;
            ido.Number = book.Number;
            ido.EntryTime = book.EntryTime;
            ido.Author = book.Author;
            ido.Category = book.Category;
            return View(ido);
        }
    }
}