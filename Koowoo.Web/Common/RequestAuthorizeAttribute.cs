using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Koowoo.Core;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Net;
using System.Text;
using System.Web.Helpers;
using Koowoo.Services.Auth;
using System.Threading;
using Koowoo.Services;
using Koowoo.Services.System;
using Koowoo.Core.Extentions;

namespace Koowoo.Web.Common
{
    /// <summary>
    /// 权限属性
    /// </summary>
    public class RequestAuthorizeAttribute: AuthorizeAttribute
    {
        private readonly string _accessName;

        /// <summary>
        /// 
        /// </summary>       
        public RequestAuthorizeAttribute( ):this("")
        {
          
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public RequestAuthorizeAttribute(string name)
        {
            this._accessName = name;
        }

       

        /// <summary>
        /// 重写基类的验证方式，加入我们自定义的Ticket验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
            if (isAnonymous)
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                var authHeader = from h in actionContext.Request.Headers where h.Key == "token" select h.Value.FirstOrDefault();
                if (authHeader != null)
                {
                    string token = authHeader.FirstOrDefault();

                    if (!string.IsNullOrEmpty(token))
                    {
                        //解密用户ticket,并校验用户名密码是否匹配

                        var userService = ContainerManager.Resolve<IUserService>();
                        var tokenService = ContainerManager.Resolve<IUserTokenService>();

                        var userToken = tokenService.GetTicket(token);

                        if (userToken != null)
                        {
                            var user = userService.GetById(userToken.UserID);
                            var permsList = userService.GetMyPermissions(user.UserID);
                            var perms = permsList!=null && permsList.Count>0? string.Join(",", permsList.ToArray()):"";
                            ClientUserData clientUserData = new ClientUserData()
                            {
                                UserId = user.UserID,
                                UserName = user.UserName,
                                UserPermission = perms
                            };

                            var MyPrincipal = new UserPrincipal(clientUserData);


                            Thread.CurrentPrincipal = MyPrincipal;

                            if (HttpContext.Current != null)
                                HttpContext.Current.User = MyPrincipal;

                            if (!_accessName.IsBlank())
                            {
                                if (!MyPrincipal.HasPermission(_accessName))
                                {
                                    GetResponseMessage(actionContext, 9997, "没有操作权限");
                                }
                                else
                                {
                                    base.OnAuthorization(actionContext);
                                }
                            }
                            else {
                                base.OnAuthorization(actionContext);
                            }
                        }
                        else
                        {
                            GetResponseMessage(actionContext, 9998, "token已失效，请重新登陆！");
                        }
                    }
                    else
                    {
                        GetResponseMessage(actionContext, 9997, "token不能为空！");
                    }
                }
                else
                {
                    //没有获取到header头内容
                    //HandleUnauthorizedRequest(actionContext);
                    GetResponseMessage(actionContext, 1, "未获取到Header！");
                }
            }                  
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="actioncontext"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        protected virtual void GetResponseMessage(HttpActionContext actioncontext,int code,string msg)
        {
           // base.HandleUnauthorizedRequest(actioncontext);

            var response = actioncontext.Response = actioncontext.Response ?? new HttpResponseMessage();
            //response.StatusCode = HttpStatusCode.Forbidden;
            var content = new
            {
                code = code,
                msg = msg
            };
            response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="actioncontext"></param>
        //protected virtual void UnLogin(HttpActionContext actioncontext)
        //{
        //  //  base.HandleUnauthorizedRequest(actioncontext);

        //    var response = actioncontext.Response = actioncontext.Response ?? new HttpResponseMessage();
        //    //response.StatusCode = HttpStatusCode.Forbidden;
        //    var content = new
        //    {
        //        code = 9998,
        //        success = "未登录，或者token已失效！"
        //    };
        //    response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");
        //}
    }
}