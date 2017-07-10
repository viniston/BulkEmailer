using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Development.Web.Models
{
    public class ErricaModel
    {
        public int Quarter { get; set; }
        public int Office { get; set; }
        public DateTime AwardDate { get; set; }
        public string Subject { get; set; }
        public HttpPostedFile SourceFile { get; set; }
    }
}