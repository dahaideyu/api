using Koowoo.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Koowoo.Services
{
    public interface IBackupDataService
    {
        ResponseResult BackupSendMail();
        bool ExeBackup(string serverip, string serverid, string serverpwd, string mydataBase, string databasefile);
        ResponseResult ExeBackup(string connectionString, string mydataBase, string databasefile);
    }


    public class BackupDataService : IBackupDataService, IDependency
    {
        public ResponseResult BackupSendMail()
        {
            try
            {
                var parentPath = HostingEnvironment.MapPath("~/download/basedata");
                var fileFullName = parentPath + "/" + DateTime.Now.ToFileTime() + ".bak";
                //  BasedataName
                var basedataName = ConfigurationManager.AppSettings["BasedataName"];
                //BataSendMailTo
                var mailto = ConfigurationManager.AppSettings["BataSendMailTo"];
                //BataSendMailTo
                var mailFrom = ConfigurationManager.AppSettings["BataSendMailFrom"];
                var connectstr = ConfigurationManager.ConnectionStrings["SqlServiceEntities"].ToString();
                var basedataResult = ExeBackup(connectstr, basedataName, fileFullName);
                if (basedataResult.ResultType != OperationResultType.Success)
                {
                    return basedataResult;
                }
                MailHelper mhelper = new MailHelper();

                MailAddress md = new MailAddress(mailFrom);
                var mailResult = mhelper.Send(md, mailto, "BackupBaseData", "\r\n Backupdata", fileFullName);
 
                return mailResult;
            }
            catch (Exception ex)
            {
                return new ResponseResult(OperationResultType.Error,ex.Message);
            }

        }
        /// <summary>
        /// 通过连接自符串备份
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="mydataBase">需要备份的数据库</param>
        /// <param name="databasefile">备份的文件名</param>
        /// <returns></returns>
        public ResponseResult ExeBackup(string connectionString, string mydataBase, string databasefile)
        {
            //  string connectionString = "server=" + serverip + ";database=master;uid=" + serverid + ";pwd=" + serverpwd;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {

                //if (!File.Exists(databasefile))
                //{

                //}
                //还原的数据库MyDataBase
                string sql = "BACKUP DATABASE " + mydataBase + " TO DISK = '" + databasefile + "'";
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.CommandType = CommandType.Text;
                comm.ExecuteNonQuery();



                conn.Close();//关闭数据库连接
                return new ResponseResult(OperationResultType.Success);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                conn.Close();
                return new ResponseResult(OperationResultType.Error, ex.Message);
            }
            finally
            {
                // oSQLServer.DisConnect();
            }
        }
        public bool ExeBackup(string serverip, string serverid, string serverpwd, string mydataBase, string databasefile)
        {
            string connectionString = "server=" + serverip + ";database=master;uid=" + serverid + ";pwd=" + serverpwd;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {


                //if (!File.Exists(databasefile))
                //{

                //}
                //还原的数据库MyDataBase
                string sql = "BACKUP DATABASE " + mydataBase + " TO DISK = '" + databasefile;
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.CommandType = CommandType.Text;
                comm.ExecuteNonQuery();



                conn.Close();//关闭数据库连接
                return true;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                conn.Close();
                return false;
            }
            finally
            {
                // oSQLServer.DisConnect();
            }


            ////数据库备份
            //string backaway = "D:/Backup/";
            //SQLDMO.Backup oBackup = new SQLDMO.BackupClass();
            //SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
            //try
            //{
            //    oSQLServer.LoginSecure = false;
            //    //下面设置登录sql服务器的ip,登录名,登录密码
            //    oSQLServer.Connect(serverip, serverid, serverpwd);
            //    oBackup.Action = 0;
            //    //下面两句是显示进度条的状态
            //    //SQLDMO.BackupSink_PercentCompleteEventHandler pceh = new SQLDMO.BackupSink_PercentCompleteEventHandler(Step2);
            //    //oBackup.PercentComplete += pceh;
            //    //数据库名称:
            //    oBackup.Database = "Community";
            //    //备份的路径
            //    oBackup.Files = @backaway;
            //    //备份的文件名
            //    oBackup.BackupSetName = "bk" + DateTime.Now.ToFileTimeUtc().ToString();
            //    oBackup.BackupSetDescription = "数据库备份";
            //    oBackup.Initialize = true;
            //    oBackup.SQLBackup(oSQLServer);
            //    //MessageBox.Show("备份成功！", "提示");
            //}
            //catch
            //{
            //  //  MessageBox.Show("备份失败！", "提示");
            //}
            //finally
            //{
            //    oSQLServer.DisConnect();
            //}
        }
    }
}
