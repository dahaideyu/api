using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Core.Pager;
using Koowoo.Data.Interface;
using Koowoo.Domain;
using Koowoo.Pojo;
using Koowoo.Pojo.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Services
{
    public interface ISyncLogServie
    {
        void InsertSyncLog(SyncLogEntity entity);

        SyncLogEntity GetById(string syncId);

        TableData GetList(QueryDateReq req);

    }

    public class SyncLogService: ISyncLogServie, IDependency
    {
        private readonly IRepository<SyncLogEntity> _syncRepository;


        public SyncLogService(IRepository<SyncLogEntity> syncRepository)
        {
            _syncRepository = syncRepository;
        }

        public void InsertSyncLog(SyncLogEntity entity)
        {
            if (entity.SyncId.IsBlank())
                entity.SyncId = Guid.NewGuid().ToString("N");
            _syncRepository.Insert(entity);
        }

        public SyncLogEntity GetById(string syncId)
        {
            var entity = _syncRepository.GetById(syncId);
            return entity;
        }

        public TableData GetList(QueryDateReq req)
        {
            var query = _syncRepository.Table;

            if (!req.communityId.IsBlank())
            {
                query = query.Where(a => a.CommunityId == req.communityId);
            }

            if (req.beginTime.HasValue)
            {
                query = query.Where(t => t.SyncTime >= req.beginTime.Value);
            }

            if (req.endTime.HasValue)
            {
                query = query.Where(t => t.SyncTime <= req.endTime.Value);
            }

            if (req.status.HasValue)
            {
                query = query.Where(a => a.SyncResult == req.status.Value);
            }


            query = query.OrderByDescending(o => o.SyncTime);
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
    }
}
