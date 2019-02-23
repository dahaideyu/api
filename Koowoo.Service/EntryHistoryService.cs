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
using Koowoo.Services.LkbResponse;
using System.Text;
using Koowoo.Pojo.Enum;
using Newtonsoft.Json;

namespace Koowoo.Services
{
    public interface IEntryHistoryService
    {
        void Create(EntryHistoryDto model);

        TableData GetList(EntryListReq req);

        TableData GetListByPerson(EntryListReq req);

        void GetSyncHistroyList();
    }

    public class EntryHistoryService : IEntryHistoryService, IDependency
    {
        private readonly IRepository<EntryHistoryEntity> _entryRepository;
        private readonly IRepository<AreaEntity> _areaRepository;
        private readonly IRepository<CardNoConvertEntity> _cardNoConvertRepository;
        private readonly IRepository<RoomUserCardEntity> _roomUserCardRepository;
        private readonly IRepository<PersonEntity> _personRepository;
        private readonly IRepository<RoomUserEntity> _roomUserRepository;
        private readonly IRepository<DeviceEntity> _deviceRepository;
        private readonly IRepository<DoorEntity> _doorRepository;
        private readonly IRepository<CardEntity> _cardRepository;
        private readonly IRepository<ManageCardEntity> _manageCardRepository;
        private readonly ISyncLogServie _syncService;


        public EntryHistoryService(ISyncLogServie syncService
            , IRepository<EntryHistoryEntity> entryRepository
            , IRepository<CardNoConvertEntity> cardNoConvertRepository
            , IRepository<RoomUserCardEntity> roomUserCardRepository
            , IRepository<PersonEntity> personRepository
            , IRepository<RoomUserEntity> roomUserRepository
            , IRepository<DeviceEntity> deviceRepository
            , IRepository<DoorEntity> doorRepository
            , IRepository<ManageCardEntity> manageCardRepository
            , IRepository<CardEntity> cardRepository
            , IRepository<AreaEntity> areaRepository)
        {
            _entryRepository = entryRepository;
            _areaRepository = areaRepository;
            _cardNoConvertRepository = cardNoConvertRepository;
            _roomUserCardRepository = roomUserCardRepository;
            _personRepository = personRepository;
            _roomUserRepository = roomUserRepository;
            _deviceRepository = deviceRepository;
            _doorRepository = doorRepository;
            _cardRepository = cardRepository;
            _syncService = syncService;
            _manageCardRepository = manageCardRepository;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public void Create(EntryHistoryDto model)
        {
            Log.Debug("insert entry start:" );
            var entity = model.MapTo<EntryHistoryEntity>();
            Log.Debug("insert entry start entity:" );
            var cardNoConvertEntity = _cardNoConvertRepository.Table.FirstOrDefault(t => t.CardConvertNo == model.CardNo);
            if (cardNoConvertEntity != null)
            {
                entity.CardUUID = cardNoConvertEntity.CardUUID;
                var roomUserCardEntity = _roomUserCardRepository.Table.FirstOrDefault(t => t.CardUUID == entity.CardUUID);
                if (roomUserCardEntity != null)
                {
                    var roomUserEntity = _roomUserRepository.Table.FirstOrDefault(t => t.RoomUserUUID == roomUserCardEntity.RoomUserUUID);
                    if (roomUserEntity != null)
                    {
                        entity.PersonUUID = roomUserEntity.PersonUUID;
                    }
                }
                if (string.IsNullOrWhiteSpace(entity.PersonUUID))
                {
                    var mEntity = _manageCardRepository.Table.FirstOrDefault(t=>t.CardUUID== entity.CardUUID);
                    if (mEntity != null)
                    {
                        entity.PersonUUID = mEntity.PersonUUID;
                    }
                }
            }
            else
            {
                return;
            }
            Log.Debug("insert entry start deviceEntity:" );
            var deviceEntity = _deviceRepository.Table.FirstOrDefault(t => t.SNNumber == model.DeviceSN);

            if (deviceEntity != null)
            {
                if (model.DoorNo != 0)
                {
                    var doorEntity = _doorRepository.Table.FirstOrDefault(t => t.DeviceUUID == deviceEntity.DeviceUUID && t.DoorNo == model.DoorNo.ToString());
                    if (doorEntity != null)
                    {
                        entity.DoorUUID = doorEntity.DoorUUID;
                    }
                }
                else
                {
                    var doorEntity = _doorRepository.Table.FirstOrDefault(t => t.DeviceUUID == deviceEntity.DeviceUUID);
                    if (doorEntity != null)
                    {
                        entity.DoorUUID = doorEntity.DoorUUID;
                    }
                }
            }
            entity.OperationType = 243;
            entity.EntryUUID = Guid.NewGuid().ToString("N");
            entity.Deleted = false;
            entity.CreateTime = DateTime.Now;
            entity.SyncStatus = false;
            entity.SyncVersion = 0;
            Log.Debug("insert entry start insert entity");
            _entryRepository.Insert(entity);
            Log.Debug("insert entry:" + entity);
            Synchronization(entity);

        }


        public TableData GetList(EntryListReq req)
        {
            var query = from e in _entryRepository.Table
                        join uc in _personRepository.Table on e.PersonUUID equals uc.PersonUUID
                        join c in _cardRepository.Table on e.CardUUID equals c.CardUUID
                        join d in _doorRepository.Table on e.DoorUUID equals d.DoorUUID
                        where !uc.Deleted
                        && (string.IsNullOrEmpty(req.communityId) ? true : uc.CommunityUUID == req.communityId)
                        && (req.StartTime.HasValue ? e.EntryTime >= req.StartTime.Value:true)
                        && (req.EndTime.HasValue ? e.EntryTime <= req.EndTime.Value: true)
                         && (string.IsNullOrEmpty(req.PersonName) ? true : uc.PersonName.Contains(req.PersonName))
                         && (string.IsNullOrEmpty(req.personId) ? true : uc.PersonUUID.Equals(req.personId))
                         && (string.IsNullOrEmpty(req.CardNo) ? true : uc.IDCardNo.Contains(req.CardNo))
                         && (string.IsNullOrEmpty(req.DoorUUID) ? true : e.DoorUUID.Equals(req.DoorUUID))

                        orderby e.CreateTime descending
                        select new EntryHistoryDto
                        {
                            EntryUUID = e.EntryUUID,
                            PersonUUID = e.PersonUUID,
                            Sex = uc.Sex,
                            PersonPic = "",
                            PersonName = uc.PersonName,
                            PersonIDCardNo = uc.IDCardNo,
                            DoorName = d.DoorName,
                            EntryTime = e.EntryTime,
                            CardNo = c.CardNo,
                            Photo = e.Photo
                        };                     

            var pagedList = query.ToPagedList(req.page, req.pageSize);

            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = pagedList
            };
        }


        /// <summary>
        /// 根据用户ID获取出入进路
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public TableData GetListByPerson(EntryListReq req)
        {
            var query = _entryRepository.Table.Where(a => !a.Deleted);

            if (!req.personId.IsBlank())
            {
                query = query.Where(a => a.PersonUUID == req.personId);
            }

            //if (!req.communityId.IsBlank())
            //{
            //    query = query.Where(a => a.CommunityUUID == req.communityId);
            //}

            query = query.OrderByDescending(o => o.CreateTime);
            var pagedList = query.ToPagedList(req.page, req.pageSize);

            var historyList = new List<EntryHistoryDto>();

            foreach (var item in pagedList)
            {
                var dto = item.MapTo<EntryHistoryDto>();
                dto.EntryUUID = item.EntryUUID;
                var person = item.Person;
                if (person != null)
                {
                    dto.PersonName = person.PersonName;
                    dto.PersonPic = "";
                }
                var door = item.Door;
                if (door != null)
                {
                    dto.DoorName = door.DoorName;
                    dto.DoorName = door.DoorTypeDict != null ? door.DoorTypeDict.DictName : "";
                }
                dto.OperationType = item.OperationType.ToString();
                dto.EntryTime = item.EntryTime;
                historyList.Add(dto);
            }

            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = historyList
            };
        }

        private void Synchronization(EntryHistoryEntity entity)
        {
            if (Constant.Sysc)
            {
                try
                {
                    var requestXml = GetXml(entity);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.CrjlServiceImplService();
                    var responseXml = areaS.insertCrjl(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertCrjl.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = "";

                    var resultRes = responseXml.Deserial<ResultResponse>();
                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            var entity2 = _entryRepository.GetById(entity.EntryUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _entryRepository.Update(entity2);
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

        private string GetXml(EntryHistoryEntity entity)
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
            xmlBuilder.Append("<crjl>");
            xmlBuilder.AppendFormat("<cardno>{0}</cardno>", "0000" + entity.Card.CardNo);
            xmlBuilder.AppendFormat("<person_uuid>{0}</person_uuid>", entity.Person.PersonUUID);
            xmlBuilder.AppendFormat("<mac>{0}</mac>", entity.Door.Device.Mac);
            xmlBuilder.AppendFormat("<opentype>{0}</opentype>", "C");
            xmlBuilder.AppendFormat("<area_uuid>{0}</area_uuid>", entity.Door.AreaUUID);
            xmlBuilder.AppendFormat("<slide_date>{0}</slide_date>", entity.EntryTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<dev_uuid>{0}</dev_uuid>", entity.Door.DoorUUID);  //对应的门ID
            xmlBuilder.AppendFormat("<area_code>{0}</area_code>", entity.Door.Area.AreaCode);
            xmlBuilder.AppendFormat("<dev_date>{0}</dev_date>", "");
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</crjl>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }


        public void GetSyncHistroyList()
        {
            var query = _entryRepository.Table.Where(a => !a.SyncStatus && a.PersonUUID != null);
            query = query.OrderBy(a => a.CreateTime);

            var syncList = query.ToPagedList(1,100);
            var totalPage = syncList.TotalPageCount;
            for (var page=1;page<= totalPage; page++)
            {
                if (page > 1)
                {
                    syncList = query.ToPagedList(1, 100);
                }

                syncList.ForEach(p => {
                    Synchronization(p);
                });
            }
        }
    }
}
