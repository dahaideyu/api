using Koowoo.Core;
using Koowoo.Domain.System;
using Koowoo.Pojo;
using Koowoo.Pojo.System;
using System;
using System.Linq;
using Koowoo.Core.Extentions;
using System.Collections.Generic;
using Koowoo.Data.Interface;

namespace Koowoo.Services.System
{
    public interface IDictTypeService
    {
        IList<DictTypeDto> GetList();
        DictTypeDto GetByCode(string dictCode);

        void Create(DictTypeDto model);

        void Update(DictTypeDto model);
        void Delete(DictTypeDto model);
        void Delete(string code);
    }

    public class DictTypeService : IDictTypeService, IDependency
    {
        private readonly IRepository<DictTypeEntity> _dictTypeRepository;

        public DictTypeService(IRepository<DictTypeEntity> dictTypeRepository)
        {
            _dictTypeRepository = dictTypeRepository;
        }

        public IList<DictTypeDto> GetList()
        {
            var list = _dictTypeRepository.Table.ToList();
            if (list != null && list.Count>0)
            {
                return list.MapToList<DictTypeDto>();
            }
            return null;            
        }
      
        public DictTypeDto GetByCode(string dictCode)
        {
            var dict = _dictTypeRepository.Table.Where(a =>a.DictTypeCode == dictCode).FirstOrDefault();
            if (dict != null)
            {
                return dict.ToModel();
            }
            return null;
        }

        public void Create(DictTypeDto model)
        {
            var entity = model.MapTo<DictTypeEntity>();
            _dictTypeRepository.Insert(entity);
        }

        public void Update(DictTypeDto model)
        {
            var entity = _dictTypeRepository.GetById(model.DictTypeCode);
            entity.DictTypeName = model.DictTypeName;
            _dictTypeRepository.Update(entity);
        }

        public void Delete(DictTypeDto model)
        {
            var entity = _dictTypeRepository.GetById(model.DictTypeCode);
            _dictTypeRepository.Delete(entity);
        }
        public void Delete(string code)
        {
            var entity = _dictTypeRepository.GetById(code);
            _dictTypeRepository.Delete(entity);
        }
    }
}
