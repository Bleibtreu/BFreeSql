using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql
{
    class MSSqlManagement : DatabaseManagement
    {
        public override IFreeSql Init(DatabaseSetting setting)
        {
            base.databaseSetting = setting;
            base.connstr = @"Data Source=.;Integrated Security=True;Initial Catalog=" + base.databaseSetting.Database
                        + ";Pooling=" + base.databaseSetting.Pooling + ";Min Pool Size=" + base.databaseSetting.MinPoolSize + "";
            return base.Init();
        }
    }
}
