using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Core.Pager;
using Koowoo.Domain;
using Koowoo.Domain.System;
using Koowoo.Data.Interface;
using Koowoo.Pojo;
using Koowoo.Pojo.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using Koowoo.Services;
using Koowoo.Data;
using Koowoo.Services.LkbResponse;
using System.Text;
using Koowoo.Services.System;

namespace Koowoo.Services
{
    public interface IAreaService
    {
        TableData GetList(int pageIndex, int pageSize, string key, string parentId);

        TableData GetCommunityList(int pageIndex = 1, int pageSize = int.MaxValue, string key = null);

        AreaTreeDto GetCommunityTree(string communityId);

        AreaTreeDto GetCommunityTreeBySearch(string communityId, string keyword);
        List<AreaDto> GetCommunityListBySearch(string communityId, string keyword);

        IList<AreaTreeDto> GetCommunityTreeList();

        IList<AreaSelectListDto> GetCommunityTree2(string areaId);

        IList<AreaTreeDto> GetTree();

        IList<AreaDto> GetListByParentId(string communityId, string parentCode);

        AreaDto GetById(string areaId);

        AreaDto GetDtoByCode(string code);

        void Create(AreaDto model);

        void Update(AreaDto config);

        void Delete(string ids);

        /// <summary>
        /// 类别面包屑格式-类别名称
        /// </summary>
        /// <param name="category">类别</param>
        /// <param name="separator">分隔符号</param>
        /// <returns>面包屑格式</returns>
        string GetFormattedBreadCrumb(AreaEntity area, string separator = ">>");

        void GetSyncAreaList();
    }

    public class AreaService : IAreaService, IDependency
    {
        private readonly IRepository<AreaEntity> _areaRepository;
        private readonly IDictService _dictService;
        private readonly IRoomService _roomService;
        private readonly ISyncLogServie _syncService;
        private readonly IDbContext _dbContext;

        public AreaService(IRepository<AreaEntity> areaRepository, IDictService dictService, 
            IRoomService roomService,
            ISyncLogServie syncService,
            IDbContext dbContext)
        {
            _areaRepository = areaRepository;
            _dictService = dictService;
            _roomService = roomService;
            _syncService = syncService;
            this._dbContext = dbContext;
        }

        /// <summary>
        /// 区域列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public TableData GetList(int pageIndex, int pageSize, string key, string parentId)
        {
            var query = _areaRepository.Table;
            query = query.Where(a => !a.Deleted);

            if (!key.IsBlank())
            {
                query = query.Where(a => a.ChineseName.Contains(key) || a.EnglishName.Contains(key));
            }

            if (!parentId.IsBlank())
            {
                var parent = _areaRepository.GetById(parentId);
                if (parent != null)
                {
                    query = query.Where(a => a.ParentCode == parent.Code);
                }
            }

            query = query.OrderByDescending(o => o.CreateTime);
            var pagedList = query.ToPagedList(pageIndex, pageSize);
            var areaList = new List<AreaDto>();
            foreach (var area in pagedList)
            {
                var AreaTypeDict = _dictService.GetByCode(DictTypeEnum.AreaType, area.AreaType.ToString());
                var item = new AreaDto();
                item = area.ToModel();
                item.AreaTypeName = AreaTypeDict != null ? AreaTypeDict.DictName : "";
                areaList.Add(item);
            }
            return new TableData
            {
                currPage = pageIndex,
                pageSize = pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = areaList
            };
        }


        /// <summary>
        /// 社区列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public TableData GetCommunityList(int pageIndex, int pageSize, string key)
        {
            var query = _areaRepository.Table.Where(a => a.AreaType == 8 && !a.Deleted);
            query = query.OrderBy(o => o.CreateTime);
            var pagedList = query.ToPagedList(pageIndex, pageSize);

            var list = (from s in pagedList
                        select new CommunityDto
                        {
                            CommunityUUID = s.AreaUUID,
                            CommunityName = s.ChineseName
                        }).ToList();

            return new TableData
            {
                currPage = pageIndex,
                pageSize = pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = list
            };
        }

        public IList<AreaDto> GetListByParentId(string communityId, string parentCode)
        {
            var query = _areaRepository.Table.Where(a => !a.Deleted && a.ParentCode == parentCode);
            return query.MapToList<AreaDto>();
        }


        //  此接口暂无没用到
        /// <summary>
        /// 获取单个社区树结构（社区、幢、单元、门）  
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public AreaTreeDto GetCommunityTree(string areaId)
        {
            var modal = _areaRepository.GetById(areaId);
            if (modal != null)
            {
                var query = _areaRepository.Table.Where(a => !a.Deleted);
                var list = SortForTree(modal.Code, query.ToList());

                var dto = new AreaTreeDto()
                {
                    AreaUUID = modal.AreaUUID,
                    AreaCode = modal.Code,
                    AreaName = modal.ChineseName
                };
                dto.Children = list;
                return dto;
            }
            return null;
        }


        /// <summary>
        /// 获取社区树结构列表（社区、幢、单元、门）
        /// </summary>
        /// <returns></returns>
        public IList<AreaTreeDto> GetCommunityTreeList()
        {
            var query = _areaRepository.Table.Where(a => !a.Deleted);
            var communityList = query.Where(a => a.AreaType == 8);
            var list = new List<AreaTreeDto>();
            foreach (var item in communityList)
            {
                var dto = new AreaTreeDto();
                dto.AreaUUID = item.AreaUUID;
                dto.AreaCode = item.Code;
                dto.AreaName = item.ChineseName;
                dto.Children = SortForTree(item.Code, query.ToList());
                list.Add(dto);
            }
            return list;
        }


        /// <summary>
        /// 根据社区ID获取房间（含幢、单元、房间）非树型
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public IList<AreaSelectListDto> GetCommunityTree2(string areaId)
        {
            IList<AreaSelectListDto> areaList = new List<AreaSelectListDto>();
            var modal = _areaRepository.GetById(areaId);
            if (modal != null)
            {
                var query = _areaRepository.Table.Where(a => !a.Deleted);

                var unsortedElements = query.ToList();
                //sort categories
                var sortedCategories = unsortedElements.SortElementsForTree(modal.Code, true);

                foreach (var item in sortedCategories)
                {
                    var dto = new AreaSelectListDto();
                    dto.AreaUUID = item.AreaUUID;
                    dto.Code = item.Code;
                    dto.ChineseName = item.ChineseName;
                    dto.FullAreaName = GetFormattedBreadCrumb(item);
                    areaList.Add(dto);
                }
            }

            return areaList;
        }


        /// <summary>
        /// 通过关键字搜索获取社区树结构
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="communityId"></param>
        /// <returns></returns>
        public List<AreaDto> GetCommunityListBySearch(string communityId, string keyword)
        {
            var modal = _areaRepository.GetById(communityId);
            if (modal != null)
            {

                var querySql = "select * from biz_Area as a where ParentCode in ";
                querySql += "(select Code from biz_Area as b where AreaType = 10 and ParentCode in ";
                querySql += "(select code from biz_Area where AreaType = 9 and ParentCode in ";
                querySql += "(select Code from biz_Area where AreaType = 8 and AreaUUID = '{0}')))";
                querySql += " and a.AreaType = 11 {1}";
                querySql += " union ";
                //--取单元
                querySql += "select t_unit.* from biz_Area as t_unit inner join(";
                querySql += "select * from biz_Area as a where ";
                querySql += "ParentCode in (select Code from biz_Area as b where AreaType = 10 and ParentCode in (select code from biz_Area where AreaType = 9 and ParentCode in (select Code from biz_Area where AreaType = 8 and AreaUUID = '{0}')) )";
                querySql += "and a.AreaType = 11 {1}) as t_room on t_room.ParentCode = t_unit.Code";
                //--取楼栋
                querySql += " union ";
                querySql += " select t_building.*from biz_area as t_building inner join";
                querySql += "(select t_unit.* from biz_Area as t_unit inner join";
                querySql += "(select * from biz_Area as a where";
                querySql += " ParentCode in (select Code from biz_Area as b where AreaType = 10 and ParentCode in (select code from biz_Area where AreaType = 9 and ParentCode in (select Code from biz_Area where AreaType = 8  and AreaUUID = '{0}')))";
                querySql += " and a.AreaType = 11 {1}) as t_room on t_room.ParentCode = t_unit.Code) as t_unit2 on t_unit2.parentcode = t_building.Code";

                //var querySql = @"with tab as
                //(
                // select * from [dbo].[biz_Area] where AreaUUID='{0}'--父节点
                //{1}
                // union all
                // select b.*
                // from
                //  tab a,--父节点数据集
                //  [dbo].[biz_Area] b--子节点数据集 
                // where b.[ParentCode]=a.Code and b.Deleted=0 {1}--子节点数据集.ID=父节点数据集.parendID
                //)
                //select * from tab;";
                var keyFilth = string.Empty;
                if (!keyword.IsBlank())
                {
                    keyFilth = string.Format(" and a.ChineseName like '%{0}%'", keyword);
                }

                string cmdText = string.Format(querySql, communityId, keyFilth);
                var query = _dbContext.SqlQuery<AreaEntity>(cmdText).ToList();

                return query.MapToList<AreaDto>();
            }
            return null;
        }
        /// <summary>
        /// 通过关键字搜索获取社区树结构
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="communityId"></param>
        /// <returns></returns>
        public AreaTreeDto GetCommunityTreeBySearch(string communityId, string keyword)
        {
            var modal = _areaRepository.GetById(communityId);
            if (modal != null)
            {
                var querySql = "select * from biz_Area as a where ParentCode in ";
                querySql += "(select Code from biz_Area as b where AreaType = 10 and ParentCode in ";
                querySql += "(select code from biz_Area where AreaType = 9 and ParentCode in ";
                querySql += "(select Code from biz_Area where AreaType = 8 and AreaUUID = '{0}')))";
                querySql += " and a.AreaType = 11 {1}";
                querySql += " union ";
                //--取单元
                querySql += "select t_unit.* from biz_Area as t_unit inner join(";
                querySql += "select * from biz_Area as a where ";
                querySql += "ParentCode in (select Code from biz_Area as b where AreaType = 10 and ParentCode in (select code from biz_Area where AreaType = 9 and ParentCode in (select Code from biz_Area where AreaType = 8 and AreaUUID = '{0}')) )";
                querySql += "and a.AreaType = 11 {1}) as t_room on t_room.ParentCode = t_unit.Code";
                //--取楼栋
                querySql += " union ";
                querySql += " select t_building.*from biz_area as t_building inner join";
                querySql += "(select t_unit.* from biz_Area as t_unit inner join";
                querySql += "(select * from biz_Area as a where";
                querySql += " ParentCode in (select Code from biz_Area as b where AreaType = 10 and ParentCode in (select code from biz_Area where AreaType = 9 and ParentCode in (select Code from biz_Area where AreaType = 8  and AreaUUID = '{0}')))";
                querySql += " and a.AreaType = 11 {1}) as t_room on t_room.ParentCode = t_unit.Code) as t_unit2 on t_unit2.parentcode = t_building.Code";
               // var querySql = @"with tab as
                //(
                // select * from [dbo].[biz_Area] where AreaUUID='{0}'--父节点
                //{1}
                // union all
                // select b.*
                // from
                //  tab a,--父节点数据集
                //  [dbo].[biz_Area] b--子节点数据集 
                // where b.[ParentCode]=a.Code and b.Deleted=0 {1}--子节点数据集.ID=父节点数据集.parendID
                //)
                //select * from tab;";
                var keyFilth = string.Empty;
                if (!keyword.IsBlank())
                {
                    keyFilth = string.Format(" and a.ChineseName like '%{0}%'", keyword);
                }

                string cmdText = string.Format(querySql, communityId, keyFilth);
                var query = _dbContext.SqlQuery<AreaEntity>(cmdText).ToList();


                var dto = new AreaTreeDto();
                dto.AreaUUID = modal.AreaUUID;
                dto.AreaName = modal.ChineseName;
                dto.AreaCode = modal.Code;
                dto.Children = SortForTree(modal.Code, query);

                return dto;
            }
            return null;
        }


        /// <summary>
        /// 区域树结构
        /// </summary>
        /// <returns></returns>
        public IList<AreaTreeDto> GetTree()
        {
            var query = _areaRepository.Table.Where(a => !a.Deleted);
            var list = SortForTree(null, query.ToList());
            return list;
        }


        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public AreaDto GetById(string areaId)
        {
            var entity = _areaRepository.GetById(areaId);
            if (entity != null)
            {
                var dto = entity.MapTo<AreaDto>();
                var parentArea = this.GetByCode(dto.ParentCode);

                dto.ParentName = parentArea?.ChineseName??"顶级";
                return dto;
            }
            else
                return null;
        }

        public AreaDto GetDtoByCode(string code)
        {
            var query = from c in _areaRepository.Table
                        orderby c.CreateTime
                        where !c.Deleted && c.Code == code
                        select c;

            var entity = query.FirstOrDefault();

            if (entity != null)
                return entity.MapTo<AreaDto>();
            else
                return null;
        }




        public AreaEntity GetByCode(string areaCode)
        {
            return _areaRepository.Table.Where(a => a.Code == areaCode).FirstOrDefault();

        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public void Create(AreaDto model)
        {
            var entity = model.MapTo<AreaEntity>();
            entity.AreaUUID = Guid.NewGuid().ToString("N");
            var child = _areaRepository.Table.Where(a => a.ParentCode == model.ParentCode).ToList();

            var parentModel = _areaRepository.Table.FirstOrDefault(a => a.Code == model.ParentCode && !a.Deleted);
            if (parentModel != null)
            {
                entity.Code = parentModel.Code + (child.Count + 1).ToString().PadLeft(4, '0');
                entity.AreaType = parentModel.AreaType + 1;
            }
            else
            {
                entity.Code = (child.Count + 1).ToString().PadLeft(4, '0');
                entity.AreaType = 1;
            }
            entity.IsParent = entity.AreaType == 11 ? "0" : "1";
            entity.Deleted = false;
            entity.CreateTime = DateTime.Now;
            entity.UpdateTime = DateTime.Now;
            entity.SyncStatus = false;
            entity.SyncVersion = 0;
            _areaRepository.Insert(entity);

            Synchronization(entity.AreaUUID);

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="config"></param>
        public void Update(AreaDto config)
        {
            var entity = _areaRepository.GetById(config.AreaUUID);
            entity.ChineseName = config.ChineseName;
            entity.EnglishName = config.EnglishName;
            entity.Remark = config.Remark;
            entity.AreaCode = config.AreaCode;
            entity.OrderID = config.OrderID;
            entity.SyncStatus = false;
            _areaRepository.Update(entity);

            Synchronization(entity.AreaUUID);

        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p).ToList();
            //  var entities = ids.Select(id => Repository.Find(id)).ToList();
            foreach (var item in idList1)
            {
                var entity = _areaRepository.GetById(item);
                entity.Deleted = true;
                entity.SyncStatus = false;
                _areaRepository.Update(entity);


                Synchronization(entity.AreaUUID);

                //删除房间有问题

                _roomService.DeleteByAreaUUID(ids);
            }

            //删除房间

        }

        private void Synchronization(string areaId)
        {
            if (Constant.Sysc)
            {              

                try
                {
                    var entity = _areaRepository.GetById(areaId);

                    var requestXml = GetXml(entity);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.AreaServiceImplService();
                    var responseXml = areaS.insertArea(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertArea.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = "";

                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            entity.SyncVersion += 1;
                            entity.SyncStatus = true;
                            _areaRepository.Update(entity);

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

        private string GetXml(AreaEntity entity)
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
            xmlBuilder.Append("<area>");
            xmlBuilder.AppendFormat("<uuid>{0}</uuid>", entity.AreaUUID);
            xmlBuilder.AppendFormat("<cusr>{0}</cusr>", Constant.LkbAccount);
            xmlBuilder.AppendFormat("<state>{0}</state>", entity.Status);
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<udate>{0}</udate>", entity.UpdateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<order>{0}</order>", entity.OrderID);
            xmlBuilder.AppendFormat("<code>{0}</code>", entity.Code);
            xmlBuilder.AppendFormat("<cname>{0}</cname>", entity.ChineseName);
            xmlBuilder.AppendFormat("<ename>{0}</ename>", entity.EnglishName);
            xmlBuilder.AppendFormat("<areacode>{0}</areacode>", entity.AreaCode);
            xmlBuilder.AppendFormat("<type>{0}</type>", entity.AreaType);
            xmlBuilder.AppendFormat("<remark>{0}</remark>", entity.Remark);
            xmlBuilder.AppendFormat("<keycode>{0}</keycode>", entity.KeyCode);
            xmlBuilder.AppendFormat("<keycode1>{0}</keycode1>", entity.KeyCode1);
            xmlBuilder.AppendFormat("<ifparent>{0}</ifparent>", entity.IsParent);
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</area>");
            xmlBuilder.Append("</ReqBody> ");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }

        /// <summary>
        /// 递归树结构
        /// </summary>
        /// <param name="parentId">父节点</param>
        /// <returns></returns>
        private List<AreaTreeDto> SortForTree(string parentId, IList<AreaEntity> areaList)
        {
            var model = new List<AreaTreeDto>();
            if (areaList != null)
            {
                foreach (var p in areaList.Where(t => t.ParentCode == parentId).OrderBy(t => t.OrderID))
                {
                    var area = new AreaTreeDto
                    {
                        AreaUUID = p.AreaUUID,
                        AreaName = p.ChineseName,
                    };
                    area.Children.AddRange(SortForTree(p.Code, areaList));
                    model.Add(area);
                }
            }
            return model;
        }

        /// <summary>
        /// 类别面包屑格式-类别名称
        /// </summary>
        /// <param name="area">类别</param>
        /// <param name="separator">分隔符号</param>
        /// <returns>面包屑格式</returns>
        public virtual string GetFormattedBreadCrumb(AreaEntity area, string separator = ">>")
        {
            if (area == null)
                throw new ArgumentNullException("element");

            string result = string.Empty;

            var alreadyProcessedElementIds = new List<string>() { };

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
                area = this.GetByCode(area.ParentCode);

            }

            return result;
        }

        public void GetSyncAreaList()
        {
            var query = _areaRepository.Table.Where(a => !a.SyncStatus && a.AreaType>=8);

            var syncList = query.ToList();

            syncList.ForEach(p => {               
                Synchronization(p.AreaUUID);
            });
        }
    }
}
