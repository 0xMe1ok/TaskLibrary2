using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLibrary2.Entities
{
    class BookOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        public int LibraryId { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime DateOfOrder { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime DateOfReturn { get; set; }

        public Client Client { get; set; }

        public Book Book { get; set; }
    }
}
