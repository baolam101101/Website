using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class StatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Statics
        public ActionResult Index()
        {
            var total_post = db.Post.Count();
            decimal Total_Idea_of_QA = 0;
            decimal Total_Idea_of_IT = 0;
            decimal Total_Idea_of_Business = 0;
            decimal Total_Idea_of_HR = 0;
            var post = db.Post.ToList();

            foreach (var posts in post)
            {
                var user = db.Users.Where(x => x.Id == posts.AuthorId).FirstOrDefault();
                if (user.Department.Name == "QA")
                {
                    Total_Idea_of_QA++;
                }
                else if (user.Department.Name == "IT")
                {
                    Total_Idea_of_IT++;
                }
                else if (user.Department.Name == "Bussiness")
                {
                    Total_Idea_of_Business++;
                }
                else if (user.Department.Name == "Human Resources")
                {
                    Total_Idea_of_HR++;
                }

            }
            var statitics = new Statistic();
            {
                statitics.TotalIdeaofIT = Total_Idea_of_IT;
                statitics.TotalIdeaofQA = Total_Idea_of_QA;
                statitics.TotalIdeaofHR = Total_Idea_of_HR;
                statitics.TotalIdeaofBusiness = Total_Idea_of_Business;
            }

            return View(statitics);
        }
    }
}