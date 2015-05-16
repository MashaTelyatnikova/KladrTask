using System.ComponentModel.DataAnnotations;

namespace KladrTask.Domain.Entities
{
    public class Region
    {
        [Key]
        public string Code { get; set; }
        public string Index { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
