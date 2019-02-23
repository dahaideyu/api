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
using Koowoo.Services.LkbResponse;
using Koowoo.Pojo.Enum;

namespace Koowoo.Services
{
    public interface IDeviceStatusService
    {
        DeviceStatusDto GetById(string deviceId);
        void Save(DeviceStatusDto dto);
        void GetSyncStatusList();
    }

    public class DeviceStatusService : IDeviceStatusService, IDependency
    {
        private readonly IRepository<DeviceStatusEntity> _statusRepository;

        private readonly ISyncLogServie _syncService;

        private readonly IDoorService _doorService;

        public DeviceStatusService(IRepository<DeviceStatusEntity> statusRepository, ISyncLogServie syncService, IDoorService doorService)
        {
            _statusRepository = statusRepository;
            _syncService = syncService;
            _doorService = doorService;
        }

        public DeviceStatusDto GetById(string configId)
        {
            var dto = _statusRepository.GetById(configId);
            if (dto != null)
                return dto.MapTo<DeviceStatusDto>();
            else
                return null;
        }

        public void Save(DeviceStatusDto config)
        {
            
            //
            var entity = _statusRepository.GetById(config.DeviceUUID);
            if (entity != null)
            {
                entity = config.ToEntity(entity);
                entity.UpdateTime = DateTime.Now;
                entity.SyncStatus = false;
                _statusRepository.Update(entity);
            }
            else
            {
                entity = config.ToEntity();
                entity.CreateTime = DateTime.Now;
                entity.UpdateTime = DateTime.Now;
                entity.SyncStatus = false;
                entity.SyncVersion = 0;
                _statusRepository.Insert(entity);
            }

            Synchronization(entity);
        }

        private void Synchronization(DeviceStatusEntity entity)
        {
            if (Constant.Sysc)
            {
                

                try
                {
                    //同步设备信息是实际上同步的是门，所以要根据设备ID 查找关联的门
                    var door = _doorService.GetDoorByDevice(entity.DeviceUUID);
                    if (door == null)
                        return;

                    var requestXml = GetXml(entity, door);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.DeviceStatusServiceImplService();
                    var responseXml = areaS.insertDeviceStatus(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();
                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertDeviceStatus.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = "";//entity.CommunityUUID;

                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            var entity2 = _statusRepository.GetById(entity.DeviceUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _statusRepository.Update(entity2);
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

        private string GetXml(DeviceStatusEntity entity,DoorDto door)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";            

            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<deviceStatus>");
            xmlBuilder.AppendFormat("<dev_uuid>{0}</dev_uuid>", entity.DeviceUUID);
            xmlBuilder.AppendFormat("<status>{0}</status>", entity.DeviceStatus);
            xmlBuilder.AppendFormat("<hwversion>{0}</hwversion>", entity.HardwardVersion);
            xmlBuilder.AppendFormat("<sfversion>{0}</sfversion>", entity.SoftwareVersion);
            xmlBuilder.AppendFormat("<imsi>{0}</imsi>", entity.IMSI);
            xmlBuilder.AppendFormat("<msisdn>{0}</msisdn>", entity.MISSDN);
            xmlBuilder.AppendFormat("<battery>{0}</battery>", entity.Battery);
            xmlBuilder.AppendFormat("<temperature>{0}</temperature>", entity.Temperature);
            xmlBuilder.AppendFormat("<signal>{0}</signal>", entity.Signal);
            xmlBuilder.AppendFormat("<udate>{0}</udate>", entity.UpdateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<cardPopedomCapacity>{0}</cardPopedomCapacity>", entity.CardCapacity);
            xmlBuilder.AppendFormat("<cardPopedomCount>{0}</cardPopedomCount>", entity.CardWhiteListCount);
            xmlBuilder.AppendFormat("<fingerCapacity>{0}</fingerCapacity>", entity.FingerCapacity);
            xmlBuilder.AppendFormat("<fingerCount>{0}</fingerCount>", entity.FingerCount);
            xmlBuilder.AppendFormat("<opened>{0}</opened>", entity.IsOPened);
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<cuser>{0}</cuser>", Constant.LkbAccount);
            xmlBuilder.AppendFormat("<workmode>{0}</workmode>", entity.WorkMode);
            xmlBuilder.AppendFormat("<powermode>{0}</powermode>", entity.PowerMode);
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</deviceStatus>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }

        public void GetSyncStatusList()
        {
            var query = _statusRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                Synchronization(p);
            });
        }
    }
}
