using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookManager.Models;

namespace BookManager.Controllers
{
    public class BorrowController : Controller
    {
        // GET: Borrow
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
        //图书查询
        public ActionResult Index(int page=1,  int size=20)
        {
            var Count = EF.Book.Count();
            var pageCount = Count / size;
            if (Count % size != 0)
                pageCount++;
            var mod = EF.Book.OrderBy(x=>x.ID).Skip((page-1)*size).Take(size).ToList();
            ViewBag.page = page;
            ViewBag.pageCount = pageCount;
            return View( mod);
        }
        //借书
        public ActionResult Borrowing()
        {
            var id = Convert.ToInt32(Request["id"]);
            var mod = EF.Book.FirstOrDefault(x=>x.ID==id);
            return View(mod);
        }
        //还书
        public ActionResult Return()
        {
            return Content("还书成功");
        }
    }
}