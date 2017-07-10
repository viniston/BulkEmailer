using System;
using System.Collections.Generic;
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
                    "<p style=\"margin-left: 0cm; margin-right: 0cm\"><span style=\"font-size: 12pt\"><span style=\"font-family: Calibri, sans-serif\"><span style=\"color: black\">From <a href=\"mailto:" +
                    nominee.NominatorEmail + "\" style=\"color: #0563c1; text-decoration: underline\">" +
                    nominee.NominatorEmail + "</a></span></span></span></p>";
                var messageContent =
                    " <p style=\"margin-left: 0cm; margin-right: 0cm\">\r\n<span style=\"font-size: 12pt\">\r\n<span style=\"font-family: Calibri, sans-serif\">\r\n<span style=\"font-size: 10.5pt\">\r\n<span class=\"nominatorMessageContent\" style=\"color: black\">\r\n\r\n" +
                    nominee.Message + "</span>\r\n</span>\r\n</span>\r\n</span>\r\n</p>";
                messageContent +=
                    "<p style=\"margin-left: 0cm; margin-right: 0cm\"><span style=\"font-size: 12pt\"><span style=\"font-family: Calibri, sans-serif\">&nbsp;</span></span></p>";
                messageHeader += messageContent;
                messageBlock += messageHeader;
            }
            return messageBlock;
        }
    }
}
