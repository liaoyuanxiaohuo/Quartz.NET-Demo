using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageQuartzNet.Models
{
    /// <summary>
    /// 组
    /// </summary>
    public class JobGroup
    {
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }

        public IScheduler Scheduler { get; set; }

        public List<Job> JobList { get; set; }
    }


    /// <summary>
    /// job
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Job名
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>                    
        public TriggerState JobState { get; set; }

        // 
        #region IJobExecutionContext
        public DateTimeOffset? NextFireTimeUtc { get; set; }

        public DateTimeOffset? PreviousFireTimeUtc { get; set; }

        public DateTimeOffset? ScheduledFireTimeUtc { get; set; }

        public DateTimeOffset? FireTimeUtc { get; set; }
        #endregion

        #region ITrigger
        //// public DateTimeOffset StartTimeUtc { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// public DateTimeOffset? EndTimeUtc { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// public DateTimeOffset? FinalFireTimeUtc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? FinalFireTime { get; set; }

        //DateTimeOffset? GetNextFireTimeUtc();
        /// public DateTimeOffset? NextFireTimeUtc2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? NextFireTime2 { get; set; }

        // DateTimeOffset? GetPreviousFireTimeUtc();
        /// public DateTimeOffset? PreviousFireTimeUtc2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? PreviousFireTime2 { get; set; }

        #endregion

    }
}