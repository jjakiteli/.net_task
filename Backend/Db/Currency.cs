using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Db
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}