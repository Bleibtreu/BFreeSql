using FreeSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BFreeSql
{
    class DatabaseFactory: IDisposable
    {

        private static DatabaseManagement database { get; set; }

        /// <summary>
        /// 创建数据库工厂对象
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static DatabaseManagement NewInstance(DatabaseSetting setting)
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

        public static DatabaseManagement NewInstance(DataType type, DatabaseSetting setting)
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
