using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Development.Core.Common.ApiDtos;

namespace Development.Core.Utility {
    public static class EmailMessagehelper {
        public static string GetEmailBodyMessage(List<EricaNominee> nomineeList) {
            string messageBlock = "";
            foreach (var nominee in nomineeList) {
                var messageHeader =
                    "<p style=\"margin-left: 0cm; margin-right: 0cm\"><span style=\"font-size: 10.5pt\"><span style=\"font-family: 'Source Sans Pro';\"><span style=\"color: black\">From <strong><a style=\"text-decoration: none;\" href=\"mailto:" +
                    nominee.NominatorEmail + "\" style=\"color: #0563c1;\">" + nominee.NominatorName + "</a></strong> - " + nominee.AwardName + "</span></span></span></p>";
                var messageContent =
                    " <p style=\"margin-left: 0cm; margin-right: 0cm\">\r\n<span style=\"font-size: 10.5pt\">\r\n<span style=\"font-family: 'Source Sans Pro';\">\r\n<span style=\"font-size: 10.5pt\">\r\n<span class=\"nominatorMessageContent\" style=\"color: black\">\r\n\r\n" +
                    nominee.Message + "</span>\r\n</span>\r\n</span>\r\n</span>\r\n</p>";
                //messageContent +=
                //    "<p style=\"margin-left: 0cm; margin-right: 0cm\"><span style=\"font-size: 10.5pt\"><span style=\"font-family: Calibri, Source Sans Pro\">&nbsp;</span></span></p>";
                messageHeader += messageContent;
                messageBlock += messageHeader;
            }
            return messageBlock;
        }

        public static string GetHtmlMailBody(string mailBodyMessage) {
            var mailTemplatePath = ConfigurationManager.AppSettings["MailTemplate"];
            StreamReader sr = new StreamReader(mailTemplatePath);
            string body = sr.ReadToEnd();
            sr.Close();
            body = body.Replace("#MailBody#", mailBodyMessage);
            return body;
        }
    }
}
