using BFreeSql.TestExample;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BFreeSql.Repository
{

    /**
     * 兼容问题
     * FreeSql 支持五种数据库，分别为 MySql/SqlServer/PostgreSQL/Oracle/Sqlite/达梦，
     * 虽然他们都为关系型数据库，但各自有着独特的技术亮点，有许多亮点值得我们使用；
     * 比如SqlServer提供的 output inserted 特性，在表使用了自增或数据库定义了默认值的时候，
     * 使用它可以快速将 insert 的数据返回。
     * PostgreSQL 也有相应的功能，如此方便却不是每个数据库都支持。
     * IRepository 接口定义：
     * TEntity Insert(TEntity entity);
     * Task<TEntity> InsertAsync(TEntity entity);
     * 于是我们做了两种仓库层实现：
     * BaseRepository 采用 ExecuteInserted 执行；
     * GuidRepository 采用 ExecuteAffrows 执行（兼容性好）；
     * 当采用了不支持的数据库时（Sqlite/MySql/Oracle），建议：
     * 使用 uuid 作为主键（即 Guid）；
     * 避免使用数据库的默认值功能；
     * 仓储层实现请使用 GuidRepository；
     */

    /// <summary>
    /// Repository 作为扩展，实现了通用仓储层功能。与其他规范标准一样，仓储层也有相应的规范定义。
    /// FreeSql.Repository 参考 abp vnext 接口，定义和实现基础的仓储层（CURD）
    /// Select/Attach 快照对象，Update 只更新变化的字段；
    /// Insert 插入数据，适配各数据库优化执行 ExecuteAffrows/ExecuteIdentity/ExecuteInserted；
    /// InsertOrUpdate 插入或更新；
    /// SaveMany 方法快速保存导航对象（一对多、多对多）；
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class SongRepository : BaseRepository<Song, int>
    {
        IFreeSql fsql { get; set; }

        BaseRepository<Song> baseRepository { get; set; }

        /// <summary>
        /// 这里以创建的Song表举例
        /// </summary>
        /// <param name="fsql"></param>
        public SongRepository(IFreeSql fsql) : base(fsql, null, null)
        {
            this.fsql = fsql;
            baseRepository = fsql.GetRepository<Song>();

            // 在这里编写CRUD以外的方法

            // 实体变化事件
            // 全局设置：
            fsql.SetDbContextOptions(opt =>
            {
                opt.OnEntityChange = report =>
                {
                    Console.WriteLine(report);
                };
            });

            // UnitOfWork 可将所有操作放置到一个单元操作，类似事务
            using (var unitOfWork = fsql.CreateUnitOfWork())
            {
                var songRepo = unitOfWork.GetRepository<Song>();
                var userRepo = unitOfWork.GetRepository<Tag>();

                //上面两个仓储，由同一UnitOfWork uow 创建
                //在此执行仓储操作

                //这里不受异步方便影响
                
                unitOfWork.Commit();
            }

            // 单独设置 DbContext 
            var ctx = fsql.CreateDbContext();
            ctx.Options.OnEntityChange = report =>
            {
                Console.WriteLine(report);
            };

            // 单独设置 UnitOfWork
            var uow = fsql.CreateUnitOfWork();
            Console.WriteLine(uow.EntityChangeReport);
        }

        /// <summary>
        /// 参数 report 是一个 List 集合，集合元素的类型定义如下：
        /// </summary>
        public class EntityChangeInfo
        {
            public object Object { get; set; }
            public EntityChangeType Type { get; set; }
        }
        public enum EntityChangeType { Insert, Update, Delete, SqlRaw }


        #region 未了解清楚依赖注入用法
        /*
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IFreeSql>(fsql);
            services.AddFreeRepository(filter => filter
                .Apply<ISoftDelete>("SoftDelete", a => a.IsDeleted == false)
                .Apply<ITenant>("Tenant", a => a.TenantId == 1,this.GetType()）.Assembly
        );

        /// <summary>
        /// 在控制器使用
        /// </summary>
        /// <param name="repos1"></param>
        public SongsController(GuidRepository<Song> repos1)
        {
        }
        */
        #endregion

    }
}
