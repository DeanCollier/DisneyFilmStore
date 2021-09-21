using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.ShippingInformationModels
{
    public class ShippingInfoCreate
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
