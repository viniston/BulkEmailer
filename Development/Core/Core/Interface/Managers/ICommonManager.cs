using Development.Core.Common.ApiDtos;

namespace Development.Core.Core.Interface.Managers {
    public interface ICommonManager {

        #region Instance of Classes In ServiceLayer reference
        /// <summary>
        /// Returns File class.
        /// </summary>

        #endregion

        #region Methods

        SearchResponse AvailabilitySearch(SearchRequest request);

        bool ProcessEricaExcel(SearchRequest request, bool firstRowHasFieldNames);

        #endregion

    }
}
