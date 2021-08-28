using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Data
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }
        [Required]
        public Guid OwnerId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public double Rating { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public DateTime YearReleased { get; set; }
        [Required]
        public double MemberCost { get; set; }
        [Required]
        public double NonMemberCost { get; set; }
    }
}
