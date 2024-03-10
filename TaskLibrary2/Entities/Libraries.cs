using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLibrary2.Entities
{
    class Libraries
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LibraryId { get; set; }

        public Library Library { get; set; }
    }
}
