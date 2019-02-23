using System.Web.Http;
using Koowoo.Services;
using Koowoo.Core.Extentions;
using Koowoo.Pojo;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 社区模块
    /// </summary>
    [RoutePrefix("api/community")]
    public class CommunityController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IAreaService areaService { get; set; }

        /// <summary>
        /// 获取社区列表（应该有社区管理页面，现在没有用到）
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [HttpGet, Route("list")] //, RequestAuthorize("sys:community:list")
        public IHttpActionResult GetList(int page = 1, int pageSize = 20, string keyword = "")
        {
            var table = areaService.GetCommunityList(page, pageSize, keyword);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }


        /// <summary>
        /// 社区选择下拉框（管理中用到，先选取社区在显示社区后的数据）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("select")]
        public IHttpActionResult GetSelectList()
        {
            var table = areaService.GetCommunityList();
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table.list
            });
        }

        /// <summary>
        /// 社区结构列表含社区、幢，单元，房间四级,（考虑取消该接口，实际应用中无用，通过选择社区后显示社区结构比较合理）  可能没有用到
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("treeAll")]
        public IHttpActionResult GetTree()
        {
            var result = areaService.GetCommunityTreeList();
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = result
            });
        }

        /// <summary>
        ///  根据社区ID获取树形结构（可能没有用到）
        /// </summary>
        /// <param name="communityId">社区ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [HttpGet, Route("communitylist")]
        public IHttpActionResult GetCommunityList(string communityId, string keyword = null)
        {
            var result = areaService.GetCommunityListBySearch(communityId, keyword);

            return Ok(new
            {
                code = 0,
                msg = "success",
                data = result
            });
        }


        /// <summary>
        ///  根据社区ID获取树形结构（同 GetCommunityList(string communityId, string keyword = null)）
        /// </summary>
        /// <param name="communityId">社区ID</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [HttpGet, Route("tree")]
        public IHttpActionResult GetTree(string communityId,string keyword=null)
        {
            AreaTreeDto result = areaService.GetCommunityTreeBySearch(communityId, keyword);            
            
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = result
            });
        }

        /// <summary>
        /// 根据社区ID获取房间（含幢、单元、房间），非树型，，，可能没有用到
        /// </summary>
        /// <param name="communityId">社区ID</param>
        /// <returns></returns>
        [HttpGet, Route("selectfull")]
        public IHttpActionResult GetRoomList(string communityId)
        {
            var result = areaService.GetCommunityTree2(communityId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = result
            });
        }

        /// <summary>
        /// 获取子类区域(可能没有用到)
        /// </summary>
        /// <param name="communityId">社区ID</param>
        /// <param name="parentCode">父级code</param>
        /// <returns></returns>
        [HttpGet, Route("listByParent")]
        public IHttpActionResult GetListByParent(string communityId,string parentCode)
        {
            var result = areaService.GetListByParentId(communityId, parentCode);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = result
            });
        }
    }
}
