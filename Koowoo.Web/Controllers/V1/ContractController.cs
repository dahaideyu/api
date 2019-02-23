using Koowoo.Services;
using Koowoo.Pojo;
using System.Web.Http;
using Koowoo.Pojo.Request;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/contract")]
    public class ContractController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IRentalContractService contractService { get; set; }

        /// <summary>
        /// 根据用户ID获取合同列表
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("listByPerson")] //, RequestAuthorize("sys:contract:list")
        public IHttpActionResult GetList(string personId)
        {
            var table = contractService.GetList(personId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 获取合同信息
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet, Route("info")] //, RequestAuthorize("sys:contract:info")
        public IHttpActionResult GetByContractId(string contractId)
        {
            var dto = contractService.GetById(contractId);
            if (dto != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = dto
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    msg = "数据不存在"
                });
            }
        }

        /// <summary>
        /// 根据房间号获取合同信息
        /// </summary>
        /// <param name="roomId">房间ID</param>
        /// <returns></returns>
        [HttpGet, Route("infoByRoomId")]//, RequestAuthorize("sys:contract:info")
        public IHttpActionResult GetByRoomId(string roomId)
        {
            var dto = contractService.GetByRoomId(roomId);
            if (dto != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = dto
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    msg = "数据不存在"
                });
            }
        }

        /// <summary>
        /// 添加合同
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add")]//, RequestAuthorize("sys:contract:add")
        public ResponseModel Create([FromBody] RentalContractDto model)
        {
            var contract = contractService.GetByRoomId(model.RoomUUID);
            if (contract != null)
            {
                // 如果现有的租房结束时间大于新租约的开始时间则提示租赁到期时间
                if (contract.DateTo >= model.DateFrom)
                {
                    return new ResponseModel() { code = 1, msg = "房间到期时间为"+contract.DateTo.Value.ToString("yyyy-MM-dd")+"，请重新选择租期" };
                }
            }

            contractService.Create(model);
            return new ResponseModel();

        }

        /// <summary>
        /// 修改合同
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("update")] //, RequestAuthorize("sys:contract:update")
        public ResponseModel Update([FromBody] RentalContractDto model)
        {
            contractService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete")] //, RequestAuthorize("sys:contract:delete")
        public ResponseModel Delete([FromBody]DeleteDto dto)
        {
            contractService.Delete(dto.ids);
            return new ResponseModel();
        }
    }
}
