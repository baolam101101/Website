using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class ExcelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public FileContentResult ExportExcel()
        {
            string csv = "\"Id\",\"Title\",\"Time\",\"Content\",\"DocumentName\" \n";
            var List = db.Post.ToList(); //get this list from database 
            foreach (Post item in List)
            {
                csv = csv + String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\" \n",
                                           item.Id,
                                           item.Title,
                                           item.Time,
                                           item.Content,
                                           item.DocumentName);
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "excel.csv");
        }
    }
}