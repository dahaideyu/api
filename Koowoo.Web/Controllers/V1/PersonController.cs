using Koowoo.Services;
using Koowoo.Pojo;
using System.Web.Http;
using Koowoo.Pojo.Request;
using Koowoo.Web.Common;
using Koowoo.Core.Extentions;
using System.Linq;
using System.Threading.Tasks;
using Koowoo.Core;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/person")]
    public class PersonController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IPersonService personService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICardService cardService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("test")]
        public IHttpActionResult GetTest(string cardNo)
        {
            var text = cardService.GetCardListByCardNo(cardNo);

            return Ok(new
            {
                code = 0,
                msg = "success",
                data = text
            });
        }

        /// <summary>
        /// 人员列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("admin:person:list")]
        public IHttpActionResult GetList([FromUri]PersonReq req)
        {
            if (null == req)
            {
                req = new PersonReq();
            }

            if (!req.ICCardNo.IsBlank())
            {
                var persons = cardService.GetCardListByCardNo(req.ICCardNo);
                req.PersonIds = persons;
            }

            if (!req.IDCardInternalNO.IsBlank())
            {
                var persons = cardService.GetCardListByCardNo(req.IDCardInternalNO);
                req.PersonIds = req.PersonIds.Union(persons).ToList<string>();
                
            }



            var table = personService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("info"), RequestAuthorize("admin:person:info")]
        public IHttpActionResult Get(string personId)
        {
            var dto = personService.GetByBaseId(personId);
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

        [HttpGet, Route("infoByCardNo"), RequestAuthorize("admin:person:info")]
        public IHttpActionResult GetByCardNo(string cardNo)
        {
            var dto = personService.GetByCardNo(cardNo);
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
        /// 添加人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("admin:person:add")]
        public ResponseModel Create([FromBody] PersonDto model)
        {
            personService.Create(model);
            return new ResponseModel();
        }


        /// <summary>
        /// 修改人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("update"), RequestAuthorize("admin:person:update")]
        public ResponseModel Update([FromBody] PersonDto model)
        {
            personService.Update(model);
            return new ResponseModel();
        }


        /// <summary>
        /// 修改人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("UpdateFaceImg"), RequestAuthorize("admin:person:update")]
        public ResponseModel UpdateFaceImg([FromBody] PersonDto model)
        {
            var persondto = personService.GetById(model.PersonUUID);
            persondto.IDCardImg = model.IDCardImg;
            persondto.FaceImg = model.FaceImg;
            personService.UpdateFaceImg(model);
            return new ResponseModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetFaceImg"), RequestAuthorize("admin:person:info")]
        public IHttpActionResult GetFaceImg(string personId)
        {
            var img = personService.GetFaceImgById(personId);
            if (img != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = ImgUtil.ImgToByt(img)
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
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetCertificateImg"), RequestAuthorize("admin:person:info")]
        public IHttpActionResult GetCertificateImg(string personId)
        {
            var img = personService.GetCertificateImgById(personId);
            if (img != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = ImgUtil.ImgToByt(img)
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
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetIDImg"), RequestAuthorize("admin:person:info")]
        public IHttpActionResult GetIDImgById(string personId)
        {
            var img = personService.GetIDImgById(personId);
            if (img != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = ImgUtil.ImgToByt(img)
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
        /// 删除人员信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("admin:person:delete")]
        public ResponseModel Delete([FromBody]DeleteDto dto)
        {
            personService.Delete(dto.ids);
            return new ResponseModel();
        }
    }
}
