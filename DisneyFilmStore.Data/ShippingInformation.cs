using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Data
{
    public class ShippingInformation
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }

        [Required, ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required, ForeignKey(nameof(Film))]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
