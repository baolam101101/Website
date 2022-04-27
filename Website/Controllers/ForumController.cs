using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Website.Models;

namespace Website.Controllers
{
    public class ForumController : BaseController
    {
        protected ApplicationDbContext data = new ApplicationDbContext();
        public ActionResult Index(int Page = 1, int PageSize = 5)
        {
            var post = data.Post.ToList();
            var page_posts = post.OrderByDescending(x => x.Time).ToPagedList(Page, PageSize);
            return View(page_posts);
        }

        public ActionResult Fail()
        {
            return View();
        }

        public ActionResult FailComment()
        {
            return View("FailComment");
        }

        public ActionResult FailDelete()
        {
            return View("FailDelete");
        }
    }
}