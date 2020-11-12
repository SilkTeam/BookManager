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

            //分类
            var classify = EF.Category.ToList();
            ViewBag.ify = classify;
            //图书信息
            var Count = EF.Book.Count();
            var pageCount = Count / size;
            if (page < 0)
                pageCount = 1;
            if (Count % size != 0)
                pageCount++;
            if (page > pageCount)
                page = pageCount;
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
            var Quantity = mod.Number;
            if (Quantity < 0)
                return Content("很抱歉，目前没有此书");
            mod.Number = Quantity - 1;
            EF.SaveChanges();
            return Content("借书成功");
        }
        //还书
        public ActionResult Return()
        {
            return Content("还书成功");
        }
    }
}