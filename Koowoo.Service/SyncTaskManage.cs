using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koowoo.Core;

namespace Koowoo.Services
{
    public class SyncTaskManage
    {
        /// <summary>
        /// 同步区域信息
        /// </summary>
        public static void SyncArea()
        {
            var areaService = ContainerManager.Resolve<IAreaService>();
            areaService.GetSyncAreaList();
        }

        /// <summary>
        /// 同步区域扩展信息
        /// </summary>
        public static void SyncAreaExtend()
        {
            var roomService = ContainerManager.Resolve<IRoomService>();
            roomService.GetSyncRoomList();
        }

        /// <summary>
        /// 同步设备信息
        /// </summary>
        public static void SyncDevice()
        {
            var doorService = ContainerManager.Resolve<IDoorService>();
            doorService.GetSyncDoorList();
        }

        /// <summary>
        /// 同步设备状态信息
        /// </summary>
        public static void SyncDeviceStatus()
        {
            var deviceStatus = ContainerManager.Resolve<IDeviceStatusService>();
            deviceStatus.GetSyncStatusList();
        }

        /// <summary>
        /// 同步设备报警信息
        /// </summary>
        public static void SyncDeviceAlarm()
        {
            var alarmService = ContainerManager.Resolve<IDeviceAlarmService>();
            alarmService.GetSyncAlarmList();
        }

        /// <summary>
        /// 同步人员信息
        /// </summary>
        public static void SyncPerson()
        {
            var personService = ContainerManager.Resolve<IPersonService>();
            var syncList = personService.GetSyncPersonList();
        }

        /// <summary>
        /// 同步人员相关信息
        /// </summary>
        public static void SyncRenter()
        {
            var renterService = ContainerManager.Resolve<IRenterService>();
            renterService.GetSyncRenterList();
        }

        /// <summary>
        /// 同步卡信息
        /// </summary>
        public static void SyncCard()
        {
            var cardService = ContainerManager.Resolve<ICardService>();
            cardService.GetSyncCardList();
        }

        /// <summary>
        /// 同步卡权限信息
        /// </summary>
        public static void SyncCardAuth()
        {
            var cardService = ContainerManager.Resolve<ICardService>();
            cardService.GetSyncCardAuthList();
        }

        /// <summary>
        /// 同步用户住户信息
        /// </summary>
        public static void SyncRoomUser()
        {
            var roomUserService = ContainerManager.Resolve<IRoomUserService>();
            roomUserService.GetSyncRoomUserList();
        }

        /// <summary>
        /// 同步用户卡信息
        /// </summary>
        public static void SyncPersonCard()
        {
            var cardService = ContainerManager.Resolve<ICardService>();
            cardService.GetSyncPersonCardList();
        }

        /// <summary>
        /// 同步出入信息
        /// </summary>
        public static void SyncEntryHistory()
        {
            var entryService = ContainerManager.Resolve<IEntryHistoryService>();
            entryService.GetSyncHistroyList();
        }

        public static void SyncRoomUserStatus()
        {
            Log.Info("开始更改离开人员状态");
            var roomUserService = ContainerManager.Resolve<IRoomUserService>();

            var roomUserList = roomUserService.GetListByLeaveTime();

            foreach (var user in roomUserList)
            {
                roomUserService.UpdateLeaveStatus(user);
            }
            Log.Info(string.Format("更改离开人员状态结束，更新了{0}条",roomUserList.Count));

        }
        
        /// <summary>
        /// 备份数据库
        /// </summary>
        public static void SyncBasedataAndSendMail()
        {
            var backupService = ContainerManager.Resolve<IBackupDataService>();
            backupService.BackupSendMail();
        }
    }
}
