using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Development.Core.Common.ApiDtos
{
    public partial class SearchRequest
    {

        private SearchRequestLoginDetails loginDetailsField;

        private SearchRequestSearchDetails searchDetailsField;

        /// <remarks/>
        public SearchRequestLoginDetails LoginDetails
        {
            get
            {
                return this.loginDetailsField;
            }
            set
            {
                this.loginDetailsField = value;
            }
        }

        /// <remarks/>
        public SearchRequestSearchDetails SearchDetails
        {
            get
            {
                return this.searchDetailsField;
            }
            set
            {
                this.searchDetailsField = value;
            }
        }
    }

    public partial class SearchRequestLoginDetails
    {

        private string loginField;

        private string passwordField;

        /// <remarks/>
        public string Login
        {
            get
            {
                return this.loginField;
            }
            set
            {
                this.loginField = value;
            }
        }

        /// <remarks/>
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }
    }

    public partial class SearchRequestSearchDetails
    {

        private System.DateTime arrivalDateField;

        private int durationField;

        private PropertyReferenceIDs propertyReferenceIDsField;

        private SearchRequestSearchDetailsResort[] resortsField;

        private ushort regionIDField;

        private byte mealBasisIDField;

        private int minStarRatingField;

        private SearchRequestSearchDetailsRoomRequests roomRequestsField;

        private string[] textField;

        public System.DateTime ArrivalDate
        {
            get
            {
                return this.arrivalDateField;
            }
            set
            {
                this.arrivalDateField = value;
            }
        }

        /// <remarks/>
        public int Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        public SearchRequestSearchDetailsResort[] Resorts
        {
            get
            {
                return this.resortsField;
            }
            set
            {
                this.resortsField = value;
            }
        }

        /// <remarks/>
        public ushort RegionID
        {
            get
            {
                return this.regionIDField;
            }
            set
            {
                this.regionIDField = value;
            }
        }

        /// <remarks/>
        public byte MealBasisID
        {
            get
            {
                return this.mealBasisIDField;
            }
            set
            {
                this.mealBasisIDField = value;
            }
        }

        /// <remarks/>
        public int MinStarRating
        {
            get
            {
                return this.minStarRatingField;
            }
            set
            {
                this.minStarRatingField = value;
            }
        }

        public PropertyReferenceIDs PropertyReferenceIDs
        {
            get
            {
                return this.propertyReferenceIDsField;
            }
            set
            {
                this.propertyReferenceIDsField = value;
            }
        }

        /// <remarks/>
        public SearchRequestSearchDetailsRoomRequests RoomRequests
        {
            get
            {
                return this.roomRequestsField;
            }
            set
            {
                this.roomRequestsField = value;
            }
        }

        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    public partial class SearchRequestSearchDetailsRoomRequests
    {

        private SearchRequestSearchDetailsRoomRequestsRoomRequest roomRequestField;

        /// <remarks/>
        public SearchRequestSearchDetailsRoomRequestsRoomRequest RoomRequest
        {
            get
            {
                return this.roomRequestField;
            }
            set
            {
                this.roomRequestField = value;
            }
        }
    }

    public partial class SearchRequestSearchDetailsRoomRequestsRoomRequest
    {

        private int adultsField;

        private int childrenField;

        private int infantsField;

        /// <remarks/>
        public int Adults
        {
            get
            {
                return this.adultsField;
            }
            set
            {
                this.adultsField = value;
            }
        }

        /// <remarks/>
        public int Children
        {
            get
            {
                return this.childrenField;
            }
            set
            {
                this.childrenField = value;
            }
        }

        /// <remarks/>
        public int Infants
        {
            get
            {
                return this.infantsField;
            }
            set
            {
                this.infantsField = value;
            }
        }
    }

    public partial class SearchRequestSearchDetailsResort
    {

        private ushort resortIDField;

        /// <remarks/>
        public ushort ResortID
        {
            get
            {
                return this.resortIDField;
            }
            set
            {
                this.resortIDField = value;
            }
        }
    }


    [XmlRoot(ElementName = "PropertyReferenceIDs")]
    public class PropertyReferenceIDs
    {
        [XmlElement(ElementName = "PropertyReferenceID")]
        public int[] PropertyReferenceID { get; set; }
    }





    #region searchresult

    [XmlRoot(ElementName = "ReturnStatus")]
    public class ReturnStatus
    {
        [XmlElement(ElementName = "Success")]
        public string Success { get; set; }
        [XmlElement(ElementName = "Exception")]
        public string Exception { get; set; }
    }

    [XmlRoot(ElementName = "RoomType")]
    public class RoomType
    {
        [XmlElement(ElementName = "Seq")]
        public string Seq { get; set; }
        [XmlElement(ElementName = "PropertyRoomTypeID")]
        public string PropertyRoomTypeID { get; set; }
        [XmlElement(ElementName = "MealBasisID")]
        public string MealBasisID { get; set; }
        [XmlElement(ElementName = "RoomType")]
        public string RoomTypeVal { get; set; }
        [XmlElement(ElementName = "RoomView")]
        public string RoomView { get; set; }
        [XmlElement(ElementName = "MealBasis")]
        public string MealBasis { get; set; }
        [XmlElement(ElementName = "SubTotal")]
        public string SubTotal { get; set; }
        [XmlElement(ElementName = "Discount")]
        public string Discount { get; set; }
        [XmlElement(ElementName = "OnRequest")]
        public string OnRequest { get; set; }
        [XmlElement(ElementName = "Total")]
        public string Total { get; set; }
        [XmlElement(ElementName = "Adults")]
        public string Adults { get; set; }
        [XmlElement(ElementName = "Children")]
        public string Children { get; set; }
        [XmlElement(ElementName = "Infants")]
        public string Infants { get; set; }
        [XmlElement(ElementName = "Adjustments")]
        public string Adjustments { get; set; }
        [XmlElement(ElementName = "Errata")]
        public string Errata { get; set; }
        [XmlElement(ElementName = "OptionalSupplements")]
        public string OptionalSupplements { get; set; }
        [XmlElement(ElementName = "BookingToken")]
        public string BookingToken { get; set; }
    }

    [XmlRoot(ElementName = "RoomTypes")]
    public class RoomTypes
    {
        [XmlElement(ElementName = "RoomType")]
        public List<RoomType> RoomType { get; set; }
    }

    [XmlRoot(ElementName = "PropertyResult")]
    public class PropertyResult
    {
        [XmlElement(ElementName = "TotalProperties")]
        public string TotalProperties { get; set; }
        [XmlElement(ElementName = "PropertyID")]
        public string PropertyID { get; set; }
        [XmlElement(ElementName = "PropertyReferenceID")]
        public string PropertyReferenceID { get; set; }
        [XmlElement(ElementName = "PropertyName")]
        public string PropertyName { get; set; }
        [XmlElement(ElementName = "Rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "OurRating")]
        public string OurRating { get; set; }
        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }
        [XmlElement(ElementName = "Region")]
        public string Region { get; set; }
        [XmlElement(ElementName = "Resort")]
        public string Resort { get; set; }
        [XmlElement(ElementName = "SearchURL")]
        public string SearchURL { get; set; }
        [XmlElement(ElementName = "RoomTypes")]
        public RoomTypes RoomTypes { get; set; }
    }

    [XmlRoot(ElementName = "PropertyResults")]
    public class PropertyResults
    {
        [XmlElement(ElementName = "PropertyResult")]
        public List<PropertyResult> PropertyResult { get; set; }
    }

    [XmlRoot(ElementName = "SearchResponse")]
    public class SearchResponse
    {
        [XmlElement(ElementName = "ReturnStatus")]
        public ReturnStatus ReturnStatus { get; set; }
        [XmlElement(ElementName = "SearchURL")]
        public string SearchURL { get; set; }
        [XmlElement(ElementName = "PropertyResults")]
        public PropertyResults PropertyResults { get; set; }
    }
    #endregion
}
