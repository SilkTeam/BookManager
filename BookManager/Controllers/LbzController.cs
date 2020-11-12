using System;
using System.Collections.Generic;
using System.IO;
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
            var list = EF.Category.ToList();
            ViewBag.aa = list;

            return View();

        }
        [HttpPost]
        public ActionResult BAdd(Models.Book book)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }

            var wj = Request.Files["Img"];
            var gebmu = Server.MapPath("/");
            //修改文件
            var wjm = wj.FileName;
            var fwjm = wjm.Split(new char[] { '.' });
            var kzm = fwjm[fwjm.Length - 1];
            var xwjm = Guid.NewGuid().ToString("N") + "." + kzm;

            //修改储存位置
            var wjjlj = "/Upload/auhefjkaw/";
            var wjlj = gebmu + wjjlj;
            if (!Directory.Exists(wjlj))
            { Directory.CreateDirectory(wjlj); }

            //文件大小
            var fsize = wj.ContentLength;
            var msize = 1024 * 8521;
            if (fsize > msize)
            {

                return Content("文件太大");
            }

            //文件格式
            List<string> exlit = new List<string>() { "jpg", "png", "git", "jpeg", "bmp" };
            int nunm = exlit.Where(x => x == kzm).Count();
            if (nunm == 0)
            {
                return Content("只能允许上传图片");

            }

            var wjxdlj = wjjlj + xwjm;

            wj.SaveAs(wjlj + xwjm);

            book.Img = wjxdlj;


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
            if (!ModelState.IsValid)
            {
                return View();

            }

            var ID = Convert.ToInt32(Request["ID"]);
            var ido = EF.Book.FirstOrDefault(x => x.ID == ID);
           

            var fs = Request.Files["Img"];
            if (fs.ContentLength > 0)
            {
                var fileSize = fs.ContentLength; // 字节b     1kb = 1024b  1mb = 1024kb
                var maxSize = 1024 * 2566; // 250kb
                if (fileSize > maxSize)
                    return Content("文件限制2566kb");

                var wlPath = Server.MapPath("/");
                var fileXDPath = $"/Upload/{DateTime.Now.ToString(@"yyyyMMdd")}/";
                // 文件存放位置
                wlPath = wlPath + fileXDPath; // F:/MVC/UploadOne/UploadOne/Upload/2020-11-04/
                                              // 创建文件夹
                if (!Directory.Exists(wlPath)) // 判断是否存在此目录
                    Directory.CreateDirectory(wlPath); // 创建

                // 文件重名覆盖
                // Split 分割字符串
                var names = fs.FileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);// ["007", "png"]
                var expendName = names[names.Length - 1]; // png
                var newName = Guid.NewGuid().ToString("N") + "." + expendName; // name.png
                var newPath = wlPath + newName; // F:/MVC/UploadOne/name.png

                // 定义允许上传的文件格式
                List<string> exlist = new List<string>() { "jpg", "png", "git", "jpeg", "bmp" };
                // Where 传入一个条件，返回符合条件的数据集合
                // Count 获取集合中对象数量
                int number = exlist.Where(x => x == expendName).Count();
                if (number == 0)
                    return Content("文件格式错误，仅限图片");

                fileXDPath = fileXDPath + newName;
                // F:\MVC\UploadOne\UploadOne\/Upload/20201105/f67048180b884ac3a6aff03760adf37b.png
                fs.SaveAs(newPath);

             ido.Img= fileXDPath;
            }
          
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
            if (!ModelState.IsValid)
            {
                return View();

            }
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
            if (!ModelState.IsValid)
            {
                return View();

            }
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
    
