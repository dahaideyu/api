using FluentScheduler;
using System;
using System.Diagnostics;
using Koowoo.Services;

namespace Koowoo.Web
{
    public class TaskTime : Registry
    {
        public TaskTime()
        {
            //每15分钟执行一次，
            //Schedule<SyncEntryHistory>().ToRunNow().AndEvery(60).Minutes();

            // 每60分钟同步一次出入记录
            Schedule<SyncEntryHistory>().ToRunNow().AndEvery(60).Minutes();

            // Schedule a simple job to run at a specific time
            // 在每天的凌晨1点同步到期人员状态
            Schedule<SyncRoomUserStatus>().ToRunEvery(1).Days().At(1, 00);
            // 在每天的凌晨1点同步到期人员状态
            Schedule<BackupDataBase>().ToRunEvery(1).Days().At(1, 00);
        }


        /// <summary>
        /// 更改用户状态（是否到期离开）
        /// </summary>
        public class SyncRoomUserStatus : IJob
        {
            void IJob.Execute()
            {
                SyncTaskManage.SyncRoomUserStatus();
            }
        }

        //同步出入记录
        public class SyncEntryHistory : IJob
        {
            void IJob.Execute()
            {
                SyncTaskManage.SyncEntryHistory();
            }
        }


        /// <summary>
        /// 备份数据库
        /// </summary>
        public class BackupDataBase : IJob
        {
            void IJob.Execute()
            {
                try
                {
                    SyncTaskManage.SyncBasedataAndSendMail();
                }
                catch (Exception ex)
                {

                }

            }
        }
    }
}