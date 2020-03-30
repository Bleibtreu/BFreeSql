using DatabaseFreeSql.Entity;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DatabaseFreeSql
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Blog blog = new Blog()
            {
                Url = "www.douyu.com"
            };
            List<Blog> blogs = new List<Blog>();
            try
            {
                IDatabaseManagement database = DatabaseFactory.NewInstance(DataType.SqlServer,
                    new DatabaseSetting()
                    {
                        Database = "Blog",
                        UseAutoSyncStructure = true,

                        MonitorCommandExecuting = cmd => Trace.WriteLine(cmd.CommandText),
                        MonitorCommandExecuted = (cmd, s) => Console.WriteLine(s)
                    }); ;

                // database.freeSql.Update<Transportation>().ExecuteAffrows();
                // database.freeSql.Update<Car>().ExecuteAffrows();
                // database.freeSql.Update<Aircraft>().ExecuteAffrows();
                // database.freeSql.Update<Driver>().ExecuteAffrows();

                #region One to One
                /*
                var repoA = database.freeSql.GetRepository<User>();
                var repoB = database.freeSql.GetRepository<UserExt>();

                var user = new User();
                repoA.Insert(user);
                var userExt = new UserExt { id = user.Id };
                repoB.Insert(userExt);
                */
                #endregion

                #region One to Many
                
                var repo = database.freeSql.GetRepository<Tag>();

                var tag = new Tag()
                {
                    Name = "aaaaaa",
                    Tags = new[] {
                    new Tag { Name = "sub1" },
                    new Tag { Name = "sub2" },
                    new Tag 
                    {
                        Name = "sub3",
                        Tags = new[] 
                        {
                            new Tag { Name = "sub3_01" }
                        }
                    }
        }
                };
                repo.Insert(tag);
                
                #endregion

                #region Parent



                #endregion

                #region Many to Many
                /*
                var tags = new[]
                {
                    new Tag
                    {
                        Name = "流行"
                    },
                    new Tag
                    {
                        Name = "80后"
                    },
                    new Tag
                    {
                        Name = "00后"
                    },
                    new Tag
                    {
                        Name = "摇滚"
                    }
                };
                var ss = new[]
                {
                    new Song
                    {
                        Name = "爱你一万年.mp3",
                        Tags = new List<Tag>(new[]
                        {
                            tags[0], tags[1]
                        })
                    },
                    new Song
                    {
                        Name = "李白.mp3",
                        Tags = new List<Tag>(new[]
                        {
                            tags[0], tags[2]
                        })
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
                
                var repo = database.freeSql.GetRepository<Song>();
                repo.Insert(song);
                */
                #endregion

                // database.freeSql.Insert<Blog>().AppendData(blog).ExecuteInserted();
                // blogs = database.freeSql.Select<Blog>().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (blogs.Count != 0)
            {
                blogs.ForEach((s) => { Console.WriteLine(s.BlogId); });
            }
            Console.ReadLine();
        }
    }
}
