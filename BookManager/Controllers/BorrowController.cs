﻿using System;
using System.Collections.Generic;
using System.Data;
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
            return View(mod);
        }
        public ActionResult Bowing(Models.Book book)
        {
            var uname = Request["uname"];
            if (uname == null)
                return Content("读者姓名不能为空");
            var mod = EF.User.FirstOrDefault(x=>x.Name==uname);
            if (mod == null)
                return Content("没有该读者");
            var bookmod = EF.Book.FirstOrDefault(x=>x.ID==book.ID);
            var Number = bookmod.Number;
            if (Number == 0)
                return Content("目前没有该书");
            bookmod.Number = Number - 1;
            var Bmod = new Models.Borrow();
            Bmod.CardID = mod.ID;
            Bmod.GetTime= DateTime.Now;
            EF.Book.Add(bookmod);
            EF.Borrow.Add(Bmod);
            EF.SaveChanges();
            return Content("借书成功");
        }
        //还书
        public ActionResult Returnlogin()
        {
            return View();
        }
        public ActionResult Return(Models.Borrow borrow)
        {
            var CardID = Convert.ToInt32(Request["CardID "]);
            var mod = EF.Borrow.Where(x=>x.ID== CardID).Count();
            if (mod == 0)
                return Content("目前没有书可还");
            var bow = EF.Borrow.FirstOrDefault(x=>x.ID== CardID);
            var B1 = EF.Book.FirstOrDefault(x=>x.ID==bow.BookID);
            var U1 = EF.User.FirstOrDefault(x=>x.ID==bow.CardID);
            ViewBag.mod=mod;
            ViewBag.Book = B1;
            ViewBag.User = U1;
            return View();
        }
        public ActionResult Returning()
        {
            var id = Convert.ToInt32(Request["id"]);
            var mod = EF.Borrow.FirstOrDefault(x=>x.ID==id);
            mod.LoseTime = DateTime.Now;
            EF.Borrow.Add(mod);
            var bookmod = EF.Book.FirstOrDefault(x => x.ID == book.ID);
            var Number = bookmod.Number;
            bookmod.Number = Number +1;
            EF.Book.Add(bookmod);
            EF.SaveChanges();
            return Content("还书成功");
        }
    }
}