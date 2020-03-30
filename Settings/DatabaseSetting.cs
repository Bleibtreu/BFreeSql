using FreeSql;
using FreeSql.Internal;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace DatabaseFreeSql
{
    /// <summary>
    /// 数据库配置，可根据对应的数据库进行对应配置
    /// </summary>
    class DatabaseSetting
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DatabaseType { get; set; }
        /// <summary>
        /// HOST地址
        /// </summary>
        public String Host { get; set; }
        /// <summary>
        /// 数据库使用端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 数据库用户ID
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// 数据库用户密码
        /// </summary>
        public String Password { get; set; }
        /// <summary>
        /// 数据库名
        /// </summary>
        public String Database { get; set; }
        /// <summary>
        /// 数据库编码
        /// </summary>
        public String Charset { get; set; }
        /// <summary>
        /// 是否启用线程池
        /// </summary>
        public bool Pooling { get; set; }
        /// <summary>
        /// 最小线程数
        /// </summary>
        public int MinPoolSize { get; set; }

        /// <summary>
        /// 配置设置从数据库，支持多个
        /// </summary>
        public String[] UseSlave { get; set; }

        /// <summary>
        /// 设置自定义数据库连接对象（放弃内置对象连接池技术）
        /// </summary>
        public bool UseConnectionFactory { get; set; }
        public Func<DbConnection> connectionFactory { get; set; }

        /// <summary>
        /// 是否(开发环境必备)自动同步实体结构到数据库，程序运行中检查实体创建或修改表结构
        /// </summary>
        public bool UseAutoSyncStructure { get; set; }
        /// <summary>
        /// 是否转小写同步结构，适用 PostgreSQL
        /// </summary>
        public bool UseSyncStructureToLower { get; set; }
        /// <summary>
        /// 是否转大写同步结构，适用 Oracle/达梦
        /// </summary>
        public bool UseSyncStructureToUpper { get; set; }
        /// <summary>
        /// 是否不使用命令参数化执行，针对 Insert/Update，也可临时使用 IInsert/IUpdate.NoneParameter()
        /// </summary>
        public bool UseNoneCommandParameter { get; set; }
        /// <summary>
        /// 是否生成命令参数化执行，针对 lambda 表达式解析
        /// </summary>
        public bool UseGenerateCommandParameterWithLambda { get; set; }
        /// <summary>
        /// 是否开启延时加载功能
        /// </summary>
        public bool UseLazyLoading { get; set; }

        /// <summary>
        /// 配置监视全局 SQL 执行前后操作（SQL执行前）
        /// </summary>
        public Action<DbCommand> MonitorCommandExecuting { get; set; }
        /// <summary>
        /// 配置监视全局 SQL 执行前后操作（SQL执行后）
        /// </summary>
        public Action<DbCommand, string> MonitorCommandExecuted { get; set; }

        /// <summary>
        /// 是否自动转换实体属性名称 Entity Property -> Db Filed
        /// </summary>
        public bool UseEntityPropertyNameConvert { get; set; }
        /// <summary>
        /// 选择字符处理类型，默认None
        /// </summary>
        public StringConvertType stringConvertType { get; set; }

        /// <summary>
        /// 是否启用全局导航配置
        /// </summary>
        public bool EnableAddOrUpdateNavigateList { get; set; }

        /// <summary>
        /// 配置参数初始化
        /// </summary>
        public DatabaseSetting()
        {
            Host = "127.0.0.1";
            Port = 3306;
            Charset = "UTF-8";
            Pooling = true;
            MinPoolSize = 10;
            UseSlave = new String[0];
            UseConnectionFactory = false;
            UseAutoSyncStructure = false;
            UseSyncStructureToLower = false;
            UseSyncStructureToUpper = false;
            UseNoneCommandParameter = false;
            UseGenerateCommandParameterWithLambda = false;
            UseLazyLoading = false;
            MonitorCommandExecuting = null;
            MonitorCommandExecuted = null;
            UseEntityPropertyNameConvert = false;
            stringConvertType = StringConvertType.None;
            EnableAddOrUpdateNavigateList = true;
        }
    }
}
