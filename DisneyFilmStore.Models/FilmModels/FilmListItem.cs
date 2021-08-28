using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.FilmModels
{
    public class FilmListItem
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
    }
}
