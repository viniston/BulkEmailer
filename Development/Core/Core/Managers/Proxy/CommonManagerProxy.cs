using System.Collections.Generic;
using Development.Core.Common.ApiDtos;
using Development.Core.Core.Interface.Managers;
using Development.Core.Interface;
using Development.Dal.Common.Model;

namespace Development.Core.Core.Managers.Proxy {
    internal partial class CommonManagerProxy : ICommonManager, IManagerProxy {
        // Reference to the DevelopmentManager
        private readonly DevelopmentManager _developmentManager = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonManagerProxy"/> class.
        /// </summary>
        /// <param name="developmentManager">The Development manager.</param>
        internal CommonManagerProxy(DevelopmentManager developmentManager) {
            _developmentManager = developmentManager;

            // Do some initialization.... 
            // i.e. cache logged in user specific things (or maybe use lazy loading for that)
        }

        // Reference to the DevelopmentManager (only internal)
        /// <summary>
        /// Gets the Development manager.
        /// </summary>
        /// <value>
        /// The Development manager.
        /// </value>
        internal DevelopmentManager DevelopmentManager {
            get { return _developmentManager; }
        }

        #region Availabilty Search

        public List<EricaNomineeDao> SaveEricaConfiguration(EricaDto request) {
            return CommonManager.Instance.SaveEricaConfiguration(this, request);
        }
        #endregion

        public bool ProcessEricaExcel(EricaNomineeDao request) {
            return CommonManager.Instance.ProcessEricaExcel(this, request);
        }

        public List<EricaNomineeDao> GetEricaTemplates() {
            return CommonManager.Instance.GetEricaTemplates(this);
        }

        public List<EricaNomineeListDao> GetEricaNomineeList(int ericaId) {
            return CommonManager.Instance.GetEricaNomineeList(this, ericaId);
        }

        public EricaNomineeListDao GetEricaNominatorMessage(int nominationId) {
            return CommonManager.Instance.GetEricaNominatorMessage(this, nominationId);
        }

        public bool SendEmail(EmailDto dto) {
            return CommonManager.Instance.SendEmail(this, dto);
        }


    }

}
