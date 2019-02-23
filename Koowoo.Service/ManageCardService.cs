using Koowoo.Core;
using Koowoo.Data.Interface;
using Koowoo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Services
{
    public interface IManageCardService
    {
      List< ManageCardEntity> GetByPersonId(string personId);
    }

    public class ManageCardService : IManageCardService, IDependency
    {
        private readonly IRepository<ManageCardEntity> _manageCardRepository;

        public ManageCardService(IRepository<ManageCardEntity> manageCardRepository)
        {
            _manageCardRepository = manageCardRepository;
        }
        /// <summary>
        /// 根据人员ID获取用户入住信息
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public List<ManageCardEntity> GetByPersonId(string personId)
        {
            var dto = _manageCardRepository.Table.Where(a => a.PersonUUID == personId).ToList();
            if (dto != null)
                return dto;
            else
                return null;
        }


    }
}
