using Koowoo.Core;
using Koowoo.Domain.System;
using Koowoo.Pojo;
using Koowoo.Pojo.System;
using System;
using System.Linq;
using Koowoo.Core.Extentions;
using Koowoo.Data.Interface;
using Koowoo.Core.Pager;
using Koowoo.Pojo.Request;
using Koowoo.Domain;
using System.Collections.Generic;
using System.Text;
using Koowoo.Services.System;
using Koowoo.Services.LkbResponse;
using Koowoo.Pojo.Enum;

namespace Koowoo.Services
{
    public interface IDeviceAlarmService
    {
        TableData GetList(QueryDateReq req);
        DeviceAlarmDto GetById(string doorId);
        void Create(DeviceAlarmDto config);
        void Update(DeviceAlarmDto config);
        void Delete(string ids);
        void GetSyncAlarmList();
    }

    public class DeviceAlarmService : IDeviceAlarmService, IDependency
    {
        private readonly IRepository<DeviceAlarmEntity> _alarmRepository;
        private readonly IAreaService _areaService;
        private readonly IDictService _dictService;
        private readonly IDoorService _doorService;
        private readonly ISyncLogServie _syncService;

        public DeviceAlarmService(IRepository<DeviceAlarmEntity> alarmRepository,
            IAreaService areaService,
            ISyncLogServie syncService,
            IDoorService doorService,
            IDictService dictService)
        {
            _alarmRepository = alarmRepository;
            _areaService = areaService;
            _dictService = dictService;
            _doorService = doorService;
            _syncService = syncService;
        }

        public TableData GetList(QueryDateReq req)
        {
            var query = _alarmRepository.Table;


            if (req.beginTime.HasValue)
            {
                query = query.Where(t => t.OccurTime >= req.beginTime.Value);
            }

            if (req.endTime.HasValue)
            {
                query = query.Where(t => t.OccurTime <= req.endTime.Value);
            }

            if (!req.keyword.IsBlank())
            {
                query = query.Where(a => a.Device.DeviceName.Contains(req.keyword));
            }

            query = query.OrderByDescending(o => o.CreateTime);
            var pagedList = query.ToPagedList(req.page, req.pageSize);

            var alarmList = new List<DeviceAlarmDto>();
            foreach (var item in pagedList)
            {
                var dto = item.MapTo<DeviceAlarmDto>();
                dto.DeviceName = item.Device != null ? item.Device.DeviceName : "";
                dto.AlarmTypeName = item.AlarmTypeDict.DictName;
                var community = _areaService.GetById(item.CommunityUUID);
                dto.CommunityName = community!=null? community.ChineseName:"";
                alarmList.Add(dto);
            }

            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = alarmList
            };
        }

        public void Create(DeviceAlarmDto config)
        {
            var entity = config.MapTo<DeviceAlarmEntity>();
            entity.AlarmUUID = Guid.NewGuid().ToString("N");
            entity.CreateTime = DateTime.Now;
            entity.Deleted = false;
            entity.SyncStatus = false;
            entity.SyncVersion = 0;
            _alarmRepository.Insert(entity);

            Synchronization(entity);
        }

        public void Update(DeviceAlarmDto config)
        {
            var entity = _alarmRepository.GetById(config.AlarmUUID);
            entity = config.ToEntity(entity);
            entity.UpdateTime = DateTime.Now;
            entity.SyncStatus = false;           
            _alarmRepository.Update(entity);

            Synchronization(entity);
        }

        public DeviceAlarmDto GetById(string configId)
        {
            var entity = _alarmRepository.GetById(configId);
            if (entity != null)
            {
                var dto = entity.MapTo<DeviceAlarmDto>();
                dto.AlarmTypeName = entity.AlarmTypeDict?.DictName ?? string.Empty;
                dto.DeviceName = entity.Device?.DeviceName ?? string.Empty;
                return dto;
            }
            else
                return null;
        }


        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { ",", ";", "，", "；" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var item in idList1)
            {
                var entity = _alarmRepository.GetById(item);
                entity.Deleted = true;
                entity.SyncStatus = false;
                _alarmRepository.Delete(entity);

                Synchronization(entity);
            }
        }

        private void Synchronization(DeviceAlarmEntity entity)
        {
            if (Constant.Sysc)
            {
               

                try
                {
                    //同步报警信息的时候需要根据设备ID找到对应的门，这里实际上是门作为设备来处理的
                    var door = _doorService.GetById(entity.DeviceUUID);
                    var requestXml = GetXml(entity, door);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.DeviceAlarmServiceImplService();
                    var responseXml = areaS.insertDeviceAlarm(requestXml);

                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertDeviceAlarm.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = entity.CommunityUUID;

                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            var entity2 = _alarmRepository.GetById(entity.AlarmUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _alarmRepository.Update(entity2);
                            syncLog.SyncResult = 1;
                        }
                    }
                    _syncService.InsertSyncLog(syncLog);

                }
                catch (Exception ex)
                {
                    Log.Error(null, ex.Message);
                }             
            }
        }

        private string GetXml(DeviceAlarmEntity entity,DoorDto door)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";

            var AlarmTypeDict = _dictService.GetById(entity.AlarmType);

            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<deviceAlarm>");
            xmlBuilder.AppendFormat("<uuid>{0}</uuid>", entity.AlarmUUID);
            xmlBuilder.AppendFormat("<occur_date>{0}</occur_date>", entity.OccurTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<type>{0}</type>", AlarmTypeDict.DictCode);
            xmlBuilder.AppendFormat("<status>{0}</status>", entity.Status);
            xmlBuilder.AppendFormat("<udate>{0}</udate>", entity.UpdateTime.HasValue?entity.UpdateTime.Value.ToString("yyyyMMddHHmmss"):"");
            xmlBuilder.AppendFormat("<mac>{0}</mac>", "");
            xmlBuilder.AppendFormat("<area_uuid>{0}</area_uuid>", door.AreaUUID);
            xmlBuilder.AppendFormat("<account_uuid>{0}</account_uuid>", Constant.LkbAccount);           
            xmlBuilder.AppendFormat("<device_uuid>{0}</device_uuid>", entity.DeviceUUID); //存的时候是不是存门ID
            xmlBuilder.AppendFormat("<remark>{0}</remark>", entity.Remark);
            xmlBuilder.AppendFormat("<cardno>{0}</cardno>", string.Format("0000{0}",entity.CardNo));
            xmlBuilder.AppendFormat("<dev_date>{0}</dev_date>", "");
            xmlBuilder.AppendFormat("<ctrl_mac>{0}</ctrl_mac>", "");          
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</deviceAlarm>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }

        public void GetSyncAlarmList()
        {
            var query = _alarmRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                Synchronization(p);
            });

        }
    }
}
