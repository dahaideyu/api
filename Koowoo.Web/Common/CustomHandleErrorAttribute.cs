using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Koowoo.Core;
using System.Net.Http.Formatting;

namespace Koowoo.Web.Common
{
    /// <summary>
    /// 错误过滤器
    /// </summary>
    public class CustomHandleErrorAttribute:ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写OnException
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var code = 9999;
            var message = "服务器异常，请求失败!请联系管理员";

            var requestParameters = JsonHelper.SerializeObject(actionExecutedContext.ActionContext.ActionArguments.Values);
           // Core.Log.Error(actionExecutedContext.Exception);
            Core.Log.Debug(this.GetType().ToString(), "异常信息：" + actionExecutedContext.Exception.Message + "\r\n堆栈信息：" + actionExecutedContext.Exception.StackTrace + "\r\n请求参数为" + requestParameters);
            //获取action的请求参数
            actionExecutedContext.Response = GetResponseMessage(code, message);        
        }


        private HttpResponseMessage GetResponseMessage(int code, string message)
        {
            var resultModel = new ApiModelsBase() { code = code, msg = message };

            return new HttpResponseMessage()
            {
                Content = new ObjectContent<ApiModelsBase>(
                    resultModel,
                    new JsonMediaTypeFormatter(),
                    "application/json"
                    )
            };
        }
    }

    internal class ApiModelsBase
    {
        public int code { get; set; }
        public string msg { get; set; }
    }
}
