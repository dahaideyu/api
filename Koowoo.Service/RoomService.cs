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
using Koowoo.Services;
using Koowoo.Services.LkbResponse;
using System.Text;
using Koowoo.Pojo.Enum;
using System.Diagnostics;

namespace Koowoo.Services
{
    public interface IRoomService
    {
        TableData GetList(int pageIndex = 1, int pageSize = int.MaxValue, string key = null, string parentId = "", string communityId = "");
        RoomDto GetById(string roomId);
        RoomDto GetByRoomUserId(string roomUserId);
        string GetHomeNoByAreaId(string areaUUID);
        RoomDto GetByAreaId(string areaId);
        IList<RoomListDto> GetRoomList(string communityId, string areaUUID = null);
        void DeleteByAreaUUID(string areaId);
        void Insert(RoomDto model);
        void Update(RoomDto model);
        void Delete(string ids);
        string[] GetHomeNos(string communityId);

        void GetSyncRoomList();
    }

    public class RoomServie : IRoomService, IDependency
    {
        private readonly IRepository<AreaEntity> _areaRepository;
        private readonly IRepository<RoomEntity> _roomRepository;
        private readonly IRepository<RoomUserEntity> _roomUserRepository;

        private readonly IRepository<DictEntity> _dictRepository;
        private readonly ISyncLogServie _syncService;

        public RoomServie(IRepository<RoomEntity> roomRepository,
            IRepository<AreaEntity> areaRepository,
            IRepository<RoomUserEntity> roomUserRepository,
            ISyncLogServie syncService,
            IRepository<DictEntity> dictRepository)
        {
            _syncService = syncService;
            _roomRepository = roomRepository;
            _areaRepository = areaRepository;
            _dictRepository = dictRepository;
            _roomUserRepository = roomUserRepository;
        }


        public virtual string[] GetHomeNos(string communityId)
        {
            var query = _roomRepository.Table;
            query = query.Where(a => a.CommunityUUID == communityId);
            var filter = query.Select(m => m.HomeNo).Distinct().ToList();

            return filter.ToArray();
        }

        public TableData GetList(int pageIndex, int pageSize, string key, string parentId, string communityId)
        {
            var query = _roomRepository.Table.Where(a => !a.Deleted);

            if (!communityId.IsBlank())
            {
                query = query.Where(a => a.CommunityUUID == communityId);
            }

            if (!parentId.IsBlank())
            {
                var parentArea = _areaRepository.GetById(parentId);
                if (parentArea != null)
                    query = query.Where(a => a.Area.Code.StartsWith(parentArea.Code));
            }

            if (!key.IsBlank())
            {
                query = query.Where(a => a.RoomName.Contains(key));
            }
            query = query.OrderByDescending(o => o.CreateTime);
            var pagedList = query.ToPagedList(pageIndex, pageSize);

            var roomList = new List<RoomListDto>();
            foreach (var item in pagedList)
            {
                var dto = new RoomListDto();
                dto.RoomUUID = item.RoomUUID;
                dto.AreaCode = item.Area.AreaCode;
                dto.RoomName = item.RoomName;
                dto.OtherCode = item.OtherCode;
                dto.ResidenceTypeName = item.ResidenceTypeDict?.DictName ?? "";
                //  dto.Code = item.Area.Code;
                dto.AreaTypeName = _dictRepository.TableNoTracking.Where(a => a.DictType == Pojo.Enum.DictTypeEnum.AreaType.ToString() && a.DictCode == item.Area.AreaType.ToString()).FirstOrDefault().DictName;
                roomList.Add(dto);
            }

            return new TableData
            {
                currPage = pageIndex,
                pageSize = pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = roomList
            };
        }

        public string GetHomeNoByAreaId(string areaUUID)
        {
            var query = _roomRepository.Table.Where(a => !a.Deleted);

            var parentArea = _areaRepository.GetById(areaUUID);
            if (parentArea != null)
            {
                if (parentArea.AreaType == 10 || parentArea.AreaType == 11)
                {
                    query = query.Where(a => a.Area.Code.StartsWith(parentArea.Code));
                    var entity = query.FirstOrDefault();
                    if (entity != null)
                    {
                        return entity.HomeNo;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public IList<RoomListDto> GetRoomList(string communityId, string areaId = null)
        {
            var query = _roomRepository.Table.Where(a => !a.Deleted);

            if (!communityId.IsBlank())
            {
                query = query.Where(a => a.CommunityUUID == communityId);
            }

            if (!areaId.IsBlank())
            {
                query = query.Where(a => a.AreaUUID == areaId);
            }

            query = query.OrderBy(o => o.RoomName);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var pagedList = query.ToList();
            sw.Stop();
            var listTotal = sw.ElapsedMilliseconds;
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            var roomList = new List<RoomListDto>();
            var areaList = _areaRepository.Table.ToList();
            foreach (var item in pagedList)
            {
                var area = areaList.Find(t => t.AreaUUID == item.AreaUUID);
                var dto = new RoomListDto();
                dto.RoomUUID = item.RoomUUID;
                dto.RoomName = item.RoomName;
                dto.RoomFullName = GetFormattedBreadCrumb(area, areaList);
                //dto.RoomFullName = item.RoomFullName; //GetFormattedBreadCrumb(area);
                roomList.Add(dto);
            }
            sw1.Stop();
            var buTotal = sw1.ElapsedMilliseconds;
            return roomList;
        }
        public RoomDto GetByAreaId(string areaId)
        {
            var dto = _roomRepository.GetEntity(t => t.AreaUUID == areaId);
            if (dto != null)
                return dto.MapTo<RoomDto>();
            else
                return null;
        }

        public RoomDto GetByRoomUserId(string roomUserId)
        {
            var roomUserEntity = _roomUserRepository.GetById(roomUserId);
            if (roomUserEntity == null)
            {
                return null;
            }
            var dto = _roomRepository.GetById(roomUserEntity.RoomUUID);
            if (dto != null)
                return dto.MapTo<RoomDto>();
            else
                return null;
        }
        public RoomDto GetById(string roomId)
        {
            var dto = _roomRepository.GetById(roomId);
            if (dto != null)
                return dto.MapTo<RoomDto>();
            else
                return null;
        }

        public void Insert(RoomDto dto)
        {
            var entity = new RoomEntity();
            entity = dto.ToEntity();
            entity.RoomUUID = Guid.NewGuid().ToString("N");
            entity.CreateTime = DateTime.Now;
            entity.SyncStatus = false;
            entity.SyncVersion = 0;
            _roomRepository.Insert(entity);

            Synchronization(entity);
        }

        public void Update(RoomDto model)
        {
            var entity = _roomRepository.GetById(model.RoomUUID);
            entity = model.ToEntity(entity);
            entity.SyncStatus = false;
            _roomRepository.Update(entity);

            Synchronization(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p).ToList();
            foreach (var item in idList1)
            {
                var entity = _roomRepository.GetById(item);
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.SyncStatus = false;
                    _roomRepository.Update(entity);
                    // Synchronization(entity);
                }
            }
        }

        public void DeleteByAreaUUID(string areaId)
        {
            var entity = _roomRepository.Table.Where(a => a.AreaUUID == areaId && !a.Deleted).FirstOrDefault();
            if (entity != null)
            {
                entity.Deleted = true;
                _roomRepository.Update(entity);

                Synchronization(entity);
            }
        }

        private void Synchronization(RoomEntity entity)
        {
            if (Constant.Sysc)
            {
               

                try
                {
                    var requestXml = GetXml(entity);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.AreaServiceImplService();
                    var responseXml = areaS.insertAreaExtend(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertAreaExtend.ToString();
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
                            var entity2 = _roomRepository.GetById(entity.RoomUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _roomRepository.Update(entity2);

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

        private string GetXml(RoomEntity entity)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";

            var ResidenceTypeDict = entity.ResidenceType.HasValue ? _dictRepository.GetById(entity.ResidenceType.Value) : null;
            var RoomTypeDict = entity.RoomType.HasValue ? _dictRepository.GetById(entity.RoomType.Value) : null;
            var OwnershipTypeDict = entity.OwnershipType.HasValue ? _dictRepository.GetById(entity.OwnershipType.Value) : null;
            var NatureDict = entity.Nature.HasValue ? _dictRepository.GetById(entity.Nature.Value) : null;
            var ManageLevelDict = entity.ManageLevel.HasValue ? _dictRepository.GetById(entity.ManageLevel.Value) : null;
            var RentalTypeDict = entity.RentalType.HasValue ? _dictRepository.GetById(entity.RentalType.Value) : null;
            var RoomStyleDict = entity.RoomStyle.HasValue ? _dictRepository.GetById(entity.RoomStyle.Value) : null;


            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<areaextend>");
            xmlBuilder.AppendFormat("<reluuid>{0}</reluuid>", entity.AreaUUID);
            xmlBuilder.AppendFormat("<jzlx>{0}</jzlx>", ResidenceTypeDict != null ? ResidenceTypeDict.DictCode : "");
            xmlBuilder.AppendFormat("<fwlx>{0}</fwlx>", RoomTypeDict != null ? RoomTypeDict.DictCode : "");
            xmlBuilder.AppendFormat("<fwsyqlx>{0}</fwsyqlx>", OwnershipTypeDict != null ? OwnershipTypeDict.DictCode : "");
            xmlBuilder.AppendFormat("<fwxxdz>{0}</fwxxdz>", entity.Address);
            xmlBuilder.AppendFormat("<fczh>{0}</fczh>", entity.CertificationNo);
            xmlBuilder.AppendFormat("<sspcs>{0}</sspcs>", entity.PoliceStation);
            xmlBuilder.AppendFormat("<mjxx>{0}</mjxx>", entity.Policeman);
            xmlBuilder.AppendFormat("<czyt>{0}</czyt>", entity.RentalUseage);
            xmlBuilder.AppendFormat("<djba>{0}</djba>", entity.Registration);
            xmlBuilder.AppendFormat("<babdate>{0}</babdate>", entity.RegFrom.HasValue ? entity.RegFrom.Value.ToString("yyyyMMddHHmmss") : "");
            xmlBuilder.AppendFormat("<baedate>{0}</baedate>", entity.RegTo.HasValue ? entity.RegTo.Value.ToString("yyyyMMddHHmmss") : "");
            xmlBuilder.AppendFormat("<xz>{0}</xz>", NatureDict != null ? NatureDict.DictCode : "");
            xmlBuilder.AppendFormat("<gldj>{0}</gldj>", ManageLevelDict != null ? ManageLevelDict.DictCode : "");
            xmlBuilder.AppendFormat("<zlzt>{0}</zlzt>", RentalTypeDict != null ? RentalTypeDict.DictCode : "");
            xmlBuilder.AppendFormat("<jshx>{0}</jshx>", RoomStyleDict != null ? RoomStyleDict.DictCode : "");
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</areaextend>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }

        /// <summary>
        /// 类别面包屑格式-类别名称
        /// </summary>
        /// <param name="area">类别</param>
        /// <param name="separator">分隔符号</param>
        /// <returns>面包屑格式</returns>
        public virtual string GetFormattedBreadCrumb(AreaEntity area, List<AreaEntity> areList, string separator = ">>")
        {
            if (area == null)
                throw new ArgumentNullException("element");

            string result = string.Empty;

            var alreadyProcessedElementIds = new List<string>() { };
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (area != null &&  //not null
                                    //  !area.Deleted &&  //not deleted
                !alreadyProcessedElementIds.Contains(area.AreaUUID)) //prevent circular references
            {
                if (String.IsNullOrEmpty(result))
                {
                    result = area.ChineseName;
                }
                else
                {
                    result = string.Format("{0} {1} {2}", area.ChineseName, separator, result);
                }

                alreadyProcessedElementIds.Add(area.AreaUUID);
                if (area.AreaType == 9)
                    break;
                area = areList.Find(a => a.Code == area.ParentCode);

            }
            sw.Stop();
            var roomTotal = sw.ElapsedMilliseconds;

            return result;
        }

       

        public void GetSyncRoomList()
        {
            var query = _roomRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                Synchronization(p);
            });
        }
    }
}
