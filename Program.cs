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
                        // MonitorCommandExecuting = cmd => Console.WriteLine(cmd.CommandText),
                        // MonitorCommandExecuted = (cmd, s) => Console.WriteLine(cmd.CommandText)
                    });;
                database.fsql.Insert<Blog>().AppendData(blog).ExecuteInserted();
                blogs = database.fsql.Select<Blog>().ToList();
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
