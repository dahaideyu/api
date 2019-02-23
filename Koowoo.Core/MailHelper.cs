using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Core
{
    public class MailHelper
    {

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="MessageFrom">发件人邮箱地址</param>
        /// <param name="MessageTo">收件人邮箱地址</param>
        /// <param name="MessageSubject">邮件主题</param>
        /// <param name="MessageBody">邮件内容</param>
        /// <param name="SUpFile">附件</param>
        /// <returns></returns>
        public ResponseResult Send(MailAddress MessageFrom, string MessageTo, string MessageSubject, string MessageBody, string SUpFile)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = MessageFrom;
                message.To.Add(MessageTo); //收件人邮箱地址可以是多个以实现群发

                message.Subject = MessageSubject;
                message.Body = MessageBody;

                if (SUpFile != "")
                {

                    //  SUpFile = System.Web.HttpContext.Current.Server.MapPath("~/发邮件/Upfile/" + SUpFile);//获得附件在本地地址
                    //将文件进行转换成Attachments
                    Attachment data = new Attachment(SUpFile, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(SUpFile);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(SUpFile);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(SUpFile);

                    message.Attachments.Add(data);
                    System.Net.Mime.ContentType ctype = new System.Net.Mime.ContentType();
                }

                message.IsBodyHtml = true; //是否为html格式
                message.Priority = MailPriority.Normal; //发送邮件的优先等级
                SmtpClient sc = new SmtpClient();
                sc.Host = "smtp.sina.com"; //指定发送邮件的服务器地址或IP
                sc.Port = 25; //指定发送邮件端口
                var sendAccount = ConfigurationManager.AppSettings["sendAccount"];
                var sendPassword = ConfigurationManager.AppSettings["sendPassword"];

                sc.Credentials = new System.Net.NetworkCredential(sendAccount, sendPassword); //指定登录服务器的try

                sc.Send(message); //发送邮件
            }
            catch (Exception ex)
            {
                return new ResponseResult(OperationResultType.Error, ex.Message);
            }
            return new ResponseResult(OperationResultType.Success);
        }
    }
}
