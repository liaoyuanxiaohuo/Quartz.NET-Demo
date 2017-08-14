using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuartzNet.QuartzJobs
{
    public class SendJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(SendJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("开始发送：" + DateTime.Now.ToString());
                SendEmail();
                _logger.Info("发送结束：" + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
                _logger.Info("发送邮件任务出错：" + DateTime.Now.ToString());
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public void SendEmail()
        {
            MailMessage msg = new MailMessage();

            // msg.To.Add("收件人地址@qq.com");
            msg.To.Add("AAAAAAA@qq.com");
            // msg.CC.Add("抄送人地址@qq.com");

            // msg.From = new MailAddress("发件人邮箱@qq.com", "名称");
            msg.From = new MailAddress("XXXXXXX@qq.com", "XXXX");

            msg.Subject = "邮件标题";
            //标题格式为UTF8  
            msg.SubjectEncoding = Encoding.UTF8;

            msg.Body = "邮件内容" + DateTime.Now.ToString();
            //内容格式为UTF8 
            msg.BodyEncoding = Encoding.UTF8;

            SmtpClient client = new SmtpClient();
            //SMTP服务器地址 
            client.Host = "smtp.qq.com";
            //SMTP端口，QQ邮箱填写587  
            client.Port = 587;
            //启用SSL加密  
            client.EnableSsl = true;
            //client.Credentials = new NetworkCredential("发件人邮箱账号@qq.com", "密码或者授权码");
            client.Credentials = new NetworkCredential("XXXX@qq.com", "AAAAAA");
            //发送邮件  
            client.Send(msg);
        }
    }



}
