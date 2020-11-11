using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class LbzController : Controller
    {

        private Models.BookManagerEntities _ef;
        private Models.BookManagerEntities EF
        {
            get
            {

                if (_ef == null)
                {
                    _ef = new Models.BookManagerEntities();

                }
                return _ef;
            }


        }
        public ActionResult Index()
        {
           
            return View();




        }


        public ActionResult BIndex()
        {


            var list = EF.Book.ToList();
            return View(list);

        }


        [HttpGet]
        public ActionResult BAdd()
        {
          var list=  EF.Category.ToList();
            ViewBag.aa = list;

            return View();

        }
        [HttpPost]
        public ActionResult BAdd(Models.Book book)
        {

            EF.Book.Add(book);
            EF.SaveChanges();
           

            return Redirect("/lbz/BIndex");

        }


        public ActionResult Bdelete(Models.Borrow borrow)
        {
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Book.FirstOrDefault(x => x.ID == ID);
            if (!borrow.Use)
            {
                EF.Book.Remove(ido);
                EF.SaveChanges();

                return Redirect("/lbz/BIndex");
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult Eidt(Models.Category category)
        {
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Book.FirstOrDefault(x => x.ID == ID);
            var list = EF.Category.ToList();
            ViewBag.cate = list;
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
            EF.SaveChanges();
            return Redirect("/lbz/BIndex");


        }

    }
}
    
