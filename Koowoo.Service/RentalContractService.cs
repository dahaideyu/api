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
using Koowoo.Domain;
using System.Collections.Generic;

namespace Koowoo.Services
{
    public interface IRentalContractService
    {
        TableData GetList(QueryListReq req);
        IList<RentalContractListDto> GetList(string personId);
        RentalContractDto GetById(string doorId);
        RentalContractListDto GetByRoomId(string roomId);
        void Create(RentalContractDto config);
        void Update(RentalContractDto config);
        void Delete(string ids);
    }

    public class RentalContractService : IRentalContractService, IDependency
    {
        private readonly IRepository<RentalContractEntity> _contractRepository;
        private readonly IRepository<AreaEntity> _areaRepository;
        private readonly IRepository<PersonEntity> _personRepository;


        public RentalContractService(IRepository<RentalContractEntity> contractRepository, 
            IRepository<AreaEntity> areaRepository,
            IRepository<PersonEntity> personRepository)
        {
            _contractRepository = contractRepository;
            _areaRepository = areaRepository;
            _personRepository = personRepository;
        }

        /// <summary>
        /// 获取合同列表分页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public TableData GetList(QueryListReq req)
        {
            var query = _contractRepository.Table;

           
            query = query.OrderByDescending(o => o.DateFrom);
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

        /// <summary>
        /// 根据个人ID获取合同列表
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public IList<RentalContractListDto> GetList(string personId)
        {
            var query = _contractRepository.Table.Where(a=>a.PersonUUID == personId);
            

            query = query.OrderByDescending(o => o.DateFrom);
            var pagedList = query.ToList();

            var contractList = new List<RentalContractListDto>();
            foreach (var item in pagedList)
            {
                var area = item.Room!=null? _areaRepository.GetById(item.Room.AreaUUID):null;
                var dto = item.MapTo<RentalContractListDto>();
                //dto.DateFrom = item.DateFrom;
                //dto.DateTo = item.DateTo;
                dto.RoomFullPath = GetFormattedBreadCrumb(area);
                //  dto.RoomFullPath = GetFormattedBreadCrumb(area);
                contractList.Add(dto);
            }
            return contractList;
        }

        /// <summary>
        /// 根据合同ID获取合同
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public RentalContractDto GetById(string configId)
        {
            var dto = _contractRepository.GetById(configId);
            if (dto != null)
                return dto.MapTo<RentalContractDto>();
            else
                return null;
        }

       
        /// <summary>
        /// 根据房间号获取合同
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public RentalContractListDto GetByRoomId(string roomId)
        {
            var dateNow = DateTime.Now.Date;
            var dto = _contractRepository.Table.Where(a => a.RoomUUID == roomId && a.DateTo> dateNow).FirstOrDefault();
            if (dto != null)
            {
                var model = dto.MapTo<RentalContractListDto>();
                var area = dto.Room != null ? _areaRepository.GetById(dto.Room.AreaUUID) : null; 
                model.PersonName = dto.Person != null ? dto.Person.PersonName : "";
                model.RoomFullPath = GetFormattedBreadCrumb(area);
                return model;
            }
               
            else
                return null;
        }

        /// <summary>
        /// 添加合同
        /// </summary>
        /// <param name="dto"></param>
        public void Create(RentalContractDto dto)
        {
            var entity = dto.MapTo<RentalContractEntity>();
            entity.ContractUUID = Guid.NewGuid().ToString("N");
            _contractRepository.Insert(entity);

            var person = _personRepository.GetById(dto.PersonUUID);
            person.IsRenter = true;
            _personRepository.Update(person);

        }

        /// <summary>
        /// 修改合同
        /// </summary>
        /// <param name="dto"></param>
        public void Update(RentalContractDto dto)
        {
            var entity = _contractRepository.GetById(dto.ContractUUID);
            entity = dto.ToEntity(entity);
            _contractRepository.Update(entity);

            var person = _personRepository.GetById(dto.PersonUUID);
            person.IsRenter = true;
            _personRepository.Update(person);
        }

        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
            foreach (var item in idList1)
            {
                var model = _contractRepository.GetById(item);
                _contractRepository.Delete(model);
            }
        }

        /// <summary>
        /// 类别面包屑格式-类别名称
        /// </summary>
        /// <param name="area">类别</param>
        /// <param name="separator">分隔符号</param>
        /// <returns>面包屑格式</returns>
        public virtual string GetFormattedBreadCrumb(AreaEntity area, string separator = ">>")
        {
            if (area == null)
                throw new ArgumentNullException("element");

            string result = string.Empty;

            var alreadyProcessedElementIds = new List<string>() { };

            while (area != null &&  //not null
                                    //  !area.Deleted &&  //not deleted
                !alreadyProcessedElementIds.Contains(area.AreaUUID)) //prevent circular references
            {
                if (String.IsNullOrEmpty(result))
                {
                    result = area.ChineseName;
                }
                else
                {
                    result = string.Format("{0} {1} {2}", area.ChineseName, separator, result);
                }

                alreadyProcessedElementIds.Add(area.AreaUUID);
                if (area.AreaType == 9)
                    break;
                area = _areaRepository.Table.Where(a => a.Code == area.ParentCode).FirstOrDefault();

            }

            return result;
        }
    }
}
