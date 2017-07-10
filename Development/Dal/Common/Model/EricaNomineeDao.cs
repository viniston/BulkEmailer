using System;

namespace Development.Dal.Common.Model {

    /// <summary>
    /// UserDao object for table 'Error'.
    /// </summary>
    public partial class EricaNomineeDao : BaseDao {
        public virtual int Id { get; set; }
        public virtual int Quarter { get; set; }
        public virtual int Office { get; set; }
        public virtual string Subject { get; set; }
        public virtual string AwardDate { get; set; }
        public virtual string SourceFileName { get; set; }
        public virtual DateTime? DateCreated { get; set; }
    }

}
