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
    public interface IDoorService
    {
        List<DoorListDto>  GetListByDoorIds(List<string> idsList);
        TableData GetList(QueryListReq req);
        IList<DoorSelectListDto> GetList(string communityId);
        DoorDto GetById(string doorId);
        void Create(DoorDto config);
        void Update(DoorDto config);
        void Delete(string ids);
        IList<DoorListDto> GetDoorListBy(string areaUUID, int floor=0, string communityId=null);
        DoorDto GetDoorByDevice(string deviceId);
        DoorDto GetDoorDevice(string doorId);
        void GetSyncDoorList();
    }

    public class DoorService : IDoorService, IDependency
    {
        private readonly IRepository<DoorEntity> _doorRepository;
        private readonly IRepository<AreaEntity> _areaRepository;
        private readonly IRepository<DeviceEntity> _deviceRepository;
        private readonly IDictService _dictService;
        private readonly IRepository<CardAuthEntity> _cardAuthRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISyncLogServie _syncService;


        public DoorService(IRepository<DoorEntity> configRepository, IRepository<AreaEntity> areaRepository,
            IRepository<DeviceEntity> deviceRepository, IDictService dictService,
            IRepository<CardAuthEntity> cardAuthRepository,
             ISyncLogServie syncService,
            IUnitOfWork unitOfWork)
        {
            _doorRepository = configRepository;
            _areaRepository = areaRepository;
            _deviceRepository = deviceRepository;
            _dictService = dictService;
            _cardAuthRepository = cardAuthRepository;
            _unitOfWork = unitOfWork;
            _syncService = syncService;
        }


        public List<DoorListDto> GetListByDoorIds(List<string> idsList)
        {
            if (idsList == null||idsList.Count<1)
            {
                return null;
            }
            var query = _doorRepository.Table.Where(t=>idsList.Contains(t.DoorUUID)).ToList();
            if (query == null || query.Count < 1)
            {
                return null;
            }
            var doorList = new List<DoorListDto>();
            foreach (var item in query.ToList())
            {
                var device = item.Device;
                var model = new DoorListDto();
                model.DoorUUID = item.DoorUUID;
                model.DoorName = item.DoorName;
                model.Floor = item.Floor;
                model.DoorNo = item.DoorNo;
                model.Status = item.Status;
                model.DeviceIP = device.IPAddress;
                model.DevicePort = device.Port;
                model.DeviceSN = device.SNNumber;
                model.CardConvertType = device.CardConvertType;
                model.DeviceType = device.DeviceType.HasValue ? device.DeviceType.Value : 0;
                model.AreaUUID = item.AreaUUID;
                doorList.Add(model);
            }
            return doorList;
        }


        public TableData GetList(QueryListReq req)
        {
            var query = _doorRepository.Table;

            if (!req.communityId.IsBlank())
            {
                query = query.Where(a => a.CommunityUUID == req.communityId);
            }

            if (!req.keyword.IsBlank())
            {
                query = query.Where(a => a.DoorName.Contains(req.keyword));
            }

            query = query.OrderByDescending(o => o.CreateTime);
            var pagedList = query.ToPagedList(req.page, req.pageSize);

            var doorList = new List<DoorDto>();
            foreach (var item in pagedList)
            {
                var dto = item.MapTo<DoorDto>();
                dto.DeviceName = item.Device != null ? item.Device.DeviceName : "";
                dto.DoorTypeName = item.DoorTypeDict.DictName;
                dto.DeviceType = item.Device != null ? item.Device.DeviceType : 0;  
                dto.DeviceIP = item.Device.IPAddress;
                var community = _areaRepository.GetById(item.CommunityUUID);
                dto.CommunityName = community != null ? community.ChineseName : "";
                doorList.Add(dto);
            }
            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = doorList
            };
        }

        public IList<DoorSelectListDto> GetList(string communityId)
        {
            var query = _doorRepository.Table.Where(a => a.CommunityUUID == communityId);
            query = query.OrderByDescending(o => o.CreateTime);
            var doorList = new List<DoorSelectListDto>();
            foreach (var item in query.ToList())
            {
                var dto = new DoorSelectListDto();
                dto.DoorID = item.DoorUUID;
                dto.DoorName = item.DoorName;
                doorList.Add(dto);
            }
            return doorList;
        }

        public DoorDto GetById(string configId)
        {
            var entity = _doorRepository.GetById(configId);
            if (entity != null)
            {
                var dto = entity.MapTo<DoorDto>();
                dto.DeviceType = entity.Device?.DeviceType ?? 0;
                dto.DoorTypeName = entity.DoorTypeDict?.DictName ?? string.Empty;
                dto.AreaName = entity.Area?.ChineseName ?? string.Empty;
                return dto;
            }
            else
                return null;
        }

        public DoorDto GetDoorDevice(string configId)
        {
            var entity = _doorRepository.GetById(configId);
            if (entity != null)
            {
                var device = _deviceRepository.GetById(entity.DeviceUUID);
                if (device == null)
                {
                    return null;
                }
                var dto = entity.MapTo<DoorDto>();
                dto.DeviceType = device.DeviceType ?? 0;
                dto.DoorTypeName = entity.DoorTypeDict?.DictName ?? string.Empty;
                dto.AreaName = entity.Area?.ChineseName ?? string.Empty;
                dto.DeviceIP = device.IPAddress;
                dto.DeviceSN = device.SNNumber;
                dto.DevicePort = device.Port;

                return dto;
            }
            else
                return null;
        }

        public DoorDto GetDoorByDevice(string deviceId)
        {
            var query = from c in _doorRepository.Table
                        where c.DeviceUUID == deviceId
                        orderby c.DoorNo
                        select c;
            var entity = query.FirstOrDefault();

            if (entity != null)
                return entity.MapTo<DoorDto>();
            else
                return null;
        }


        public void Create(DoorDto config)
        {
            var entity = config.MapTo<DoorEntity>();
            entity.DoorUUID = Guid.NewGuid().ToString("N");
            entity.CreateTime = DateTime.Now;
            entity.SyncStatus = false;
            entity.SyncVersion = 0;
            entity.Deleted = false;
            _doorRepository.Insert(entity);

            Synchronization(entity);
        }

        public void Update(DoorDto config)
        {
            var entity = _doorRepository.GetById(config.DoorUUID);
            entity = config.ToEntity(entity);
            entity.Deleted = false;
            entity.SyncStatus = false;
            _doorRepository.Update(entity);

            Synchronization(entity);
        }

        public void Delete(string ids)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var idList1 = ids.Trim(',').Split(new string[] { ",", ";", "，", "；" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var item in idList1)
                {
                   // _cardAuthRepository.Delete(t => t.DoorUUID == item);
                    var entity = _doorRepository.GetById(item);
                    entity.Deleted = true;
                    entity.SyncStatus = false;
                    _doorRepository.Delete(entity);

                    Synchronization(entity);
                }
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log.Error(null, ex.Message);
                _unitOfWork.Rollback();
            }
        }

        public IList<DoorListDto> GetDoorListBy(string areaUUID, int floor=0, string communityId=null)
        {
          

            var query = _doorRepository.Table.Where(a => a.Status==1);
            if (floor > 0)
            {
                query = query.Where(a => a.Floor == floor);
            }
            if (!string.IsNullOrWhiteSpace(areaUUID))
            {
                query = query.Where(a => a.AreaUUID == areaUUID);
            }
            if (!communityId.IsBlank())
            {
                query = query.Where(a => a.CommunityUUID == communityId);
            }

            var doorList = new List<DoorListDto>();
            foreach (var item in query.ToList())
            {
                var device = item.Device;
                var model = new DoorListDto();
                model.DoorUUID = item.DoorUUID;
                model.DoorName = item.DoorName;
                model.Floor = item.Floor;
                model.DoorNo = item.DoorNo;
                model.Status=item.Status;
                model.DeviceIP = device.IPAddress;
                model.DevicePort = device.Port;
                model.DeviceSN = device.SNNumber;
                model.CardConvertType = device.CardConvertType;
                model.DeviceType = device.DeviceType.HasValue ? device.DeviceType.Value : 0;
                model.AreaUUID = item.AreaUUID;
                model.DoorTypeName = item.DoorTypeDict.DictName;
                doorList.Add(model);
            }
            return doorList;
        }

        //门作为设备来处理
        public void Synchronization(DoorEntity entity)
        {
            if (Constant.Sysc)
            {             

                try
                {
                    var device = _deviceRepository.GetById(entity.DeviceUUID);
                    if (device == null)
                        return;
                    var requestXml = GetXml(entity, device);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.DeviceServiceImplService();
                    var responseXml = areaS.insertDevice(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertDevice.ToString();
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
                            var entity2 = _doorRepository.GetById(entity.DoorUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _doorRepository.Update(entity2);

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



        private string GetXml(DoorEntity door, DeviceEntity device)
        {
            var flag = door.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (door.Deleted)
                flag = "D";
            var area = _areaRepository.GetById(door.AreaUUID);
            var DeviceTypeDict = device.DeviceType.HasValue ? _dictService.GetById(device.DeviceType.Value) : null;
            var LockTypeDict = device.LockType.HasValue ? _dictService.GetById(device.LockType.Value) : null;

            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<device>");
            xmlBuilder.AppendFormat("<uuid>{0}</uuid>", door.DoorUUID);
            xmlBuilder.AppendFormat("<name>{0}</name>", device.DeviceName);
            xmlBuilder.AppendFormat("<mac>{0}</mac>", device.Mac);
            xmlBuilder.AppendFormat("<type>{0}</type>", DeviceTypeDict != null ? DeviceTypeDict.DictCode : "");  //设备类型 必传
            xmlBuilder.AppendFormat("<gw_uuid>{0}</gw_uuid>", ""); //所属控制器（网关）的uuid
            xmlBuilder.AppendFormat("<status>{0}</status>", door.Status==1?1:2); //文档中未提及
            xmlBuilder.AppendFormat("<area_uuid>{0}</area_uuid>", area.AreaUUID); //证件号码(需要加密) 必填
            xmlBuilder.AppendFormat("<area_layercode>{0}</area_layercode>", area.AreaCode); //房屋区域层级编码 必填
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", door.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<udate>{0}</udate>", door.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<inner_key>{0}</inner_key>", device.InnerKey);
            xmlBuilder.AppendFormat("<cuser>{0}</cuser>", Constant.LkbAccount);
            xmlBuilder.AppendFormat("<locktype>{0}</locktype>", LockTypeDict != null ? LockTypeDict.DictCode : "");
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</device>");
            xmlBuilder.Append("</ReqBody>");
            xmlBuilder.Append("</Tpp2Fpp>");
            return xmlBuilder.ToString();
        }

        public void GetSyncDoorList()
        {
            var query = _doorRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                Synchronization(p);
            });
        }
    }
}
