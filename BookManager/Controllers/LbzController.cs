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


        public ActionResult BIndex(int page = 1, int size = 10)
        {


            var count = EF.Book.Count();
            var pageCount = count / size;

            if (count % size != 0)
                pageCount++;
            if (page > pageCount)
            {
                page = pageCount;
            }
            else if (page < 1)
            {
                page = 1;
            }

            var mod = EF.Sigin;
            var list = EF.Book.OrderBy(x => x.ID).Skip((page - 1) * size).Take(size).ToList();
            //var list = EF.Book.ToList();
            ViewBag.page = page;
            ViewBag.pageCount = pageCount;
            return View(list);



        }


        [HttpGet]
        public ActionResult BAdd()
        {
            var list = EF.Category.ToList();
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

        [HttpGet]
        public ActionResult CIndex(int page = 1, int size = 10)
        {



            var count = EF.Category.Count();
            var pageCount = count / size;

            if (count % size != 0)
                pageCount++;
            if (page > pageCount)
            {
                page = pageCount;
            }
            else if (page < 1)
            {
                page = 1;
            }

            var mod = EF.Sigin;
            var list = EF.Category.OrderBy(x => x.ID).Skip((page - 1) * size).Take(size).ToList();
            ViewBag.page = page;
            ViewBag.pageCount = pageCount;
            return View(list);






        }

    

        [HttpGet]
        public ActionResult CAdd()
        {

            return View();

        }
        [HttpPost]
        public ActionResult CAdd(Models.Category book)
        {

            EF.Category.Add(book);
            EF.SaveChanges();


            return Redirect("/lbz/CIndex");

        }

        public ActionResult Cdelete(int ID)

        {
            var mod = EF.Book.FirstOrDefault(x => x.Category == ID);
            if (mod != null)
            {
                return Content("");
            }
            else
            {
                var ido = EF.Category.FirstOrDefault(x => x.ID == ID);


                EF.Category.Remove(ido);
                EF.SaveChanges();

                return Redirect("/lbz/CIndex");
            }
         


        }
        [HttpGet]
        public ActionResult CEidt()
        {
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Category.FirstOrDefault(x => x.ID == ID);
         
           
            return View(ido);

        }
        [HttpPost]
        public ActionResult CEidt( Models.Category category)
        {
            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Category.FirstOrDefault(x => x.ID == ID);
            ido.Name = category.Name;
            EF.SaveChanges();
            return Redirect("/lbz/CIndex");


        }

    }
}
    
