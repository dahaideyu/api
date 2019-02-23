namespace Koowoo.Pojo
{
    public class ResponseModel
    {
        /// <summary>
        /// 操作消息【当Status不为 200时，显示详细的错误信息】
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 操作状态码，200为正常
        /// </summary>
        public string msg { get; set; }

        
    }

    /// <summary>
    /// WEBAPI通用返回泛型基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// 回传的结果
        /// </summary>
        public T data { get; set; }
    }
}
