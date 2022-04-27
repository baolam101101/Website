using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ApplicationDbContext context;

        public PostsController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Post.Include(p => p.Author);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (post.Anonymous == true)
                {
                    post.AuthorName = "Anonymous";
                }
                else
                {
                    post.AuthorName = post.Author.FullName;
                }
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create(Post post, int id)
        {
            post.submissionID = id;
            var submit = db.Submissions.Find(id);

            if (DateTime.Now > submit.Closure_Date)
            {
                return RedirectToAction("Fail", "Forum");
            }
            else
            {
                ViewBag.CategoryId = new SelectList(context.Categories.ToList(), "Id", "Name");
                Post newidea = new Post() {submissionID = id};
                return View(newidea);
            }
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,Anonymous,categoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.AuthorId = User.Identity.GetUserId();

                if (Request.Files != null)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Files"),
                        Path.GetFileName(file.FileName));
                        //Save file using Path+fileName take from above string
                        file.SaveAs(path);
                        post.DocumentName = Path.GetFileName(file.FileName);
                    }
                }

                    db.Post.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", post.AuthorId);
            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", post.categoryId);
            return View(post);
        }

        /*[HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Files"),
                    Path.GetFileName(file.FileName));

                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
*/
        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Time,AuthorId,AuthorName,Content,Anonymous,DocumentName,submissionId")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Post.Find(id);
            db.Post.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
