using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Data.Interface;
using Koowoo.Domain;
using Koowoo.Pojo;
using Koowoo.Pojo.Enum;
using Koowoo.Services.LkbResponse;
using Koowoo.Services.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koowoo.Services
{
    public interface ICardService
    {
        void UpdateList(List<CardDto> models);
        List<CardDto> GetCardByIds(List<string> cardIds);
        IList<CardListDto> GetManageCardListByPerson(string personId);
        IList<CardListDto> GetCardListByPerson(string personId);
        List<string> GetCardListByCardNo(string cardNo);
        CardDto GetCardById(string cardId);
        CardDto GetCardByCardNo(string cardNo);
        void Create(CardDto model);
        void Update(CardDto model);
        ResponseModel Delete(string cardId);
        RoomAuthDto RoomAuth(AuthDto dto);
        IList<DoorAuthDto> DoorAuth(AuthDto dto);

        void InsertCardNoConvert();
        //IList<CardListDto> GetCardListByRoomUserID(string roomUserId);

        void SynchronizationCard(string cardId);
        void SynchronizationPersonCard(string cardId);
        //根据卡号获取用户信息
        PersonEntity GetPersonByCardId(string cardId);
        bool SaveCardAuth(string cardUUID, List<CardAuthDto> authList, string authType);
        bool DeleteCardAuth(string cardUUID, string authType);


        void GetSyncCardList();
        void GetSyncPersonCardList();
        void GetSyncCardAuthList();
    }

    public class CardService : ICardService, IDependency
    {
        private readonly IRepository<CardEntity> _cardRepository;
        private readonly IRepository<CardNoConvertEntity> _convertRepository;
        private readonly IRepository<RoomUserEntity> _roomUserRepository;
        private readonly IRepository<RoomUserCardEntity> _userCardRepository;
        private readonly IRepository<PersonEntity> _personRepository;
        private readonly IRepository<CardAuthEntity> _cardAuthRepository;
        private readonly IRepository<RoomEntity> _roomRepository;
        private readonly IRepository<ManageCardEntity> _manageCardRepository;
        private readonly IDoorService _doorService;
        private readonly ISyncLogServie _syncLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAreaService _areaService;
        private readonly IDictService _dictService;

        public CardService(IRepository<CardEntity> cardRepository,
            IUnitOfWork unitOfWork,
            IRepository<RoomEntity> roomRepository,
            IRepository<RoomUserCardEntity> userCardRepository,
            IRepository<CardNoConvertEntity> convertRepository,
            IRepository<RoomUserEntity> roomUserRepository,
            IRepository<PersonEntity> personRepository,
            IRepository<CardAuthEntity> cardAuthRepository,
            ISyncLogServie syncLogService,
            IDoorService doorService,
            IDictService dictService,
        IRepository<ManageCardEntity> manageCardRepository,
             IAreaService areaService)
        {
            _unitOfWork = unitOfWork;
            _cardRepository = cardRepository;
            _userCardRepository = userCardRepository;
            _convertRepository = convertRepository;
            _roomRepository = roomRepository;
            _roomUserRepository = roomUserRepository;
            _personRepository = personRepository;
            _cardAuthRepository = cardAuthRepository;
            _manageCardRepository = manageCardRepository;
            _areaService = areaService;
            _doorService = doorService;
            _syncLogService = syncLogService;
            _dictService = dictService;
        }




        ///// <summary>
        ///// 根据个人ID持有卡列表
        ///// </summary>
        ///// <param name="personId"></param>
        ///// <returns></returns>
        public virtual IList<CardListDto> GetCardListByPerson(string personId)
        {
            var query = from c in _cardRepository.Table
                        join uc in _userCardRepository.Table on c.CardUUID equals uc.CardUUID
                        join ru in _roomUserRepository.Table on uc.RoomUserUUID equals ru.RoomUserUUID
                        join p in _personRepository.Table on ru.PersonUUID equals p.PersonUUID
                        where !c.Deleted && p.PersonUUID == personId
                        orderby c.CreateTime descending
                        select c;

            var cardList = new List<CardListDto>();

            foreach (var item in query.ToList())
            {
                var model = new CardListDto();
                model.CardUUID = item.CardUUID;
                model.CardNo = item.CardNo;
                model.CardType = item.CardType;
                model.CardTypeName = item.CardTypeDict != null ? item.CardTypeDict.DictName : "";
                model.CreateTime = DateTime.Now;
                cardList.Add(model);
            }

            return cardList;
        }
        ///// <summary>
        ///// 根据个人ID持有卡列表
        ///// </summary>
        ///// <param name="personId"></param>
        ///// <returns></returns>
        public virtual IList<CardListDto> GetManageCardListByPerson(string personId)
        {
            var query = from c in _cardRepository.Table
                        join p in _manageCardRepository.Table on c.CardUUID equals p.CardUUID
                        where !c.Deleted && p.PersonUUID == personId
                        orderby c.CreateTime descending
                        select c;

            var cardList = new List<CardListDto>();

            foreach (var item in query.ToList())
            {
                var model = new CardListDto();
                model.CardUUID = item.CardUUID;
                model.CardNo = item.CardNo;
                model.CardType = item.CardType;
                model.CardTypeName = item.CardTypeDict != null ? item.CardTypeDict.DictName : "";
                model.CreateTime = DateTime.Now;
                cardList.Add(model);
            }

            return cardList;
        }
        ///// <summary>
        ///// 根据个人ID持有卡列表
        ///// </summary>
        ///// <param name="personId"></param>
        ///// <returns></returns>
        public virtual List<string> GetCardListByCardNo(string cardNo)
        {
            var query = from ru in _roomUserRepository.Table
                        join uc in _userCardRepository.Table on ru.RoomUserUUID equals uc.RoomUserUUID
                        join c in _cardRepository.Table on uc.CardUUID equals c.CardUUID
                        where !c.Deleted && c.CardNo.Contains(cardNo)
                        orderby c.CreateTime descending
                        select ru;

            return query.Select(a => a.PersonUUID).ToList();

        }

        //public IList<CardListDto> GetCardListByRoomUserID(string roomUserId)
        //{
        //    var query = from c in _cardRepository.Table
        //                join uc in _userCardRepository.Table on c.CardUUID equals uc.CardUUID
        //                where uc.RoomUserUUID == roomUserId
        //                orderby c.CreateTime descending
        //                select c;

        //    var cardList = new List<CardListDto>();

        //    foreach (var item in query.ToList())
        //    {
        //        var model = new CardListDto();
        //        model.CardUUID = item.CardUUID;
        //        model.CardNo = item.CardNo;
        //        model.CardType = item.CardType;
        //        model.CardTypeName = item.CardTypeDict != null ? item.CardTypeDict.DictName : "";
        //        model.CreateTime = DateTime.Now;
        //        cardList.Add(model);
        //    }

        //    return cardList;
        //}


        /// <summary>
        /// 办卡，存入卡信息，生成用户和卡的关联
        /// </summary>
        /// <param name="model"></param>
        public virtual void Create(CardDto model)
        {
            var result = false;
            try
            {
                _unitOfWork.BeginTransaction();

                var entity = model.MapTo<CardEntity>();
                entity.CreateTime = DateTime.Now;
                entity.UpdateTime = entity.CreateTime;
                entity.CardLast4NO = entity.CardNo.Substring(entity.CardNo.Length - 4, 4);
                entity.Deleted = false;
                entity.SyncStatus = false;
                entity.SyncVersion = 0;
                _cardRepository.Insert(entity);


                var convertList = new List<CardNoConvertEntity>();

                foreach (var type in Constant.ConvertTypeList)
                {
                    var convertDto = new CardNoConvertEntity
                    {
                        CardNOConverUUID = Guid.NewGuid().ToString("N"),
                        CardUUID = model.CardUUID,
                        CardConvertType = type,
                        CardConvertNo = CardNoConvert.ConvertCardNo(model.CardNo, type, model.CardType)
                    };
                    convertList.Add(convertDto);
                }             


                if (convertList != null && convertList.Count > 0)
                    _convertRepository.Insert(convertList);

                RoomUserCardEntity userCardEntity = null;

                if (model.IsManage)
                {
                    var manageCard = new ManageCardEntity()
                    {
                        Guid = Guid.NewGuid().ToString("N"),
                        PersonUUID = model.PersonUUID,
                        CardUUID = entity.CardUUID
                    };
                    _manageCardRepository.Insert(manageCard);

                }
                else
                {
                    userCardEntity = new RoomUserCardEntity();
                    userCardEntity.RoomUserCardUUID = Guid.NewGuid().ToString("N");
                    userCardEntity.RoomUserUUID = model.RoomUserUUID;
                    userCardEntity.CardUUID = entity.CardUUID;
                    userCardEntity.CreateTime = entity.CreateTime;
                    userCardEntity.UpdateTime = entity.UpdateTime;
                    userCardEntity.Deleted = false;
                    userCardEntity.SyncStatus = false;
                    userCardEntity.SyncVersion = 0;
                    _userCardRepository.Insert(userCardEntity);
                }


                _unitOfWork.Commit();

                result = true;


                if (result)
                {
                    SynchronizationCard(entity.CardUUID);
                    SynchronizationPersonCard(entity.CardUUID);
                }

            }
            catch (Exception ex)
            {
                Log.Error(null, ex.Message);
                _unitOfWork.Rollback();
            }
        }

        public virtual void Update(CardDto model)
        {
            var entity = _cardRepository.GetById(model.CardUUID);
            entity.ValidFrom = model.ValidFrom;
            entity.ValidTo = model.ValidTo;
            entity.UpdateTime = DateTime.Now;
            entity.Deleted = false;
            entity.SyncStatus = false;
            _cardRepository.Update(entity);

            SynchronizationCard(entity.CardUUID);
            SynchronizationPersonCard(entity.CardUUID);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models"></param>
        public virtual void UpdateList(List<CardDto> models)
        {
            List<CardEntity> entitys = new List<CardEntity>();
            foreach(var item in models)
            {
                var entity = _cardRepository.GetById(item.CardUUID);
                entity.ValidFrom = item.ValidFrom;
                entity.ValidTo = item.ValidTo;
                entity.UpdateTime = DateTime.Now;
                entity.Deleted = false;
                entity.SyncStatus = false;

                entitys.Add(entity);
                SynchronizationCard(entity.CardUUID);
                SynchronizationPersonCard(entity.CardUUID);
            }
            _cardRepository.Update(entitys);

        }
        /// <summary>
        /// 根据carduuidList获取对象
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public virtual List<CardDto> GetCardByIds(List<string> cardIds)
        {
            if (cardIds == null||cardIds.Count<1)
            {
                return null;
            }
            var entitys = _cardRepository.Table.Where(t=> cardIds.Contains(t.CardUUID)).ToList();
            if (entitys != null)
                return entitys.MapToList<CardDto>();
            else
                return null;
        }
        /// <summary>
        /// 根据carduuid获取对象
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public virtual CardDto GetCardById(string cardId)
        {
            var entity = _cardRepository.GetById(cardId);
            if (entity != null)
                return entity.MapTo<CardDto>();
            else
                return null;
        }

        public virtual PersonEntity GetPersonByCardId(string cardId)
        {
            var query = from p in _personRepository.Table
                        join ru in _roomUserRepository.Table on p.PersonUUID equals ru.PersonUUID
                        join uc in _userCardRepository.Table on ru.RoomUserUUID equals uc.RoomUserUUID
                        where uc.CardUUID == cardId && !uc.Deleted && !ru.Deleted && !p.Deleted
                        select p;

            return query.FirstOrDefault();

        }

        /// <summary>
        /// 根据卡号获取对象
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public virtual CardDto GetCardByCardNo(string cardNo)
        {
            var query = from c in _cardRepository.Table
                        orderby c.CreateTime
                        where c.CardNo == cardNo
                        select c;

            var entity = query.FirstOrDefault();



            if (entity != null)
                return entity.MapTo<CardDto>();
            else
                return null;
        }

        public virtual void SynchronizationCard(string cardId)
        {

            if (Constant.Sysc)
            {
                try
                {
                    var entity = _cardRepository.GetById(cardId);


                    var person = GetPersonByCardId(cardId);
                    if (entity == null || person == null)
                    {
                        return;
                    }

                    var requestXml = GetCardXml(entity, person);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    if (requestXml.IsBlank())
                    {
                        return;
                    }


                    var areaS = new Lkb.CrkServiceImplService();
                    var responseXml = areaS.insertCrk(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertCrk.ToString();
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
                            entity.SyncVersion += 1;
                            entity.SyncStatus = true;
                            _cardRepository.Update(entity);

                            syncLog.SyncResult = 1;
                        }
                    }

                    _syncLogService.InsertSyncLog(syncLog);
                }
                catch (Exception ex)
                {
                    Log.Error(null, ex.Message);
                }
            }

        }

        public virtual void SynchronizationPersonCard(string userCardId)
        {
            if (Constant.Sysc)
            {

                try
                {
                    var entity = _userCardRepository.GetEntity(a=>a.CardUUID==userCardId);

                    if (entity == null)
                    {
                        return;
                    }

                    if (entity.Deleted && entity.SyncVersion == 0)
                    {
                        return;
                    }
                    var person = GetPersonByCardId(entity.CardUUID);

                    if (person == null)
                    {
                        return;
                    }
                    var requestXml = GetRoomUserCardXml(entity, person);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    if (requestXml.IsBlank())
                    {
                        return;
                    }

                    var areaS = new Lkb.PersonServiceImplService();
                    var responseXml = areaS.insertPersonCard(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertPersonCard.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = person.CommunityUUID;

                    var resultRes = responseXml.Deserial<ResultResponse>();
                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            entity.SyncVersion += 1;
                            entity.SyncStatus = true;
                            _userCardRepository.Update(entity);
                            syncLog.SyncResult = 1;
                        }
                    }
                    _syncLogService.InsertSyncLog(syncLog);
                }
                catch (Exception ex)
                {
                    Log.Error(null, ex.Message);
                }
            }

        }

        public virtual void SynchronizationCardAuth(string cardId, string authType)
        {

            if (Constant.Sysc)
            {
                var cardAuthList = _cardAuthRepository.Table.Where(a => a.CardUUID == cardId && a.AuthType == authType && !a.SyncStatus);
                foreach (var item in cardAuthList)
                {
                    SynchronizationCardAuthById(item.CardAuthUUID);
                }
            }

        }

        private string GetCardXml(CardEntity entity, PersonEntity person)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";

            //根据卡号需要找到房间ID  得到AreaCode及LayerCode
            var query = from r in _roomRepository.Table
                        join ru in _roomUserRepository.Table on r.RoomUUID equals ru.RoomUUID
                        join uc in _userCardRepository.Table on ru.RoomUserUUID equals uc.RoomUserUUID
                        where uc.CardUUID == entity.CardUUID
                        orderby uc.CreateTime descending
                        select new {
                            r.AreaUUID,
                            ru.FamilyRelation
                        };

            var room = query.FirstOrDefault();
            if (room == null)
                return string.Empty;

            var familyRelationDict = _dictService.GetById(room.FamilyRelation.Value);

            var area = _areaService.GetById(room.AreaUUID);

            var type1 = "2";
            if (person.PersonType == 272)
            {
                type1 = "1";
            }

            var type2 = familyRelationDict?.DictCode??"0";

            if (type2 == "10")
            {
                type2 = "9";
            }


            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<crk>");
            xmlBuilder.AppendFormat("<uuid>{0}</uuid>", entity.CardUUID);
            xmlBuilder.AppendFormat("<crkno>{0}</crkno>", entity.CardNo);
            xmlBuilder.AppendFormat("<idcode>{0}</idcode>", DESHelper.Encrypt3Des(person.IDCardNo+type1+type2, Constant.DescKeyBytes));  //身份信息编码( 参见杭州市规范 ) ，护照正常传入，不用编码(需要加密)
            xmlBuilder.AppendFormat("<addrcode>{0}</addrcode>", ""); //住址信息编码( 参见杭州市规范 )
            xmlBuilder.AppendFormat("<udate>{0}</udate>", Constant.FormatDateTime(entity.UpdateTime));
            xmlBuilder.AppendFormat("<cuser>{0}</cuser>", Constant.LkbAccount);
            xmlBuilder.AppendFormat("<state>{0}</state>", "2"); //出入卡状态(0:初始化;1:操作中;2:正常;3:挂失;4:注销)
            xmlBuilder.AppendFormat("<stime>{0}</stime>", Constant.FormatDateTime(entity.ValidFrom));
            xmlBuilder.AppendFormat("<etime>{0}</etime>", Constant.FormatDateTime(entity.ValidTo));
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<key>{0}</key>", entity.AreaKey); //区域权限因子(二级) （定长为 4位）
            xmlBuilder.AppendFormat("<mac>{0}</mac>", ""); //
            xmlBuilder.AppendFormat("<layercode>{0}</layercode>", area.AreaCode);  //归属区域层级编码
            xmlBuilder.AppendFormat("<crkno4>{0}</crkno4>", entity.CardLast4NO);
            xmlBuilder.AppendFormat("<key1>{0}</key1>", entity.AreaKey1); //区域权限因子(一级) （定长为 4位）
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</crk>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }

        private string GetCardAuthXml(CardAuthEntity entity)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";

            var door = _doorService.GetById(entity.DoorUUID);


            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<crkqx>");
            xmlBuilder.AppendFormat("<crk_uuid>{0}</crk_uuid>", entity.CardUUID);
            xmlBuilder.AppendFormat("<dev_uuid>{0}</dev_uuid>", entity.DoorUUID);
            xmlBuilder.AppendFormat("<state>{0}</state>", "2"); //出入卡状态(0:初始化;1:操作中;2:正常;3:挂失;4:注销)
            xmlBuilder.AppendFormat("<stime>{0}</stime>", Constant.FormatDateTime(entity.ValidFrom));
            xmlBuilder.AppendFormat("<etime>{0}</etime>", Constant.FormatDateTime(entity.ValidTo));
            xmlBuilder.AppendFormat("<cuser>{0}</cuser>", Constant.LkbAccount);
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<udate>{0}</udate>", Constant.FormatDateTime(entity.UpdateTime));
            xmlBuilder.AppendFormat("<area_uuid>{0}</area_uuid>", door.AreaUUID); //区域权限因子(二级) （定长为 4位）
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</crkqx>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }

        private string GetRoomUserCardXml(RoomUserCardEntity entity, PersonEntity person)
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
            xmlBuilder.Append("<personCard>");
            xmlBuilder.AppendFormat("<person_uuid>{0}</person_uuid>", person.PersonUUID);
            xmlBuilder.AppendFormat("<crk_uuid>{0}</crk_uuid>", entity.CardUUID);
            xmlBuilder.AppendFormat("<cuser>{0}</cuser>", Constant.LkbAccount);
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<udate>{0}</udate>", entity.UpdateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<idcard>{0}</idcard>", DESHelper.Encrypt3Des(person.IDCardNo, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</personCard>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }

        public virtual void SynchronizationCardAuthById(string cardAuthId)
        {
            if (Constant.Sysc)
            {
                try
                {
                    var entity = _cardAuthRepository.GetById(cardAuthId);

                    if (entity == null)
                    {
                        return;
                    }

                    if (entity.Deleted && entity.SyncVersion == 0)
                    {
                        return;
                    }

                    if (entity.SyncStatus)
                    {
                        return;
                    }

                    var requestXml = GetCardAuthXml(entity);

                    if (requestXml.IsBlank())
                    {
                        return;
                    }

                    Log.Debug(this.GetType().ToString(), requestXml);
                    var areaS = new Lkb.CrkServiceImplService();
                    var responseXml = areaS.insertCrkqx(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertCrkqx.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = ""; // entity.CommunityUUID;

                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            entity.SyncVersion += 1;
                            entity.SyncStatus = true;
                            _cardAuthRepository.Update(entity);

                            syncLog.SyncResult = 1;
                        }
                    }

                    _syncLogService.InsertSyncLog(syncLog);
                }
                catch (Exception ex)
                {
                    Log.Error(null, ex.Message);
                }
            }

        }

        /// <summary>
        /// 删除卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public virtual ResponseModel Delete(string cardId)
        {
            var result = new ResponseModel();
            try
            {
                _unitOfWork.BeginTransaction();

                var card = _cardRepository.GetById(cardId);
                if (card != null)
                {
                    card.Deleted = true;
                    card.SyncStatus = false;
                    _cardRepository.Update(card);
                }

                var userCard = _userCardRepository.Table.Where(a => a.CardUUID == cardId).FirstOrDefault();
                if (userCard != null)
                {
                    userCard.Deleted = true;
                    userCard.SyncStatus = false;
                    _userCardRepository.Update(userCard);
                }


                // var authList = _cardAuthRepository.Table.Where(a => a.CardUUID == cardId);
                _cardAuthRepository.Delete(a => a.CardUUID == cardId);

                // var converList = _convertRepository.Table.Where(a => a.CardUUID == cardId);
                _convertRepository.Delete(a => a.CardUUID == cardId);

                //  IList<CardAuthEntity> authList = _cardAuthRepository.Table.Where(a => a.CardUUID == cardId).ToList();
                //  _userCardRepository.Delete(a => a.CardUUID == cardId);

                _unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                Log.Error(null, ex.Message);
                _unitOfWork.Rollback();
                result.code = 1;
                result.msg = "数据错误！";
            }

            if (result.code == 0)
            {
                SynchronizationCard(cardId);
                SynchronizationPersonCard(cardId);
            }
            return result;
        }

        /// <summary>
        /// 房间授权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual RoomAuthDto RoomAuth(AuthDto dto)
        {
            return null;
        }

        /// <summary>
        /// 楼道门授权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual IList<DoorAuthDto> DoorAuth(AuthDto dto)
        {
            var authList = new List<DoorAuthDto>();
            return authList;
        }

        public virtual bool SaveCardAuth(string cardUUID, List<CardAuthDto> authList, string authType)
        {
            bool result = false;
            try
            {
                _unitOfWork.BeginTransaction();

                var card = _cardRepository.GetById(cardUUID);
                // card.CardAuthList;         //  card.CardNoConvertList; //
                var oldAuthList = _cardAuthRepository.Table.Where(a => a.CardUUID == cardUUID && a.AuthType == authType && !a.Deleted).ToList();
                //var oldConvertList = _convertRepository.Table.Where(a => a.CardUUID == cardUUID).ToList();

                var authEntityList = new List<CardAuthEntity>();

                if (authList != null && authList.Count > 0)
                    authEntityList = authList.MapToList<CardAuthEntity>();

                //var convertEntityList = new List<CardNoConvertEntity>();
                //if (convertList != null && convertList.Count > 0)
                //    convertEntityList = convertList.MapToList<CardNoConvertEntity>();

                // 授权卡
                foreach (var item in oldAuthList)
                {
                    if (authEntityList.Where(a => a.DoorUUID == item.DoorUUID && a.AuthType == authType).Count() == 0)
                    {
                        var delAuth = _cardAuthRepository.GetEntity(a => a.DoorUUID == item.DoorUUID && a.CardUUID == item.CardUUID && a.AuthType == authType && !a.Deleted);
                        delAuth.SyncStatus = false;
                        delAuth.Deleted = true;
                        _cardAuthRepository.Update(delAuth);
                    }
                }


                //卡转码
                //foreach (var item in oldConvertList)
                //{
                //    if (convertEntityList.Where(a => a.CardConvertNo == item.CardConvertNo).Count() == 0)
                //    {
                //        var delAuth = _convertRepository.GetEntity(a => a.CardConvertNo == item.CardConvertNo && a.CardUUID == item.CardUUID);
                //        _convertRepository.Delete(delAuth);
                //    }
                //}


                if (oldAuthList != null && oldAuthList.Count() > 0)
                {
                    foreach (var item in authEntityList)
                    {
                        if (authType == "door")
                        {
                            var checkExist = oldAuthList.Where(a => a.AuthType == "Door" && a.DoorUUID == item.DoorUUID);
                            if (checkExist.Count() == 0)
                            {
                                _cardAuthRepository.Insert(item);
                            }
                            else
                            {
                                var auth = _cardAuthRepository.GetEntity(a => a.AuthType == "Door" && a.DoorUUID == item.DoorUUID && a.CardUUID == item.CardUUID && !a.Deleted);
                                auth.ValidFrom = item.ValidFrom;
                                auth.ValidTo = item.ValidTo;
                                auth.SyncStatus = false;

                                _cardAuthRepository.Update(auth);
                            }
                        }
                        else
                        {
                            var checkExist = oldAuthList.Where(a => a.AuthType == "Room" && !a.Deleted);
                            if (checkExist.Count() == 0)
                            {
                                _cardAuthRepository.Insert(item);
                            }
                            else
                            {
                                var auth = _cardAuthRepository.GetEntity(a => a.AuthType == "Room" && a.CardUUID == item.CardUUID && !a.Deleted);
                                auth.ValidFrom = item.ValidFrom;
                                auth.ValidTo = item.ValidTo;
                                auth.SyncStatus = false;
                                _cardAuthRepository.Update(auth);
                            }
                        }
                    }
                }
                else
                {
                    if (authEntityList != null)
                        _cardAuthRepository.Insert(authEntityList);
                }


                //if (oldConvertList != null && oldConvertList.Count() > 0)
                //{
                //    foreach (var item in convertEntityList)
                //    {
                //        var checkExist = oldConvertList.Where(a => a.CardConvertNo == item.CardConvertNo);
                //        if (checkExist.Count() == 0)
                //        {
                //            _convertRepository.Insert(item);
                //        }
                //        else
                //        {

                //        }
                //    }
                //}
                //else
                //{
                //    if (convertEntityList != null)
                //        _convertRepository.Insert(convertEntityList);
                //}
                _unitOfWork.Commit();
                result = true;

                SynchronizationCardAuth(cardUUID, authType);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();

                Log.Error(ex, "SaveCardAuth保存授权信息");
                result = false;
            }
            return result;
        }


        public virtual bool DeleteCardAuth(string cardUUID, string authType)
        {
            var result = false;
            try
            {
                _unitOfWork.BeginTransaction();
                var authList = _cardAuthRepository.TableNoTracking.Where(a => a.CardUUID == cardUUID && a.AuthType == authType && !a.Deleted);
                foreach (var item in authList)
                {
                    var entity = _cardAuthRepository.GetById(item.CardAuthUUID);
                    entity.SyncStatus = false;
                    entity.Deleted = true;
                    _cardAuthRepository.Update(entity);
                }

                _unitOfWork.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                Log.Error(ex, "删除授权表信息失败。" + cardUUID);
            }

            if (result)
            {
                SynchronizationCardAuth(cardUUID, authType);
            }
            return result;
        }

        /// <summary>
        /// 卡转码（临时用）
        /// </summary>
        public void InsertCardNoConvert()
        {
            var query = _cardRepository.Table;

            try
            {
                _unitOfWork.BeginTransaction();

                var convertList = new List<CardNoConvertEntity>();

                foreach (var item in query.ToList())
                {
                    foreach(var type in Constant.ConvertTypeList)
                    {
                        var convertDto = new CardNoConvertEntity
                        {
                            CardNOConverUUID = Guid.NewGuid().ToString("N"),
                            CardUUID = item.CardUUID,
                            CardConvertType = type,
                            CardConvertNo = CardNoConvert.ConvertCardNo(item.CardNo, type, item.CardType)
                        };
                        convertList.Add(convertDto);
                    }                                
                }
                if (convertList != null && convertList.Count > 0)
                    _convertRepository.Insert(convertList);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log.Error(null, ex.Message);
                _unitOfWork.Rollback();
            }
        }

        public void GetSyncCardList()
        {
            var query = _cardRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                SynchronizationCard(p.CardUUID);
            });
        }

        public void GetSyncPersonCardList()
        {
            var query = _userCardRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                SynchronizationPersonCard(p.CardUUID);
            });
        }

        public void GetSyncCardAuthList()
        {
            var query = _cardAuthRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                SynchronizationCardAuthById(p.CardAuthUUID);
            });
        }
    }
}