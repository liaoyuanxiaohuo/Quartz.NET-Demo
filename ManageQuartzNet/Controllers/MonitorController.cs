using CrystalQuartz.Core.SchedulerProviders;
using ManageQuartzNet.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageQuartzNet.Controllers
{
    public class MonitorController : Controller
    {
        private static RemoteSchedulerProvider remoteSchedulerProvider = new RemoteSchedulerProvider();

        //private static string scheme = "tcp";

        //private static string server = "localhost";

        //private static string port = "555";
        private IScheduler scheduler = null;
        static IList<JobGroup> groupList = null;

        public void InitRemoteScheduler(string instanceName, string scheme, string server, string port)
        {
            try
            {
                NameValueCollection properties = new NameValueCollection();
                //   properties["quartz.scheduler.instanceName"] = "schedMaintenanceService"; //QuartzTest
                properties["quartz.scheduler.instanceName"] = instanceName; // "QuartzTest";
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = string.Format("{0}://{1}:{2}/QuartzScheduler", scheme, server, port);
                ISchedulerFactory sf = new StdSchedulerFactory(properties);

                scheduler = sf.GetScheduler();
            }
            catch (Exception ex)
            {
                // LogHelper.WriteLog("初始化远程任务管理器失败" + ex.StackTrace);
            }
        }

        public void InitGroup(string IP, string port)
        {
            if (scheduler != null)
            {
                IList<string> groupNameList = scheduler.GetJobGroupNames(); //获取组名
                foreach (var item in groupNameList)
                {
                    JobGroup group = new JobGroup();
                    group.GroupName = item;
                    group.IP = IP;
                    group.Port = port;
                    group.Scheduler = scheduler;
                    #region job
                    group.JobList = new List<Job>();
                    GroupMatcher<JobKey> matcher = GroupMatcher<JobKey>.GroupEquals(item);
                    var jobList = scheduler.GetJobKeys(matcher); ////获取组的job
                                                                 // .Skip(1).Take(1);  //获取组的job
                    foreach (var jobItem in jobList)
                    {
                        Job job = new Job();
                        job.JobName = jobItem.Name;
                        group.JobList.Add(job);
                        #region Trigger触发器
                        //IList<ITrigger> GetTriggersOfJob(JobKey jobKey);
                        JobKey jk = new JobKey(jobItem.Name, item);
                        IList<ITrigger> triggerList = scheduler.GetTriggersOfJob(jk);
                        foreach (var triggerItem in triggerList) //这里只有一个触发器（一个job一个触发器）
                        {
                            //  TriggerState GetTriggerState(TriggerKey triggerKey); 获取触发器的状态
                            TriggerState triggerState = scheduler.GetTriggerState(triggerItem.Key);
                            job.JobState = triggerState;
                            //job.StartTimeUtc = triggerItem.StartTimeUtc;
                            //job.EndTimeUtc = triggerItem.EndTimeUtc;
                            job.StartTime = triggerItem.StartTimeUtc.LocalDateTime;
                            if (triggerItem.EndTimeUtc.HasValue)
                            {
                                job.EndTime = triggerItem.EndTimeUtc.Value.LocalDateTime;
                            }
                            if (triggerItem.FinalFireTimeUtc.HasValue)
                            {
                                job.FinalFireTime = triggerItem.FinalFireTimeUtc.Value.LocalDateTime;
                            }

                            var pre = triggerItem.GetPreviousFireTimeUtc();
                            if (pre.HasValue)
                            {
                                job.PreviousFireTime2 = pre.Value.LocalDateTime;
                            }
                            var next = triggerItem.GetNextFireTimeUtc();
                            if (next.HasValue)
                            {
                                job.NextFireTime2 = next.Value.LocalDateTime;
                            }

                        }
                        #endregion
                    }
                    #endregion

                    groupList.Add(group);
                }
            }
        }


        // GET: Monitor
        public ActionResult Index()
        {
            //remoteSchedulerProvider.SchedulerHost = System.Configuration.ConfigurationManager.AppSettings["SchedulerHost"]; //SchedulerHost
            try
            {
                groupList = new List<JobGroup>();
                #region 一个scheduler
                InitRemoteScheduler("QuartzTest", "tcp", "localhost", "555");
                InitGroup("localhost", "555");
                #endregion

                #region
                InitRemoteScheduler("RemoteServer1", "tcp", "127.0.0.1", "556");
                InitGroup("127.0.0.1", "556");
                #endregion

                #region 另一个scheduler
                InitRemoteScheduler("RemoteServer", "tcp", "127.0.0.1", "558");
                InitGroup("127.0.0.1", "558");
                #endregion

              

                return View(groupList);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "初始化远程任务管理器失败" + ex.StackTrace;
                return View();
            }
        }


        /// <summary>
        /// 恢复开始任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Start(string jobName, string group)
        {
            remoteSchedulerProvider.Scheduler.ResumeJob(new JobKey(jobName, group));
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Stop(string jobName, string group)
        {
            remoteSchedulerProvider.Scheduler.PauseJob(new JobKey(jobName, group));
            return RedirectToAction("Index");
        }


        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="JobKey"></param>
        [HttpPost]
        public string PauseJob(string ip, string port, string jobName, string group)
        {
            try
            {
                var groupFind = groupList.Where(m => m.IP == ip && m.Port == port).FirstOrDefault();
                if (groupFind == null)
                {
                    return "任务job不存在";
                }
                scheduler = groupFind.Scheduler;
                JobKey jk = new JobKey(jobName, group);
                if (scheduler.CheckExists(jk))
                {
                    //任务已经存在则暂停任务
                    scheduler.PauseJob(jk);
                    //LogHelper.WriteLog(string.Format("任务“{0}”已经暂停", JobKey));
                    string str = string.Format("任务“{0}”已经暂停", jobName);
                    return str;
                }
                return "任务job不存在";
            }
            catch (Exception ex)
            {
                // throw ex;
                return ex.ToString();
            }
        }

        /// <summary>
        /// 重新启动任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public string ResumeJob(string ip, string port, string jobName, string group)
        {
            try
            {
                var groupFind = groupList.Where(m => m.IP == ip && m.Port == port).FirstOrDefault();
                if (groupFind == null)
                {
                    return "任务job不存在";
                }
                scheduler = groupFind.Scheduler;
                JobKey jk = new JobKey(jobName, group);
                if (scheduler.CheckExists(jk))
                {
                    //任务已经存在则重新启动任务
                    scheduler.ResumeJob(jk);
                    //LogHelper.WriteLog(string.Format("任务“{0}”已经暂停", JobKey));
                    string str = string.Format("任务“{0}”已经启动", jobName);
                    return str;
                }
                else
                {
                    return "任务job不存在";
                }
            }
            catch (Exception ex)
            {
                // throw ex;
                return ex.ToString();
            }
        }
    }
}