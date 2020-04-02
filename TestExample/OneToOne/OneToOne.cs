using System;
using System.Collections.Generic;
using System.Text;

namespace BFreeSql.TestExample
{
    class OneToOne
    {
        /// <summary>
        /// 一对一示例
        /// </summary>
        public static void Example(DatabaseManagement database)
        {
            // 获取仓库对象
            var repoA = database.freeSql.GetRepository<Lockstitch>();
            var repoB = database.freeSql.GetRepository<Key>();

            // 存入实体
            var lockstitch = new Lockstitch() { Name = "大头锁" };
            repoA.Insert(lockstitch);

            var key = new Key() { Id = lockstitch.Id, Name = "大头钥匙" };
            repoB.Insert(key);
        }
    }

    // 乐观锁注解
    // [Column(IsVersion = true)] 
    // 为表建立索引，指定唯一
    // [Index("Name", "Name", true)]
    class Lockstitch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Key Key { get; set; }
    }

    class Key
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Lockstitch Lockstitch { get; set; }
    }

}
