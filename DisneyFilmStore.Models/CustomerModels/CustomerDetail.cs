using DisneyFilmStore.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.CustomerModels
{
    public class CustomerDetail
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Address { get; set; }

        [Required]
        public bool Member { get; set; } = false;

        [Required]
        public IEnumerable<OrderListItem> Orders { get; set; }

        // favorite movies?

        // most recent movies?
    }
}
