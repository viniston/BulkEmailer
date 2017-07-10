using System;

namespace Development.Dal.Common.Model {

    /// <summary>
    /// UserDao object for table 'Error'.
    /// </summary>
    public class EricaNomineeListDao : BaseDao {
        public virtual int Id { get; set; }
        public virtual string NomineeName { get; set; }
        public virtual string NomineeEmail { get; set; }
        public virtual string NominatorEmailList { get; set; }
        public virtual string AwardName { get; set; }
        public virtual string Message { get; set; }
        public virtual int EricaId { get; set; }
        public virtual string Status { get; set; }
        public virtual string MailBody { get; set; }
    }

}
