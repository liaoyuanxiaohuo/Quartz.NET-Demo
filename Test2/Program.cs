using Common.Logging;
using Quartz;
using Quartz.Impl;
using StudyQuartzNet.QuartzJobs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
          

            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServer1"; //"RemoteServer";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // set remoting exporter
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "556";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            // reject non-local requests
            // properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";

            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();

            //log.Info("------- Initialization Complete -----------");

            //log.Info("------- Not scheduling any Jobs - relying on a remote client to schedule jobs --");

            //log.Info("------- Starting Scheduler ----------------");
            Console.WriteLine("------- Starting Scheduler ----------------");

            DateTimeOffset runTime = DateBuilder.EvenSecondDateAfterNow();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run on the next round minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartAt(runTime)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            sched.ScheduleJob(job, trigger);
            Console.WriteLine(string.Format("{0} will run at: {1}", job.Key, runTime.ToString("r")));

            // start the schedule
            sched.Start();

        }
    }
}
