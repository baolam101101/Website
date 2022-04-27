using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using System.IO;
using Website.Models;

namespace Website.Controllers
{
    public class ZIPController : Controller
    {
        // GET: ZIP
        public ActionResult Index()
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/Files/"));
            List<ZIPModel> files = new List<ZIPModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new ZIPModel()
                {
                    ZIPName = Path.GetFileName(filePath),
                    ZIPPath = filePath
                });
            }



            return View(files);
        }



        [HttpPost]
        public ActionResult Index(List<ZIPModel> files)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");
                foreach (ZIPModel file in files)
                {
                    if (file.Selected)
                    {
                        zip.AddFile(file.ZIPPath, "Files");
                    }
                }
                string zipName = String.Format("FilesZip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.Save(memoryStream);
                    return File(memoryStream.ToArray(), "application/zip", zipName);
                }
            }
        }

    }
}