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
using System.Collections.Generic;

namespace Koowoo.Services.System
{
    public interface IConfigService
    {
        TableData GetList(QueryListReq req);
        ConfigDto GetById(int userId);
       List <ConfigDto> GetByKey(String key);
        void Create(ConfigDto config);
        void Update(ConfigDto config);
        void Delete(string ids);
    }

    public class ConfigService : IConfigService,IDependency
    {
        private readonly IRepository<ConfigEntity> _configRepository;

        public ConfigService(IRepository<ConfigEntity> configRepository)
        {
            _configRepository = configRepository;
        }

        public TableData GetList(QueryListReq req)
        {
            var query = _configRepository.Table;

            if (!req.keyword.IsBlank())
            {
                query = query.Where(a => a.ConfigName.Contains(req.keyword));
            }

            query = query.OrderByDescending(o => o.ConfigID);
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
        public List<ConfigDto> GetByKey(String key)
        {
            var dto = _configRepository.Table.Where(t=>t.ConfigKey== key).ToList();
            if (dto != null)
                return dto.MapToList<ConfigDto>();
            else
                return null;
        }
        public ConfigDto GetById(int configId)
        {
            var dto = _configRepository.GetById(configId);
            if (dto != null)
                return dto.ToModel();
            else
                return null;
        }

        public void Create(ConfigDto config)
        {
            var entity = config.ToEntity();
            _configRepository.Insert(entity);

        }

        public void Update(ConfigDto config)
        {
            var entity = _configRepository.GetById(config.ConfigID);
            entity = config.ToEntity(entity);
            _configRepository.Update(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
            foreach (var item in idList1)
            {
                var model = _configRepository.GetById(item);
                _configRepository.Delete(model);
            }
        }
    }
}
