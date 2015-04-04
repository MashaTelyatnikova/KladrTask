using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KladrTask.Domain.Entities
{
    public class House
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Index { get; set; }
    }
}
