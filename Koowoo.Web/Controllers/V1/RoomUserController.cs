using Koowoo.Pojo;
using Koowoo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Koowoo.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/roomuser")]
    public class RoomUserController : BaseApiController
    {

        /// <summary>
        /// 
        /// </summary>
        public IRoomUserService roomUserService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRoomUserCardService roomUserCardService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICardAuthService cardAuthService { get; set; }
        public ICardService cardService { get; set; }


        /// <summary>
        /// 根据房间ID获取其他住户
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet, Route("listByRoom")]
        public IHttpActionResult GetList(string roomId)
        {
            var result = roomUserService.GetPersonListByRoomId(roomId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = result
            });
        }

        /// <summary>
        ///  入住信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("checkIn")]
        public ResponseModel CheckIn([FromBody] RoomUserDto model)
        {
            var orgiModel = roomUserService.GetByPersonId(model.PersonUUID);

            if (orgiModel != null && orgiModel.Status==1)
            {
                // 两次的入住的房间不同
                if (orgiModel.RoomUUID != model.RoomUUID)
                {
                    // var cardAuthList = roomUserCardService.GetUserCardsByUserID(orgiModel.RoomUserUUID);
                    var cardIds = roomUserCardService.GetUserCardsByUserID(orgiModel.RoomUserUUID);
                    var cardAuthList = cardAuthService.GetCardAuthList(cardIds);
                    if (cardAuthList != null && cardAuthList.Count > 0)
                    {
                        return new ResponseModel() { code = 1, msg = "请先销权后在进行入住！" };
                    }
                }

                ///更新卡的时间
                var updateCardIds = roomUserCardService.GetUserCardsByUserID(orgiModel.RoomUserUUID);
                if (updateCardIds != null)
                {
                    var updateCardIdsList = updateCardIds.ToList();
                    var entitys = cardService.GetCardByIds(updateCardIdsList);
                    foreach (var item in entitys)
                    {
                        item.ValidFrom = model.LiveDate;
                        item.ValidTo = model.LeaveDate;

                    }
                    cardService.UpdateList(entitys);
                }
            }

            roomUserService.AddOrUpdate(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("checkOut")]
        public ResponseModel CheckOut([FromBody] RoomUserDto model)
        {
            var orgiModel = roomUserService.GetByPersonId(model.PersonUUID);

            if (orgiModel == null)
            {
                return new ResponseModel() { code = 1, msg = "未找到入住信息！" };
            }

            roomUserService.AddOrUpdate(model);
            return new ResponseModel();
        }


        /// <summary>
        /// 根据用户ID获取入住信息
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("infoByPersonId")]
        public IHttpActionResult GetByPersonId(string personId)
        {
            var dto = roomUserService.GetByPersonId(personId);
            if (dto != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "",
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
    }
}
