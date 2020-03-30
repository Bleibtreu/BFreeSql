using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql
{
    class MySqlManagement : DatabaseManagement
    {
        public override IFreeSql Init(DatabaseSetting setting)
        {
            base.databaseSetting = setting;
            base.connstr = @"Data Source=" + base.databaseSetting.Host + ";Port=" 
                + base.databaseSetting.Port + ";User ID=" + base.databaseSetting.Id + ";Password=" 
                + base.databaseSetting.Password + "; Initial Catalog=" + base.databaseSetting.Database + ";Charset=" 
                + base.databaseSetting.Charset + "; SslMode=none;Min pool size=" + base.databaseSetting.MinPoolSize + "";
            return base.Init();
        }
    }
}
