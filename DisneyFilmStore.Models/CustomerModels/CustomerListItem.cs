using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.CustomerModels
{
    public class CustomerListItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

    }
}
