using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.ShippingInformationModels
{
    public class ShippingInfoListItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}
