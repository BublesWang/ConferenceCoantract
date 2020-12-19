using ConferenceContractAPI.CCDBContext;
using ConferenceContractAPI.Common;
using ConferenceContractAPI.DBModels;
using GrpcConferenceContractServiceNew;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ConferenceContractService
{
    public partial class ConferenceContractImpService
    {
        public async Task<newGetReportNoticResponse> GetPersonNoticeReportAsync(newGetReportNoticeRequest request)
        {
            try
            {
                var conferenceId = request.ConferenceId.Trim();
                var perContractNumer = request.PerContractNumber.Trim();
                var memberPK = request.MemberPK.Trim();
                var year = request.Year.Trim();

                if (string.IsNullOrEmpty(year))
                {
                    throw new ArgumentException("年份不能为空");
                }

                newGetReportNoticResponse response = new newGetReportNoticResponse();
                bool isNeiZi = true;

                //SNEC第十五届(2021)国际太阳能光伏与智慧能源(上海)大会
                //var conferenceId = "5534a0c7-697d-45aa-91a7-fa4def2a5205";
                //var memberPK = "a34af51d-185f-40a3-8739-136ead41954a";
                //var perContractNumer = "21SNECC0002CW04001";
                //var personContractId = string.Empty;

                List<Report_ConferenceVM> conferenceVMList = new List<Report_ConferenceVM>();

                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var dbPersonContract = dbContext.PersonContract;
                    var dbCompanyContract = dbContext.CompanyContract;
                    var dbCompanyServicePack = dbContext.CompanyServicePack;

                    var dbConference = dbContext.Conference;
                    var dbApplyConference = dbContext.ApplyConference;
                    var dbCfAddress = dbContext.CFAddress;
                    var dbParticipant = dbContext.Participant;
                    var dbParticipantType = dbContext.ParticipantType;
                    var dbActivity = dbContext.Activity;
                    var dbActivityParticipantMap = dbContext.ActivityParticipantMap;
                    var dbTalk = dbContext.Talk;
                    var dbTalkParticipantMap = dbContext.TalkParticipantMap;


                    #region 获得个人信息
                    var personContract = await dbPersonContract.FirstOrDefaultAsync(x => x.PerContractNumber == perContractNumer);
                    if (personContract == null)
                    {
                        throw new Exception("找不到个人合同");
                    }
                    var memberEmail = await GetPersonEmailAsync(personContract.MemberPK.ObjToGuid());
                    var companyNameTranslation = (await dbCompanyContract.FirstOrDefaultAsync(n => n.ContractId == personContract.ContractId))?.ComNameTranslation;
                    var comServicePackTranslation = (await dbCompanyServicePack.FirstOrDefaultAsync(n => n.CompanyServicePackId == personContract.CompanyServicePackId))?.Translation;
                    var personInfo = new NewPersonContractInfoStruct
                    {
                        PersonContractId = personContract.PersonContractId.ToString(),
                        PerContractNumber = personContract.PerContractNumber,
                        MemberPK = personContract.MemberPK.ToString(),
                        MemTranslation = personContract.MemTranslation,
                        MemEmail = memberEmail,
                        CompanyNameTranslation = companyNameTranslation,
                        ComServicePackTranslation = comServicePackTranslation,
                        PerPrice = personContract.PerPrice
                    };
                    response.Person = personInfo;
                    #endregion

                    #region 获取业务员信息
                    var owner = await GetOwnerInfoAsync(personContract.Owerid.ObjToGuid());
                    response.Owner = owner;
                    #endregion


                    //套餐内的会议集合
                    var inServicePackConferenceList = await GetInServicePackConferenceAsync(dbContext, personContract);
                    //勾选的会议集合
                    var applyConferenceList = await dbApplyConference.Where(x => x.Year == year
                                                                                        && x.PersonContractId == personContract.PersonContractId.ToString()
                                                                                       && x.IsConfirm == true).AsNoTracking().ToListAsync();
                    var tagTypeList = await dbContext.TagType.AsNoTracking().ToListAsync();


                    #region 将数据拿到内存
                    var participantTypeList = await dbParticipantType.AsNoTracking().ToListAsync();
                    //所有的会议
                    var conferenceList = await (from c in dbConference
                                                join address in dbCfAddress
                                                on c.CFAddressPK equals address.CFAddressPK into addressT
                                                from address1 in addressT.DefaultIfEmpty()
                                                where c.ParentID == conferenceId
                                                orderby c.Sort
                                                select new
                                                {
                                                    ConferenceId = c.ConferenceID,
                                                    ConferenceName = c.Translation,
                                                    Date = c.StartDate,
                                                    TimeRange = c.TimeRange,
                                                    Address = address1.Translation,
                                                    Sort = c.Sort,
                                                    StartTime = c.StartDateTime ?? string.Empty
                                                }).AsNoTracking().ToListAsync();

                    //所有的活动
                    var activityList = await dbActivity.AsNoTracking().OrderBy(x => x.Sort).ToListAsync();
                    //所有的活动和嘉宾关系
                    var activityParticipantMapList = await dbActivityParticipantMap.AsNoTracking().ToListAsync();
                    //所有的TalkList
                    var talkList = await dbTalk.OrderBy(x => x.Sort).AsNoTracking().ToListAsync();
                    //所有的Talk和嘉宾的关系
                    var talkParticipantMapList = await dbTalkParticipantMap.AsNoTracking().ToListAsync();
                    #endregion
                    {
                        #region 获取当前人员信息
                        var participantModel = await dbContext.Participant
                        .Where(x => x.MemberPK == memberPK && x.IsDelete == false && x.Year == year && x.ShowOnFont == 0)
                        .Select(x => new
                        {
                            ParticipantName = x.ParticipantNameTranslation,
                            Job = x.JobTranslation,
                            Title = x.AppellationTranslation,
                            Company = x.CompanyTranslation,
                            Country = x.CountryTranslation,
                            ParticipantID = x.ParticipantID
                        }).FirstOrDefaultAsync();
                        #endregion



                        foreach (var item in conferenceList)
                        {
                            Report_ConferenceVM conferenceVMModel = new Report_ConferenceVM();
                            conferenceVMModel.Address = TransformJson(item.Address, isNeiZi);
                            conferenceVMModel.ConferneceName = TransformJson(item.ConferenceName, isNeiZi);
                            conferenceVMModel.Date = TransformJson(item.Date, isNeiZi);
                            conferenceVMModel.TimeRange = item.TimeRange;
                            conferenceVMModel.Sort = item.Sort;
                            conferenceVMModel.StartTime = item.StartTime;

                            #region 查找applyConfenrence是否包含当前项
                            var applyConferenceModel = applyConferenceList.Find(x => x.SessionConferenceId == item.ConferenceId.ToString());
                            if (applyConferenceModel != null)
                            {
                                conferenceVMModel.IsChecked = true;
                                conferenceVMModel.Remark = TransformJson(applyConferenceModel.RemarkTranslation, isNeiZi);
                                var tagTypeCodes = applyConferenceModel.TagTypeCodes;
                                if (!string.IsNullOrEmpty(tagTypeCodes))
                                {
                                    List<string> tagTypeStrList = new List<string>();
                                    var codes = tagTypeCodes.Split(',');
                                    foreach (var code in codes)
                                    {
                                        var tagTypeModel = tagTypeList.Find(x => x.Code == code);
                                        if (tagTypeModel != null)
                                        {
                                            tagTypeStrList.Add(TransformJson(tagTypeModel.NameTranslation, isNeiZi));
                                        }
                                    }
                                    conferenceVMModel.ParticipateType = string.Join(',', tagTypeStrList);
                                }
                            }

                            #endregion

                            #region 判断此项是否包含在套餐内
                            var isExist = inServicePackConferenceList.Any(x => x.ConferenceId == item.ConferenceId.ToString());
                            if (isExist)
                            {
                                conferenceVMModel.IsInServicePack = true;
                            }
                            #endregion

                            //得到当前会议下的活动列表
                            var list = activityList.Where(x => x.ConferenceID == item.ConferenceId).ToList();
                            if (list.Count > 0)
                            {
                                List<Report_ActivityVM> activityVMList = new List<Report_ActivityVM>();
                                foreach (var activityItem in list)
                                {
                                    Report_ActivityVM activityModel = new Report_ActivityVM();
                                    activityModel.ActivityName = TransformJson(activityItem.Translation, isNeiZi);
                                    activityModel.Sort = activityItem.Sort;
                                    activityModel.StartTime = activityItem.StartDate;
                                    activityModel.TimeLength = activityItem.TimeLength.ObjToInt();

                                    List<Report_TalkVM> talkVMList = new List<Report_TalkVM>();
                                    //当前嘉宾不为空
                                    if (participantModel != null)
                                    {
                                        if (talkList.Count > 0)
                                        {
                                            #region 在每一个activity中找到下边对应的talk集合
                                            var filterTalkList = talkList.Where(x => x.ActivityID == activityItem.ActivityID)
                                                                         .OrderBy(x => x.Sort).ToList();
                                            if (filterTalkList.Count > 0)
                                            {
                                                foreach (var talkModel in filterTalkList)
                                                {
                                                    var talkId = talkModel.TalkID;

                                                    var mapList = talkParticipantMapList.Where(x => x.ConferenceID == item.ConferenceId
                                                                                       && x.TalkID == talkId
                                                                                       && x.ParticipantID == participantModel.ParticipantID)
                                                                                       .Select(x => new
                                                                                       {
                                                                                           ParticipantTypeID = x.ParticipantTypeID,
                                                                                       }).ToList();

                                                    if (mapList.Count > 0)
                                                    {
                                                        Report_TalkVM talkVM = new Report_TalkVM();
                                                        List<Report_ParticipantVM> participantVMList = new List<Report_ParticipantVM>();
                                                        #region 拼接talk
                                                        talkVM.TimeLength = talkModel.TimeLength.ObjToInt();
                                                        talkVM.Sort = talkModel.Sort;
                                                        talkVM.StartTime = talkModel.StartDate;
                                                        var tModel = talkList.Find(x => x.TalkID == talkId);
                                                        talkVM.TalkName = TransformJson(tModel.Translation, isNeiZi);
                                                        talkVM.TopicName = TransformJson(tModel.CFTopicName, isNeiZi);

                                                        foreach (var mapModel in mapList)
                                                        {
                                                            #region 拼接Report_ParticipantVM
                                                            Report_ParticipantVM participantVM = new Report_ParticipantVM();
                                                            participantVM.ParticipantName = TransformJson(participantModel.ParticipantName, isNeiZi);
                                                            participantVM.ParticipantJob = TransformJson(participantModel.Job, isNeiZi);
                                                            participantVM.ParticipantTitle = TransformJson(participantModel.Title, isNeiZi);
                                                            participantVM.ParticipantCompany = TransformJson(participantModel.Company, isNeiZi);
                                                            participantVM.ParticipantCountry = TransformJson(participantModel.Country, isNeiZi);
                                                            var participantTypeTranslation = participantTypeList.FirstOrDefault(x => x.ParticipantTypeID == mapModel.ParticipantTypeID).ParticipantTypeTranslation;
                                                            participantVM.ParticipantType = TransformJson(participantTypeTranslation, isNeiZi);
                                                            participantVMList.Add(participantVM);
                                                            #endregion
                                                        }
                                                        talkVM.ParticipantVMList.AddRange(participantVMList);
                                                        talkVMList.Add(talkVM);
                                                        #endregion
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                //查询ActivityParticipantMap表数据，看是否存在人员
                                                var mapList = activityParticipantMapList.Where(x => x.ParticipantID == participantModel.ParticipantID && x.ConferenceID == item.ConferenceId).ToList();
                                                if (mapList.Count > 0)
                                                {
                                                    List<Report_ParticipantVM> participantVMList = new List<Report_ParticipantVM>();
                                                    activityModel.TimeLength = activityItem.TimeLength.ObjToInt();
                                                    activityModel.StartTime = activityItem.StartDate;
                                                    foreach (var mapModel in mapList)
                                                    {
                                                        Report_ParticipantVM participantVM = new Report_ParticipantVM();
                                                        if (activityItem.ActivityID == mapModel.ActivityId)
                                                        {
                                                            var participantTypeTranslation = participantTypeList.FirstOrDefault(x => x.ParticipantTypeID == mapModel.ParticipantTypeID).ParticipantTypeTranslation;
                                                            participantVM.ParticipantType = TransformJson(participantTypeTranslation, isNeiZi);
                                                            participantVM.ParticipantName = TransformJson(participantModel.ParticipantName, isNeiZi);
                                                            participantVM.ParticipantJob = TransformJson(participantModel.Job, isNeiZi);
                                                            participantVM.ParticipantTitle = TransformJson(participantModel.Title, isNeiZi);
                                                            participantVM.ParticipantCompany = TransformJson(participantModel.Company, isNeiZi);
                                                            participantVM.ParticipantCountry = TransformJson(participantModel.Country, isNeiZi);
                                                            participantVMList.Add(participantVM);
                                                        }
                                                    }
                                                    activityModel.ParticipantVMList.AddRange(participantVMList);
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            //查询ActivityParticipantMap表数据，看是否存在人员
                                            var mapList = activityParticipantMapList.Where(x => x.ParticipantID == participantModel.ParticipantID && x.ConferenceID == item.ConferenceId).ToList();
                                            if (mapList.Count > 0)
                                            {
                                                List<Report_ParticipantVM> participantVMList = new List<Report_ParticipantVM>();
                                                activityModel.TimeLength = activityItem.TimeLength.ObjToInt();
                                                activityModel.StartTime = activityItem.StartDate;
                                                foreach (var mapModel in mapList)
                                                {
                                                    Report_ParticipantVM participantVM = new Report_ParticipantVM();
                                                    if (activityItem.ActivityID == mapModel.ActivityId)
                                                    {
                                                        var participantTypeTranslation = participantTypeList.FirstOrDefault(x => x.ParticipantTypeID == mapModel.ParticipantTypeID).ParticipantTypeTranslation;
                                                        participantVM.ParticipantType = TransformJson(participantTypeTranslation, isNeiZi);
                                                        participantVM.ParticipantName = TransformJson(participantModel.ParticipantName, isNeiZi);
                                                        participantVM.ParticipantJob = TransformJson(participantModel.Job, isNeiZi);
                                                        participantVM.ParticipantTitle = TransformJson(participantModel.Title, isNeiZi);
                                                        participantVM.ParticipantCompany = TransformJson(participantModel.Company, isNeiZi);
                                                        participantVM.ParticipantCountry = TransformJson(participantModel.Country, isNeiZi);
                                                        participantVMList.Add(participantVM);
                                                    }
                                                }
                                                activityModel.ParticipantVMList.AddRange(participantVMList);
                                            }
                                        }
                                        activityModel.TalkVMList.AddRange(talkVMList);
                                    }
                                    activityVMList.Add(activityModel);
                                }
                                conferenceVMModel.ActivityVMList.AddRange(activityVMList);
                            }
                            conferenceVMList.Add(conferenceVMModel);
                        }
                    }
                }
                conferenceVMList = HandleTimeSpan(conferenceVMList);
                response.Calendar.AddRange(conferenceVMList);

                return response;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 获得个人邮箱
        /// </summary>
        /// <param name="memberPK"></param>
        /// <returns></returns>
        public async Task<string> GetPersonEmailAsync(Guid memberPK)
        {
            try
            {
                using (var memberdbContext = new MemberDBContext(_member_options))
                {
                    var dbMember = memberdbContext.Member;
                    var memberEmail = (await dbMember.FirstOrDefaultAsync(x => x.MemberPK == memberPK))?.MemEmail;
                    return memberEmail;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 根据ownerId获取业务员信息
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<OwnerInfo> GetOwnerInfoAsync(Guid ownerId)
        {
            try
            {
                using (var roledbContext = new RoleDBContext(_role_options))
                {
                    #region 获得业务员信息
                    var user = await roledbContext.User.Where(x => x.UserPK == ownerId).FirstOrDefaultAsync();
                    OwnerInfo owner = new OwnerInfo();
                    owner.Email = user.UserEmail;
                    owner.Tel = user.UserTel;

                    TransFull nameTrans = new TransFull()
                    {
                        CN = user.UserRealNameCn,
                        EN = user.UserRealNameEn,
                        JP = user.UserRealNameJp
                    };
                    TransFull addressTrans = new TransFull()
                    {
                        CN = user.UserAddresseCn,
                        EN = user.UserAddresseEn,
                        JP = user.UserAddresseJp
                    };
                    owner.Name = JsonConvert.SerializeObject(nameTrans);
                    owner.Address = JsonConvert.SerializeObject(addressTrans);
                    #endregion

                    return owner;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        /// <summary>
        /// 根据个人合同号获得套餐内的会议集合
        /// </summary>
        /// <param name="dbContractContext"></param>
        /// <param name="perContractNumer"></param>
        /// <returns></returns>
        public async Task<List<InServicePackConferenceVM>> GetInServicePackConferenceAsync(ConCDBContext dbContractContext, PersonContract personContract)
        {
            List<InServicePackConferenceVM> list = new List<InServicePackConferenceVM>();

            var dbCompanyServicePackMap = dbContractContext.CompanyServicePackMap;
            var dbServicePack = dbContractContext.ServicePack;
            var dbServicePackActivityMap = dbContractContext.ServicePackActivityMap;

            var companyServicePackMap = await dbCompanyServicePackMap.FirstOrDefaultAsync(x => x.CompanyServicePackId == personContract.CompanyServicePackId);
            if (companyServicePackMap != null)
            {
                var servicePack = dbServicePack.Where(x => x.ServicePackId == companyServicePackMap.ServicePackId);
                var servicePackActivityMap = dbServicePackActivityMap.Where(x => x.ServicePackId == companyServicePackMap.ServicePackId);

                //套餐包含的活动
                list = await (from sp in servicePack
                              join spm in servicePackActivityMap
                              on sp.ServicePackId equals spm.ServicePackId
                              select new InServicePackConferenceVM
                              {
                                  ConferenceId = spm.SessionConferenceID ?? string.Empty,
                              }).ToListAsync();
            }
            return list;
        }
        private string TransformJson(string str, bool isNeizi)
        {
            var model = JsonConvert.DeserializeObject<TransFull>(str);
            return isNeizi ? model.CN : model.EN;
        }

        /// <summary>
        /// 根据起始时间,时长,得到时间范围
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="timeLength"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private string GetTimeRange(string startTime, int timeLength, out string endTime)
        {
            DateTime date = DateTime.Parse(startTime);
            var newDate = date.AddMinutes(timeLength);
            endTime = newDate.ToString("HH:mm");

            var result = $"{startTime}-{endTime}";
            return result;
        }

        /// <summary>
        /// 处理集合中时间段
        /// </summary>
        /// <param name="conferencelist"></param>
        /// <returns></returns>
        private List<Report_ConferenceVM> HandleTimeSpan(List<Report_ConferenceVM> conferencelist)
        {
            if (conferencelist.Count > 0)
            {
                foreach (var conference in conferencelist)
                {
                    var activityList = conference.ActivityVMList;
                    if (activityList.Count > 0)
                    {
                        string endTime = string.Empty;//前一项的结束时间作为后一项的起始时间
                        for (int i = 0; i < activityList.Count; i++)
                        {
                            if (i == 0)
                            {
                                endTime = !string.IsNullOrEmpty(conference.StartTime) ? conference.StartTime : activityList[i].StartTime;
                            }
                            var talkList = activityList[i].TalkVMList;
                            if (talkList.Count > 0)
                            {
                                for (int j = 0; j < talkList.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                        endTime = talkList[j].StartTime;
                                    }
                                    var startTime = endTime;
                                    var timeLength = talkList[j].TimeLength;

                                    var timeRange = GetTimeRange(startTime, timeLength, out endTime);
                                    talkList[j].TimeRange = timeRange;
                                }
                            }
                            else
                            {
                                var startTime = endTime;
                                var timeLength = activityList[i].TimeLength;
                                var timeRange = GetTimeRange(startTime, timeLength, out endTime);
                                activityList[i].TimeRange = timeRange;
                            }

                        }
                    }

                }
            }
            return conferencelist;
        }
    }

}
