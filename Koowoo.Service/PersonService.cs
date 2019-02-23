using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Core.Pager;
using Koowoo.Data;
using Koowoo.Data.Interface;
using Koowoo.Domain;
using Koowoo.Domain.System;
using Koowoo.Pojo;
using Koowoo.Pojo.Enum;
using Koowoo.Pojo.Request;
using Koowoo.Services.LkbResponse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Koowoo.Services
{
    public interface IPersonService
    {
        TableData GetList(PersonReq req);
        PersonDto GetById(string personId);
        void Create(PersonDto dto);
        void Update(PersonDto dto);
        void UpdateFaceImg(PersonDto dto);
        PersonDto GetByBaseId(string personId);
        PersonDto GetByCardNo(string cardNo);
        void Delete(string ids);
        Task<Image> GetCertificateImgByIdAsync(string personId);
        Task<Image> GetFaceImgByIdAsync(string personId);
        Task<Image> GetIDImgByIdAsync(string personId);
        Image GetCertificateImgById(string personId);
        Image GetFaceImgById(string personId);
        Image GetIDImgById(string personId);

        IList<PersonEntity> GetSyncPersonList();
    }

    public class PersonService : IPersonService, IDependency
    {
        private readonly IRepository<PersonEntity> _personRepository;
        private readonly IRepository<RenterEntity> _renterRepository;
        private readonly IRepository<AreaEntity> _areaRepository;
        private readonly IRepository<DictEntity> _dictRepository;
        private readonly IRepository<RoomEntity> _roomRepository;
        private readonly IRepository<RoomUserEntity> _roomUserRepository;
        private readonly IRenterService _renterService;
        private readonly ISyncLogServie _syncService;
        private readonly IDbContext _dbContext;


        public PersonService(IRepository<PersonEntity> personRepository,
            IRepository<AreaEntity> areaRepository,
            IRepository<RenterEntity> renterRepository,
            IRenterService renterService,
            ISyncLogServie syncService,
            IRepository<RoomEntity> roomRepository,
            IRepository<RoomUserEntity> roomUserRepository,
            IDbContext dbContext,
            IRepository<DictEntity> dictRepository)
        {
            _personRepository = personRepository;
            _renterRepository = renterRepository;
            _areaRepository = areaRepository;
            _renterService = renterService;
            _dictRepository = dictRepository;
            _roomRepository = roomRepository;
            _syncService = syncService;
            _roomUserRepository = roomUserRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 取消无用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //public TableData GetList222(PersonReq req)
        //{
        //    var query = _personRepository.Table.Where(a => !a.Deleted);

        //    if (req.AreaUUID.IsNotBlank())
        //    {
        //        if (req.AreaUUID == "uncheckin")
        //        {
        //            // 未入住
        //            query = query.Where(a => a.IsLived==0 && a.PersonType != 273);
        //        }
        //        else if (req.AreaUUID == "managecard")
        //        {
        //            query = query.Where(a => a.PersonType == 273);
        //        }
        //        else
        //        {
        //            var area = _areaRepository.GetById(req.AreaUUID);
        //            if (area != null)
        //            {
        //                query = from p in _personRepository.Table
        //                        join ru in _roomUserRepository.Table on p.PersonUUID equals ru.PersonUUID
        //                        join r in _roomRepository.Table on ru.RoomUUID equals r.RoomUUID
        //                        join ar in _areaRepository.Table on r.AreaUUID equals ar.AreaUUID
        //                        where !p.Deleted && !ru.Deleted
        //                        && ar.Code.StartsWith(area.Code) && ru.Status == 1
        //                        orderby p.CreateTime descending
        //                        select p;
        //            }
        //        }
        //    }

        //    if (!req.PersonName.IsBlank())
        //    {
        //        query = query.Where(a => a.PersonName.Contains(req.PersonName));
        //    }

        //    if (req.BirthDay.HasValue && req.BirthDay.Value != DateTime.MinValue)
        //    {
        //        query = query.Where(b => b.Birthday == req.BirthDay.Value);
        //    }

        //    if (!req.IDCardNo.IsBlank())
        //    {
        //        query = query.Where(a => a.IDCardNo.Contains(req.IDCardNo));
        //    }

        //    if (req.Sex.HasValue)
        //    {
        //        query = query.Where(a => a.Sex == req.Sex.Value);
        //    }

        //    if (!req.PhoneNo.IsBlank())
        //    {
        //        query = query.Where(a => a.PhoneNo.Contains(req.PhoneNo));
        //    }

        //    if (!req.RegAddress.IsBlank())
        //    {
        //        query = query.Where(a => a.RegAddress.Contains(req.RegAddress));
        //    }

        //    if (!req.IDCardInternalNO.IsBlank() || !req.ICCardNo.IsBlank())
        //    {
        //        if (req.PersonIds != null && req.PersonIds.Count > 0)
        //        {
        //            query = query.Where(a => req.PersonIds.Contains(a.PersonUUID));
        //        }
        //        else
        //        {
        //            query = query.Where(a => a.PersonUUID.Contains("unknow"));
        //        }
        //    }



        //    if (req.IsRenter)
        //        query = query.Where(a => a.IsRenter);

        //    if (!req.communityId.IsBlank())
        //    {
        //        query = query.Where(a => a.CommunityUUID == req.communityId);
        //    }

        //    if (req.DateFrom.HasValue)
        //        query = query.Where(b => req.DateFrom.Value <= b.CreateTime);
        //    if (req.DateTo.HasValue)
        //        query = query.Where(b => req.DateTo.Value >= b.CreateTime);

        //    query = query.OrderByDescending(o => o.CreateTime);
        //    var pagedList = query.ToPagedList(req.page, req.pageSize);

        //    var personList = new List<PersonListDto>();
        //    foreach (var item in pagedList)
        //    {
        //        var dto = new PersonListDto();
        //        dto.PersonUUID = item.PersonUUID;
        //        dto.PersonName = item.PersonName;
        //        dto.IDCardNo = item.IDCardNo;
        //        dto.PhoneNo = item.PhoneNo;
        //        dto.SexName = item.SexDict?.DictName ?? string.Empty;
        //        dto.NationName = item.NationDict?.DictName ?? string.Empty;
        //        dto.BirthdayVal = item.Birthday.ToString("yyyy-MM-dd");
        //        dto.ValidFromVal = item.ValidFrom.HasValue ? item.ValidFrom.Value.ToString("yyyy-MM-dd") : string.Empty;
        //        dto.ValidToVal = item.ValidTo.HasValue ? item.ValidTo.Value.ToString("yyyy-MM-dd") : string.Empty;
        //        dto.RegAddress = item.RegAddress;
        //        dto.IsLocalVal = item.IsLocal == 1 ? "是" : "否";
        //        dto.IsRenterVal = item.IsRenter ? "是" : "否";
        //        dto.CreateTimeVal = item.CreateTime.ToString("yyyy-MM-dd");
        //        var community = _areaRepository.GetById(item.CommunityUUID);
        //        dto.CommunityName = community != null ? community.ChineseName : "";
        //        personList.Add(dto);
        //    }
        //    return new TableData
        //    {
        //        currPage = req.page,
        //        pageSize = req.pageSize,
        //        pageTotal = pagedList.TotalPageCount,
        //        totalCount = pagedList.TotalItemCount,
        //        list = personList
        //    };
        //}

        public TableData GetList(PersonReq req)
        {
            var areaCode = "";
            var listType = "";
            if (req.AreaUUID.IsNotBlank())
            {
                if (req.AreaUUID == "uncheckin")
                {
                    // 未入住
                    listType = "uncheckin";
                }
                if (req.AreaUUID == "isLeave")
                {
                    // 未入住
                    listType = "isLeave";
                }
                else if (req.AreaUUID == "managecard")
                {
                    listType = "managecard";
                }
                else
                {
                    var area = _areaRepository.GetById(req.AreaUUID);
                    if (area != null)
                    {
                        areaCode = area.Code;
                    }
                }
            }

            string showField = "p.PersonUUID,p.PersonName,p.IDCardNo,p.Sex,p.Nation,p.BirthDay,p.ValidFrom,p.ValidTo,p.RegAddress,p.IsLocal,";
            showField += "p.IsRenter, p.CommunityUUID,p.CreateTime, p.PhoneNo,r.InsurCompany,";
            showField += "ru.FamilyRelation,room.RoomName";

            var querySql = @"select {0} from biz_Person p "
                          + " left join biz_Renter r on p.personuuid = r.personuuid"
                           + " left join biz_RoomUser ru on p.PersonUUID = ru.PersonUUID"
                         + " left join biz_Room room on room.RoomUUID = ru.RoomUUID"
                        + " left join biz_Area area on room.AreaUUID = area.AreaUUID"
                        + " where p.Deleted = 0 and(room.Deleted = 0 or room.Deleted is null) ";

         

            if (!string.IsNullOrEmpty(areaCode))
            {
                querySql += " and area.Code like '" + areaCode + "%'";
            }

            if (listType == "uncheckin")
            {
                querySql += " and p.IsLived =0 and (p.PersonType != 273 or P.PersonType IS NULL)";
            }

            if (listType == "isLeave")
            {
                querySql += " and p.IsLived =2";
            }
            else
            {
                querySql += " and (ru.Status = 1 or ru.Status is null)";
            }

            if (listType == "managecard")
            {
                querySql += " and p.PersonType = 273";
            }            

            if (!req.PersonName.IsBlank())
            {
                querySql += string.Format(" and p.personName like '%{0}%'", req.PersonName);
            }

            if (req.BirthDay.HasValue && req.BirthDay.Value != DateTime.MinValue)
            {
                querySql += string.Format("and p.Birthday='{0}'", req.BirthDay.Value);
            }

            if (!req.IDCardNo.IsBlank())
            {
                querySql += string.Format(" and p.IDCardNo like '%{0}%'", req.IDCardNo);
            }

            if (req.Sex.HasValue)
            {
                querySql += string.Format("and p.sex={0}", req.Sex.Value);
            }

            if (!req.PhoneNo.IsBlank())
            {
                querySql += string.Format(" and p.PhoneNo like '%{0}%'", req.PhoneNo);
            }

            if (!req.RegAddress.IsBlank())
            {
                querySql += string.Format(" and p.RegAddress like '%{0}%'", req.RegAddress);
            }

            if (!req.IDCardInternalNO.IsBlank() || !req.ICCardNo.IsBlank())
            {
                //  query = query.Where(a => req.PersonIds.Contains(a.PersonUUID));
                //if (req.PersonIds != null && req.PersonIds.Count > 0)
                //{
                querySql += string.Format(" and p.PersonUUID IN ({0})", Utils.Array2Strin(req.PersonIds));
                //}                
            }

            if (req.IsRenter)
            {
                querySql += " and p.IsRenter=1";
            }

            if (!req.communityId.IsBlank())
            {
                querySql += string.Format(" and p.CommunityUUID='{0}'", req.communityId);
            }

            if (req.DateFrom.HasValue)
                querySql += string.Format(" and p.CreateTime>='{0}'",req.DateFrom.Value);
            if (req.DateTo.HasValue)
                querySql += string.Format(" and p.CreateTime<='{0}'", req.DateTo.Value);

            querySql += " order by p.CreateTime desc";

            var query = _dbContext.SqlQuery<PersonListDto>(string.Format(querySql, showField)).ToList();
            var pagedList = new PagedList<PersonListDto>(query, req.page, req.pageSize);


            //var personList = new List<PersonListDto>();
            foreach (var item in pagedList)
            {
                var SexDict = _dictRepository.GetById(item.Sex);
                var NationDict = _dictRepository.GetById(item.Nation);
                var FamilyRelationDict = _dictRepository.GetById(item.FamilyRelation);
                
                item.SexName = SexDict?.DictName ?? string.Empty;
                item.NationName = NationDict?.DictName ?? string.Empty;
                item.FamilyRelationName = FamilyRelationDict?.DictName ?? string.Empty;

                item.BirthdayVal = item.Birthday.ToString("yyyy-MM-dd");
                item.ValidFromVal = item.ValidFrom.HasValue ? item.ValidFrom.Value.ToString("yyyy-MM-dd") : string.Empty;
                item.ValidToVal = item.ValidTo.HasValue ? item.ValidTo.Value.ToString("yyyy-MM-dd") : string.Empty;
                item.IsLocalVal = item.IsLocal == 1 ? "是" : "否";
                item.IsRenterVal = item.IsRenter ? "是" : "否";
                item.CreateTimeVal = item.CreateTime.ToString("yyyy-MM-dd");

                var community = _areaRepository.GetById(item.CommunityUUID);
                item.CommunityName = community != null ? community.ChineseName : "";




            }
            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = pagedList
            };
        }

        public async Task<Image> GetCertificateImgByIdAsync(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                return null;
            }
            var entity = await _personRepository.GetEntityAsync(t => t.PersonUUID == personId);
            if (entity == null)
            {
                return null;
            }

            var parentPath = HostingEnvironment.MapPath("~");

            var imgfile = parentPath + entity.CertificateIPic;
            if (!File.Exists(imgfile))
            {
                return null;
            }

            return Image.FromFile(imgfile);


        }

        public async Task<Image> GetFaceImgByIdAsync(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                return null;
            }
            var entity = await _personRepository.GetEntityAsync(t => t.PersonUUID == personId);
            if (entity == null)
            {
                return null;
            }

            var parentPath = HostingEnvironment.MapPath("~");

            var imgfile = parentPath + entity.FacePic;
            if (!File.Exists(imgfile))
            {
                return null;
            }

            return Image.FromFile(imgfile);


        }

        public async Task<Image> GetIDImgByIdAsync(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                return null;
            }
            var entity = await _personRepository.GetEntityAsync(t => t.PersonUUID == personId);
            if (entity == null)
            {
                return null;
            }

            var parentPath = HostingEnvironment.MapPath("~");

            var imgfile = parentPath + entity.IDCardPic;
            if (!File.Exists(imgfile))
            {
                return null;
            }

            return Image.FromFile(imgfile);


        }

        public Image GetCertificateImgById(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                return null;
            }
            var entity = _personRepository.GetEntity(t => t.PersonUUID == personId);
            if (entity == null)
            {
                return null;
            }

            var parentPath = HostingEnvironment.MapPath("~");

            var imgfile = parentPath + entity.CertificateIPic;
            if (!File.Exists(imgfile))
            {
                return null;
            }

            return Image.FromFile(imgfile);


        }

        public Image GetFaceImgById(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                return null;
            }
            var entity = _personRepository.GetEntity(t => t.PersonUUID == personId);
            if (entity == null)
            {
                return null;
            }

            var parentPath = HostingEnvironment.MapPath("~");

            var imgfile = parentPath + entity.FacePic;
            if (!File.Exists(imgfile))
            {
                return null;
            }

            return Image.FromFile(imgfile);


        }

        public Image GetIDImgById(string personId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                return null;
            }
            var entity = _personRepository.GetEntity(t => t.PersonUUID == personId);
            if (entity == null)
            {
                return null;
            }

            var parentPath = HostingEnvironment.MapPath("~");

            var imgfile = parentPath + entity.IDCardPic;
            if (!File.Exists(imgfile))
            {
                return null;
            }

            return Image.FromFile(imgfile);


        }

        public PersonDto GetById(string personId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var entity = _personRepository.GetById(personId);
            sw.Stop();
            var perTotal = sw.ElapsedMilliseconds;

            if (entity != null)
            {
                var model = entity.MapTo<PersonDto>();
                var parentPath = HostingEnvironment.MapPath("~");
                sw.Restart();
                var imgfile = parentPath + model.CertificateIPic;
                if (File.Exists(imgfile))
                {
                    Image img = Image.FromFile(imgfile);
                    model.CertificateIIMG = ImgUtil.ImgToByt(img);
                }
                sw.Stop();

                var img1total = sw.ElapsedMilliseconds;
                sw.Restart();
                var idIimgfile = parentPath + model.IDCardPic;
                if (File.Exists(idIimgfile))
                {
                    Image img = Image.FromFile(idIimgfile);
                    model.IDCardImg = ImgUtil.ImgToByt(img);
                }
                sw.Stop();
                var img1tota2 = sw.ElapsedMilliseconds;
                sw.Restart();
                var faceIimgfile = parentPath + model.FacePic;
                if (File.Exists(faceIimgfile))
                {
                    Image img = Image.FromFile(faceIimgfile);
                    model.FaceImg = ImgUtil.ImgToByt(img);
                }
                sw.Stop();
                var img1tota3 = sw.ElapsedMilliseconds;
                return model;
            }

            else
                return null;
        }

        public PersonDto GetByBaseId(string personId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var entity = _personRepository.GetById(personId);
            sw.Stop();
            var perTotal = sw.ElapsedMilliseconds;

            if (entity != null)
            {
                if (entity.Deleted)
                {
                    return null;
                }
                var model = entity.MapTo<PersonDto>();
                return model;
            }

            else
                return null;
        }



        /// <summary>
        /// 根据身份证号码获取信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public PersonDto GetByCardNo(string cardNo)
        {
            var entity = _personRepository.GetEntity(a => a.IDCardNo == cardNo && !a.Deleted);

            if (entity != null)
            {
                if (entity.Deleted)
                {
                    return null;
                }
                var model = entity.MapTo<PersonDto>();
                return model;
            }
            else
            {
                return null;
            }
        }

        public void Create(PersonDto config)
        {
            var entity = config.MapTo<PersonEntity>();
            //entity.PersonUUID = Guid.NewGuid().ToString("N");
            if (Utils.IsDateTime(config.ValidFrom))
            {
                entity.ValidFrom = Convert.ToDateTime(config.ValidFrom);
            }

            if (Utils.IsDateTime(config.ValidTo))
            {
                entity.ValidTo = Convert.ToDateTime(config.ValidTo);

            }
            else
            {
                if (config.ValidTo == "长期")
                {
                    entity.ValidTo = DateTime.MaxValue;
                }
            }
            //保存图片
            String Tpath = "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";

            var parentPath = HostingEnvironment.MapPath("~/Upload");
            string FilePath = parentPath + "/" + Tpath + "/";
            if (config.CertificateIIMG != null && config.CertificateIIMG.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(config.CertificateIIMG);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.CertificateIPic = "Upload/" + Tpath + "/" + FileName;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "图片上传失败");
                }
            }
            if (config.IDCardImg != null && config.IDCardImg.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(config.IDCardImg);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.IDCardPic = "Upload/" + Tpath + "/" + FileName;

                }
                catch (Exception ex)
                {
                    Log.Error(ex, "保存图片失败");
                }
            }
            if (config.FaceImg != null && config.FaceImg.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(config.FaceImg);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.FacePic = "Upload/" + Tpath + "/" + FileName;

                }
                catch (Exception ex)
                {
                    Log.Error(ex, "保存图片失败");
                }
            }
            entity.CreateTime = DateTime.Now;
            entity.Deleted = false;
            entity.IsBasicInfo = true;
            entity.IsVerified = false;
            entity.IsLived = 0;
            entity.IsRenter = false;
            entity.SyncStatus = false;
            entity.SyncVersion = 0;
            _personRepository.Insert(entity);
            Synchronization(entity);

        }


        public void Update(PersonDto model)
        {
            //  var entity = config.MapTo<PersonEntity>();
            var entity = _personRepository.GetById(model.PersonUUID);
            entity = model.ToEntity(entity);
            entity.IsBasicInfo = true;
            //保存图片
            String Tpath = "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";

            var parentPath = HostingEnvironment.MapPath("~/Upload");
            string FilePath = parentPath + "/" + Tpath + "/";
            if (model.CertificateIIMG != null && model.CertificateIIMG.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(model.CertificateIIMG);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.CertificateIPic = "Upload/" + Tpath + "/" + FileName;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "图片上传失败");
                }
            }
            if (model.IDCardImg != null && model.IDCardImg.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(model.IDCardImg);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.IDCardPic = "Upload/" + Tpath + "/" + FileName;

                }
                catch (Exception ex)
                {
                    Log.Error(ex, "保存图片失败");
                }
            }
            if (model.FaceImg != null && model.FaceImg.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(model.FaceImg);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.FacePic = "Upload/" + Tpath + "/" + FileName;

                }
                catch (Exception ex)
                {
                    Log.Error(ex, "保存图片失败");
                }
            }


            entity.UpdateTime = DateTime.Now;
            entity.SyncStatus = false;
            _personRepository.Update(entity);
            Synchronization(entity);
        }

        public void UpdateFaceImg(PersonDto model)
        {

            //  var entity = config.MapTo<PersonEntity>();
            var entity = _personRepository.GetById(model.PersonUUID);
            //保存图片
            String Tpath = "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";

            var parentPath = HostingEnvironment.MapPath("~/Upload");
            string FilePath = parentPath + "/" + Tpath + "/";
            if (model.CertificateIIMG != null && model.CertificateIIMG.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(model.CertificateIIMG);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.CertificateIPic = "Upload/" + Tpath + "/" + FileName;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "图片上传失败");
                }
            }

            if (model.FaceImg != null && model.FaceImg.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    Image img = ImgUtil.BytToImg(model.FaceImg);
                    string _ImageExtension = ImgUtil.GetImageExtension(img);
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + _ImageExtension;
                    var imgFullPath = Path.Combine(FilePath, FileName);
                    img.Save(imgFullPath);
                    entity.FacePic = "Upload/" + Tpath + "/" + FileName;

                }
                catch (Exception ex)
                {
                    Log.Error(ex, "保存图片失败");
                }
            }

            entity.IsVerified = true;
            //entity = model.ToEntity(entity);
            entity.UpdateTime = DateTime.Now;
            entity.SyncStatus = false;
            _personRepository.Update(entity);
            Synchronization(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
            foreach (var item in idList1)
            {
                var entity = _personRepository.GetById(item);
                entity.Deleted = true;
                entity.SyncStatus = false;
                _personRepository.Delete(entity);
            }

            _renterService.Delete(ids);
        }


        public IList<PersonEntity> GetSyncPersonList()
        {
            var query = _personRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p=> {
                Log.Debug(p.PersonName);
                Synchronization(p);
            });

            return syncList;
        }


        private void Synchronization(PersonEntity entity)
        {
            if (Constant.Sysc)
            {              

                try
                {
                    var requestXml = GetXml(entity);

                    if (requestXml == null)
                    {
                        return;
                    }

                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.PersonServiceImplService();
                    var responseXml = areaS.insertPerson(requestXml);

                    Log.Debug(this.GetType().ToString(), responseXml);

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertPerson.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = entity.CommunityUUID;

                    var resultRes = responseXml.Deserial<ResultResponse>();
                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            var entity2 = _personRepository.GetById(entity.PersonUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _personRepository.Update(entity2);

                            syncLog.SyncResult = 1;
                        }
                    }

                    _syncService.InsertSyncLog(syncLog);

                }
                catch (Exception ex)
                {
                    Log.Error(null, ex.Message);
                }
            }
        }

        private string GetXml(PersonEntity entity)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";

            var SexDict = _dictRepository.GetById(entity.Sex);
            var nationDict = _dictRepository.GetById(entity.Nation);
            var IDDocumentTypeDict = entity.IDDocumentType.HasValue ? _dictRepository.GetById(entity.IDDocumentType.Value) : null;
            var PersonTypeDict = entity.PersonType.HasValue ? _dictRepository.GetById(entity.PersonType.Value) : null;
            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<person>");
            xmlBuilder.AppendFormat("<uuid>{0}</uuid>", entity.PersonUUID);
            xmlBuilder.AppendFormat("<cusr>{0}</cusr>", Constant.LkbAccount);
            xmlBuilder.AppendFormat("<state>{0}</state>", entity.Status);
            xmlBuilder.AppendFormat("<cdate>{0}</cdate>", entity.CreateTime.ToString("yyyyMMddHHmmss"));
            xmlBuilder.AppendFormat("<udate>{0}</udate>", entity.UpdateTime.HasValue ? entity.UpdateTime.Value.ToString("yyyyMMddHHmmss") : "");
            xmlBuilder.AppendFormat("<name>{0}</name>", DESHelper.Encrypt3Des(entity.PersonName, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<sex>{0}</sex>", SexDict.DictCode);
            xmlBuilder.AppendFormat("<nation>{0}</nation>", nationDict.DictCode);
            xmlBuilder.AppendFormat("<birthday>{0}</birthday>", entity.Birthday.ToString("yyyyMMdd"));
            xmlBuilder.AppendFormat("<regaddress>{0}</regaddress>", entity.RegAddress);
            xmlBuilder.AppendFormat("<oregaddress>{0}</oregaddress>", entity.OriginRegAddress);
            xmlBuilder.AppendFormat("<curaddress>{0}</curaddress>", entity.LivingAddress);
            xmlBuilder.AppendFormat("<tel>{0}</tel>", !entity.PhoneNo.IsBlank() ? DESHelper.Encrypt3Des(entity.PhoneNo, Constant.DescKeyBytes) : "");
            xmlBuilder.AppendFormat("<tel2>{0}</tel2>", !entity.BackUpPhoneNo.IsBlank() ? DESHelper.Encrypt3Des(entity.BackUpPhoneNo, Constant.DescKeyBytes) : "");
            xmlBuilder.AppendFormat("<idcard>{0}</idcard>", !entity.IDCardNo.IsBlank() ? DESHelper.Encrypt3Des(entity.IDCardNo, Constant.DescKeyBytes) : "");
            xmlBuilder.AppendFormat("<authorgan>{0}</authorgan>", entity.IssueOrg);
            xmlBuilder.AppendFormat("<avbdate>{0}</avbdate>", entity.ValidFrom.HasValue ? entity.ValidFrom.Value.ToString("yyyyMMdd") : "");
            xmlBuilder.AppendFormat("<avedate>{0}</avedate>", entity.ValidTo.HasValue ? entity.ValidTo.Value.ToString("yyyyMMdd") : "");
            xmlBuilder.AppendFormat("<photopath>{0}</photopath>", ""); //IDCardPic  证件照片需加密
            xmlBuilder.AppendFormat("<houseid>{0}</houseid>", "");  //居住HoseeID
            xmlBuilder.AppendFormat("<focal>{0}</focal>", entity.IsFocal);
            xmlBuilder.AppendFormat("<proof>{0}</proof>", IDDocumentTypeDict != null ? IDDocumentTypeDict.DictCode : "");
            xmlBuilder.AppendFormat("<islocal>{0}</islocal>", entity.IsLocal);
            xmlBuilder.AppendFormat("<cardisproving>{0}</cardisproving>", entity.IsIDVerified);
            xmlBuilder.AppendFormat("<ename>{0}</ename>", entity.EnglishName);
            xmlBuilder.AppendFormat("<flow>{0}</flow>", PersonTypeDict != null && PersonTypeDict.DictCode == "1" ? "1" : "");     //租客
            xmlBuilder.AppendFormat("<family>{0}</family>", PersonTypeDict != null && PersonTypeDict.DictCode == "2" ? "1" : ""); //家属
            xmlBuilder.AppendFormat("<owner>{0}</owner>", PersonTypeDict != null && PersonTypeDict.DictCode == "3" ? "1" : "");   //房东
            xmlBuilder.AppendFormat("<patrol>{0}</patrol>", PersonTypeDict != null && PersonTypeDict.DictCode == "4" ? "1" : ""); //巡防
            xmlBuilder.AppendFormat("<finger1>{0}</finger1>", "");  //指纹一
            xmlBuilder.AppendFormat("<finger2>{0}</finger2>", "");  //指纹二
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</person>");
            xmlBuilder.Append("</ReqBody>");
            xmlBuilder.Append("</Tpp2Fpp> ");
            return xmlBuilder.ToString();
        }
    }
}
