using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Comment
    {
        // foreign key
        public int? StockId { get; set; }

        // navigation access to Stock entity
        public Stock? Stock { get; set; }
         
    }
}