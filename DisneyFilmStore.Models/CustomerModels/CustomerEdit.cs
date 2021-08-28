using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.CustomerModels
{
    public class CustomerEdit
    {
        [Required]
        [Display(Name = "Customer Id Number")]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [MinLength(1, ErrorMessage = "Name cannot be empty.")]
        [MaxLength(200, ErrorMessage = "Name is too long.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MinLength(1, ErrorMessage = "Name cannot be empty.")]
        [MaxLength(200, ErrorMessage = "Name is too long.")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [MinLength(1, ErrorMessage = "Email cannot be empty.")]
        [MaxLength(200, ErrorMessage = "Email is too long.")]
        public string Email { get; set; }

        [MinLength(1, ErrorMessage = "Address cannot be empty.")]
        [MaxLength(200, ErrorMessage = "Address is too long.")]
        public string Address { get; set; }

        public bool Member { get; set; } = false;
    }
}
