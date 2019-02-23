using Koowoo.Core;
using Koowoo.Domain.System;
using Koowoo.Pojo;
using System;
using System.Linq;
using Koowoo.Core.Extentions;
using Koowoo.Data.Interface;
using Koowoo.Core.Pager;
using Koowoo.Pojo.Request;
using Koowoo.Domain;
using System.Collections.Generic;
using System.Text;
using Koowoo.Services.System;
using Koowoo.Services.LkbResponse;
using Koowoo.Pojo.Enum;

namespace Koowoo.Services
{
    public interface IRenterService
    {
        RenterDto GetById(string doorId);
        void Save(RenterDto config);
        void Delete(string ids);
        void GetSyncRenterList();
    }

    public class RenterService : IRenterService, IDependency
    {
        private readonly IRepository<PersonEntity> _personRepository;
        private readonly IRepository<RenterEntity> _renterRepository;
        private readonly IRepository<DictEntity> _dictRepository;
        private readonly ISyncLogServie _syncService;


        public RenterService(IRepository<RenterEntity> renterRepository,
            IRepository<DictEntity> dictRepository,
            ISyncLogServie syncService,
            IRepository<PersonEntity> personRepository)
        {
            _renterRepository = renterRepository;
            _dictRepository = dictRepository;
            _personRepository = personRepository;
            _syncService = syncService;
        }

     

        public RenterDto GetById(string configId)
        {
            var dto = _renterRepository.GetById(configId);
            if (dto != null)
                return dto.MapTo<RenterDto>();
            else
                return null;
        }

        public void Save(RenterDto model)
        {
            model.IsAdd = false;
            var entity = _renterRepository.GetById(model.PersonUUID);

            if (entity == null)
            {
                model.IsAdd = true;
                entity = model.ToEntity();
                entity.CreateTime = DateTime.Now;
                entity.SyncVersion = 0;
            }
            else
            {
                entity = model.ToEntity(entity);
            }

            entity.SyncStatus = false;
            entity.Deleted = false;

            if (model.IsAdd)
                _renterRepository.Insert(entity);
            else
                _renterRepository.Update(entity);
 
            Synchronization(entity);

        }



        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
            foreach (var item in idList1)
            {
                var entity = _renterRepository.GetById(item);
                if(entity != null)
                {
                    entity.Deleted = true;
                    entity.SyncStatus = false;
                    _renterRepository.Delete(entity);

                    Synchronization(entity);
                }               
            }
        }


        private void Synchronization(RenterEntity entity)
        {
            if (Constant.Sysc)
            {             
                try
                {
                    var requestXml = GetXml(entity);
                    Log.Debug(this.GetType().ToString(), requestXml);

                    var areaS = new Lkb.PersonServiceImplService();
                    var responseXml = areaS.insertPersonFlow(requestXml);
                    Log.Debug(this.GetType().ToString(), responseXml);
                    var resultRes = responseXml.Deserial<ResultResponse>();

                    var syncLog = new SyncLogEntity();
                    syncLog.SyncType = SyncLogEnum.InsertPersonFlow.ToString();
                    syncLog.ResquestXml = requestXml;
                    syncLog.ResponseXml = responseXml;
                    syncLog.SyncTime = DateTime.Now;
                    syncLog.SyncResult = 0;
                    syncLog.CommunityId = entity.CommunityUUID;

                    if (resultRes != null && resultRes.Header != null)
                    {
                        var header = resultRes.Header;
                        if (header.RspCode.Equals("0"))
                        {
                            var entity2 = _renterRepository.GetById(entity.PersonUUID);
                            entity2.SyncVersion += 1;
                            entity2.SyncStatus = true;
                            _renterRepository.Update(entity2);

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

        private string GetXml(RenterEntity entity)
        {
            var flag = entity.SyncVersion == 0 ? "C" : "U"; //增｜删｜改，C｜D｜U
            if (entity.Deleted)
                flag = "D";

            var PoliticalStatus = entity.PoliticalStatus.HasValue ? _dictRepository.GetById(entity.PoliticalStatus.Value) : null;
            var EduLevel = entity.EduLevel.HasValue ? _dictRepository.GetById(entity.EduLevel.Value) : null;
            var ResidenceType = entity.ResidenceType.HasValue ? _dictRepository.GetById(entity.ResidenceType.Value) : null;
            var RegCertType = entity.RegCertType.HasValue ? _dictRepository.GetById(entity.RegCertType.Value) : null;
            var RentalStatus = entity.RentalStatus.HasValue ? _dictRepository.GetById(entity.RentalStatus.Value) : null;
            var LivingReason = entity.LivingReason.HasValue ? _dictRepository.GetById(entity.LivingReason.Value) : null;

            var xmlBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlBuilder.Append("<Tpp2Fpp>");
            xmlBuilder.Append("<ReqHeader>");
            xmlBuilder.AppendFormat("<ReqSeqNo>{0}</ReqSeqNo>", Utils.MakeRndName());
            xmlBuilder.AppendFormat("<ReqSPID>{0}</ReqSPID>", DESHelper.Encrypt3Des(Constant.LkbAccount, Constant.DescKeyBytes));
            xmlBuilder.AppendFormat("<ReqCode>{0}</ReqCode>", DESHelper.Encrypt3Des(Constant.LkbPassword, Constant.DescKeyBytes));
            xmlBuilder.Append("</ReqHeader>");
            xmlBuilder.Append("<ReqBody>");
            xmlBuilder.Append("<personFlow>");
            xmlBuilder.AppendFormat("<reluuid>{0}</reluuid>", entity.PersonUUID);
            xmlBuilder.AppendFormat("<polity>{0}</polity>", PoliticalStatus != null ? PoliticalStatus.DictCode : "");
            xmlBuilder.AppendFormat("<education>{0}</education>", EduLevel != null ? EduLevel.DictCode : "");
            xmlBuilder.AppendFormat("<comdate>{0}</comdate>", entity.LocalArriveDate.HasValue?entity.LocalArriveDate.Value.ToString("yyyyMMdd") : "");
            xmlBuilder.AppendFormat("<pestilence>{0}</pestilence>", entity.EpidemicPrevention);
            xmlBuilder.AppendFormat("<study>{0}</study>", entity.StudyHistory);
            xmlBuilder.AppendFormat("<inoculability>{0}</inoculability>", entity.Inoculability);
            xmlBuilder.AppendFormat("<staycard>{0}</staycard>", entity.StayCard);
            xmlBuilder.AppendFormat("<workaddress>{0}</workaddress>", entity.WorkingAddress);
            xmlBuilder.AppendFormat("<marry>{0}</marry>", entity.MarriageStatus);
            xmlBuilder.AppendFormat("<matename>{0}</matename>", !entity.MateName.IsBlank()?DESHelper.Encrypt3Des(entity.MateName, Constant.DescKeyBytes):"");
            xmlBuilder.AppendFormat("<mateno>{0}</mateno>", !entity.MateNo.IsBlank() ? DESHelper.Encrypt3Des(entity.MateNo, Constant.DescKeyBytes) : "");
            xmlBuilder.AppendFormat("<girs>{0}</girs>", entity.Daughter);
            xmlBuilder.AppendFormat("<boys>{0}</boys>", entity.Sions);
            xmlBuilder.AppendFormat("<pregnantno>{0}</pregnantno>", entity.GestationNo);
            xmlBuilder.AppendFormat("<pregnantdate>{0}</pregnantdate>", entity.PregnancyTestDate.HasValue ? entity.PregnancyTestDate.Value.ToString("yyyyMMdd") : "");
            xmlBuilder.AppendFormat("<pregnantcont>{0}</pregnantcont>", entity.ContraceptionCount);
            xmlBuilder.AppendFormat("<ispregnant>{0}</ispregnant>", entity.IsPregnant);
            xmlBuilder.AppendFormat("<isshbx>{0}</isshbx>", entity.IsSocialInsur);
            xmlBuilder.AppendFormat("<isldht>{0}</isldht>", entity.IsLaborContract);
            xmlBuilder.AppendFormat("<cbdwmc>{0}</cbdwmc>", entity.InsurCompany);
            xmlBuilder.AppendFormat("<cbdwlxr>{0}</cbdwlxr>", !entity.InsurCompanyContact.IsBlank() ? DESHelper.Encrypt3Des(entity.InsurCompanyContact, Constant.DescKeyBytes) : "");
            xmlBuilder.AppendFormat("<cbdwlxdh>{0}</cbdwlxdh>", entity.ICCPhoneNo);
            xmlBuilder.AppendFormat("<jyqk>{0}</jyqk>", entity.EduHistory);
            xmlBuilder.AppendFormat("<jdxx>{0}</jdxx>", entity.School);
            xmlBuilder.AppendFormat("<jzlx>{0}</jzlx>", ResidenceType!=null? ResidenceType.DictCode:"");
            xmlBuilder.AppendFormat("<hjlx>{0}</hjlx>", RegCertType!=null? RegCertType.DictCode:"");
            xmlBuilder.AppendFormat("<jzzj>{0}</jzzj>", entity.RegCert);
            xmlBuilder.AppendFormat("<zlzt>{0}</zlzt>", RentalStatus!=null? RentalStatus.DictCode:"");
            xmlBuilder.AppendFormat("<livereason>{0}</livereason>", LivingReason!=null? LivingReason.DictCode:"");
            xmlBuilder.AppendFormat("<hzlsh>{0}</hzlsh>", entity.RegCertNo);
            xmlBuilder.AppendFormat("<remark>{0}</remark>", entity.Remark);
            xmlBuilder.AppendFormat("<flag>{0}</flag>", flag);
            xmlBuilder.Append("</personFlow>");
            xmlBuilder.Append("</ReqBody>");
            xmlBuilder.Append("</Tpp2Fpp>");
            return xmlBuilder.ToString();
        }

        public void GetSyncRenterList()
        {
            var query = _renterRepository.Table.Where(a => !a.SyncStatus);

            var syncList = query.ToList();

            syncList.ForEach(p => {
                Synchronization(p);
            });
        }
    }
}
