using System;
using System.ComponentModel.DataAnnotations;

namespace Development.Web.Models
{
    public class SearchRequestDto
    {
        public string ArrivalDate { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int MinStarRating { get; set; }
        [Required]
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }
        public int RegionId { get; set; }
    }
}