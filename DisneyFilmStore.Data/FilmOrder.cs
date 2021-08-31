using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Data
{
    public class FilmOrder
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required, ForeignKey(nameof(Film))]
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }

    }
}
