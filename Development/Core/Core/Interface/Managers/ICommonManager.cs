using Development.Core.Common.ApiDtos;
using Development.Dal.Common.Model;
using System.Collections.Generic;

namespace Development.Core.Core.Interface.Managers {
    public interface ICommonManager {

        #region Instance of Classes In ServiceLayer reference
        /// <summary>
        /// Returns File class.
        /// </summary>

        #endregion

        #region Methods

        List<EricaNomineeDao> SaveEricaConfiguration(EricaDto request);

        bool ProcessEricaExcel(EricaNomineeDao reques);

        List<EricaNomineeDao> GetEricaTemplates();

        #endregion

        List<EricaNomineeListDao> GetEricaNomineeList(int ericaId);

        EricaNomineeListDao GetEricaNominatorMessage(int nominationId);

        bool SendEmail(EmailDto dto);

    }
}
