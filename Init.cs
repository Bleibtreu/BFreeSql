using BFreeSql.TestExample;
using BFreeSql.TestExample.Parent;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BFreeSql
{
    /// <summary>
    /// 初始化IFreeSql对象
    /// </summary>
    class Init
    {
        public static void Main(string[] args)
        {
            try
            {
                /*
                 * 创建Database对象入口
                 * 配置DatabaseSetting模板，除数据库名外，其余可不配置，使用默认值
                 */
                DatabaseManagement database = DatabaseFactory.NewInstance(
                    DataType.SqlServer,
                    new DatabaseSetting()
                    {
                        Database = "FSQL",
                        UseAutoSyncStructure = true,
                        MonitorCommandExecuting = cmd => Trace.WriteLine(cmd.CommandText),
                        MonitorCommandExecuted = (cmd, s) => Console.WriteLine(s)
                    });

                // 展示方法请打开注释

                #region One to One

                // OneToOne.Example(database);

                #endregion

                #region One to Many              

                // OneToMany.Example1(database);

                #endregion

                #region inherit(未设计)

                //Inherit.Example();

                #endregion

                #region Many to Many

                //ManyToMany.Example(database);

                #endregion

                #region 外部同线程事务

                /*
                //freeSql.Ado.TransactionCurrentThread 此属性可得到事务对象
                database.freeSql.Transaction(() =>
                {
                    var affrows = database.freeSql.Update<User>().Set(a => a.Wealth - 100)
                        .Where(a => a.Wealth >= 100)
                        //判断别让用户余额扣成负数
                        .ExecuteAffrows();
                    if (affrows < 1)
                        throw new Exception("用户余额不足");
                    //抛出异常，事务退出

                    affrows = database.freeSql.Update<Goods>()
                      .Set(a => a.Stock - 1)
                      .Where(a => a.Stock > 0)
                      //判断别让用库存扣成负数
                      .ExecuteAffrows();
                    if (affrows < 1)
                        throw new Exception("商品库存不足");
                    //抛出异常，回滚事务，事务退出
                    //用户余额的扣除将不生效

                    //程序执行在此处，说明都扣成功了，事务完成并提交
                });
                */

                #endregion

                #region 测试代码

                /*
                List<Song> songs = database.Select<Song, Tag, Song_tag>(a => a.Id == 1, b => b.Name == "流行");

                songs.ForEach(a => Console.WriteLine(a.Id));
                Console.WriteLine("-----------------");
                songs.ForEach(a => Console.WriteLine(a.Name));
                */

                #endregion

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }

    /// <summary>
    /// 事务测试实体类
    /// </summary>
    class User 
    {
        public int Id { get; set; }
        public int Wealth { get; set; }
    }

    /// <summary>
    /// 事务测试实体类
    /// </summary>
    class Goods 
    {
        public int Id { get; set; }
        public int Stock { get; set; }
    }

}
