using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql
{
    interface IDatabaseManagement
    {
        public IFreeSql freeSql { get; set; }
        public IFreeSql Init(DatabaseSetting setting);
    }
}
