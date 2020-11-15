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

        // 借阅信息详单生成
        public ActionResult Index(int ID)
        {
            // 通过图书ID查找图书
            ViewBag.BookInfo = EF.Book.FirstOrDefault(x => x.ID == ID);
            // 通过用户ID查找用户
            ViewBag.UserInfo = EF.User.FirstOrDefault(x => x.ID == Convert.ToInt32(Session["BorrowID"]));
            return View();
        }

        public ActionResult Give(int UserID, int BookID, Borrow borrow, Log log)
        {
            // 通过图书ID查找图书
            var book = EF.Book.FirstOrDefault(x => x.ID == BookID);
            // 通过用户ID查找用户
            var user = EF.User.FirstOrDefault(x => x.ID == UserID);
            // 判断当前登录的用户
            if (Convert.ToInt32(Session["Identity"]) < 1)
            {
                log.Uid = user.ID;
                log.Bid = book.ID;
                log.Info = "Send";
                log.EntryTime = DateTime.Now;
                EF.Log.Add(log);
                EF.SaveChanges();
                return Content("sendok");
            }
            else
            {
                borrow.BookID = book.ID;
                borrow.CardID = user.ID;
                borrow.Use = true;
                borrow.GetTime = DateTime.Now;
                EF.Borrow.Add(borrow);
                EF.SaveChanges();
                return Content("success");
            }
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

