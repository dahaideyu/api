using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Domain;
using Koowoo.Data.Interface;
using Koowoo.Pojo;
using Koowoo.Services.LkbResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koowoo.Services
{
    public interface IRoomUserService
    {
        RoomUserDto GetById(string roomUserId);
        RoomUserDto GetByPersonId(string personId);
        //  RoomUserDto GetByPersonIdAndRoomId(string personId, string roomId);
        IList<RoomUserListDto> GetPersonListByRoomId(string communityId);
        void AddOrUpdate(RoomUserDto model);
        void Delete(string ids);
        void GetSyncRoomUserList();
        IList<RoomUserEntity> GetListByLeaveTime();

        void UpdateLeaveStatus(RoomUserEntity roomUser);
        
    }

    public class RoomUserService : IRoomUserService, IDependency
    {
        private readonly IRepository<RoomUserEntity> _roomUserRepository;
        private readonly IRepository<PersonEntity> _personRepository;
        private readonly IRepository<RoomUserCardEntity> _userCardRepository;
        private readonly IRepository<CardEntity> _cardRepository;
        private readonly ISyncLogServie _syncService;

        private readonly IRoomService _roomService;
        private readonly IAreaService _areaService;

        public RoomUserService(IRepository<RoomUserEntity> roomUserRepository,
            IRoomService roomService,
            IRepository<CardEntity> cardRepository,
            ISyncLogServie syncService,
        IRepository<RoomUserCardEntity> userCardRepository,
            IRepository<PersonEntity> personRepository, IAreaService areaService)
        {
            _roomUserRepository = roomUserRepository;
            _personRepository = personRepository;
            _areaService = areaService;
            _roomService = roomService;
            _syncService = syncService;
            _cardRepository = cardRepository;
            _userCardRepository = userCardRepository;
        }

        /// <summary>
        /// 根据房间号获取入住人员列表
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public IList<RoomUserListDto> GetPersonListByRoomId(string roomId)
        {
            var query = _roomUserRepository.Table.Where(a => a.RoomUUID == roomId);

            query = query.Where(a => a.Status == 1);

            query = query.OrderByDescending(o => o.CreateDate);
            var pagedList = query.ToList();

            var roomList = new List<RoomUserListDto>();
            foreach (var item in pagedList)
            {
                var person = item.Person;
                var dto = new RoomUserListDto();
                if (person != null)
                {
                    dto.SexName = person.SexDict.DictName;
                    dto.PersonName = person.PersonName;
                    dto.IDCardNo = person.IDCardNo;
                    dto.PhoneNo = person.PhoneNo;
                }
                //public string FamilyRelationName { get; set; }
                //public string SexName { get; set; }
                //public string PersonName { get; set; }
                //public string IDCardNo { get; set; }
                dto.LiveDate = item.LiveDate;
                dto.LeaveTime = item.LeaveDate;
                dto.FamilyRelationName = item.FamilyRelationDict != null ? item.FamilyRelationDict.DictName : "";
                roomList.Add(dto);
            }

            return roomList;
        }

        /// <summary>
        /// 根据RoomUserID获取用户入住信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public RoomUserDto GetById(string RoomUserID)
        {
            var dto = _roomUserRepository.GetById(RoomUserID);
            if (dto != null)
                return dto.MapTo<RoomUserDto>();
            else
                return null;
        }

        /// <summary>
        /// 根据人员ID获取用户入住信息
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public RoomUserDto GetByPersonId(string personId)
        {
            var dto = _roomUserRepository.Table.Where(a => a.PersonUUID == personId).FirstOrDefault();
            if (dto != null)
                return dto.MapTo<RoomUserDto>();
            else
                return null;
        }

        //public RoomUserDto GetByPersonIdAndRoomId(string personId, string roomId)
        //{
        //    if (string.IsNullOrEmpty(personId))
        //        return null;

        //    if (string.IsNullOrEmpty(roomId))
        //        return null;

        //    var query = from p in _roomUserRepository.Table
        //                orderby p.CreateDate
        //                where !p.Deleted &&
        //                p.PersonUUID == personId && p.RoomUUID==roomId
        //                select p;
        //    var entity = query.FirstOrDefault();

        //    return entity.MapTo<RoomUserDto>();
        //}


        /// <summary>
        /// 添加用户入住信息
        /// </summary>
        /// <param name="config"></param>
        public void AddOrUpdate(RoomUserDto model)
        {
            RoomUserEntity entity;
            if (model.IsAdd)
            {
                entity = model.MapTo<RoomUserEntity>();
                entity.CreateDate = DateTime.Now;
                entity.Deleted = false;
                entity.SyncStatus = false;
                entity.SyncVersion = 0;
                _roomUserRepository.Insert(entity);
            }
            else
            {
                entity = _roomUserRepository.GetById(model.RoomUserUUID);
                entity.RoomUUID = model.RoomUUID;
                entity.LiveDate = model.LiveDate;
                entity.LeaveDate = model.LeaveDate;
                entity.FamilyRelation = model.FamilyRelation;
                entity.Status = model.Status;
                entity.SyncStatus = false;
                _roomUserRepository.Update(entity);
            }

            var person = _personRepository.GetById(entity.PersonUUID);
            person.IsLived = model.Status == 1 ? 1 : 2;
            _personRepository.Update(person);

            Synchronization(entity.RoomUserUUID);
        }

        public IList<RoomUserEntity> GetListByLeaveTime()
        {
            var nowTime = DateTime.Now;
            var query = _roomUserRepository.Table.Where(a => a.LeaveDate < nowTime && a.Status == 1);
            return query.ToList();
        }


        public void UpdateLeaveStatus(RoomUserEntity roomUser)
        {
            var entity = _roomUserRepository.GetById(roomUser.RoomUserUUID);
            entity.Status = 0;
            _roomUserRepository.Update(entity);

            var person = _personRepository.GetById(entity.PersonUUID);
            person.IsLived = 2;
            _personRepository.Update(person);

            Synchronization(entity.RoomUserUUID);
        }


        /// <summary>
        /// 删除用户入住信息
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p).ToList();
            foreach (var item in idList1)
            {
                var entity = _roomUserRepository.GetById(item);
                entity.SyncStatus = false;
                entity.Deleted = true;
                _roomUserRepository.Update(entity);

                Synchronization(entity.RoomUserUUID);
                //_roomUserRepository.Delete(a => a.RoomUUID == item);
            }
        }

        public void GetSyncRoomUserList()
        {
            var query = _roomUserRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p =>
            {
                Synchronization(p.RoomUserUUID);
            });
        }

        private void Synchronization(string roomUserId)
        {
            if (Constant.Sysc)
            {

                try
                {
                    var entity = _roomUserRepository.GetById(roomUserId);

                    var requestXml = GetXml(entity);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.PersonServiceImplService();
                    var responseXml = areaS.insertUserhouse(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = Pojo.Enum.SyncLogEnum.InsertUserhouse.ToString();
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
                            var entity2 = _roomUserRepository.GetById(entity.RoomUserUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _roomUserRepository.Update(entity2);

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

      


        private string GetXml(RoomUserEntity entity)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";

            var person = _personRepository.GetById(entity.PersonUUID);  //用户
            var room = _roomService.GetById(entity.RoomUUID);  //房间信息
            var area = _areaService.GetById(room.AreaUUID); // 区域信息

            //卡
            var cardQuery = from c in _cardRepository.Table
                            join uc in _userCardRepository.Table on c.CardUUID equals uc.CardUUID
                            where !uc.Deleted && uc.RoomUserUUID == entity.RoomUserUUID
                            orderby c.CreateTime descending
                            select c;

            var card = cardQuery.FirstOrDefault();

            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<userhouse>");
            xmlBuilder.AppendFormat("<userid>{0}</userid>", entity.RoomUserUUID);
            xmlBuilder.AppendFormat("<houseid>{0}</houseid>", room.AreaUUID);
            xmlBuilder.AppendFormat("<date>{0}</date>", entity.LiveDate.HasValue ? entity.LiveDate.Value.ToString("yyyyMMddHHmmss") : "");
            xmlBuilder.AppendFormat("<udate>{0}</udate>", entity.CreateDate.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<reltype>{0}</reltype>", entity.FamilyRelationDict?.DictCode ?? ""); //关联类型
            xmlBuilder.AppendFormat("<parm1>{0}</parm1>", ""); //持卡类型：1：主卡， 2：附属卡
            xmlBuilder.AppendFormat("<parm2>{0}</parm2>", "");
            xmlBuilder.AppendFormat("<edate>{0}</edate>", entity.LeaveDate.HasValue ? entity.LeaveDate.Value.ToString("yyyyMMddHHmmss") : "");
            xmlBuilder.AppendFormat("<state>{0}</state>", ""); //文档中未提及
            xmlBuilder.AppendFormat("<idcard>{0}</idcard>", DESHelper.Encrypt3Des(person.IDCardNo, Constant.DescKeyBytes)); //证件号码(需要加密) 必填
            xmlBuilder.AppendFormat("<arcode>{0}</arcode>", area.AreaCode); //房屋区域层级编码 必填
            xmlBuilder.AppendFormat("<crk_uuid>{0}</crk_uuid>", card.CardUUID);
            xmlBuilder.AppendFormat("<crk_crkno>{0}</crk_crkno>", card.CardNo);
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</userhouse>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }
    }
}
