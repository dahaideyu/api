using Koowoo.Core;
using Koowoo.Core.Pager;
using Koowoo.Domain.System;
using Koowoo.Pojo;
using Koowoo.Pojo.System;
using System;
using System.Linq;
using Koowoo.Core.Extentions;
using System.Collections.Generic;
using Koowoo.Data.Interface;
using Koowoo.Pojo.Enum;
using Koowoo.Pojo.Request;

namespace Koowoo.Services.System
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDictService
    {
        TableData GetList(string dictType,QueryListReq req);

        Dictionary<string, object> GetListByCode(string code);

        DictDto GetById(int dictId);

        DictDto GetInfoByCode(string dictType, string dictCode);

        DictDto GetByCode(DictTypeEnum dictType, string dictValue);

        IList<DictEntity> GetListByType(DictTypeEnum dictType);

        void Create(DictDto config);

        void Update(DictDto config);

        void Delete(string ids);
    }


    public class DictService : IDictService, IDependency
    {
        private readonly IRepository<DictEntity> _dictRepository;

        public DictService(IRepository<DictEntity> dictRepository)
        {
            _dictRepository = dictRepository;
        }

        public TableData GetList(string dictType, QueryListReq req)
        {
            var query = _dictRepository.Table;
            if (!dictType.IsBlank())
            {
                query = query.Where(a => a.DictType.ToLower() == dictType.ToLower());
            }

            if (!req.keyword.IsBlank())
            {
                query = query.Where(a => a.DictName.Contains(req.keyword));
            }
            query = query.OrderByDescending(o => o.DictID);
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

        public Dictionary<string,object> GetListByCode(string codes)
        {
            var codeList = codes.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p).ToList();

            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (var item in codeList)
            {
                var query = _dictRepository.TableNoTracking.Where(s => s.DictType.ToLower() == item.ToLower());
                var dtoList = new List<DictDto>();
                var list = query.OrderBy(s => s.OrderID).ToList();

                foreach (var entity in list)
                {
                    var model = entity.ToModel();
                    dtoList.Add(model);
                }
                dict.Add(item, dtoList);
            }           
            return dict;
        }

        public DictDto GetById(int dictId)
        {
            var entity = _dictRepository.GetById(dictId);
            if (entity != null)
                return entity.ToModel();
            else
                return null;
        }

        /// <summary>
        /// 根据字典类型和字典值获取字典信息
        /// </summary>
        /// <param name="dictType"></param>
        /// <param name="dictCode"></param>
        /// <returns></returns>
        public DictDto GetInfoByCode(string dictType, string dictCode)
        {
            var query = from p in _dictRepository.Table
                        orderby p.DictID
                        where p.DictType.ToLower() == dictType.ToLower() && p.DictCode == dictCode
                        select p;
            var entity = query.FirstOrDefault();


            if (entity != null)
                return entity.ToModel();
            else
                return null;
        }

        public IList<DictEntity> GetListByType(DictTypeEnum dictType)
        {
            var query = _dictRepository.Table.Where(a => a.DictType == dictType.ToString());
            return query.ToList();
        }

        public DictDto GetByCode(DictTypeEnum dictType, string dictValue)
        {
            var dict = _dictRepository.Table.Where(a => a.DictType == dictType.ToString() && a.DictCode == dictValue).FirstOrDefault();
            if (dict != null)
            {
                return dict.ToModel();
            }
            return null;
        }


        public void Create(DictDto config)
        {
            var entity = config.MapTo<DictEntity>();
            _dictRepository.Insert(entity);
        }

        public void Update(DictDto config)
        {
            var entity = _dictRepository.GetById(config.DictID);
            entity.DictName = config.DictName;
            entity.DictCode = config.DictCode;
            entity.DictType = config.DictType;
            entity.OrderID = config.OrderID;
            _dictRepository.Update(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
            foreach (var item in idList1)
            {
                var model = _dictRepository.GetById(item);
                _dictRepository.Delete(model);
            }
        }
    }
}
