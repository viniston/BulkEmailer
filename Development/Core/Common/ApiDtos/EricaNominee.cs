using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Development.Core.Common.ApiDtos {
    public class EricaNominee {
        public string NomineeEmail { get; set; }
        public string NominatorEmail { get; set; }
        public string AwardName { get; set; }
        public string NomineeName { get; set; }
        public string Message { get; set; }
    }
}
