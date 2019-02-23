using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Core.Pager;
using Koowoo.Domain;
using Koowoo.Data.Interface;
using Koowoo.Pojo;
using Koowoo.Pojo.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koowoo.Services
{
    public interface IDeviceService
    {
        TableData GetList(QueryListReq req);
        DeviceDto GetById(string deviceId);
        void Create(DeviceDto config);
        void Update(DeviceDto config);
        void Delete(string ids);
        
    }

    public class DeviceService : IDeviceService, IDependency
    {
        private readonly IRepository<DeviceEntity> _deviceRepository;
        private readonly IAreaService _areaService;

        public DeviceService(IRepository<DeviceEntity> configRepository, IAreaService areaService)
        {
            _deviceRepository = configRepository;
            _areaService = areaService;
        }

        public TableData GetList(QueryListReq req)
        {
            var query = _deviceRepository.Table.Where(a=>!a.Deleted);

            if (!req.communityId.IsBlank())
            {
                query = query.Where(a => a.CommunityUUID == req.communityId);
            }

            if (!req.keyword.IsBlank())
            {
                query = query.Where(a => a.DeviceName.Contains(req.keyword));
            }

            query = query.OrderByDescending(o => o.CreateTime);
            var pagedList = query.ToPagedList(req.page, req.pageSize);

            var deviceList = new List<DeviceListDto>();
            foreach (var item in pagedList)
            {
                var dto = item.MapTo<DeviceListDto>(); 
                dto.DeviceTypeName = item.DeviceTypeDict?.DictName??"";
                var community = _areaService.GetById(item.CommunityUUID);
                dto.CommunityName = community != null ? community.ChineseName : "";
                deviceList.Add(dto);
            }

            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = deviceList
            };
        }

        public DeviceDto GetById(string deviceId)
        {
            var entity = _deviceRepository.GetById(deviceId);
            if (entity != null)
            {
                var dto = entity.MapTo<DeviceDto>();
                dto.DeviceTypeName = entity.DeviceTypeDict?.DictName??string.Empty;
                return dto;
            }                
            else
            {
                return null;
            }
        }

        public void Create(DeviceDto config)
        {
            var entity = config.MapTo<DeviceEntity>();
            entity.DeviceUUID = Guid.NewGuid().ToString("N");
            entity.CreateTime = DateTime.Now;
            entity.Deleted = false;
            entity.SyncStatus = false;
            entity.SyncVersion = 0;       

            _deviceRepository.Insert(entity);

           // Synchronization(entity);
        }

        public void Update(DeviceDto config)
        {
            var entity = _deviceRepository.GetById(config.DeviceUUID);
            entity = config.ToEntity(entity);           
            entity.UpdateTime = DateTime.Now;
            entity.SyncStatus = false;
            _deviceRepository.Update(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p).ToList();
            foreach (var item in idList1)
            {
                var entity = _deviceRepository.GetById(item);
                entity.Deleted = true;
                entity.SyncStatus = false;
                _deviceRepository.Update(entity);

            }
        }

      
    }
}
