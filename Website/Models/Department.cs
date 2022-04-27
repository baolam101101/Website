using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Display(Name = "Departments Name")]
        public string Name { get; set; }
    }
}