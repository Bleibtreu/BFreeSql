using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DatabaseFreeSql
{
    abstract class DatabaseManagement : IDatabaseManagement
    {
        protected String connstr { get; set; }

        public IFreeSql freeSql { get; set; }

        protected DatabaseSetting databaseSetting { get; set; }

        protected DatabaseManagement()
        {
            this.connstr = null;
        }

        public virtual IFreeSql Init(DatabaseSetting setting)
        {
            return freeSql;
        }

        /// <summary>
        /// 根据DatabaseSetting实例化对应的IFreeSql对象
        /// </summary>
        public IFreeSql Init()
        {
            FreeSqlBuilder SqlBuilder = new FreeSqlBuilder().UseConnectionString(FreeSql.DataType.SqlServer, this.connstr);

            if (connstr == null && databaseSetting.UseConnectionFactory && databaseSetting.connectionFactory != null)
            {
                SqlBuilder = SqlBuilder.UseConnectionFactory(databaseSetting.DatabaseType, databaseSetting.connectionFactory);
            }

            SqlBuilder = SqlBuilder.UseSlave(databaseSetting.UseSlave);
            SqlBuilder = SqlBuilder.UseAutoSyncStructure(databaseSetting.UseAutoSyncStructure);
            SqlBuilder = SqlBuilder.UseSyncStructureToLower(databaseSetting.UseSyncStructureToLower);
            SqlBuilder = SqlBuilder.UseSyncStructureToUpper(databaseSetting.UseSyncStructureToUpper);
            SqlBuilder = SqlBuilder.UseNoneCommandParameter(databaseSetting.UseNoneCommandParameter);
            SqlBuilder = SqlBuilder.UseGenerateCommandParameterWithLambda(databaseSetting.UseGenerateCommandParameterWithLambda);
            SqlBuilder = SqlBuilder.UseLazyLoading(databaseSetting.UseLazyLoading);
            SqlBuilder = SqlBuilder.UseMonitorCommand(databaseSetting.MonitorCommandExecuting, databaseSetting.MonitorCommandExecuted);
            SqlBuilder = SqlBuilder.UseEntityPropertyNameConvert(databaseSetting.stringConvertType);
       
            freeSql = SqlBuilder.Build();

            freeSql = freeSql.SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = databaseSetting.EnableAddOrUpdateNavigateList);

            return freeSql;
        }
    }
}
