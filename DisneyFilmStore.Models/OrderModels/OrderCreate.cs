using DisneyFilmStore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.OrderModels
{
    public class OrderCreate
    {
        [Required]
        public double TotalOrderCost { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public IEnumerable<int> FilmIds { get; set; }

    }
}
