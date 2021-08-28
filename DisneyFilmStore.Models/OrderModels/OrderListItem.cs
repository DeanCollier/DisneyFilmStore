using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Models.OrderModels
{
    public class OrderListItem
    {
        public int OrderId { get; set; }
        public double TotalOrderCost { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
    }
}
