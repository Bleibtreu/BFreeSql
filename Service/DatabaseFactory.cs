using FreeSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql
{
    class DatabaseFactory: IDisposable
    {

        private static IDatabaseManagement database;

        public static IDatabaseManagement NewInstance(DatabaseSetting setting)
        {
            if (database != null)
                return database;
            switch (setting.DatabaseType)
            {
                case DataType.SqlServer:
                    database = new MSSqlManagement();
                    database.Init(setting);
                    break;
                case DataType.MySql:
                    database = new MySqlManagement();
                    database.Init(setting);
                    break;
                default: throw new Exception("设定数据库类型错误");
            }
            return database;
        }

        public static IDatabaseManagement NewInstance(DataType type, DatabaseSetting setting)
        {
            setting.DatabaseType = type;
            return NewInstance(setting);
        }

        public void Dispose()
        {
            database = null;
        }
    }
}
