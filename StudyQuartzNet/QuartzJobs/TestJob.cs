using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudyQuartzNet.QuartzJobs
{
    public sealed class TestJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TestJob));
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //_logger.Info("描述：" + context.JobDetail.Description);
                //_logger.Info("Key:" + context.JobDetail.Key);
                //Thread d = new Thread(Show);
                //d.Start();
                //_logger.Info("线程数:" + Process.GetCurrentProcess().Threads.Count);
                _logger.Info("TestJob:运行" + DateTime.Now.ToLongTimeString());
                InsertPerson();
            }
            catch(Exception ex)
            {
                _logger.Info(ex.Message);
                _logger.Info("测试添加任务出错");

                JobExecutionException e2 = new JobExecutionException(ex);
                e2.RefireImmediately = true; //出错后马上恢复执行
            }
        }

        public string[] name = { "梅", "兰", "菊", "竹", "美", "华", "名", "天", "张", "启", "应", "英", "田", "涛", "万", "秀", "风", "云", "高", "知", "志" };
        public string[] fisrtName = { "林", "王", "马", "郑", "张", "蔡", "何", "贺", "宋", "秦", "邱", "赵", "孔", "陈", "黄", "金", "朱" };

        public void InsertPerson()
        {
            try
            {
                _logger.Info("开始插入");
                Random ran = new Random();
                int randKey0 = ran.Next(fisrtName.Length);
                int randKey1 = ran.Next(name.Length);
                int randKey2 = ran.Next(name.Length);
                DateTime nowTime = DateTime.Now;
                Person one = new Person()
                {
                    Name = fisrtName[randKey0] + name[randKey1] + name[randKey2],
                    Sex = randKey1 % 2 == 0,
                    Age = 20 + randKey2,
                    Height = 170 + randKey1,
                    Weight = 110 + randKey0,
                    CityID = "110000",
                    CreateTime = nowTime,
                    EditTime = nowTime
                };
                using (var ctx = new MyTestEntities())
                {
                    ctx.People.Add(one);
                    ctx.SaveChanges();
                }
                _logger.Info("插入成功");
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }
        }

        //public void Show()
        //{
        //    _logger.Info("线程数2:" + Process.GetCurrentProcess().Threads.Count);
        //    _logger.Info("时间：" + DateTime.Now.ToLongTimeString());
        //}
    }
}
