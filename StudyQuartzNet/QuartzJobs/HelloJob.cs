using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuartzNet.QuartzJobs
{
    public class HelloJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TestJob));
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("HelloJob:运行" + DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex)
            {
                _logger.Info("HelloJob出错");
                JobExecutionException e2 = new JobExecutionException(ex);
                e2.RefireImmediately = true; //出错后马上恢复执行
            }
        }
    }
}
