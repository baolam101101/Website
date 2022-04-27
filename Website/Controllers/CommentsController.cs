using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Create(int id)
        {
            var test = db.Post.Find(id);
            var test_date = db.Submissions.Find(test.submissionID);
            if (DateTime.Now > test_date.Final_Date)
            {
                return RedirectToAction("FailComment", "Forum");
            }
            else
            {
                return RedirectToAction("Create", "Comments", new Website.Models.Comment { IdeaId = id, submissionId = test.submissionID});
            }
        }

        [HttpPost]
        public ActionResult Create(Comment dt)
        {
            try
            {
                dt.AuthorId = User.Identity.GetUserId();

                if (dt.Anonymous == true)
                {
                    dt.AuthorId = "Anonymous";
                }
                else
                {
                    dt.AuthorId = dt.AuthorId;
                }

                db.Comment.Add(dt);
                db.SaveChanges();

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
    }
}