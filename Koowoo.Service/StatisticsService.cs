using Koowoo.Core;
using Koowoo.Data;
using Koowoo.Data.Interface;
using Koowoo.Domain;
using Koowoo.Pojo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Services
{
    public interface IStatisticsService
    {
        ChartDto GetCommunnityEntryState(DateTime startDate, DateTime dateTime);

        ChartDto GetCommunnityState();
    }

    public class StatisticsService : IStatisticsService, IDependency
    {
        private readonly IDbContext _dbContext;
        private readonly IRepository<RoomUserEntity> _roomUserRepository;
        private readonly IRepository<RoomEntity> _roomRepository;
        private readonly IRepository<AreaEntity> _areaRepository;

        public StatisticsService(IDbContext dbContext, IRepository<RoomUserEntity> roomUserRepository, IRepository<RoomEntity> roomRepository, IRepository<AreaEntity> areaRepository)
        {
            _dbContext = dbContext;
            _roomUserRepository = roomUserRepository;
            _roomRepository = roomRepository;
            _areaRepository = areaRepository;
        }

        public ChartDto GetCommunnityEntryState(DateTime startDate, DateTime endDate)
        {
            var querySql = @"select convert(char(8),EntryTime,112) StateDate, count(*) StateNum,CommunityId,
                        (select ChineseName from biz_Area where AreaUUID= t.CommunityId and Deleted=0) CommunityName
                        from biz_EntryHistory t";
            querySql += " where convert(char(8),EntryTime,112)>=" + startDate.ToString("yyyyMMdd");
            querySql += " and convert(char(8),EntryTime,112)<=" + endDate.ToString("yyyyMMdd");

            querySql += "group by convert(char(8),EntryTime,112),CommunityId";


            var queryList = _dbContext.SqlQuery<MappingData>(querySql).ToList();

            var columns = new List<string>();
            var rows = new List<Dictionary<string, object>>();

            columns.Add("日期");

            var communityList = queryList.Select(p => p.CommunityName).Distinct().ToList();
            columns = columns.Union(communityList).ToList();

            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                var dict = new Dictionary<string, object>();

                
                dict.Add("日期", dt.ToString("MM/dd"));
                foreach(var community in communityList)
                {
                    var item = queryList.Where(a => a.CommunityName == community && a.StateDate == dt.ToString("yyyyMMdd")).FirstOrDefault();
                    dict.Add(community, item?.StateNum??0);
                }
                rows.Add(dict);
            }

            var chart = new ChartDto() {
                columns = columns,
                rows = rows
            };
            return chart;
        }

        public ChartDto GetCommunnityState()
        {
            var communityList = from p in _areaRepository.Table
                                where p.AreaType == 8 && !p.Deleted
                                select new
                                {
                                    CommunityUUID = p.AreaUUID,
                                    CommunityName = p.ChineseName
                                };

            var rows = new List<Dictionary<string, object>>();

            foreach (var community in communityList)
            {
                var dict = new Dictionary<string, object>();

                int roomCount = GetRoomCountByCommuity(community.CommunityUUID);
                int usedRoomCount = GetRentRoomCountByCommuity(community.CommunityUUID);
                dict.Add("小区名称", community.CommunityName);
                dict.Add("房间数", roomCount);
                dict.Add("已租房间数", usedRoomCount);
                dict.Add("居住人数", GetRoomPersonCountByCommuity(community.CommunityUUID));
                dict.Add("入住率", Math.Round((double)usedRoomCount / roomCount, 2));
                rows.Add(dict);
            }

            var chartData = new ChartDto()
            {
                columns = new List<string>() { "小区名称", "房间数", "已租房间数", "居住人数" }, //, "入住率"
                rows = rows
            };

            return chartData;
        }

        /// <summary>
        /// 居住人数
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        private int GetRoomPersonCountByCommuity(string communityId)
        {
            var num = _roomUserRepository.Table.Where(a => a.CommunityUUID == communityId && !a.Deleted && a.Status == 1).Count();

            return num;
        }


        /// <summary>
        /// 出租房间数
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        private int GetRentRoomCountByCommuity(string communityId)
        {
            var num = _roomUserRepository.Table.Where(a => a.CommunityUUID == communityId && !a.Deleted && a.Status == 1).Select(a => a.RoomUUID).Distinct().Count();

            return num;
        }

        private int GetRoomCountByCommuity(string communityId)
        {
            var num = _roomRepository.Table.Where(a => a.CommunityUUID == communityId && !a.Deleted).Count();

            return num;
        }
    }

    internal class MappingData
    {
        public string StateDate { get; set; }
        public int StateNum { get; set; }
        public string CommunityId { get; set; }
        public string CommunityName { get; set; }
    }


}
