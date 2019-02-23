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
    public interface IEvevatorConfigService
    {
        EvevatorConfigDto GetByCommunityId(string communityId);
        EvevatorConfigDto GetById(int dictId);
        void Create(EvevatorConfigDto config);

        void Update(EvevatorConfigDto config);

        void Delete(string ids);
    }


    public class EvevatorConfigService : IEvevatorConfigService, IDependency
    {
        private readonly IRepository<EvevatorConfigEntity> _evevatorConfigRepository;

        public EvevatorConfigService(IRepository<EvevatorConfigEntity> evevatorConfigRepository)
        {
            _evevatorConfigRepository = evevatorConfigRepository;
        }

        public EvevatorConfigDto GetByCommunityId(string communityId)
        {
            var entity = _evevatorConfigRepository.GetEntity(t => t.CommunityUUID == communityId);
            if (entity != null)
                return entity.ToModel();
            else
                return null;
        }

        public EvevatorConfigDto GetById(int dictId)
        {
            var entity = _evevatorConfigRepository.GetById(dictId);
            if (entity != null)
                return entity.ToModel();
            else
                return null;
        }





        public void Create(EvevatorConfigDto config)
        {

            var configEntity = _evevatorConfigRepository.GetEntity(t => t.CommunityUUID == config.CommunityUUID);
            if (configEntity == null)
            {
                configEntity = new EvevatorConfigEntity();
                configEntity.EvevatorCount = config.EvevatorCount;
                configEntity.AutoCall = config.AutoCall;
                configEntity.EvevatorSN = config.EvevatorSN;
                configEntity.MutiEvevator = config.MutiEvevator;
                _evevatorConfigRepository.Insert(configEntity);
            }
            else
            {
                configEntity.EvevatorCount = config.EvevatorCount;
                configEntity.AutoCall = config.AutoCall;
                configEntity.EvevatorSN = config.EvevatorSN;
                configEntity.MutiEvevator = config.MutiEvevator;
                _evevatorConfigRepository.Update(configEntity);
            }

        }

        public void Update(EvevatorConfigDto config)
        {
            var entity = _evevatorConfigRepository.GetById(config.CommunityUUID);
            entity.MutiEvevator = config.MutiEvevator;
            entity.AutoCall = config.AutoCall;
            entity.EvevatorCount = config.EvevatorCount;
            entity.EvevatorSN = config.EvevatorSN;
            entity.CommunityUUID = config.CommunityUUID;

            _evevatorConfigRepository.Update(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
            foreach (var item in idList1)
            {
                var model = _evevatorConfigRepository.GetById(item);
                _evevatorConfigRepository.Delete(model);
            }
        }
    }
}
