using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        // Foreign Key
        public int StockId { get; set; }
        public string AppuserId { get; set; }


        // Navigation property
        public Stock Stock { get; set; }
        public AppUser AppUser { get; set; }

    }
}