using System.ComponentModel.DataAnnotations;

namespace KladrTask.Domain.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Locality { get; set; }

        [Required]
        public string Road { get; set; }

        [Required]
        public string House { get; set; }

        public string Housing { get; set; }

        public string Apartment { get; set; }

        [Required]
        public string Index { get; set; }

        [Required]
        public string RegionCode { get; set; }

        [Required]
        public string LocalityCode { get; set; }

        [Required]
        public string RoadCode { get; set; }

        public override bool Equals(object obj)
        {
            var other = (Address)obj;
            return Region == other.Region && Locality == other.Locality && Road == other.Road && House == other.House &&
                  Index == other.Index && RegionCode == other.RegionCode && LocalityCode == other.LocalityCode &&
                  RoadCode == other.RoadCode && House == other.House && Apartment == other.Apartment;
        }
    }
}
