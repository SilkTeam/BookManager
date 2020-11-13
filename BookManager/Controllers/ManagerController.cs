using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookManager.Filter;
using BookManager.Models;

namespace BookManager.Controllers
{
    [Auth]
    [Identity]
    public class ManagerController : Controller
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Book()
        {
            ViewBag.list = EF.Book.ToList();
            return View();
        }

        public ActionResult Category()
        {
            ViewBag.list = EF.Category.ToList();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Sigin()
        {
            if (Session["Identity"] != null)
                return Redirect("/Manager/Index");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Sigin(Sigin sigin)
        {
            var mod = EF.Sigin.FirstOrDefault(x => x.Username == sigin.Username && x.Password == sigin.Password);
            if (mod != null)
            {
                Session["User"] = mod;
                Session["Identity"] = mod.Identity;
                return Content("success");
            }
            else
            {
                return Content("用户名或密码错误");
            }
        }

        public new ActionResult User()
        {
            return View(EF.User.ToList());
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(Sigin sigin, User user)
        {
            if (EF.Sigin.FirstOrDefault(x => x.Username ==  user.Name) != null)
            {
                return Content("当前用户名已被占用！");
            }
            else
            {
                sigin.Username = user.Name;
                if (Convert.ToInt32(Session["Identity"]) == 1)
                    sigin.Identity = 0;

                EF.Sigin.Add(sigin);
                EF.SaveChanges();
                user.Uid = EF.Sigin.FirstOrDefault(x => x.Username == user.Name).ID;
                user.EntryTime = DateTime.Now;
                EF.User.Add(user);
                EF.SaveChanges();
                return Content("success");
            }
        }

        [HttpGet]
        public ActionResult EditUser(int ID)
        {
            var mod = EF.User.FirstOrDefault(x => x.ID == ID);
            return View(mod);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            var mod = EF.User.FirstOrDefault(x => x.ID == user.ID);
            if (mod != null)
            {
                mod = user;
                EF.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("非法访问！");// 用户不存在
            }
        }


        [HttpGet]
        public ActionResult AddBook()
        {
            ViewBag.list = EF.Category.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            if (Convert.ToInt32(Session["Identity"]) == 1)
            {
                return Content("<script>alert('权限不足');</script>");
            }
            else
            {
                if (book.Img != null)
                {
                    // 接收前端传回来的文件
                    var file = Request.Files["Img"];

                    // 限制文件大小
                    var fileSize = file.ContentLength;
                    var maxSize = fileSize * 2048;
                    if (fileSize > maxSize)
                        return Content("<script>alert('文件超出大小限制2M');</script>");//文件超出大小限制250kb

                    // 定义文件存储路径
                    var wlPath = Server.MapPath("/");
                    wlPath += $"/Upload/{DateTime.Now:yyyyMMdd}/";
                    if (!Directory.Exists(wlPath))
                        Directory.CreateDirectory(wlPath);

                    // 后端重命名文件、获取后缀名
                    var names = file.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    var expendName = names[names.Length - 1];
                    var newName = Guid.NewGuid().ToString("N") + "." + expendName;
                    var newPath = wlPath + newName;

                    // 判断后缀名是否允许
                    List<string> exlist = new List<string>() { "jpg", "jpeg", "git", "png", "bmp" };
                    if (exlist.Where(x => x == expendName).Count() == 0)
                        return Content("<script>alert('只可以上传图片');</script>");// 只可以上传图片！

                    // 保存并返回
                    file.SaveAs(newPath);
                    book.Img = $"/Upload/{DateTime.Now:yyyyMMdd}/" + newName;
                }

                book.EntryTime = DateTime.Now;
                EF.Book.Add(book);
                EF.SaveChanges();
                return Redirect("/Manager/Book");
            }
        }

        [HttpGet]
        public ActionResult EditBook(int ID)
        {
            var mod = EF.Book.FirstOrDefault(x => x.ID == ID);
            if(mod != null)
            {
                ViewBag.list = EF.Category.ToList();
                return View(mod);
            }
            else
            {
                return Content("非法访问");
            }
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            var mod = EF.Book.FirstOrDefault(x => x.ID == book.ID);

            mod = book;
            if (book.Img != null)
            {
                // 接收前端传回来的文件
                var file = Request.Files["Img"];

                // 限制文件大小
                var fileSize = file.ContentLength;
                var maxSize = fileSize * 2048;
                if (fileSize > maxSize)
                    return Content("<script>alert('文件超出大小限制2M');</script>");//文件超出大小限制250kb

                // 定义文件存储路径
                var wlPath = Server.MapPath("/");
                wlPath += $"/Upload/{DateTime.Now:yyyyMMdd}/";
                if (!Directory.Exists(wlPath))
                    Directory.CreateDirectory(wlPath);

                // 后端重命名文件、获取后缀名
                var names = file.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var expendName = names[names.Length - 1];
                var newName = Guid.NewGuid().ToString("N") + "." + expendName;
                var newPath = wlPath + newName;

                // 判断后缀名是否允许
                List<string> exlist = new List<string>() { "jpg", "jpeg", "git", "png", "bmp" };
                if (exlist.Where(x => x == expendName).Count() == 0)
                    return Content("<script>alert('只可以上传图片');</script>");// 只可以上传图片！

                // 保存并返回
                file.SaveAs(newPath);
                book.Img = $"/Upload/{DateTime.Now:yyyyMMdd}/" + newName;
            }

            EF.SaveChanges();
            return Redirect("/Manager/Book");
        }

        public ActionResult Delete(int ID)
        {
            var user = EF.User.FirstOrDefault(x => x.ID == ID);
            var sigin = EF.Sigin.FirstOrDefault(x => x.ID == user.Uid);
            if (sigin.Identity >= Convert.ToInt32(Session["Identity"]))
                return Content("权限不足");

            if (user != null && sigin != null)
            {
                EF.User.Remove(user);
                EF.Sigin.Remove(sigin);
                EF.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("用户不存在");
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = Session["Identity"] = null;
            return Content("success");
;       }
    }
}