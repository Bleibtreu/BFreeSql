using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql.Entity
{
    class Driver
    {   
        [Column(IsIdentity =true, IsPrimary =true)]
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
    }
}
