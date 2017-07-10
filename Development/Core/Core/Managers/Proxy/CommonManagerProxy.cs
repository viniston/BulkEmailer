using Development.Core.Interface.Managers;
using Development.Core.Interface;
using Development.Core.Common.ApiDtos;
using Development.Core.Core.Interface.Managers;
using Development.Core.Core.Managers;

namespace Development.Core.Managers.Proxy
{
    internal partial class CommonManagerProxy : ICommonManager, IManagerProxy
    {
        // Reference to the DevelopmentManager
        private DevelopmentManager _DevelopmentManager = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonManagerProxy"/> class.
        /// </summary>
        /// <param name="DevelopmentManager">The Development manager.</param>
        internal CommonManagerProxy(DevelopmentManager DevelopmentManager)
        {
            _DevelopmentManager = DevelopmentManager;

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
        internal DevelopmentManager DevelopmentManager
        {
            get { return _DevelopmentManager; }
        }

        #region Availabilty Search

        public SearchResponse AvailabilitySearch(SearchRequest request)
        {
            return CommonManager.Instance.AvailabilitySearch(this, request);
        }
        #endregion

        public bool ProcessEricaExcel(SearchRequest request, bool firstRowHasFieldNames)
        {
            return CommonManager.Instance.ProcessEricaExcel(this, request, firstRowHasFieldNames);
        }




    }

}
