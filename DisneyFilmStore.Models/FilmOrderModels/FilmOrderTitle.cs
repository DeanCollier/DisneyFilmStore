using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.FilmOrderModels
{
    public class FilmOrderTitle
    {
        [Required]
        public string FilmTitle { get; set; }
    }
}
