using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql.Entity
{
    class Transportation
    {
        [Column(IsIdentity = true,IsPrimary = true)]
        public int id { get; set; }
        public string type { get; set; }
    }
}
