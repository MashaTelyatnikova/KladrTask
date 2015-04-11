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

        [Required]
        public string Index { get; set; }

    }
}
