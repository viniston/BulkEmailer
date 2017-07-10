using System;
using System.Web;

namespace Development.Core.Common.ApiDtos
{
    public class EricaDto
    {
        public int Quarter { get; set; }
        public int Office { get; set; }
        public string AwardDate { get; set; }
        public string Subject { get; set; }
        public HttpPostedFile SourceFile { get; set; }
        public string SourceFileName { get; set; }
    }
}