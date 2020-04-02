using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BFreeSql.TestExample
{
    class ManyToMany
    {
        public static void Example(DatabaseManagement database)
        {
            #region 创建测试实体对象
            var tags = new[]
            {
                    new Tag { Name = "流行"},
                    new Tag { Name = "80后"},
                    new Tag { Name = "00后"},
                    new Tag { Name = "摇滚"}
            };

            var ss = new[]
            {
                    new Song
                    {
                        Name = "爱你一万年.mp3",
                        Tags = new List<Tag>(new[] { tags[0], tags[1]})
                    },
                    new Song
                    {
                        Name = "李白.mp3",
                        Tags = new List<Tag>(new[] { tags[0], tags[3] })
                    }
                };

            var song = new Song()
            {
                Id = 0,
                Name = "爱你一万年.mp3",
                Tags = new List<Tag>(new[]
                        {
                            tags[0], tags[1]
                        })
            };

            #endregion

            var repo = database.freeSql.GetRepository<Song>();
           
            // 追加保存，即不处理已存在的数据
            // repo.DbContextOptions.EnableAddOrUpdateNavigateList = true; //需要手工开启
            // repo.Insert(ss);

            // 完整保存,在现有的数据上保存，内部会和现有数据进行对比，计算出应该插入、更新、删除的子记录
            repo.Insert(ss);
            repo.SaveMany(song, "Tags");

        }
    }

    
    
    class Song
    {
        [Column(IsIdentity = true, IsPrimary = true)]               // 自增标识，主键标识
        public int Id { get; set; }
        [Column(IsNullable = false, DbType = "varchar(128)")]       // 非空标识 字段类型
        public string Name { get; set; }
        [Column(ServerTime = DateTimeKind.Utc, CanUpdate = false)]  // 服务器时间及是否自动更新
        public DateTime CreateTime { get; set; }
        [Column(IsIgnore = true)]                                   // 忽略标识，开启后FreeSql会自动忽略该属性
        public string IgnoreString { get; set; }
        [Column(IsIgnore = true)]                                   // [Column(IsVersion = true)]乐观锁注解
        public string OptimisticLock { get; set; }
        [Column(IsIgnore = true)]                                   // [Index("index", "index", true)]为表建立索引，指定唯一
        public string Index { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }

    class Song_tag
    {
        public int Song_id { get; set; }
        public virtual Song Song { get; set; }

        public int Tag_id { get; set; }
        public virtual Tag Tag { get; set; }
    }


    class Tag
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        [Column(StringLength = 128)]
        public string Name { get; set; }

        public int? Parent_id { get; set; }
        public virtual Tag Parent { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }

    /// <summary>
    /// 自定义类型映射(MapType)
    /// BigInteger 也可以映射使用,
    /// 请注意：仅仅是 CURD 方便， Equals == 判断可以使用，无法实现 + - * / 等操作
    /// </summary>
    class EnumTestMap
    {
        public Guid id { get; set; }
        [Column(MapType = typeof(string))]
        public ToStringMapEnum enum_to_string { get; set; }
        [Column(MapType = typeof(string))]
        public ToStringMapEnum? enumnullable_to_string { get; set; }
        [Column(MapType = typeof(int))]
        public ToStringMapEnum enum_to_int { get; set; }
        [Column(MapType = typeof(int?))]
        public ToStringMapEnum? enumnullable_to_int { get; set; }
        [Column(MapType = typeof(string))]
        public BigInteger biginteger_to_string { get; set; }
        [Column(MapType = typeof(string))]
        public BigInteger? bigintegernullable_to_string { get; set; }
    }
    public enum ToStringMapEnum { 中国人, abc, 香港 }

    /// <summary>
    /// 自定义插入值(InsertValueSql)
    /// INSERT INTO `type`(`Name`) VALUES('xxx')
    /// </summary>
    [Table(DisableSyncStructure = true)]            // 禁用自动迁移此表
    class Type
    {
        [Column(InsertValueSql = "'xxx'")]
        public string Name { get; set; }
    }


}
