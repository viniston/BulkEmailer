using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Development.Web.Helper {
    public class UIhelper {

        /// <summary>
        /// Return a unique identifier based on system's full date (yyyymmdd) and time (hhmissms).
        /// 
        /// Output sample: 2006040212445099
        /// </summary>
        public static string GenerateUniqueId() {
            DateTime date = DateTime.Now;
            string uniqueId =
                $"{date.Year:0000}{date.Month:00}{date.Day:00}{date.Hour:00}{date.Minute:00}{date.Second:00}{date.Millisecond:000}";
            return uniqueId;
        }
    }
}