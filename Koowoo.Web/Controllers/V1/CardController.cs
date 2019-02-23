using System;
using System.Web.Http;
using Koowoo.Core.Extentions;
using Koowoo.Pojo;
using Koowoo.Services;

namespace Koowoo.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/card")]
    public class CardController : BaseApiController
    {

        /// <summary>
        /// 
        /// </summary>
        public ICardService cardService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPersonService personService { get; set; }
        /// <summary>
        /// 用户的获得管理卡列表
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("listByManage")]  //, RequestAuthorize("admin:card:list")
        public IHttpActionResult GetManageList(string personId)
        {
            var table = cardService.GetManageCardListByPerson(personId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 根据用户ID获取卡列表
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("listByPerson")]  //, RequestAuthorize("admin:card:list")
        public IHttpActionResult GetList(string personId)
        {
            var table = cardService.GetCardListByPerson(personId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 办卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")] //, RequestAuthorize("admin:card:add")
        public IHttpActionResult Create([FromBody] CardDto model)
        {
            if (model.CardType == 0 || model.CardTypeOfLK == 0)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "数据字典丢失，请联系系统管理员！"
                });
            }

            if (model.CardNo.IsBlank())
            {
                return Ok(new
                {
                    code = 1,
                    msg = "卡号不能为空！"
                });
            }

            var person = personService.GetById(model.PersonUUID);
            if (person == null) {
                return Ok(new
                {
                    code = 1,
                    msg = "用户信息不存在！"
                });
            }

            var dto = cardService.GetCardByCardNo(model.CardNo);
            if (dto != null)
            {
                model.CardUUID = dto.CardUUID;

                if(model.PersonUUID!= dto.PersonUUID)
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "该卡已使用",
                    });
                }

                cardService.Update(model);

             

                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = dto
                });
            }
            else
            {
                model.CardUUID = Guid.NewGuid().ToString("N");                
                cardService.Create(model);

               
                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = model
                });
            }
        }


        /// <summary>
        /// 删除卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete")]  //, RequestAuthorize("sys:card:delete")
        public ResponseModel Delete(string cardId)
        {
            var result = cardService.Delete(cardId);
                     

            return result;
        }       
    }
}
