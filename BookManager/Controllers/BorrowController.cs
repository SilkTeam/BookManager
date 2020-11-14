using BookManager.Filter;
using BookManager.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    [Auth]
    public class BorrowController : Controller
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

        public ActionResult Borrow()
        {
            var id = Convert.ToInt32(Request["id"]);
            var mod = EF.Book.FirstOrDefault(x => x.ID == id);
            return View(mod);
        }

        //借书
        public ActionResult Bowing(Book book)
        {
            var uname = Request["uname"];
            if (uname == null)
                return Content("读者姓名不能为空");
            var mod = EF.User.FirstOrDefault(x => x.Name == uname);
            if (mod == null)
                return Content("没有该读者");
            var bookmod = EF.Book.FirstOrDefault(x => x.ID == book.ID);
            var Number = bookmod.Number;
            if (Number == 0)
                return Content("目前没有该书");
            bookmod.Number = Number - 1;
            var Bmod = new Models.Borrow();
            Bmod.BookID = book.ID;
            Bmod.CardID = mod.ID;
            Bmod.GetTime = DateTime.Now;
            EF.SaveChanges();
            return Content("借书成功");
        }

        //还书模块
        //查询
        public ActionResult Return()
        {
            var CardID = Convert.ToInt32(Request["CardID"]);
            var mod = EF.Borrow.Where(x => x.CardID == CardID).Count();
            if (mod == 0)
                return Content("目前没有书可还");
            var bow = EF.Borrow.FirstOrDefault(x => x.ID == CardID);
            var B1 = EF.Book.FirstOrDefault(x => x.ID == bow.BookID);
            var U1 = EF.User.FirstOrDefault(x => x.ID == bow.CardID);
            ViewBag.mod = mod;
            ViewBag.Book = B1;
            ViewBag.User = U1;
            return View();
        }

        //还书
        public ActionResult Returning()
        {
            var id = Convert.ToInt32(Request["id"]);
            var mod = EF.Borrow.FirstOrDefault(x => x.ID == id);
            mod.LoseTime = DateTime.Now;
            EF.Borrow.Add(mod);
            var bookmod = EF.Book.FirstOrDefault(x => x.ID == mod.BookID);
            var Number = bookmod.Number;
            bookmod.Number = Number + 1;
            EF.SaveChanges();
            return Content("还书成功");
        }
    }
}

