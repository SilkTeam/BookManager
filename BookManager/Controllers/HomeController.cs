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

        private Models.BookManagerEntities _ef;
        private Models.BookManagerEntities EF
        {
            get {

                if (_ef == null)
                {
                    _ef = new Models.BookManagerEntities();

                }
                                return _ef;
            } 


        }
        public ActionResult Index()
        {
          var list=  EF.Book.ToList();
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


            return View();

        }
        [HttpPost]
        public ActionResult BAdd(Models.Book book)
        {


            EF.Book.Add(book);
            EF.SaveChanges();
            return Redirect("/Home/BIndex");

        }


        public ActionResult Bdelete(Models.Borrow borrow)
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
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Book.FirstOrDefault(x => x.ID == ID);

            return View(ido);

        }


        [HttpPost]
        public ActionResult Eidt(Models.Book book)
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