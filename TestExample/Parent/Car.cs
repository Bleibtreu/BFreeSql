using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql.Entity
{
    class Car : Transportation
    {
        /*
        [Column(IsIdentity =true,IsPrimary =true)]
        public int id { get; set; }
        */
        public string brand { get; set; }

        [Navigate(ManyToMany = typeof(Driver))]
        public List<Driver> drivers { get; set; }
    }
}
