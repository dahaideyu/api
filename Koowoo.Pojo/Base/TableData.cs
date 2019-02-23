namespace Koowoo.Pojo
{
    /// <summary>
    /// table的返回数据
    /// </summary>
    public class TableData
    {     
        /// <summary>
        /// 当前页
        /// </summary>
        public int currPage;

        /// <summary>
        /// 每页纪录数
        /// </summary>
        public int pageSize;

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int totalCount;

        /// <summary>
        /// 总页数
        /// </summary>
        public int pageTotal;

        /// <summary>
        /// 数据内容
        /// </summary>
        public dynamic list;
    }
}