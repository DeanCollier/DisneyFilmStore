using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.CustomerModels
{
    public class CustomerCreate
    {
        [Required]
        [Display(Name = "First Name")]
        [MinLength(1, ErrorMessage = "First name cannot be empty.")]
        [MaxLength(200, ErrorMessage = "First name is too long.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MinLength(1, ErrorMessage = "Last name cannot be empty.")]
        [MaxLength(200, ErrorMessage = "Last name is too long.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(1, ErrorMessage = "Email cannot be empty.")]
        [MaxLength(200, ErrorMessage = "Email is too long.")]
        public string Email { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Address cannot be empty.")]
        [MaxLength(200, ErrorMessage = "Address is too long.")]
        public string Address { get; set; }

        public bool Member { get; set; } = false;
    }
}
