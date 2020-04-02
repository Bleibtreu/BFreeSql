using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BFreeSql.TestExample
{
    /// <summary>
    /// 一对多示例
    /// </summary>
    class OneToMany
    {
        /// <summary>
        /// 多表一对多关联
        /// </summary>
        /// <param name="database"></param>
        public static void Example1(DatabaseManagement database)
        {
            // 获取该实体的仓库类，建立实体
            var repoGrade = database.freeSql.GetRepository<School_grade>();

            // 创建一个实体对象
            var school_grade = new School_grade()
            {
                Grade = "五年级",
                //实体对象里嵌套子表的实体对象
                School_class = new[]
                {
                        new School_class{ Class = "一班" },
                        new School_class{ Class = "二班" }
                    }
            };
            // Insert方法内部会解析实体对象，并分出子表对象
            // 需配合实体创建好字段
            repoGrade.Insert(school_grade);
        }

        /// <summary>
        /// 单表一对多关联（父子表）
        /// </summary>
        /// <param name="database"></param>
        public static void Example2(DatabaseManagement database)
        {
            var repo = database.freeSql.GetRepository<Box>();

            var box = new Box
            {
                Name = "testaddsublist",
                Boxs = new[]
                {
                        new Box { Name = "sub1" },
                        new Box { Name = "sub2" },
                        new Box
                        {
                            Name = "sub3",
                            Boxs = new[]
                            {
                                new Box { Name = "sub3_01" }
                            }
                        }
                    }
            };
            repo.Insert(box);
        }
    }

    // 此处以多表举例

    /// <summary>
    /// 父表
    /// 此处以 年级-班级 距离
    /// </summary>
    class School_grade
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        public string Grade { get; set; }        
        // 实体内部创建Collection属性，FreeSql内部会进行解析，但需在子表进行相应字段
        public ICollection<School_class> School_class { get; set; }
    }

    /// <summary>
    /// 子表
    /// </summary>
    class School_class
    {        
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }              
        public string Class { get; set; }
        // 子表需创建父表属性       
        public School_grade School_Grade { get; set; }
        // 并创建与父表相应的Id字段（School_GradeId、School_Grade_Id）
        // 或使用Navigate[]属性导航(需关闭EnableAddOrUpdateNavigateList)
        public int School_Grade_Id { get; set; }
    }

    /// <summary>
    /// 单表实体
    /// </summary>
    class Box
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? Parent_id { get; set; }
        // 实体以自身为子表嵌套距离
        public virtual Box Parent { get; set; }
        public virtual ICollection<Box> Boxs { get; set; }
    }
}
