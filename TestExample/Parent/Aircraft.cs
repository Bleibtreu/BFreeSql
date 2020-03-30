using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql.Entity
{
    class Aircraft :Transportation
    {
        /*
        [Column(IsIdentity = true, IsPrimary = true)]
        public int id { get; set; }
        */
        public string brand { get; set; }
    }
}
