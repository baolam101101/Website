using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        [HttpGet]
        public ActionResult Comment(int id)
        {
            var test = db.Post.Find(id);
            var test_date = db.Submissions.Find(test.submissionID);
            if (DateTime.Now > test_date.Final_Date)
            {
                return RedirectToAction("Fail", "Forum");
            }
            else
            {
                return PartialView("Comment", new Website.Models.Comment { IdeaId = id, submissionid = test.submissionID });
            }
        }

        [HttpPost]
        public ActionResult Comment(Comment dt)
        {
            try
            {
                dt.AuthorId = User.Identity.GetUserId();

                db.Comment.Add(dt);
                db.SaveChanges();

                if (dt.Anonymous == true)
                {
                    dt.AuthorId = "Anonymous";
                }
                else
                {
                    dt.AuthorId = dt.AuthorId;
                }
                return RedirectToAction("Index", "Forum");
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
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
        public ActionResult Create([Bind(Include = "Id,Title,Content,Anonymous,categoryId,submissionID")] Post post)
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

                MailMessage mails = new MailMessage();

                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                    | SecurityProtocolType.Tls11
                    | SecurityProtocolType.Tls12;
                }

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Credentials = new System.Net.NetworkCredential("baolam101101@gmail.com", "Lam101101!");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;

                mails.From = new MailAddress("baolam101101@gmail.com");

                var userId = User.Identity.GetUserId();
                var users = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (users.Department.Name == "Marketing")
                {
                    mails.To.Add("LamTBGCS190029@fpt.edu.vn");
                }
                if (users.Department.Name == "QA")
                {
                    mails.To.Add("thienvtgcs190535@fpt.edu.vn");

                }

                mails.Subject = "New idea of your department";
                mails.Body = "Your department has new idea";

                smtpClient.Send(mails);

                db.Post.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", post.AuthorId);
            ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", post.categoryId);
            return View(post);
        }

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
