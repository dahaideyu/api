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
    public interface ICardAuthService
    {
        IList<CardAuthEntity> GetCardAuthList(string cardId);
        IList<CardAuthEntity> GetCardAuthList(string[] cardIds);

        List<CardAuthEntity> GetAuthList(string cardId,string authType);
    }

    public class CardAuthService: ICardAuthService, IDependency
    { 

        private readonly IRepository<CardAuthEntity> _cardAuthRepository;

        public CardAuthService(IRepository<CardAuthEntity> cardAuthRepository)
        {
            _cardAuthRepository = cardAuthRepository;
        }
        /// <summary>
        /// 获取卡授权
        /// </summary>
        /// <param name="cardIds"></param>
        /// <returns></returns>
        public virtual IList<CardAuthEntity> GetCardAuthList(string  cardId )
        {
            if (cardId == null)
                return new List<CardAuthEntity>(); 
 
            var cardAuths = _cardAuthRepository.Table.Where(w=>w.CardUUID== cardId && !w.Deleted).ToList();

            return cardAuths;
        }
        /// <summary>
        /// 获取卡授权
        /// </summary>
        /// <param name="cardIds"></param>
        /// <returns></returns>
        public virtual IList<CardAuthEntity> GetCardAuthList(string[] cardIds)
        {
            if (cardIds == null || cardIds.Length == 0)
                return new List<CardAuthEntity>();

            var query = from p in _cardAuthRepository.Table
                        where cardIds.Contains(p.CardUUID) && !p.Deleted
                        select p;
            var cardAuths = query.ToList();            

            return cardAuths;
        }

        public virtual List<CardAuthEntity> GetAuthList(string cardId,string authType)
        {
            var query = from p in _cardAuthRepository.Table
                        where p.CardUUID == cardId && p.AuthType == authType && !p.Deleted
                        select p;
            var cardAuths = query.ToList();

            return cardAuths;
        }
    }
}
