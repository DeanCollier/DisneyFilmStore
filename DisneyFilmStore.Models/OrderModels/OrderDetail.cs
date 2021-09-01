using DisneyFilmStore.Models.FilmOrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.OrderModels
{
    public class OrderDetail
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public double TotalOrderCost { get; set; } // should be calc based on movie list

        [Required]
        public IEnumerable<FilmOrderDetail> FilmTitles { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
