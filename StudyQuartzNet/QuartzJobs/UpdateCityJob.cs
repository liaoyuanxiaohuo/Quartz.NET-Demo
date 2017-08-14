
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuartzNet.QuartzJobs
{
    public sealed class UpdateCityJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UpdateCityJob));
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("时间：" + DateTime.Now.ToString());
                UpdateUserCity();
            }
            catch(Exception ex)
            {
                _logger.Info(ex.Message);
                _logger.Info("测试添加任务出错");

                JobExecutionException e2 = new JobExecutionException(ex);
                e2.RefireImmediately = true; //出错后马上恢复执行
            }
        }

     

        public void UpdateUserCity()
        {
            _logger.Info("开始更新" + DateTime.Now.ToString());
            try
            {
                using (var ctx = new MyTestEntities())
                {
                    var cityEntiy = ctx.Base_City.ToList();
                    int count = ctx.People.Count();
                    for (int i = 0, j = 0; i < count; i = i + 100, j++)
                    {
                        var userEntity = ctx.People.OrderBy(m => m.ID).Skip(i).Take(100).ToList();
                        foreach (var item in userEntity)
                        {
                            Random ran = new Random();
                            //int randKey0 = ran.Next(0, cityEntiy.Count - 1);
                            item.CityID = cityEntiy[j % (cityEntiy.Count - 1)].CityID;
                            item.EditTime = DateTime.Now;
                        }
                    }
                   
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }
            _logger.Info("更新完成" + DateTime.Now.ToString());
        }
    }
}
