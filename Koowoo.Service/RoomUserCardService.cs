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
    public interface IRoomUserCardService
    {
        string[] GetUserCardsByUserID(string roomuserId);
    }

    public class RoomUserCardService: IRoomUserCardService, IDependency
    {
        private readonly IRepository<RoomUserCardEntity> _userCardRepository;

        public RoomUserCardService(IRepository<RoomUserCardEntity> userCardRepository)
        {
            _userCardRepository = userCardRepository;
        }

        public virtual string[] GetUserCardsByUserID(string roomuserId)
        {
            var query = _userCardRepository.Table;
            query = query.Where(a => a.RoomUserUUID == roomuserId);
            var filter = query.Select(m => m.CardUUID).ToList();

            return filter.ToArray();
        }

    }
}
