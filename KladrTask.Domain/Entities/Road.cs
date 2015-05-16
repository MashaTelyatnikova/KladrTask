using System.ComponentModel.DataAnnotations;

namespace KladrTask.Domain.Entities
{
    public class Road
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Index { get; set; }
    }
}