using FreeSql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;

namespace BFreeSql
{
    class DatabaseManagement
    {
        protected String connstr { get; set; }

        public IFreeSql freeSql { get; set; }

        protected DatabaseSetting databaseSetting { get; set; }

        protected DbTransaction transaction { get; set; }

        public DatabaseManagement() => this.connstr = null;

        #region 初始化

        /// <summary>
        /// 根据DatabaseSetting实例化对应的IFreeSql对象
        /// </summary>
        public IFreeSql Init()
        {
            FreeSqlBuilder SqlBuilder = new FreeSqlBuilder().UseConnectionString(FreeSql.DataType.SqlServer, this.connstr);

            if (connstr == null && databaseSetting.UseConnectionFactory && databaseSetting.connectionFactory != null)
            {
                SqlBuilder.UseConnectionFactory(databaseSetting.DatabaseType, databaseSetting.connectionFactory);
            }

            SqlBuilder.UseSlave(databaseSetting.UseSlave);
            SqlBuilder.UseAutoSyncStructure(databaseSetting.UseAutoSyncStructure);
            SqlBuilder.UseSyncStructureToLower(databaseSetting.UseSyncStructureToLower);
            SqlBuilder.UseSyncStructureToUpper(databaseSetting.UseSyncStructureToUpper);
            SqlBuilder.UseNoneCommandParameter(databaseSetting.UseNoneCommandParameter);
            SqlBuilder.UseGenerateCommandParameterWithLambda(databaseSetting.UseGenerateCommandParameterWithLambda);
            SqlBuilder.UseLazyLoading(databaseSetting.UseLazyLoading);
            SqlBuilder.UseMonitorCommand(databaseSetting.MonitorCommandExecuting, databaseSetting.MonitorCommandExecuted);
            SqlBuilder.UseEntityPropertyNameConvert(databaseSetting.stringConvertType);

            freeSql = SqlBuilder.Build();

            freeSql.SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = databaseSetting.EnableAddOrUpdateNavigateList);

            transaction = freeSql.Ado.TransactionCurrentThread;

            return freeSql;
        }

        public virtual IFreeSql Init(DatabaseSetting setting)
        {
            return freeSql;
        }

        #endregion

        #region 添加及更新

        /// <summary>
        /// 基础添加操作，根据传入实体类插入数据
        /// </summary>
        /// <typeparam name="Object"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Insert<T>(T entity) where T : class
        {
            return freeSql.Insert(entity).ExecuteAffrows();
        }

        /// <summary>
        /// 内部事务实现示例
        /// </summary>
        /// <typeparam name="Object"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int InsertWithTransaction<T>(T entity) where T : class
        {
            return freeSql.Insert(entity).WithTransaction(transaction).ExecuteAffrows();
        }

        /// <summary>
        /// 基础添加操作，根据List<T>插入数据
        /// </summary>
        /// <typeparam name="Object"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>       
        public virtual int Insert<T>(List<T> entity) where T : class
        {            
            return freeSql.Insert(entity).ExecuteAffrows();
        }

        /// <summary>
        /// 批次插入指定列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="exp">指定列</param>
        /// <returns></returns>
        public virtual int Insert<T>(List<T> entity, Expression<Func<T, object>> exp) where T : class
        {
             return freeSql.Insert(entity).InsertColumns(exp).ExecuteAffrows();
        }

        /// <summary>
        /// 忽略特定列，进行添加操作
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ignoreFunc">链式Lambda语句</param>
        public virtual void Insert<T>(List<T> entity, Expression<Func<List<T>, object>> ignoreFunc)
        {
            freeSql.Insert(entity).IgnoreColumns(ignoreFunc).OnDuplicateKeyUpdate().ToSql();
        }

        /// <summary>
        /// 根据主键更新实体对象，返回影响的行数支持
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="primarykey"></param>
        /// <returns></returns>
        public virtual int Update<T>(T entity, object primarykey = null) where T : class
        {
            if (primarykey == null)
            {
                return freeSql.Update<T>().SetSource(entity).ExecuteAffrows();
            }
            else
            {
                return freeSql.Update<T>(primarykey).SetSource(entity).ExecuteAffrows();
            }
        }

        /// <summary>
        /// 更新指定实体对象的指定参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="exp">链式Lambda语句</param>
        /// <returns></returns>
        public virtual int Update<T>(T entity, Expression<Func<T, object>> exp) where T : class
        {
            return freeSql.Update<T>().SetSource(entity).UpdateColumns(exp).ExecuteAffrows();
        }

        /// <summary>
        /// 更新指定实体对象,忽略特定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual int UpdateIgnore<T>(T entity, Expression<Func<T, object>> exp) where T : class
        {
            return freeSql.Update<T>().SetSource(entity).IgnoreColumns(exp).ExecuteAffrows();
        }

        #endregion

        #region 查询

        #region 单表查询

        /// <summary>
        /// 执行SQL查询，返回 T 实体所有字段的记录，若存在导航属性则一起查询返回，记录不存在时返回 Count 为 0 的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Select<T>() where T : class
        {
            return freeSql.Select<T>().ToList();
        }

        /// <summary>
        /// 分页功能示例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<T> Select<T>(int page,int num) where T : class
        {
            // total记录总数量
            // 返回查询的List对象
            return freeSql.Select<T>().Count(out var total).Page(page, num).ToList();
        }

        /// <summary>
        /// 跨表查询（分库分表功能开启）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual List<T> CrossTableQuery<T>(T entity) where T : class
        {
            return freeSql.Select<T>()
                .AsTable((type, oldname) => "table_1")
                .AsTable((type, oldname) => "table_2")
                .AsTable((type, oldname) => "table_3")
                .ToList();
        }

        /// <summary>
        /// 使用Lambda语句进行条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">链式Lambda语句</param>
        /// <returns></returns>
        public List<T> Select<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return freeSql.Select<T>().Where(exp).ToList();
        }

        /// <summary>
        /// 使用Sql语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Select<T>(string sql) where T : class
        {
            return freeSql.Select<T>().WithSql(sql).ToList();
        }

        #endregion

        #region 复杂多联表查询

        /// <summary>
        /// 复杂联表查询
        /// 若想使用导航属性查询，需配置实体类导航属性,确保导航配置为TRUE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereCascade">多表全局条件</param>
        /// <param name="expression">链式Lambda语句</param>
        /// <returns></returns>
        public List<T> Select<T>(Expression<Func<T, bool>> whereCascade = null, params Expression<Func<T, bool>>[] expression) where T : class
        {
            if (expression.Length == 0)
            {
                return null;
            }
            ISelect<T> select = freeSql.Select<T>();
            foreach (Expression<Func<T, bool>> func in expression)
            {
                select = select.LeftJoin(func);
            }

            if (whereCascade != null)
            {
                select = select.WhereCascade(whereCascade);
            }

            return select.ToList();
        }

        public List<T1> Select<T1, T2>(params Expression<Func<T1, bool>>[] expression) where T1 : class where T2 : class
        {
            if (expression.Length == 0)
            {
                return null;
            }
            ISelect<T1, T2> select = freeSql.Select<T1, T2>();
            foreach (Expression<Func<T1, bool>> func in expression)
            {
                select = select.LeftJoin(func);
            }
            return select.ToList();
        }

        public List<T1> Select<T1, T2, T3>(params Expression<Func<T1, bool>>[] expression) where T1 : class where T2 : class where T3 : class
        {
            if (expression.Length == 0)
            {
                return null;
            }
            ISelect<T1, T2, T3> select = freeSql.Select<T1, T2, T3>();
            foreach (Expression<Func<T1, bool>> func in expression)
            {
                select = select.LeftJoin(func);
            }
            return select.ToList();
        }

        public List<T1> Select<T1, T2, T3, T4>(params Expression<Func<T1, bool>>[] expression) where T1 : class where T2 : class where T3 : class where T4 : class
        {
            if (expression.Length == 0)
            {
                return null;
            }
            ISelect<T1, T2, T3, T4> select = freeSql.Select<T1, T2, T3, T4>();
            foreach (Expression<Func<T1, bool>> func in expression)
            {
                select = select.LeftJoin(func);
            }
            return select.ToList();
        }

        public List<T1> Select<T1, T2, T3, T4, T5>(params Expression<Func<T1, bool>>[] expression) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
        {
            if (expression.Length == 0)
            {
                return null;
            }
            ISelect<T1, T2, T3, T4, T5> select = freeSql.Select<T1, T2, T3, T4, T5>();
            foreach (Expression<Func<T1, bool>> func in expression)
            {
                select = select.LeftJoin(func);
            }
            return select.ToList();
        }

        #endregion

        #region 子表查询

        /// <summary>
        /// 子表Exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias">别名</param>
        /// <param name="exp">方法</param>
        /// <returns></returns>
        public List<T> Select<T>(string alias, Expression<Func<T, bool>> exp) where T : class
        {
            return freeSql.Select<T>().Where(a => freeSql.Select<T>().As(alias).Where(exp).Any()).ToList();
        }

        /// <summary>
        /// 子表In
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias">别名</param>
        /// <param name="exp">方法</param>
        /// <param name="contains">条件</param>
        /// <returns></returns>
        public List<T> Select<T>(string alias, Expression<Func<T, bool>> exp, bool contains = true) where T : class
        {
            return freeSql.Select<T>().Where(a => freeSql.Select<T>().As(alias).ToList(exp).Contains(contains)).ToList();
        }

        #endregion

        /// <summary>
        /// 高级更新，此方法可将查询转为更新对象，以便支持导航对象或其他查询功能更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual int ISelectToUpdate<T>(T entity, Expression<Func<T, bool>> condition, Expression<Func<T, object>> operating) where T : class
        {
            return freeSql.Select<T>().Where(condition).ToUpdate().Set(operating).ExecuteAffrows();
        }

        #endregion

        #region 删除

        /**
        * FreeSql对删除支持并不强大，IDelect仅支持了单表有条件的删除方法
        * 若Where条件为空的时候执行方法，FreeSql仅返回0或默认值，不执行真正的SQL删除操作。
        * 为了增强系统的安全性，强烈建议在实体中增加 is_deledted 字段做软删除标识。
        */

        /// <summary>
        /// 根据传进来的动态实体进行删除操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">链式Lambda语句</param>
        /// <param name="dywhere"></param>
        /// <returns></returns>
        public int Delete<T>(object dywhere) where T : class
        {
            return freeSql.Delete<T>(dywhere).ExecuteAffrows();
        }

        /// <summary>
        /// 根据Lambda链式语句进行删除操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int Delete<T>(Expression<Func<T, bool>> exp) where T : class
        {           
            return freeSql.Delete<T>().Where(exp).ExecuteAffrows();
        }

        /// <summary>
        /// 此方法可支持多表查询后可删除，以达到多表删除的效果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int SelectToDelete<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return freeSql.Select<T>().Where(exp).ToDelete().ExecuteAffrows();
        }

        #endregion
    }
}
