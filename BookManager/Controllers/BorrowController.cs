﻿using BookManager.Filter;
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
            var Bid = Convert.ToInt32(Session["BorrowID"]);
            ViewBag.UserInfo = EF.User.FirstOrDefault(x => x.ID == Bid);
            return View();
        }

        // 图书借阅
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
                log.Info = "Send1";
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

        // 图书归还:User
        [HttpGet]
        public ActionResult Lose()
        {
            var ID = Convert.ToInt32(Session["BorrowID"]);
            var list = EF.Borrow.Where(x => x.CardID == ID && x.Use == true).ToList();
            if (list != null)
            {
                return View(list);
            }
            return View();
        }

        // 图书归还:User
        [HttpPost]
        public ActionResult Lose(int ID)
        {
            var uID = Convert.ToInt32(Session["BorrowID"]);
            var mod = EF.Borrow.FirstOrDefault(x=>x.ID == ID && x.CardID == uID && x.Use == true);
            if (mod != null)
            {
                mod.Use = false;
                mod.LoseTime = DateTime.Now;
                EF.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("越权操作！");
            }
        }
    }
}