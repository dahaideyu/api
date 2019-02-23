using Koowoo.Pojo;
using Koowoo.Services;
using System.Web.Mvc;

namespace Koowoo.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TempController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public ICardService cardService { get; set; }


        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var res = "";
            res += "<a href=\"Temp/SyncPerson\">人员SyncPerson</a><br>";
            res += "<a href=\"Temp/SyncRoomUser\">入住信息SyncRoomUser</a><br>";
            res += "<a href=\"Temp/SyncCard\">卡SyncCard</a><br>";
            res += "<a href=\"Temp/SyncAreaExtend\">区域扩展SyncAreaExtend</a><br>";
            res += "<a href=\"Temp/SyncRenter\">出租信息SyncRenter</a><br>";
            res += "<a href=\"Temp/SyncCardAuth\">卡权限SyncCardAuth</a><br>";
            res += "<a href=\"Temp/SyncArea\">区域SyncArea</a><br>";
            res += "<a href=\"Temp/SyncDevice\">设备SyncDevice</a><br>";
            res += "<a href=\"Temp/SyncDeviceStatus\">设备状态SyncDeviceStatus</a><br>";
            res += "<a href=\"Temp/SyncDeviceAlarm\">设备报警SyncDeviceAlarm</a><br>";
            res += "<a href=\"Temp/SyncPersonCard\">人员卡SyncPersonCard</a><br>";
            // res += "<a href=\"Temp/SyncEntryHistory\">SyncEntryHistory</a><br>";
            //res += "<a href=\"Temp/RoomUserStatus\">RoomUserStatus</a><br>";
            return Content(res);
        }

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncArea()
        {
            SyncTaskManage.SyncArea();
            return Content("SyncArea");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncAreaExtend()
        {
            SyncTaskManage.SyncAreaExtend();
            return Content("AreaExtend");
        }



        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncDevice()
        {
            SyncTaskManage.SyncDevice();
            return Content("SyncDevice");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncDeviceStatus()
        {
            SyncTaskManage.SyncDeviceStatus();
            return Content("SyncDeviceStatus");
        }

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncDeviceAlarm()
        {
            SyncTaskManage.SyncDeviceAlarm();
            return Content("SyncDeviceAlarm");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncPerson()
        {
            SyncTaskManage.SyncPerson();
            return Content("SyncPerson");
        }

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncRenter()
        {
            SyncTaskManage.SyncRenter();
            return Content("SyncRenter");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncCard()
        {
            SyncTaskManage.SyncCard();
            return Content("SyncCard");
        }

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncCardAuth()
        {
            SyncTaskManage.SyncCardAuth();
            return Content("SyncCardAuth");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncRoomUser()
        {
            SyncTaskManage.SyncRoomUser();
            return Content("SyncRoomUser");
        }

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncPersonCard()
        {
            SyncTaskManage.SyncPersonCard();
            return Content("SyncPersonCard");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SyncEntryHistory()
        {
            SyncTaskManage.SyncEntryHistory();
            return Content("SyncEntryHistory");
        }

        public ActionResult Convert()
        {
            cardService.InsertCardNoConvert();

            return Content("aa");
        }

        public ActionResult RoomUserStatus()
        {
            SyncTaskManage.SyncRoomUserStatus();

            return Content("RoomUserStatus");
        }

    }
}