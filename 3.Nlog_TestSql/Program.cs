using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Microsoft.Extensions.Logging;
using Npgsql;
using Dapper;
using NLog.Targets;

namespace _3.Nlog_TestSql
{
    class Program
    {
        public static string testConnString = "Server=localhost;User ID=postgres;password=613;Database=postgres";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Error("1111");
            //using (NpgsqlConnection conn = new NpgsqlConnection(testConnString))
            //{
            //    try
            //    {
            //        conn.Open();
            //        string insertSql = string.Format(@"INSERT INTO public.logger_info(time,level,info) VALUES('{0}','{1}','{2}')", DateTime.Now.ToString("HH-mm-ss"), "Warning", "test1");
            //        conn.Execute(insertSql);
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }

            //}
            Console.ReadKey();
        }
    }
}
