using Koowoo.Pojo;
using Koowoo.Pojo.System;
using Koowoo.Services;
using Koowoo.Services.Auth;
using Koowoo.Services.System;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/sys")]
    public class SysController : BaseApiController
    {
        public IBackupDataService backupservice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUserService userService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IUserTokenService tokenService { get; set; }
        [AllowAnonymous]
        [HttpGet, Route("SendMail")]
        public IHttpActionResult SendMailAddress()
        {
            var resultsend = backupservice.BackupSendMail();
            if (resultsend.ResultType != Core.OperationResultType.Success)
            {
             return   Ok(new
                {
                    code = 1,
                    msg = "fail"+resultsend.Message,

                });
            }
            return Ok(new
            {
                code = 0,
                msg = "success",

            });
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("BackupDataBase")]
        public IHttpActionResult BackupDataBase([FromBody] UserLoginDto model)
        {
            try
            {
                var serverIp = "47.96.31.20";
                var serverIp2 = "127.0.0.1";
                var connectionstr = ConfigurationManager.AppSettings["SqlServiceEntities"];
                var fileName = DateTime.Now.ToFileTimeUtc().ToString() + ".bak";
                var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/download/" + fileName);
               
                //  var result = backupservice.ExeBackup(serverIp2, "community", "61777678@qq.com", "Community", FilePath);
                var result = backupservice.ExeBackup(connectionstr, "Community", FilePath);
                var stream = new FileStream(FilePath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Wep Api Demo File.bak"
                };
                return Ok(new
                {
                    code = 0,
                    msg = "success",

                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "fail",
                    data = ex.Message
                });
            }




        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IHttpActionResult Login([FromBody] UserLoginDto model)
        {
            var result = userService.Login(model.username, model.password);
            if (result.code == 0)
            {
                //userService.SignIn(result.data);
                var userId = result.data.UserID;
                var token = Guid.NewGuid().ToString();
                var dtNow = DateTime.Now;
                #region 将身份信息保存票据表中，验证当前请求是否是有效请求
                //判断此用户是否存在票据信息
                var userToken = tokenService.GetTicketByUserId(userId);
                if (userToken != null)
                {
                    //清空重置
                    tokenService.DeleteByUserId(userId);
                }

                UserTokenDto ticket = new UserTokenDto();
                ticket.UserID = userId;
                ticket.Token = token;
                ticket.CreateDate = dtNow;
                ticket.ExpireDate = dtNow.AddDays(2); //30分钟过期
                tokenService.Insert(ticket);
                #endregion

                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = token
                });
            }
            else
            {
                return Ok(new
                {
                    code = result.code,
                    msg = result.msg
                });
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("logout")]
        public ResponseModel Logout(string token)
        {
            try
            {
                tokenService.Delete(token);
            }
            catch
            {

            }
            return new ResponseModel();
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("userinfo")]
        public IHttpActionResult MyInfo()
        {
            int UserId = CurrentUserUtils.CurrentPrincipal.UserId;

            try
            {
                var user = userService.GetById(UserId);
                if (user == null)
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "数据不存在"
                    });
                }

                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = user
                });
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);
            }
        }

        /// <summary>
        /// 获取登录用户菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("menu")]
        public IHttpActionResult MyMenus()
        {
            try
            {
                int userId = CurrentUserUtils.CurrentPrincipal.UserId;
                Hashtable hash = new Hashtable();

                hash["user"] = userService.GetById(userId);
                hash["permissions"] = userService.GetMyPermissions(userId);
                hash["menuList"] = userService.GetMyMenus(userId);

                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = hash
                });

            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("modifypwd")]
        public ResponseModel ChangePwd([FromBody] ChangePasswordDto model)
        {
            int userId = CurrentUserUtils.CurrentPrincipal.UserId;
            var result = userService.ChangePwd(userId, model.opassword, model.npassword);
            return result;
        }
    }
}
