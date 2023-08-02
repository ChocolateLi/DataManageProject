using api.Model;
using SqlSugar;
using System.Reflection;

namespace api.Data
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class SqlSugarHelper
    {
        public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
        {
            //ConnectionString = "Data Source=DESKTOP-2DUF58Q\\SQLEXPRESS;Initial Catalog=Admin_Demo;Persist Security Info=True;User ID=sa;Password=123456",//连接符字串
            ConnectionString = "Data Source=LAPTOP-O4K3OLFA;Initial Catalog=Admin_Demo;Persist Security Info=True;User ID=sa;Password=123456",//连接符字串
            DbType = DbType.SqlServer,//数据库类型
            IsAutoCloseConnection = true //不设成true要手动close
        },
        db =>
        {
            //(A)全局生效配置点，一般AOP和程序启动的配置扔这里面 ，所有上下文生效
            //调试SQL事件，可以删掉
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);//输出sql,查看执行sql 性能无影响
            };
        });

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        public static string InitDateBase()
        {
            try
            {
                //创建数据库
                Db.DbMaintenance.CreateDatabase();
                //创建表
                string nspace = "api.Model";
                Type[] ass = Assembly.LoadFrom(AppContext.BaseDirectory + "api.dll").GetTypes().Where(p => p.Namespace == nspace).ToArray();
                Db.CodeFirst.SetStringDefaultLength(200).InitTables(ass);
                //初始化数据
                //添加数据之前先清空
                Db.Deleteable<User>().ExecuteCommand();
                List<User> list = new List<User>();
                for (int i = 1; i <= 5; i++)
                {
                    list.Add(new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Tom" + i,
                        Date = DateTime.Now,
                        Address = "No. 189, Grove St, Los Angeles",
                        Order = i
                    });
                }
                for (int i = 6; i <= 10; i++)
                {
                    list.Add(new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Tom" + i,
                        Date = DateTime.Now,
                        Address = "No. 129, Grove St, Los Angeles",
                        Order = i
                    });
                }
                for (int i = 11; i <= 30; i++)
                {
                    list.Add(new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Tom" + i,
                        Date = DateTime.Now,
                        Address = "No. 87, Grove St, Los Angeles",
                        Order = i
                    });
                }
                Db.Insertable(list).ExecuteCommand();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 读取用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static Result GetUsers(Model req)
        {
            Result result = new Result();
            int total = 0;
            result.Res = Db.Queryable<User>()
                .WhereIF(!string.IsNullOrEmpty(req.KeyWord), s => s.Name.Contains(req.KeyWord) || s.Address.Contains(req.KeyWord))
                .OrderBy(s => s.Order)
                .ToOffsetPage(req.PageIndex, req.PageSize, ref total);
            result.Total = total;
            return result;
        }

        public static bool Add(AddReq req)
        {
            User info = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = req.Name,
                Date = req.Date,
                Address = req.Address,
                Order = req.Order
            };
            if (Db.Queryable<User>().Any(p => p.Name == req.Name))
            {
                return false;
            }
            return Db.Insertable(info).ExecuteCommand() > 0;
        }
        public static bool Edit(User req)
        {
            User info = Db.Queryable<User>().First(p => p.Id == req.Id);
            if (info == null)
            {
                return false;
            }
            info.Name = req.Name;
            info.Date = req.Date;
            info.Address = req.Address;
            info.Order = req.Order;
            return Db.Updateable(info).ExecuteCommand() > 0;
        }
        public static bool Del(string ids)
        {
            return Db.Ado.ExecuteCommand($"DELETE [User] WHERE Id IN({ids})") > 0;
        }
    }

    public class Model
    {
        public string KeyWord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
    public class Result
    {
        public int Total { get; set; }
        public object Res { get; set; }
    }
    public class AddReq
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public int Order { get; set; }
    }
}