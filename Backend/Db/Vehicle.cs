using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Db
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }

        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
    }
}