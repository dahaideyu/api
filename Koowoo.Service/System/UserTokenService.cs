using Koowoo.Data;
using Koowoo.Domain.System;
using Koowoo.Pojo.System;
using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koowoo.Services.System
{
    public interface IUserTokenService
    {
        UserTokenDto GetTicket(string token);

        UserTokenDto GetTicketByUserId(int userId);

        void Insert(UserTokenDto token);

        void Update(UserTokenDto token);

        void Delete(string token);

        void DeleteByUserId(int userId);
    }


    public class UserTokenService : IUserTokenService, IDependency
    {
        private readonly IRepository<UserTokenEntity> _userTokenRepository;

        public UserTokenService(IRepository<UserTokenEntity> userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }


        /// <summary>
        /// 根据token后去登录凭证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserTokenDto GetTicket(string token)
        {
            using (var kcx = new KoowooContext())
            {
                var entity = kcx.Set<UserTokenEntity>().FirstOrDefault(t => t.Token == token);

                return entity.MapTo<UserTokenDto>();
            }
            //    var query = from p in _userTokenRepository.Table
            //                where p.Token == token
            //                orderby p.CreateDate descending
            //                select p;
            //var entity = query.FirstOrDefault();

            //return entity.MapTo<UserTokenDto>();
        }


        /// <summary>
        /// 根据用户ID获取登录凭证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserTokenDto GetTicketByUserId(int userId)
        {
            var query = from p in _userTokenRepository.Table
                        orderby p.CreateDate descending
                        where p.UserID == userId
                        select p;
            var entity = query.FirstOrDefault();

            return entity.MapTo<UserTokenDto>();
        }


        /// <summary>
        /// 插入登录凭证
        /// </summary>
        /// <param name="model"></param>
        public void Insert(UserTokenDto model)
        {
            UserTokenEntity entity = model.ToEntity();
            _userTokenRepository.Insert(entity);
        }


        /// <summary>
        /// 更新登录凭证
        /// </summary>
        /// <param name="model"></param>
        public void Update(UserTokenDto model)
        {
            UserTokenEntity entity = _userTokenRepository.Table.Where(a => a.UserID == model.UserID).FirstOrDefault();
            entity = model.ToEntity(entity);
            _userTokenRepository.Update(entity);
        }


        /// <summary>
        /// 根据tokon删除登录凭证
        /// </summary>
        /// <param name="token"></param>
        public void Delete(string token)
        {
            _userTokenRepository.Delete(ut => ut.Token == token);
        }

        /// <summary>
        /// 根据用户ID删除登录凭证
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteByUserId(int userId)
        {
            _userTokenRepository.Delete(ut => ut.UserID == userId);
        }
    }
}
