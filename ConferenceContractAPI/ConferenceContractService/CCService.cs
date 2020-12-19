using ConferenceContractAPI.CCDBContext;
using ConferenceContractAPI.Common;
using ConferenceContractAPI.DBModels;
using ConferenceContractAPI.RabbitMqHelper;
using ConferenceContractAPI.ViewModel;
using Extensions.Repository;
using GrpcConferenceContractServiceNew;
using GrpcConferenceService;
using GrpcMembersService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ConferenceContractService
{
    public class CCService
    {
        private ConCDBContext _context;
        private DbContextOptionsBuilder<ConCDBContext> _options;
        public RabbitMQPublisher _rabbitMQPublisher = new RabbitMQPublisher();
        public IFreeSql fsql;

        //public CCService(ConCDBContext context)
        //{
        //    _context = context;

        //}

        public CCService(DbContextOptionsBuilder<ConCDBContext> options, string _sql)
        {
            _options = options;

            options.UseNpgsql(_sql);
            fsql = new FreeSql.FreeSqlBuilder()
                              .UseConnectionString(FreeSql.DataType.PostgreSQL, _sql)
                              .Build();
        }

        #region ConferenceContract表操作

        public async Task<List<ConferenceContract>> GetConferenceContractList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                && (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                && (string.IsNullOrEmpty(searchInfo.companyId) || n.CompanyId == searchInfo.companyId)
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                && (n.IsDelete == searchInfo.ccIsdelete)
                                //&& (n.companyContract.Where(m => m.companyServicePack.IsGive == false).Count() > 0)
                                && (searchInfo.isDiscount ? n.delegateServicePackDiscountForConferenceContract.Count == 0 : 0 == 0)
                                && (string.IsNullOrEmpty(searchInfo.exTypeCode.ToLower()) || n.ContractNumber.ToLower().Contains(searchInfo.exTypeCode.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                )
                    .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-CreatedOn" : searchInfo.orderings)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetConferenceContractListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract).ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                            .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                        && (
                                        (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                        ||
                                        (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                            ||
                                        (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                        )
                                        && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                        && (string.IsNullOrEmpty(searchInfo.companyId) || n.CompanyId == searchInfo.companyId)
                                        && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                        && (n.IsDelete == searchInfo.ccIsdelete)
                                        && (searchInfo.isDiscount ? n.delegateServicePackDiscountForConferenceContract.Count == 0 : 0 == 0)
                                        && (string.IsNullOrEmpty(searchInfo.exTypeCode.ToLower()) || n.ContractNumber.ToLower().Contains(searchInfo.exTypeCode.ToLower()))
                                        && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                        )
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ConferenceContract>> GetConferenceContractListByIsGive(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                && (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                && (n.IsDelete == searchInfo.ccIsdelete)
                                && (n.companyContract.Where(m => m.companyServicePack.IsGive == false).Count() > 0)
                                && (n.companyContract.Where(m => m.CCIsdelete == true).Count() == 0)
                                && (searchInfo.isDiscount ? n.delegateServicePackDiscountForConferenceContract.Count == 0 : 0 == 0)
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                //&& (n.ContractStatusCode != searchInfo.contractStatusCode.Split(",", StringSplitOptions.RemoveEmptyEntries)[0]
                                //    &&
                                //    n.ContractStatusCode != searchInfo.contractStatusCode.Split(",", StringSplitOptions.RemoveEmptyEntries)[1]
                                //    &&
                                //    !string.IsNullOrEmpty(n.ContractStatusCode)
                                //    )
                                && (string.IsNullOrEmpty(searchInfo.contractStatusCode) || n.ContractStatusCode == searchInfo.contractStatusCode)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                )
                    .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-CreatedOn" : searchInfo.orderings)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetConferenceContractListByIsGiveCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract)
                        .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                    && (
                                    (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                        ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    )
                                    && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                    && (n.IsDelete == searchInfo.ccIsdelete)
                                    && (n.companyContract.Where(m => m.companyServicePack.IsGive == false).Count() > 0)
                                    && (n.companyContract.Where(m => m.CCIsdelete == true).Count() == 0)
                                    && (searchInfo.isDiscount ? n.delegateServicePackDiscountForConferenceContract.Count == 0 : 0 == 0)
                                    && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                    //&& (n.ContractStatusCode != searchInfo.contractStatusCode.Split(",", StringSplitOptions.RemoveEmptyEntries)[0]
                                    //    &&
                                    //    n.ContractStatusCode != searchInfo.contractStatusCode.Split(",", StringSplitOptions.RemoveEmptyEntries)[1]
                                    //    &&
                                    //    !string.IsNullOrEmpty(n.ContractStatusCode)
                                    //    )
                                    && (string.IsNullOrEmpty(searchInfo.contractStatusCode) || n.ContractStatusCode == searchInfo.contractStatusCode)
                                    && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                    )
                        .ToListAsync();

                    total = list.Count;


                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ConferenceContract>> GetConferenceContractListByIsGiveWithAllContractStatusCode(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                && (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                && (n.IsDelete == searchInfo.ccIsdelete)
                                && (n.companyContract.Where(m => m.companyServicePack.IsGive == false).Count() > 0)
                                //&& (n.companyContract.Where(m => m.CCIsdelete == true).Count() == 0)
                                && (searchInfo.isDiscount ? n.delegateServicePackDiscountForConferenceContract.Count == 0 : 0 == 0)
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                )
                    .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-CreatedOn" : searchInfo.orderings)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetConferenceContractListByIsGiveWithAllContractStatusCodeCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract)
                        .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                    && (
                                    (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                        ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    )
                                    && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                    && (n.IsDelete == searchInfo.ccIsdelete)
                                    && (n.companyContract.Where(m => m.companyServicePack.IsGive == false).Count() > 0)
                                    //&& (n.companyContract.Where(m => m.CCIsdelete == true).Count() == 0)
                                    && (searchInfo.isDiscount ? n.delegateServicePackDiscountForConferenceContract.Count == 0 : 0 == 0)
                                    && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                    && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                    )
                        .ToListAsync();

                    total = list.Count;


                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ConferenceContract>> GetConferenceContractByCompanyIdList(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract)
                    .Where(n => (n.CompanyId == id)
                     && (n.IsDelete == false))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ConferenceContract>> GetConferenceContractByCompanyIdAndYearList(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceContract.Include(n => n.companyContract).Include(n => n.delegateServicePackDiscountForConferenceContract)
                    .Where(n => (n.CompanyId == searchInfo.companyId)
                     && (n.ContractYear == searchInfo.year)
                     && (n.IsDelete == false))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ConferenceContract> GetConferenceContractById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.ConferenceContract
                                             .Include(n => n.companyContract)
                                             .Include(n => n.delegateServicePackDiscountForConferenceContract)
                                             .FirstOrDefaultAsync(n => n.ConferenceContractId == gid
                                                                       //&& n.companyContract.Where(m => m.CCIsdelete == false).Count() > 0
                                                                  );

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public ConferenceContract GetConferenceContractByIdToUpdate(string id)
        {
            try
            {
                Guid gid = new Guid(id);
                var item = _context.ConferenceContract
                        .Include(n => n.companyContract)
                        .Include(n => n.delegateServicePackDiscountForConferenceContract)
                        .FirstOrDefault(n => n.ConferenceContractId == gid);

                return item;


            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyForCreateOtherVM> CreateConferenceContractInfo(ConferenceContract model, List<DelegateServicePackDiscountForConferenceContract> dlistdata)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    model.IsDelete = false;
                    model.ContractYear = model.ContractYear;

                    var ContractNumber = CreateContractNumber(model);

                    if (ContractNumber == "wrong" || string.IsNullOrEmpty(ContractNumber))
                    {
                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = 0, msg = "没有对应的合同规则，创建合同失败，请联系管理员！", ext1 = model.ConferenceContractId.ToString(),ext2 = model.ContractNumber };
                    }

                    model.ContractNumber = ContractNumber;


                    //如果在生成合同时，有折扣信息，把折扣信息存入DelegateServicePackDiscountForConferenceContract表内
                    if (dlistdata.Count > 0)
                    {
                        await _context.DelegateServicePackDiscountForConferenceContract.AddRangeAsync(dlistdata);
                    }

                    await _context.ConferenceContract.AddAsync(model);

                    //存入memberapi服务中membercontract表中，使用合同登录时使用
                    MemberContractStruct memberContractStruct = new MemberContractStruct
                    {
                        MemeberPK = GetCompanyById(model.CompanyId.ToString()).MemberPK,
                        MemContract = model.ContractNumber,
                        MemContractType = "C1",//C1代表ConferenceContract内合同类型，C2代表CompanyContract内合同类型，P1代表PesronContract内合同类型
                        CreatedBy = model.CreatedBy
                    };

                    var modify = CreateMemberContract(memberContractStruct);

                    if (!modify.Success && modify.ModifiedCount < 1)
                    {
                        return new ModifyReplyForCreateOtherVM { success = modify.Success, modifiedcount = modify.ModifiedCount, msg = modify.Msg, ext1 = model.ConferenceContractId.ToString(), ext2 = model.ContractNumber };
                    }

                    count = await _context.SaveChangesAsync();

                    //更新CCNumberConfig内Count字段
                    var result = UpdateCCCount(model);

                    //把ContractId插入到队列，提供给CRMApi服务使用
                    var IsRMQ = _rabbitMQPublisher.Publish("CC", "conference_contract_queue", model.ConferenceContractId.ToString());
                    if (!IsRMQ)
                    {
                        return new ModifyReplyForCreateOtherVM { success = IsRMQ, modifiedcount = 0, msg = "ConferenceContractId为" + model.ConferenceContractId.ToString() + "插入队列失败", ext1 = model.ConferenceContractId.ToString(), ext2 = model.ContractNumber };
                    }
                    return new ModifyReplyForCreateOtherVM { success = true, modifiedcount = count, msg = "创建成功", ext1 = model.ConferenceContractId.ToString(), ext2 = model.ContractNumber };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = ex.Message, ext1 = model.ConferenceContractId.ToString(), ext2 = model.ContractNumber };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateConferenceContractInfo(ConferenceContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ConferenceContractId.ToString();

                    var modified_model = GetConferenceContractByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.PaymentStatusCode = model.PaymentStatusCode;
                    modified_model.ContractStatusCode = model.ContractStatusCode;
                    modified_model.ContractCode = model.ContractCode;
                    modified_model.Owerid = model.Owerid;
                    modified_model.Ower = model.Ower;
                    modified_model.ModifiedOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.ModifiedBy = model.ModifiedBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyConferenceContractPaymentStatusCode(ConferenceContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ConferenceContractId.ToString();

                    var modified_model = GetConferenceContractByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.PaymentStatusCode = model.PaymentStatusCode;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyConferenceContractIsSendEmail(List<string> Cidlist)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in Cidlist)
                    {
                        var model = GetConferenceContractByIdToUpdate(item.ToString());
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此" + item.ToString() + "的实例！" };
                        }

                        //修改IsSendEmail为true
                        model.IsSendEmail = true;

                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        /// <summary>
        /// 修改ConferenceContract内业务员字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ModifyReplyVM> ModifyConferenceContractByOwer(ConferenceContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ConferenceContractId.ToString();

                    var modified_model = _context.ConferenceContract.FirstOrDefault(n => n.ConferenceContractId == model.ConferenceContractId);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }


                    //if (modified_model.companyContract.Count > 0)
                    //{
                    //    foreach (var item in modified_model.companyContract)
                    //    {
                    //        item.Owerid = model.Owerid;
                    //        item.Ower = model.Ower;

                    //        if (_context.PersonContract.Where(n => n.ContractId == item.ContractId).Count() > 0)
                    //        {
                    //            foreach (var item2 in _context.PersonContract.Where(n => n.ContractId == item.ContractId).ToList())
                    //            {
                    //                item2.Owerid = model.Owerid;
                    //                item2.Ower = model.Ower;
                    //            }
                    //        }
                    //    }
                    //}

                    var op_ower = modified_model.Ower;
                    modified_model.Owerid = model.Owerid;
                    modified_model.Ower = model.Ower;
                    modified_model.OtherOwnerId = model.OtherOwnerId;
                    modified_model.OtherOwner = model.OtherOwner;
                    //把ConferenceContract对象插入到队列，提供给展览合同服务使用
                    var message = JsonConvert.SerializeObject(modified_model);
                    var IsRMQ = _rabbitMQPublisher.Publish("ConContract", "conference_contract_object_queue", message);
                    if (!IsRMQ)
                    {
                        return new ModifyReplyVM { success = IsRMQ, modifiedcount = 0, msg = "ConferenceContractId为" + model.ConferenceContractId.ToString() + "插入队列失败" };
                    }

                    //插入操作记录表
                    OperateRecord operateRecord = new OperateRecord
                    {
                        Id = Guid.NewGuid(),
                        ContractNumber = modified_model.ContractNumber,
                        OperateTime = DateTime.Now.ToString("yyyy-MM-dd hh:ss:mm"),
                        Operator = model.ModifiedBy,
                        OperateContent = "接口为ModifyConferenceContractByOwer，原内容：" + op_ower + ",修改内容：" + model.Ower
                    };

                    var res = _context.OperateRecord.Add(operateRecord);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyModifyPermissionById(SearchInfo search)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = GetConferenceContractByIdToUpdate(search.conferenceContractId);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    var op_modifyPermission = modified_model.ModifyPermission;
                    modified_model.ModifyPermission = search.modifyPermission;

                    //插入操作记录表
                    OperateRecord operateRecord = new OperateRecord
                    {
                        Id = Guid.NewGuid(),
                        ContractNumber = modified_model.ContractNumber,
                        OperateTime = DateTime.Now.ToString("yyyy-MM-dd hh:ss:mm"),
                        Operator = search.userName,
                        OperateContent = "接口为ModifyModifyPermissionById，原内容：" + op_modifyPermission + ",修改内容：" + search.modifyPermission
                    };

                    var res = _context.OperateRecord.Add(operateRecord);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteConferenceContractById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var model = GetConferenceContractByIdToUpdate(id);
                    if (model == null)

                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    if (model.companyContract.Where(n => n.CCIsdelete == false).Count() > 0)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "CompanyContract表中有二级合同，不允许删除！" };
                    }

                    //虚拟删除
                    model.IsDelete = true;

                    MemberContractRequest memberContractRequest = new MemberContractRequest
                    {
                        MemberContract = model.ContractNumber
                    };
                    var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                    if (cmcount < 1 && cmcount != -1)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除Member服务内MemberContract表数据失败，请联系管理人员！" };
                    }

                    //foreach (var item2 in model.companyContract)
                    //{
                    //    item2.CCIsdelete = true;
                    //}

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteConferenceContractByList(List<string> Cidlist)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in Cidlist)
                    {
                        var model = GetConferenceContractByIdToUpdate(item.ToString());
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此" + item.ToString() + "的实例可以删除！" };
                        }

                        if (model.companyContract.Where(n => n.CCIsdelete == false).Count() > 0)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "ConferenceContractId为" + model.ConferenceContractId + ",在他CompanyContract表中有二级合同，不允许删除！" };
                        }

                        //虚拟删除
                        model.IsDelete = true;

                        MemberContractRequest memberContractRequest = new MemberContractRequest
                        {
                            MemberContract = model.ContractNumber
                        };
                        var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                        if (cmcount < 1 && cmcount != -1)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除合同号为" + model.ContractNumber + "，在Member服务内MemberContract表数据失败，请联系管理人员！" };
                        }

                        //foreach (var item2 in model.companyContract)
                        //{
                        //    item2.CCIsdelete = true;
                        //}

                    }

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteConCAndCCAndPCByConIdList(List<string> Cidlist)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in Cidlist)
                    {
                        var model = GetConferenceContractByIdToUpdate(item.ToString());
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此" + item.ToString() + "的实例可以删除！" };
                        }

                        //if (model.companyContract.Where(n => n.CCIsdelete == false).Count() > 0)
                        //{
                        //    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "ConferenceContractId为" + model.ConferenceContractId + ",在他CompanyContract表中有二级合同，不允许删除！" };
                        //}

                        //虚拟删除
                        model.IsDelete = true;

                        MemberContractRequest memberContractRequest = new MemberContractRequest
                        {
                            MemberContract = model.ContractNumber
                        };
                        var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                        if (cmcount < 1 && cmcount != -1)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除合同号为" + model.ContractNumber + "，在Member服务内MemberContract表数据失败，请联系管理人员！" };
                        }

                        if (model.companyContract.Count > 0)
                        {
                            //同时删除二级合同
                            foreach (var item2 in model.companyContract.Where(n => n.CCIsdelete == false).ToList())
                            {
                                item2.CCIsdelete = true;

                                //同时删除三级合同
                                var pclist = _context.PersonContract.Where(n => n.ContractId == item2.ContractId && n.PCIsdelete == false).ToList();

                                if (pclist.Count > 0)
                                {
                                    foreach (var pcitem in pclist)
                                    {
                                        pcitem.PCIsdelete = true;
                                    }
                                }

                            }
                        }

                    }

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        private string CreateContractNumber(ConferenceContract model)
        {
            var contractNmuber = string.Empty;
            try
            {
                var config = _context.CCNumberConfig.FirstOrDefault(n => (n.ConferenceId == model.ConferenceId) && (n.Status == 1) && (n.IsDelete == false) && (n.Year == model.ContractYear));
                if (config != null)
                {
                    var count = config.Count + 1;

                    //合同规则为：CF2020SNEC0524CS
                    //contractNmuber = config.Prefix + config.Year + config.CNano + count.ToString().PadLeft(4, '0') + model.ContractCode;

                    //合同规则为：21SNECC0001CS
                    contractNmuber = config.Year.Substring(2) + config.CNano + model.ContractCode.Substring(0,1) + count.ToString().PadLeft(4, '0') + model.ContractCode;
                }
            }
            catch (Exception ex)
            {
                contractNmuber = "wrong";
                LogHelper.Error(this, ex);
                return contractNmuber;
            }

            return contractNmuber;
        }

        #endregion

        #region CompanyContract表操作

        public async Task<List<CompanyContract>> GetCompanyContractList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyContract.Include(n => n.companyServicePack).Include(n => n.delegateServicePackDiscount).Include(n => n.personContract)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ComContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                && (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.CompanyServicePackId == new Guid(searchInfo.companyServicePackId))
                                //&& (string.IsNullOrEmpty(searchInfo.contractTypeId) || n.companyServicePack.ContractTypeId == new Guid(searchInfo.contractTypeId))
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                && (searchInfo.isOwer ? n.Owerid == searchInfo.owerid : 0 == 0)
                                && (n.CCIsdelete == searchInfo.ccIsdelete)
                                && (searchInfo.isDiscount ? n.delegateServicePackDiscount.Count == 0 : 0 == 0)
                                && (searchInfo.IsContractTypeWithECode ?
                                (string.IsNullOrEmpty(searchInfo.cTypeCode) || searchInfo.cTypeCode.ToLower().Contains(n.companyServicePack.CTypeCode.ToLower()))
                                :
                                (string.IsNullOrEmpty(searchInfo.cTypeCode) || !searchInfo.cTypeCode.ToLower().Contains(n.companyServicePack.CTypeCode.ToLower())))
                                && (searchInfo.IsCheckIn ? n.IsCheckIn == searchInfo.IsCheckIn : 0 == 0)
                                && (searchInfo.IsVerify ? n.IsVerify == searchInfo.IsVerify : 0 == 0)
                                && (searchInfo.IsFillPC ? n.personContract.Count < n.MaxContractNumber : 0 == 0)
                                && (string.IsNullOrEmpty(searchInfo.companyId) || n.CompanyId == new Guid(searchInfo.companyId))
                                && (string.IsNullOrEmpty(searchInfo.comPrice) || n.ComPrice == searchInfo.comPrice)
                                && (string.IsNullOrEmpty(searchInfo.contractStatusCode) || n.ContractStatusCode == searchInfo.contractStatusCode)
                                && (searchInfo.isGive ? n.companyServicePack.IsGive == true : n.companyServicePack.IsGive == false)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.conferenceContract.ContractYear == searchInfo.year)
                                )
                    .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-CreatedOn" : searchInfo.orderings)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetCompanyContractListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyContract.Include(n => n.conferenceContract).Include(n => n.companyServicePack).Include(n => n.delegateServicePackDiscount).Include(n => n.personContract).ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                                .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.ComContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                && (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.CompanyServicePackId == new Guid(searchInfo.companyServicePackId))
                                //&& (string.IsNullOrEmpty(searchInfo.contractTypeId) || n.companyServicePack.ContractTypeId == new Guid(searchInfo.contractTypeId))
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                && (searchInfo.isOwer ? n.Owerid == searchInfo.owerid : 0 == 0)
                                && (n.CCIsdelete == searchInfo.ccIsdelete)
                                && (searchInfo.IsContractTypeWithECode ?
                                (string.IsNullOrEmpty(searchInfo.cTypeCode) || searchInfo.cTypeCode.ToLower().Contains(n.companyServicePack.CTypeCode.ToLower()))
                                :
                                (string.IsNullOrEmpty(searchInfo.cTypeCode) || !searchInfo.cTypeCode.ToLower().Contains(n.companyServicePack.CTypeCode.ToLower())))
                                && (searchInfo.isDiscount ? n.delegateServicePackDiscount.Count == 0 : 0 == 0)
                                && (searchInfo.IsCheckIn ? n.IsCheckIn == searchInfo.IsCheckIn : 0 == 0)
                                && (searchInfo.IsVerify ? n.IsVerify == searchInfo.IsVerify : 0 == 0)
                                && (searchInfo.IsFillPC ? n.personContract.Count < n.MaxContractNumber : 0 == 0)
                                && (string.IsNullOrEmpty(searchInfo.companyId) || n.CompanyId == new Guid(searchInfo.companyId))
                                && (string.IsNullOrEmpty(searchInfo.comPrice) || n.ComPrice == searchInfo.comPrice)
                                && (string.IsNullOrEmpty(searchInfo.contractStatusCode) || n.ContractStatusCode == searchInfo.contractStatusCode)
                                && (searchInfo.isGive ? n.companyServicePack.IsGive == true : n.companyServicePack.IsGive == false)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.conferenceContract.ContractYear == searchInfo.year)
                                )
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CompanyContract>> GetCompanyContractByCompanyIdList(string id)
        {
            try
            {
                Guid cid = new Guid(id);
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyContract.Include(n => n.personContract).Include(n => n.companyServicePack).Include(n => n.delegateServicePackDiscount)
                    .Where(n => (n.CompanyId == cid)
                     && (n.CCIsdelete == false))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CompanyContract>> GetCompanyContractByConferenceContractIdList(string id)
        {
            try
            {
                Guid cid = new Guid(id);
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyContract.Include(n => n.personContract).Include(n => n.companyServicePack).Include(n => n.delegateServicePackDiscount)
                    .Where(n => (n.ConferenceContractId == cid)
                     && (n.CCIsdelete == false))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<CompanyContract> GetCompanyContractById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.CompanyContract
                        .Include(n => n.personContract)
                        .Include(n => n.companyServicePack)
                        .Include(n => n.delegateServicePackDiscount)
                        .FirstOrDefaultAsync(n => n.ContractId == gid);

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public CompanyContract GetCompanyContractByIdToUpdate(string id)
        {
            try
            {
                Guid gid = new Guid(id);
                var item = _context.CompanyContract
                    .Include(n => n.companyServicePack)
                    .Include(n => n.delegateServicePackDiscount)
                    .Include(n => n.personContract)
                    .FirstOrDefault(n => n.ContractId == gid);

                return item;


            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyForCreateOtherVM> CreateCompanyContractInfo(CompanyContract model, List<DelegateServicePackDiscount> dlistdata)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    //取到CompanyServicePack内ConferenceId字段并赋值给CompanyContract内的ConferenceId字段
                    var CompanyServicePack = GetCompanyServicePackSingleByIdToUpdate(model.CompanyServicePackId);
                    model.ConferenceId = CompanyServicePack.ConferenceId;
                    model.ConferenceName = CompanyServicePack.ConferenceName;

                    model.CCIsdelete = false;
                    model.MaxContractNumberSatUse = model.MaxContractNumber == -1 ? 0 : model.MaxContractNumber;

                    model.ComContractNumber = CreateCompanyContractNumber(model.ConferenceContractId);

                    if (model.ComContractNumber == "wrong" || string.IsNullOrEmpty(model.ComContractNumber))
                    {
                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = "合同号创建失败，请联系管理员！", ext1 = model.ContractId.ToString(), ext2 = model.ComContractNumber };
                    }

                    //如果在生成合同时，有折扣信息，把折扣信息存入delegateServicePackDiscount表内
                    if (dlistdata.Count > 0)
                    {
                        await _context.DelegateServicePackDiscount.AddRangeAsync(dlistdata);
                    }

                    await _context.CompanyContract.AddAsync(model);

                    //存入memberapi服务中membercontract表中，使用合同登录时使用
                    MemberContractStruct memberContractStruct = new MemberContractStruct
                    {
                        MemeberPK = GetCompanyById(model.CompanyId.ToString()).MemberPK,
                        MemContract = model.ComContractNumber,
                        MemContractType = "C2",//C1代表ConferenceContract内合同类型，C2代表CompanyContract内合同类型，P1代表PesronContract内合同类型
                        CreatedBy = model.CreatedBy
                    };

                    var modify = CreateMemberContract(memberContractStruct);

                    if (!modify.Success && modify.ModifiedCount < 1)
                    {
                        return new ModifyReplyForCreateOtherVM { success = modify.Success, modifiedcount = modify.ModifiedCount, msg = modify.Msg, ext1 = model.ContractId.ToString(), ext2 = model.ComContractNumber };
                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyForCreateOtherVM { success = true, modifiedcount = count, msg = "创建成功", ext1 = model.ContractId.ToString(), ext2 = model.ComContractNumber };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = ex.Message, ext1 = model.ContractId.ToString(), ext2 = model.ComContractNumber };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateCompanyContractInfo(CompanyContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ContractId.ToString();

                    var modified_model = GetCompanyContractByIdToUpdate(id);

                    if (modified_model == null)

                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    //修改后判断CompanyServicePackId是否相等，不相等的情况下，取到CompanyServicePack内ConferenceId字段并赋值给CompanyContract内的ConferenceId字段
                    if (modified_model.CompanyServicePackId != model.CompanyServicePackId)
                    {
                        modified_model.CompanyServicePackId = model.CompanyServicePackId;
                        var CompanyServicePack = GetCompanyServicePackSingleByIdToUpdate(model.CompanyServicePackId);
                        modified_model.ConferenceId = CompanyServicePack.ConferenceId;
                        modified_model.ConferenceName = CompanyServicePack.ConferenceName;
                    }



                    modified_model.MaxContractNumberSatUse = modified_model.MaxContractNumber == -1 ? modified_model.personContract.Where(n => n.PCIsdelete == false).Count() : model.MaxContractNumber;

                    modified_model.ContractStatusCode = model.ContractStatusCode;
                    modified_model.Country = model.Country;
                    modified_model.MaxContractNumber = model.MaxContractNumber;
                    modified_model.EnterpriseType = model.EnterpriseType;
                    modified_model.Owerid = model.Owerid;
                    modified_model.Ower = model.Ower;
                    modified_model.ContractCode = model.ContractCode;
                    modified_model.ComPrice = model.ComPrice;
                    modified_model.IsVerify = model.IsVerify;
                    modified_model.IsCheckIn = model.IsCheckIn;
                    modified_model.ModifiedOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.ModifiedBy = model.ModifiedBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public ModifyReplyVM ModifyMaxContractNumberSatUse(CompanyContract model)
        {
            var count = 0;
            try
            {
                string id = model.ContractId.ToString();

                var modified_model = GetCompanyContractByIdToUpdate(id);

                if (modified_model == null)
                {
                    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                }

                modified_model.MaxContractNumberSatUse = modified_model.MaxContractNumber == -1 ?
                    modified_model.personContract.Where(n => n.PCIsdelete == false).Count()
                    :
                    model.MaxContractNumber;

                count = _context.SaveChanges();

                return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyMaxContractNumber(CompanyContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ContractId.ToString();

                    var modified_model = GetCompanyContractByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.MaxContractNumber = model.MaxContractNumber;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyCCPCOwer(ModifyCCPCOwerInfo model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ContractId;

                    var modified_model = GetCompanyContractByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    //修改CompanyContract表内3个字段
                    modified_model.MaxContractNumber = model.MaxContractNumber;
                    modified_model.Ower = model.Ower;
                    modified_model.Owerid = model.Owerid;

                    //同步修改二级合同下个人合同内的业务员字段
                    var pclist = modified_model.personContract.Where(n => n.PCIsdelete == false).ToList();
                    if (pclist.Count > 0)
                    {
                        foreach (var item in pclist)
                        {
                            item.Ower = model.Ower;
                            item.Owerid = model.Owerid;
                        }
                    }


                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteCompanyContractById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var model = GetCompanyContractByIdToUpdate(id);
                    if (model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    if (model.personContract.Where(n => n.PCIsdelete == false).Count() > 0)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "personContract表中有三级合同，不允许删除！" };
                    }
                    //虚拟删除
                    model.CCIsdelete = true;

                    MemberContractRequest memberContractRequest = new MemberContractRequest
                    {
                        MemberContract = model.ComContractNumber
                    };
                    var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                    if (cmcount < 1 && cmcount != -1)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除Member服务内MemberContract表数据失败，请联系管理人员！" };
                    }

                    //foreach (var item2 in model.personContract)
                    //{
                    //    item2.PCIsdelete = true;
                    //}

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyForCreateOtherVM> RemoveCompanyContractIfPersonContractEmpty(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var model = GetCompanyContractByIdToUpdate(id);
                    if (model == null)
                    {
                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！", ext1 = string.Empty, ext2 = model.ComContractNumber };
                    }

                    if (model.personContract.Where(n => n.PCIsdelete == false).Count() > 0)
                    {
                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = "personContract表中有三级合同，不允许删除！", ext1 = model.ConferenceContractId.ToString(), ext2 = model.ComContractNumber };
                    }
                    //虚拟删除
                    model.CCIsdelete = true;

                    MemberContractRequest memberContractRequest = new MemberContractRequest
                    {
                        MemberContract = model.ComContractNumber
                    };
                    var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                    if (cmcount < 1 && cmcount != -1)
                    {
                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = cmcount, msg = "删除Member服务内MemberContract表数据失败，请联系管理人员！", ext1 = model.ConferenceContractId.ToString(), ext2 = model.ComContractNumber };
                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyForCreateOtherVM { success = true, modifiedcount = count, msg = "删除成功", ext1 = model.ConferenceContractId.ToString(), ext2 = model.ComContractNumber };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = ex.Message, ext1 = string.Empty, ext2 = string.Empty };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteCompanyContractByList(List<string> Cidlist)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in Cidlist)
                    {
                        var model = GetCompanyContractByIdToUpdate(item.ToString());
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此" + item.ToString() + "的实例可以删除！" };
                        }

                        if (model.personContract.Where(n => n.PCIsdelete == false).Count() > 0)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "ContractId为" + model.ContractId + ",在personContract表中有三级合同，不允许删除！" };
                        }

                        //虚拟删除
                        model.CCIsdelete = true;

                        MemberContractRequest memberContractRequest = new MemberContractRequest
                        {
                            MemberContract = model.ComContractNumber
                        };
                        var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                        if (cmcount < 1 && cmcount != -1)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除合同号为" + model.ComContractNumber + "，在Member服务内MemberContract表数据失败，请联系管理人员！" };
                        }


                        //foreach (var item2 in model.personContract)
                        //{
                        //    item2.PCIsdelete = true;
                        //}
                    }

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteCCAndPCByCidList(List<string> Cidlist)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in Cidlist)
                    {
                        var model = GetCompanyContractByIdToUpdate(item.ToString());
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此" + item.ToString() + "的实例可以删除！" };
                        }

                        //if (model.personContract.Where(n => n.PCIsdelete == false).Count() > 0)
                        //{
                        //    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "ContractId为" + model.ContractId + ",在personContract表中有三级合同，不允许删除！" };
                        //}

                        //虚拟删除
                        model.CCIsdelete = true;

                        MemberContractRequest memberContractRequest = new MemberContractRequest
                        {
                            MemberContract = model.ComContractNumber
                        };
                        var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                        if (cmcount < 1 && cmcount != -1)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除合同号为" + model.ComContractNumber + "，在Member服务内MemberContract表数据失败，请联系管理人员！" };
                        }

                        if (model.personContract.Count > 0)
                        {
                            foreach (var item2 in model.personContract.Where(n => n.PCIsdelete == false).ToList())
                            {
                                item2.PCIsdelete = true;

                                //删除PersonContractActivityMap表数据
                                var palist = await _context.PersonContractActivityMap.Where(n => n.PersonContractId == item2.PersonContractId.ToString()).ToListAsync();
                                if (palist.Count() > 0)
                                {
                                    _context.PersonContractActivityMap.RemoveRange(palist);
                                }

                                //同步删除ApplyConference表数据
                                var aplist = await _context.ApplyConference.Where(n => n.PersonContractId == item2.PersonContractId.ToString()).ToListAsync();
                                if (aplist.Count() > 0)
                                {
                                    _context.ApplyConference.RemoveRange(aplist);
                                }

                                //删除member.api服务内MemberContract表数据
                                //MemberContractRequest mc_model = new MemberContractRequest
                                //{
                                //    MemberContract = item2.PerContractNumber
                                //};

                                //var mc_count = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                                //if (cmcount < 1 && cmcount != -1)
                                //{
                                //    return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除Member服务内MemberContract表数据失败，请联系管理人员！" };
                                //}

                                //删除个人合同时，虚拟删除InviteCodeRecord表记录
                                var inviterecord_modle = _context.InviteCodeRecord.FirstOrDefault(n => n.InviteCodeId.ToString() == item2.InviteCodeId && n.PersonContractId == item2.PersonContractId.ToString());
                                if (inviterecord_modle != null)
                                {
                                    inviterecord_modle.IsDelete = true;
                                }
                            }
                        }
                    }

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        private string CreateCompanyContractNumber(Guid? gid)
        {
            var contractNmuber = string.Empty;
            try
            {

                var config = _context.ConferenceContract.Find(gid);
                if (config != null)
                {
                    var count = _context.CompanyContract.Where(n => n.ConferenceContractId == gid).Count() + 1;
                    contractNmuber = config.ContractNumber + count.ToString().PadLeft(2, '0');
                }

            }
            catch (Exception ex)
            {
                contractNmuber = "wrong";
                LogHelper.Error(this, ex);
                return contractNmuber;
            }
            return contractNmuber;
        }

        #endregion

        #region CCNumberConfig表操作

        public async Task<List<CCNumberConfig>> GetCCNumberConfigDic()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CCNumberConfig
                    .Where(n => n.IsDelete == false && n.Status == 1)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CCNumberConfig>> GetCCNumberConfigList(int pageindex, int pagesize)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CCNumberConfig
                    .Where(n => n.IsDelete == false)
                    .OrderByDescending(n => n.Status)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }
        public async Task<int> GetCCNumberConfigListCount()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var total = await _context.CCNumberConfig.Where(n => n.IsDelete == false).CountAsync();

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<CCNumberConfig> GetCCNumberConfigById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.CCNumberConfig.FindAsync(gid);

                    return item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public CCNumberConfig GetCCNumberConfigByIdToUpdate(string id)
        {
            try
            {

                Guid gid = new Guid(id);
                var item = _context.CCNumberConfig.Find(gid);

                return item;

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetIsUseCount()
        {
            try
            {
                var total = await _context.CCNumberConfig.Where(n => n.Status == 1).CountAsync();

                return total;

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateCCNumberConfigInfo(CCNumberConfig model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    //var isUseCount = await GetIsUseCount();

                    //if (isUseCount > 0)
                    //{
                    //    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "请先暂停正在使用的合同编号规则，再进行创建！" };
                    //}
                    await _context.CCNumberConfig.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateCCNumberConfigInfo(CCNumberConfig model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.Id.ToString();

                    var modified_model = GetCCNumberConfigByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.Prefix = model.Prefix;
                    modified_model.Year = model.Year;
                    modified_model.CNano = model.CNano;
                    modified_model.Status = model.Status;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }
        private int UpdateCCCount(ConferenceContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var CCount = _context.ConferenceContract.Where(n => n.ConferenceId == model.ConferenceId).Count();
                    var modified_model = _context.CCNumberConfig.FirstOrDefault(n => (n.ConferenceId == model.ConferenceId) && (n.Status == 1) && (n.IsDelete == false));

                    modified_model.Count = CCount;

                    count = _context.SaveChanges();

                    return count;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteCCNumberConfigById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var model = GetCCNumberConfigByIdToUpdate(id);
                    if (model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    //虚拟删除
                    model.IsDelete = true;
                    model.Status = 0;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        private string GetYear()
        {
            var config = _context.CCNumberConfig.FirstOrDefault(n => n.Status == 1 && (n.IsDelete == false));
            return config.Year;
        }

        #endregion

        #region ContractType表操作

        public async Task<List<ContractType>> GetContractTypeDic()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ContractType.ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ContractType>> GetContractTypeList(int pageindex, int pagesize)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ContractType
                    .OrderBy(n => n.Sort)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }
        public async Task<int> GetContractTypeListCount()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var total = await _context.ContractType.CountAsync();

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ContractType> GetContractTypeById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.ContractType.FindAsync(gid);

                    return item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public ContractType GetContractTypeByIdToUpdate(string id)
        {
            try
            {
                Guid gid = new Guid(id);
                var item = _context.ContractType.Find(gid);

                return item;

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateContractTypeInfo(ContractType model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.ContractType.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateContractTypeInfo(ContractType model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ContractTypeId.ToString();

                    var modified_model = GetContractTypeByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.IsSpeaker = model.IsSpeaker;
                    modified_model.IsGive = model.IsGive;
                    modified_model.Sort = model.Sort;
                    modified_model.Translation = model.Translation;
                    modified_model.CTypeCode = model.CTypeCode;
                    modified_model.ModefieldOn = model.ModefieldOn;
                    modified_model.ModefieldBy = model.ModefieldBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteContractTypeById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modle = GetContractTypeByIdToUpdate(id);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    _context.ContractType.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region CompanyServicePack表操作

        public async Task<List<CompanyServicePack>> GetCompanyServicePackDic()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyServicePack
                                     .Where(n => n.IsDelete == false)
                                     .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CompanyServicePack>> GetCompanyServicePackDicByYear(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyServicePack
                                     .Where(n => n.IsDelete == false && n.Year == searchInfo.year)
                                     .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CompanyServicePack>> GetCompanyServicePackListByIsShownOnFront(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyServicePack
                                     .Where(n => n.IsShownOnFront == true
                                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || (n.ConferenceId == searchInfo.conferenceId))
                                                && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                                && n.IsDelete == false
                                                && n.IsGive==searchInfo.isGive
                                     )
                                     .OrderBy(n => n.Sort)
                                     .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CompanyServicePack>> GetCompanyServicePackList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyServicePack
                    .Where(n => (
                                (string.IsNullOrEmpty(searchInfo.Translation))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).CN.Contains(searchInfo.Translation))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).EN.Contains(searchInfo.Translation))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).JP.Contains(searchInfo.Translation))
                                )
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || (n.ConferenceId == searchInfo.conferenceId))
                                && (string.IsNullOrEmpty(searchInfo.conferenceName) || (n.ConferenceName.Contains(searchInfo.conferenceName)))
                                && (string.IsNullOrEmpty(searchInfo.contractTypeId) || (n.ContractTypeId.ToString() == searchInfo.contractTypeId))
                                && (n.IsDelete == searchInfo.IsDelete)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year
                                && n.IsGive==searchInfo.isGive)
                                )
                    .OrderBy(n => n.Sort)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetCompanyServicePackListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyServicePack.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                            .Where(n => (
                                    (string.IsNullOrEmpty(searchInfo.Translation))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).CN.Contains(searchInfo.Translation))
                                    ||
                                        (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).EN.Contains(searchInfo.Translation))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).JP.Contains(searchInfo.Translation))
                                    )
                                    && (string.IsNullOrEmpty(searchInfo.conferenceId) || (n.ConferenceId == searchInfo.conferenceId))
                                    && (string.IsNullOrEmpty(searchInfo.conferenceName) || (n.ConferenceName.Contains(searchInfo.conferenceName)))
                                    && (string.IsNullOrEmpty(searchInfo.contractTypeId) || (n.ContractTypeId.ToString() == searchInfo.contractTypeId))
                                    && (n.IsDelete == searchInfo.IsDelete)
                                    && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                    )
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CompanyServicePack>> GetCompanyServicePackListForLunchOrDinner(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyServicePack
                    .Where(n => (string.IsNullOrEmpty(searchInfo.conferenceId) || (n.ConferenceId.Contains(searchInfo.conferenceId))
                                && (string.IsNullOrEmpty(searchInfo.cTypeCode) || (n.CTypeCode == searchInfo.cTypeCode))
                                && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                && (n.IsDelete == false)
                                ))
                    .OrderBy(n => n.Sort)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<CompanyServicePack>> GetCompanyServicePackListByContractTypeId(string id)
        {
            try
            {
                Guid ctid = new Guid(id);
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.CompanyServicePack
                                             .Where(n => (n.ContractTypeId == ctid)
                                                     && (n.IsDelete == false))
                                             .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public CompanyServicePack GetCompanyServicePackSingleByIdToUpdate(Guid? id)
        {
            try
            {
                var item = _context.CompanyServicePack.Find(id);

                return item;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<CompanyServicePackVM> GetCompanyServicePackById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.CompanyServicePackMap.Include(n => n.companyServicePack)
                                                                   .Include(n => n.servicePack)
                                                                   .FirstOrDefaultAsync(n => n.companyServicePack.CompanyServicePackId == gid);
                    return new CompanyServicePackVM
                    {
                        CompanyServicePack = item == null ? new CompanyServicePack() : item.companyServicePack,
                        ServicePackList = item == null ? new List<ServicePack>() : await _context.CompanyServicePackMap.Where(n => n.CompanyServicePackId == item.CompanyServicePackId).Select(g => g.servicePack).ToListAsync()
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public CompanyServicePackVM GetCompanyServicePackByIdToUpdate(string id)
        {
            try
            {

                Guid gid = new Guid(id);
                var item = _context.CompanyServicePackMap.Include(n => n.companyServicePack)
                                                               .Include(n => n.servicePack)
                                                               .FirstOrDefault(n => n.companyServicePack.CompanyServicePackId == gid);
                return new CompanyServicePackVM
                {
                    CompanyServicePack = item.companyServicePack,
                    ServicePackList = _context.CompanyServicePackMap.Where(n => n.CompanyServicePackId == item.CompanyServicePackId).Select(g => g.servicePack).ToList()
                };

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateCompanyServicePackInfo(CompanyServicePackVM model)
        {
            var count = 0;
            List<CompanyServicePackMap> csplist = new List<CompanyServicePackMap>();
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    model.CompanyServicePack.IsDelete = false;
                    foreach (var item in model.ServicePackList)
                    {
                        CompanyServicePackMap modelmap = new CompanyServicePackMap
                        {
                            MapId = Guid.NewGuid(),
                            CompanyServicePackId = model.CompanyServicePack.CompanyServicePackId,
                            ServicePackId = item.ServicePackId,
                        };
                        csplist.Add(modelmap);
                    }

                    await _context.CompanyServicePack.AddAsync(model.CompanyServicePack);
                    await _context.CompanyServicePackMap.AddRangeAsync(csplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateCompanyServicePackInfo(CompanyServicePackVM model)
        {
            var count = 0;
            List<CompanyServicePackMap> csplist = new List<CompanyServicePackMap>();
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.CompanyServicePack.CompanyServicePackId.ToString();

                    var modified_model = GetCompanyServicePackByIdToUpdate(id);

                    if (modified_model.CompanyServicePack == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.CompanyServicePack.ContractTypeId = model.CompanyServicePack.ContractTypeId;
                    modified_model.CompanyServicePack.Sort = model.CompanyServicePack.Sort;
                    modified_model.CompanyServicePack.Translation = model.CompanyServicePack.Translation;
                    modified_model.CompanyServicePack.PriceRMB = model.CompanyServicePack.PriceRMB;
                    modified_model.CompanyServicePack.PriceUSD = model.CompanyServicePack.PriceUSD;
                    modified_model.CompanyServicePack.IsShownOnFront = model.CompanyServicePack.IsShownOnFront;
                    modified_model.CompanyServicePack.IsSpeaker = model.CompanyServicePack.IsSpeaker;
                    modified_model.CompanyServicePack.IsGive = model.CompanyServicePack.IsGive;
                    modified_model.CompanyServicePack.RemarkTranslation = model.CompanyServicePack.RemarkTranslation;
                    modified_model.CompanyServicePack.ConferenceId = model.CompanyServicePack.ConferenceId;
                    modified_model.CompanyServicePack.ConferenceName = model.CompanyServicePack.ConferenceName;
                    modified_model.CompanyServicePack.Year = model.CompanyServicePack.Year;
                    modified_model.CompanyServicePack.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.CompanyServicePack.ModefieldBy = model.CompanyServicePack.ModefieldBy;

                    var urm = await _context.CompanyServicePackMap.Where(n => n.CompanyServicePackId == model.CompanyServicePack.CompanyServicePackId).ToListAsync();
                    _context.RemoveRange(urm);

                    foreach (var item in model.ServicePackList)
                    {
                        CompanyServicePackMap modelmap = new CompanyServicePackMap
                        {
                            MapId = Guid.NewGuid(),
                            CompanyServicePackId = model.CompanyServicePack.CompanyServicePackId,
                            ServicePackId = item.ServicePackId,
                        };
                        csplist.Add(modelmap);
                    }
                    await _context.CompanyServicePackMap.AddRangeAsync(csplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM
                {
                    success = false,
                    modifiedcount = count,
                    msg = ex.Message
                };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> CopyPackInfoByYear(SearchInfo search)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {

                    var companyservicepack_list = await _context.CompanyServicePack
                                                                .Where(n => n.IsDelete == false
                                                                   && (n.ConferenceId == search.id)
                                                                   && (n.Year == (Convert.ToInt32(search.year) - 1).ToString()))
                                                                .ToListAsync();

                    foreach (var item in companyservicepack_list)
                    {
                        item.CompanyServicePackId = Guid.NewGuid();
                        item.ConferenceId = search.conferenceId;
                        item.ConferenceName = search.conferenceName;
                        item.Year = search.year;
                        item.CreatedOn = DateTime.UtcNow.ToUniversalTime();
                        item.CreatedBy = "admin";
                    }

                    await _context.CompanyServicePack.AddRangeAsync(companyservicepack_list);

                    var servicepack_list = await _context.ServicePack
                                                         .Where(n => n.IsDelete == false
                                                            && (n.ConferenceId == search.id)
                                                            && (n.Year == (Convert.ToInt32(search.year) - 1).ToString()))
                                                         .ToListAsync();

                    foreach (var item in servicepack_list)
                    {
                        item.ServicePackId = Guid.NewGuid();
                        item.ConferenceId = search.conferenceId;
                        item.ConferenceName = search.Translation;
                        item.Year = search.year;
                        item.CreatedOn = DateTime.UtcNow.ToUniversalTime();
                        item.CreatedBy = "admin";
                    }
                    await _context.ServicePack.AddRangeAsync(servicepack_list);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> CreateCompanyServicePackMap(List<CompanyServicePackMapVM> list)
        {
            var count = 0;
            List<CompanyServicePackMap> csplist = new List<CompanyServicePackMap>();
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in list)
                    {
                        CompanyServicePackMap modelmap = new CompanyServicePackMap
                        {
                            MapId = item.MapId,
                            CompanyServicePackId = item.CompanyServicePackId,
                            ServicePackId = item.ServicePackId,
                        };

                        csplist.Add(modelmap);

                        //var modified_model = _context.CompanyServicePack.Find(item.CompanyServicePackId);

                        //modified_model.ConferenceId = item.ConferenceId;
                        //modified_model.ConferenceName = item.ConferenceName;
                    }

                    await _context.CompanyServicePackMap.AddRangeAsync(csplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> CopyPackInfoByYearForESH(SearchInfo search)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {

                    var companyservicepack_list = await _context.CompanyServicePack
                                                                .Where(n => n.IsDelete == false
                                                                   && (n.Year == search.year))
                                                                .ToListAsync();

                    foreach (var item in companyservicepack_list)
                    {
                        item.CompanyServicePackId = Guid.NewGuid();
                        item.ConferenceId = search.conferenceId;
                        item.ConferenceName = search.conferenceName;
                        item.Year = search.year;
                        item.CreatedOn = DateTime.UtcNow.ToUniversalTime();
                        item.CreatedBy = "admin";
                    }

                    await _context.CompanyServicePack.AddRangeAsync(companyservicepack_list);

                    var servicepack_list = await _context.ServicePack
                                                         .Where(n => n.IsDelete == false
                                                            && (n.Year == search.year))
                                                         .ToListAsync();

                    foreach (var item in servicepack_list)
                    {
                        item.ServicePackId = Guid.NewGuid();
                        item.ConferenceId = search.conferenceId;
                        item.ConferenceName = search.Translation;
                        item.Year = search.year;
                        item.CreatedOn = DateTime.UtcNow.ToUniversalTime();
                        item.CreatedBy = "admin";
                    }
                    await _context.ServicePack.AddRangeAsync(servicepack_list);


                    List<CompanyServicePackMap> csplist = new List<CompanyServicePackMap>();
                    foreach (var item in companyservicepack_list)
                    {
                        CompanyServicePackMap modelmap = new CompanyServicePackMap
                        {
                            MapId = Guid.NewGuid(),
                            CompanyServicePackId = item.CompanyServicePackId,
                            ServicePackId = servicepack_list[0].ServicePackId,
                        };
                        csplist.Add(modelmap);
                    }

                    await _context.CompanyServicePackMap.AddRangeAsync(csplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteCompanyServicePackById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var model = await _context.CompanyServicePack.Include(n => n.companyServicePackMap).FirstOrDefaultAsync(n => n.CompanyServicePackId == gid);

                    if (model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }
                    //if (model.companyServicePackMap.Count() > 0)
                    //{
                    //    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "此服务包正在使用中不能删除！" };
                    //}

                    //_context.CompanyServicePack.Remove(model);

                    if (_context.CompanyContract.Where(n => n.CompanyServicePackId == gid && n.CCIsdelete == false).Count() > 0)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "此服务包已被公司合同使用中不能删除！" };
                    }

                    //虚拟删除
                    model.IsDelete = true;

                    _context.CompanyServicePackMap.RemoveRange(model.companyServicePackMap);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<CompanyServicePackVM> GetCompanyServicePackVMByPersonContractNumber(SearchInfo search)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var item = _context.PersonContract.Include(n => n.companyContract)
                        .FirstOrDefault(n => n.PerContractNumber == search.perContractNumber);

                    var CompanyServicePackId = item == null ? string.Empty : item.companyContract.CompanyServicePackId.ToString();
                    var CompanyServicePackVM = await GetCompanyServicePackById(CompanyServicePackId);

                    return CompanyServicePackVM;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        #endregion

        #region ServicePack表操作

        public async Task<List<ServicePack>> GetServicePackListAll()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ServicePack
                        .Where(n => n.IsDelete == false)
                        .ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ServicePack>> GetServicePackByConferenceIdList(string cid)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ServicePack
                    .Where(n => (n.ConferenceId == cid)
                             && (n.IsDelete == false))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ServicePack>> GetServicePackList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ServicePack
                    .Where(n => (
                                (string.IsNullOrEmpty(searchInfo.Translation))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).CN.Contains(searchInfo.Translation))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).EN.Contains(searchInfo.Translation))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).JP.Contains(searchInfo.Translation))
                                )
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                &&
                                (
                                (string.IsNullOrEmpty(searchInfo.conferenceName))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.ConferenceName).CN.Contains(searchInfo.conferenceName))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.ConferenceName).EN.Contains(searchInfo.conferenceName))
                                ||
                                (JsonConvert.DeserializeObject<TranslationVM>(n.ConferenceName).JP.Contains(searchInfo.conferenceName))
                                )
                                && (n.IsDelete == searchInfo.IsDelete)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                )
                    .OrderByDescending(n => n.CreatedOn)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetServicePackListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ServicePack.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                            .Where(n => (
                                    (string.IsNullOrEmpty(searchInfo.Translation))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).CN.Contains(searchInfo.Translation))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).EN.Contains(searchInfo.Translation))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.Translation).JP.Contains(searchInfo.Translation))
                                    )
                                    && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                    &&
                                    (
                                    (string.IsNullOrEmpty(searchInfo.conferenceName))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.ConferenceName).CN.Contains(searchInfo.conferenceName))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.ConferenceName).EN.Contains(searchInfo.conferenceName))
                                    ||
                                    (JsonConvert.DeserializeObject<TranslationVM>(n.ConferenceName).JP.Contains(searchInfo.conferenceName))
                                    )
                                    && (n.IsDelete == searchInfo.IsDelete)
                                    && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                    )
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ServicePackVM> GetServicePackById(string id)
        {
            Guid gid = new Guid(id);
            List<ActivityVM> avmlist = new List<ActivityVM>();
            ServicePackVM servicePackVM = new ServicePackVM();
            servicePackVM.ActivityList = new List<ActivityVM>();
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var item = await _context.ServicePack.Include(n => n.servicePackActivityMap)
                                                     .FirstOrDefaultAsync(n => n.ServicePackId == gid);

                    servicePackVM.ServicePack = item;
                    foreach (var model in item.servicePackActivityMap)
                    {
                        ActivityVM activityVM = new ActivityVM();
                        activityVM.SessionIDs = model.SessionIDs;
                        activityVM.ActivityId = model.ActivityId;
                        activityVM.ActivityName = model.ActivityName;
                        activityVM.SessionConferenceID = model.SessionConferenceID;
                        activityVM.SessionConferenceName = model.SessionConferenceName;
                        activityVM.Sort = model.Sort;
                        avmlist.Add(activityVM);
                    }
                    var aclist = avmlist.OrderBy(n => n.Sort).ToList();
                    servicePackVM.ActivityList = aclist;


                    return servicePackVM;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public ServicePackVM GetServicePackByIdToUpdate(string id)
        {

            List<ActivityVM> avmlist = new List<ActivityVM>();
            ServicePackVM servicePackVM = new ServicePackVM();
            servicePackVM.ActivityList = new List<ActivityVM>();
            try
            {

                Guid gid = new Guid(id);
                var item = _context.ServicePack.Include(n => n.servicePackActivityMap)
                                                 .FirstOrDefault(n => n.ServicePackId == gid);

                servicePackVM.ServicePack = item;
                foreach (var model in item.servicePackActivityMap)
                {
                    ActivityVM activityVM = new ActivityVM();
                    activityVM.SessionIDs = model.SessionIDs;
                    activityVM.ActivityId = model.ActivityId.ToString();
                    activityVM.ActivityName = model.ActivityName;
                    activityVM.SessionConferenceID = model.SessionConferenceID;
                    activityVM.SessionConferenceName = model.SessionConferenceName;
                    activityVM.Sort = model.Sort;
                    avmlist.Add(activityVM);
                }
                var aclist = avmlist.OrderBy(n => n.Sort).ToList();
                servicePackVM.ActivityList = aclist;


                return servicePackVM;


            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateServicePackInfo(ServicePackVM model)
        {
            var count = 0;
            List<ServicePackActivityMap> csplist = new List<ServicePackActivityMap>();
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    model.ServicePack.IsDelete = false;
                    foreach (var item in model.ActivityList)
                    {
                        ServicePackActivityMap modelmap = new ServicePackActivityMap();

                        modelmap.MapId = Guid.NewGuid();
                        modelmap.ServicePackId = model.ServicePack.ServicePackId;
                        modelmap.SessionIDs = item.SessionIDs;
                        modelmap.ActivityId = item.ActivityId;
                        modelmap.ActivityName = item.ActivityName;
                        modelmap.SessionConferenceID = item.SessionConferenceID;
                        modelmap.SessionConferenceName = item.SessionConferenceName;
                        modelmap.Sort = item.Sort;
                        csplist.Add(modelmap);
                    }

                    await _context.ServicePack.AddAsync(model.ServicePack);
                    await _context.ServicePackActivityMap.AddRangeAsync(csplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateServicePackInfo(ServicePackVM model)
        {
            var count = 0;
            List<ServicePackActivityMap> csplist = new List<ServicePackActivityMap>();
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ServicePack.ServicePackId.ToString();

                    var modified_model = GetServicePackByIdToUpdate(id);

                    if (modified_model.ServicePack == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.ServicePack.ConferenceId = model.ServicePack.ConferenceId;
                    modified_model.ServicePack.ConferenceName = model.ServicePack.ConferenceName;
                    modified_model.ServicePack.SessionConferenceId = model.ServicePack.SessionConferenceId;
                    modified_model.ServicePack.SessionConferenceName = model.ServicePack.SessionConferenceName;
                    modified_model.ServicePack.ThirdSessionConferenceId = model.ServicePack.ThirdSessionConferenceId;
                    modified_model.ServicePack.ThirdSessionConferenceName = model.ServicePack.ThirdSessionConferenceName;
                    modified_model.ServicePack.Translation = model.ServicePack.Translation;
                    modified_model.ServicePack.PriceRMB = model.ServicePack.PriceRMB;
                    modified_model.ServicePack.PriceUSD = model.ServicePack.PriceUSD;
                    modified_model.ServicePack.SessionDate = model.ServicePack.SessionDate;
                    modified_model.ServicePack.SessionStartTime = model.ServicePack.SessionStartTime;
                    modified_model.ServicePack.SessionAddress = model.ServicePack.SessionAddress;
                    modified_model.ServicePack.Year = model.ServicePack.Year;
                    modified_model.ServicePack.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.ServicePack.ModefieldBy = model.ServicePack.ModefieldBy;

                    var urm = await _context.ServicePackActivityMap.Where(n => n.ServicePackId == model.ServicePack.ServicePackId).ToListAsync();
                    _context.RemoveRange(urm);

                    foreach (var item in model.ActivityList)
                    {
                        ServicePackActivityMap modelmap = new ServicePackActivityMap();

                        modelmap.MapId = Guid.NewGuid();
                        modelmap.ServicePackId = model.ServicePack.ServicePackId;
                        modelmap.SessionIDs = item.SessionIDs;
                        modelmap.ActivityId = item.ActivityId;
                        modelmap.ActivityName = item.ActivityName;
                        modelmap.SessionConferenceID = item.SessionConferenceID;
                        modelmap.SessionConferenceName = item.SessionConferenceName;
                        modelmap.Sort = item.Sort;
                        csplist.Add(modelmap);
                    }
                    await _context.ServicePackActivityMap.AddRangeAsync(csplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM
                {
                    success = false,
                    modifiedcount = count,
                    msg = ex.Message
                };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteServicePackById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var model = await _context.ServicePack.Include(n => n.servicePackActivityMap).FirstOrDefaultAsync(n => n.ServicePackId == gid);

                    if (model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }
                    //if (model.companyServicePackMap.Count() > 0)
                    //{
                    //    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "此服务包正在使用中不能删除！" };
                    //}

                    //_context.ServicePack.Remove(model);

                    if (_context.CompanyServicePackMap.Where(n => n.ServicePackId == gid).Count() > 0)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "此服务包正在使用中不能删除！" };
                    }

                    //虚拟删除
                    model.IsDelete = true;

                    _context.ServicePackActivityMap.RemoveRange(model.servicePackActivityMap);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<bool> IsCanDeleteAcitvity(string sCid)
        {
            var result = false;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ServicePackActivityMap.Where(n => n.SessionConferenceID == sCid).ToListAsync();
                    result = list.Count() > 0 ? false : true;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
            return result;
        }

        #endregion
         
        #region ServicePackActivityMap表操作

        public async Task<bool> IsExistSessionConferencdId(SearchInfo search)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ServicePackActivityMap
                                             .Where(n => n.SessionConferenceID == search.sessionConferenceId)
                                             .ToListAsync();

                    return list.Count > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> RemoveSCBySessionConferencdId(SearchInfo search)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ServicePackActivityMap
                                             .Where(n => n.SessionConferenceID == search.sessionConferenceId)
                                             .ToListAsync();
                    if (list.Count < 1)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有查到此sessionConferenceId的数据！" };
                    }

                    foreach (var item in list)
                    {
                        item.SessionConferenceID = string.Empty;
                        item.SessionConferenceName = string.Empty;

                        var sessionIds_list = item.SessionIDs.Split(',').ToList();

                        if (sessionIds_list.Count > 1)
                        {

                            foreach (var sessionId in sessionIds_list)
                            {
                                if (search.sessionConferenceId == sessionId)
                                {
                                    sessionIds_list.Remove(sessionId);
                                    break;
                                }
                            }

                            item.SessionIDs = String.Join(",", sessionIds_list.ToArray());
                        }
                        else
                        {
                            item.SessionIDs = string.Empty;
                        }

                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region DelegateServicePackDiscount表操作

        public async Task<List<DelegateServicePackDiscount>> GetDSPDList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.DelegateServicePackDiscount.Include(n => n.companyContract).ThenInclude(n => n.companyServicePack)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.companyContract.ComContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                && (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.companyContract.ConferenceId == searchInfo.conferenceId)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                && (n.companyContract.CCIsdelete == searchInfo.ccIsdelete))
                    .OrderByDescending(n => n.CreatedOn)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetDSPDListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.DelegateServicePackDiscount.Include(n => n.companyContract).ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                        .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.companyContract.ComContractNumber.ToLower().Contains(searchInfo.comContractNumber.ToLower()))
                                    && (
                                    (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                        ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    )
                                    && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.companyContract.ConferenceId == searchInfo.conferenceId)
                                    && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                    && (n.companyContract.CCIsdelete == searchInfo.ccIsdelete))
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<DelegateServicePackDiscount> GetDSPDById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.DelegateServicePackDiscount
                        .Include(n => n.companyContract)
                        .ThenInclude(n => n.companyServicePack)
                        .FirstOrDefaultAsync(n => n.DiscountId == gid);

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateDSPDInfo(DelegateServicePackDiscount model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.DelegateServicePackDiscount.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateDSPDInfo(DelegateServicePackDiscount model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = await _context.DelegateServicePackDiscount.FindAsync(model.DiscountId);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.ContractId = model.ContractId;
                    modified_model.PriceAfterDiscountRMB = model.PriceAfterDiscountRMB;
                    modified_model.PriceAfterDiscountUSD = model.PriceAfterDiscountUSD;
                    modified_model.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.ModefieldBy = model.ModefieldBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteDSPDById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var modle = await _context.DelegateServicePackDiscount.FindAsync(gid);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    _context.DelegateServicePackDiscount.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region DelegateServicePackDiscountForConferenceContract表操作

        public async Task<List<DelegateServicePackDiscountForConferenceContract>> GetDSPDFCCList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.DelegateServicePackDiscountForConferenceContract.Include(n => n.conferenceContract)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.conferenceContract.ContractNumber.Contains(searchInfo.comContractNumber))
                                && (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.conferenceContract.ComNameTranslation).CompanyCN.Contains(searchInfo.comNameTranslation))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.conferenceContract.ComNameTranslation).CompanyEN.Contains(searchInfo.comNameTranslation))
                                )
                                && (n.conferenceContract.IsDelete == searchInfo.IsDelete))
                    .OrderByDescending(n => n.CreatedOn)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetDSPDFCCListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.DelegateServicePackDiscountForConferenceContract.Include(n => n.conferenceContract).ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                        .Where(n => (string.IsNullOrEmpty(searchInfo.comContractNumber) || n.conferenceContract.ContractNumber.Contains(searchInfo.comContractNumber))
                                    && (
                                    (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.conferenceContract.ComNameTranslation).CompanyCN.Contains(searchInfo.comNameTranslation))
                                        ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.conferenceContract.ComNameTranslation).CompanyEN.Contains(searchInfo.comNameTranslation))
                                    )
                                    && (n.conferenceContract.IsDelete == searchInfo.IsDelete))
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<DelegateServicePackDiscountForConferenceContract> GetDSPDFCCById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.DelegateServicePackDiscountForConferenceContract
                        .Include(n => n.conferenceContract)
                        .FirstOrDefaultAsync(n => n.DiscountId == gid);

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateDSPDFCCInfo(DelegateServicePackDiscountForConferenceContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.DelegateServicePackDiscountForConferenceContract.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateDSPDFCCInfo(DelegateServicePackDiscountForConferenceContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = await _context.DelegateServicePackDiscountForConferenceContract.FindAsync(model.DiscountId);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.ConferenceContractId = model.ConferenceContractId;
                    modified_model.PriceAfterDiscountRMB = model.PriceAfterDiscountRMB;
                    modified_model.PriceAfterDiscountUSD = model.PriceAfterDiscountUSD;
                    modified_model.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.ModefieldBy = model.ModefieldBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteDSPDFCCById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var modle = await _context.DelegateServicePackDiscountForConferenceContract.FindAsync(gid);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    _context.DelegateServicePackDiscountForConferenceContract.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region PersonContract表操作

        public async Task<List<PersonContract>> GetPersonContractList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract.Include(n => n.companyContract).ThenInclude(n => n.companyServicePack)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber) || n.PerContractNumber.ToLower().Contains(searchInfo.perContractNumber.ToLower()))
                                &&
                                (
                                (string.IsNullOrEmpty(searchInfo.memTranslation))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberEN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                )
                                &&
                                (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                &&
                                (string.IsNullOrEmpty(searchInfo.companyServicePackId) ||
                                (searchInfo.companyServicePackId.Split(new char[] { ',' }).Length > 0 ?
                                searchInfo.companyServicePackId.Contains(n.companyContract.CompanyServicePackId.ToString())
                                :
                                n.companyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId)))
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                && (string.IsNullOrEmpty(searchInfo.cTypeCode) ||
                                    (searchInfo.cTypeCode.Split(new char[] { ',' }).Length > 0 ?
                                    searchInfo.cTypeCode.Contains(n.CTypeCode)
                                    :
                                    n.CTypeCode == searchInfo.cTypeCode))
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                && (string.IsNullOrEmpty(searchInfo.memberPK) || n.MemberPK == new Guid(searchInfo.memberPK))
                                && (string.IsNullOrEmpty(searchInfo.exTypeCode.ToLower()) || n.PerContractNumber.ToLower().Contains(searchInfo.exTypeCode.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.contractCode.ToLower()) || n.PerContractNumber.ToLower().Contains(searchInfo.contractCode.ToLower()))
                                && (n.PCIsdelete == searchInfo.pcIsdelete)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.companyContract.conferenceContract.ContractYear == searchInfo.year)
                                && (string.IsNullOrEmpty(searchInfo.companyId) || n.companyContract.CompanyId == new Guid(searchInfo.companyId))
                                && (searchInfo.isVerifyGive ? n.companyContract.companyServicePack.IsGive == searchInfo.isGive : 0 == 0)
                                )
                    .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-CreatedOn" : searchInfo.orderings)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetPersonContractListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract.Include(n => n.companyContract).ThenInclude(n => n.conferenceContract)
                                    .Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber) || n.PerContractNumber.ToLower().Contains(searchInfo.perContractNumber.ToLower()))
                                    && (
                                    (string.IsNullOrEmpty(searchInfo.memTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberEN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                    )
                                    &&
                                    (
                                    (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                        ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    )
                                    &&
                                    (string.IsNullOrEmpty(searchInfo.companyServicePackId) ||
                                    (searchInfo.companyServicePackId.Split(new char[] { ',' }).Length > 0 ?
                                    searchInfo.companyServicePackId.Contains(n.companyContract.CompanyServicePackId.ToString())
                                    :
                                    n.companyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId)))
                                    && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                    && (string.IsNullOrEmpty(searchInfo.cTypeCode) ||
                                        (searchInfo.cTypeCode.Split(new char[] { ',' }).Length > 0 ?
                                        searchInfo.cTypeCode.Contains(n.CTypeCode)
                                        :
                                        n.CTypeCode == searchInfo.cTypeCode))
                                    && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                    && (string.IsNullOrEmpty(searchInfo.memberPK) || n.MemberPK == new Guid(searchInfo.memberPK))
                                    && (string.IsNullOrEmpty(searchInfo.exTypeCode.ToLower()) || n.PerContractNumber.ToLower().Contains(searchInfo.exTypeCode.ToLower()))
                                    && (string.IsNullOrEmpty(searchInfo.contractCode) || n.PerContractNumber.ToLower().Contains(searchInfo.contractCode.ToLower()))
                                    && (n.PCIsdelete == searchInfo.pcIsdelete)
                                    && (string.IsNullOrEmpty(searchInfo.year) || n.companyContract.conferenceContract.ContractYear == searchInfo.year)
                                    && (string.IsNullOrEmpty(searchInfo.companyId) || n.companyContract.CompanyId == new Guid(searchInfo.companyId))
                                    && (searchInfo.isVerifyGive ? n.companyContract.companyServicePack.IsGive == searchInfo.isGive : 0 == 0)
                                    )
                        .ToListAsync();

                        total = list.Count();

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<PersonContract>> GetPersonContractByNewList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract.Include(n => n.companyContract).ThenInclude(n => n.companyServicePack)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber) || n.PerContractNumber.Contains(searchInfo.perContractNumber))
                                &&
                                (
                                (string.IsNullOrEmpty(searchInfo.memTranslation))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN.Contains(searchInfo.memTranslation))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberEN.Contains(searchInfo.memTranslation))
                                )
                                &&
                                (string.IsNullOrEmpty(searchInfo.companyServicePackId) ||
                                (searchInfo.companyServicePackId.Split(new char[] { ',' }).Length > 0 ?
                                searchInfo.companyServicePackId.Contains(n.companyContract.CompanyServicePackId.ToString())
                                :
                                n.companyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId)))
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                && (string.IsNullOrEmpty(searchInfo.cTypeCode) ||
                                    (searchInfo.cTypeCode.Split(new char[] { ',' }).Length > 0 ?
                                    searchInfo.cTypeCode.Contains(n.CTypeCode)
                                    :
                                    n.CTypeCode == searchInfo.cTypeCode))
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                && (string.IsNullOrEmpty(searchInfo.enterpriseType) || n.companyContract.EnterpriseType.ToString() == searchInfo.enterpriseType)
                                && (n.PCIsdelete == searchInfo.pcIsdelete)
                                && (string.IsNullOrEmpty(searchInfo.year)
                                || (searchInfo.isNowYear ? n.companyContract.conferenceContract.ContractYear == searchInfo.year
                                                         : n.companyContract.conferenceContract.ContractYear != searchInfo.year))
                                )
                    .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-CreatedOn" : searchInfo.orderings)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetPersonContractByNewListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract.Include(n => n.companyContract).ThenInclude(n => n.conferenceContract).ToListAsync();

                    if (searchInfo != null)
                    {
                        total = list
                            .Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber) || n.PerContractNumber.Contains(searchInfo.perContractNumber))
                                    && (
                                    (string.IsNullOrEmpty(searchInfo.memTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN.Contains(searchInfo.memTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberEN.Contains(searchInfo.memTranslation))
                                    )
                                    &&
                                    (string.IsNullOrEmpty(searchInfo.companyServicePackId) ||
                                    (searchInfo.companyServicePackId.Split(new char[] { ',' }).Length > 0 ?
                                    searchInfo.companyServicePackId.Contains(n.companyContract.CompanyServicePackId.ToString())
                                    :
                                    n.companyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId)))
                                    && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                    && (string.IsNullOrEmpty(searchInfo.cTypeCode) ||
                                        (searchInfo.cTypeCode.Split(new char[] { ',' }).Length > 0 ?
                                        searchInfo.cTypeCode.Contains(n.CTypeCode)
                                        :
                                        n.CTypeCode == searchInfo.cTypeCode))
                                    && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                    && (string.IsNullOrEmpty(searchInfo.enterpriseType) || n.companyContract.EnterpriseType.ToString() == searchInfo.enterpriseType)
                                    && (n.PCIsdelete == searchInfo.pcIsdelete)
                                    && (string.IsNullOrEmpty(searchInfo.year)
                                    || (searchInfo.isNowYear ? n.companyContract.conferenceContract.ContractYear == searchInfo.year
                                                             : n.companyContract.conferenceContract.ContractYear != searchInfo.year))
                                    )
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<PersonContract>> GetPersonContractByContractIdList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract
                    .Where(n => (n.ContractId == new Guid(searchInfo.contractId))
                                && (n.PCIsdelete == searchInfo.pcIsdelete))
                    .OrderByDescending(n => n.CreatedOn)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetPersonContractByContractIdListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list.Where(n => (n.ContractId == new Guid(searchInfo.contractId))
                                                && (n.PCIsdelete == searchInfo.pcIsdelete))
                                    .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<PersonContract>> GetPersonContractByMemberPKListWithNoPagination(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    List<PersonContract> list = new List<PersonContract>();

                    list = await _context.PersonContract.Include(n => n.companyContract).ThenInclude(n => n.conferenceContract)
                    .Where(n => (n.MemberPK == new Guid(searchInfo.memberPK))
                                &&
                                (string.IsNullOrEmpty(searchInfo.companyServicePackId) ||
                                (searchInfo.companyServicePackId.Split(new char[] { ',' }).Length > 0 ?
                                 searchInfo.companyServicePackId.Contains(n.companyContract.CompanyServicePackId.ToString())
                                 :
                                 n.companyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId))
                                 )
                                && (string.IsNullOrEmpty(searchInfo.year) || n.companyContract.conferenceContract.ContractYear == searchInfo.year)
                                && (string.IsNullOrEmpty(searchInfo.contractCode) || n.PerContractNumber.Contains(searchInfo.contractCode))
                                && (n.PCIsdelete == false)
                                )
                    .OrderByDescending(n => n.CreatedOn)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<PersonContract>> GetPersonContractByMemberPKList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract
                                .Where(n => (n.MemberPK == new Guid(searchInfo.memberPK))
                                            && (string.IsNullOrEmpty(searchInfo.exTypeCode.ToLower()) || n.PerContractNumber.ToLower().Contains(searchInfo.exTypeCode.ToLower()))
                                            && (n.PCIsdelete == false))
                                .OrderByDescending(n => n.CreatedOn)
                                .Skip(((pageindex - 1) * pagesize))
                                .Take(pagesize)
                                .ToListAsync();


                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetPersonContractByMemberPKListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list.Where(n => (n.MemberPK == new Guid(searchInfo.memberPK))
                                             && (string.IsNullOrEmpty(searchInfo.exTypeCode.ToLower()) || n.PerContractNumber.ToLower().Contains(searchInfo.exTypeCode.ToLower()))
                                             && (n.PCIsdelete == false))
                                     .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<PersonContract> GetPersonContractById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.PersonContract.Include(n => n.companyContract).ThenInclude(n => n.companyServicePack)
                        .FirstOrDefaultAsync(n => n.PersonContractId == gid);

                    return item;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<PersonContract> GetPersonContractByPersonContractNumber(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var item = await _context.PersonContract
                        .FirstOrDefaultAsync(n => n.PerContractNumber == searchInfo.perContractNumber);

                    return item;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public PersonContract GetPersonContractByIdToUpdate(string id)
        {
            try
            {

                Guid gid = new Guid(id);
                var item = _context.PersonContract
                    .FirstOrDefault(n => n.PersonContractId == gid);

                return item;


            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyForCreateOtherVM> CreatePersonContractInfo(PersonContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var percontract_count = _context.PersonContract.Where(n => n.ContractId == model.ContractId && n.PCIsdelete == false).Count();
                    var comcontract_count = _context.CompanyContract.FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).MaxContractNumber;

                    if (percontract_count >= comcontract_count)
                    {
                        if (comcontract_count != -1)
                        {
                            return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = string.Format("该公司下个人合同数为{0}已达二级合同数为{1}上限，不允许再添加!", percontract_count, comcontract_count), ext1 = string.Empty,ext2 = string.Empty };
                        }
                    }

                    if (_context.PersonContract.Where(n => n.ContractId == model.ContractId && n.MemberPK == model.MemberPK && n.PCIsdelete == false).Count() > 0)
                    {
                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = "同一个人不能添加两张一样类型的合同！This person already has a contract of the same type!", ext1 = model.PersonContractId.ToString(), ext2 = string.Empty };
                    }

                    model.PCIsdelete = false;
                    model.PerContractNumber = PersonContractNumber(model.ContractId);

                    if (model.PerContractNumber == "wrong" || string.IsNullOrEmpty(model.PerContractNumber))
                    {
                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = "个人合同号创建失败，请联系管理员！", ext1 = model.ContractId.ToString(), ext2 = model.PerContractNumber };
                    }

                    var servicePackActivityMaps = GetServicePackActivityMapList(model.ContractId);

                    await _context.PersonContract.AddAsync(model);

                    //把数据拼齐并插入PersonContractActivityMap表内
                    List<PersonContractActivityMap> plist = new List<PersonContractActivityMap>();
                    foreach (var item in servicePackActivityMaps)
                    {
                        PersonContractActivityMap pmodel = new PersonContractActivityMap
                        {
                            MapId = Guid.NewGuid(),
                            PersonContractId = model.PersonContractId.ToString(),
                            MemberPK = model.MemberPK.ToString(),
                            ActivityId = item.ActivityId.ToString(),
                            ActivityName = item.ActivityName,
                            SessionConferenceID = item.SessionConferenceID.ToString(),
                            SessionConferenceName = item.SessionConferenceName,
                            IsConfirm = false,
                            IsCheck = false,
                            Year = GetYear()
                        };
                        plist.Add(pmodel);
                    }

                    await _context.PersonContractActivityMap.AddRangeAsync(plist);

                    //存入member.api服务中membercontract表中，使用个人合同登录时使用
                    MemberContractStruct memberContractStruct = new MemberContractStruct
                    {
                        MemeberPK = model.MemberPK.ToString(),
                        MemContract = model.PerContractNumber,
                        MemContractType = "P1",//C1代表ConferenceContract内合同类型，C2代表CompanyContract内合同类型，P1代表PesronContract内合同类型
                        CreatedBy = model.CreatedBy
                    };

                    var modify = CreateMemberContract(memberContractStruct);

                    if (modify.ModifiedCount < 1)
                    {
                        return new ModifyReplyForCreateOtherVM { success = modify.Success, modifiedcount = modify.ModifiedCount, msg = modify.Msg, ext1 = model.PersonContractId.ToString(), ext2 = model.PerContractNumber };
                    }

                    //如果ContractType表内IsSpeaker为true时，插入数据到Conference服务内Participant表
                    var cresult = await CreateParticipantInfo(model);

                    if (cresult.Createcount < 0)
                    {
                        //删除member.api服务内MemberContract表数据
                        MemberContractRequest memberContractRequest = new MemberContractRequest
                        {
                            MemberContract = model.PerContractNumber
                        };

                        var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                        if (cmcount < 1 && cmcount != -1)
                        {
                            return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = cmcount, msg = "在调取CreateParticipantInfo时发生错误，先删除Member服务内MemberContract表时数据失败，请联系管理人员！", ext1 = model.PersonContractId.ToString(), ext2 = model.PerContractNumber };
                        }

                        return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = cresult.Createcount, msg = cresult.Msg, ext1 = model.PersonContractId.ToString(), ext2 = model.PerContractNumber };
                    }


                    #region 创建子合同时，创建member服务内visitor并修改member表内memkindtype字段为CONFERENCE
                    CreatePCMQVM createPCMQVM = new CreatePCMQVM();
                    createPCMQVM.ContractId = model.PersonContractId.ToString();
                    createPCMQVM.ContractNumber = model.PerContractNumber;
                    createPCMQVM.Ower = model.Ower;
                    createPCMQVM.Owerid = model.Owerid;
                    createPCMQVM.MemberPK = model.MemberPK.ToString();
                    var ContractYear = _context.CompanyContract.Include(n=>n.conferenceContract).FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).conferenceContract.ContractYear;
                    createPCMQVM.Year = ContractYear;
                    createPCMQVM.TypeCode = "CONFERENCE";

                    var message = JsonConvert.SerializeObject(createPCMQVM);
                    var IsRMQ = _rabbitMQPublisher.Publish("CP", "create_pc_queue", message);
                    if (!IsRMQ)
                    {
                        return new ModifyReplyForCreateOtherVM { success = IsRMQ, modifiedcount = 0, msg = "PersonContractId为" + model.PersonContractId.ToString() + "插入队列失败", ext1 = model.PersonContractId.ToString() };
                    }
                    #endregion

                    count = await _context.SaveChangesAsync();


                    //修改CompanyContract表内MaxContractNumberSatUse字段
                    CompanyContract comodel = new CompanyContract
                    {
                        ContractId = model.ContractId.Value,
                        MaxContractNumber = comcontract_count
                    };

                    var re_count = ModifyMaxContractNumberSatUse(comodel);

                    //if (re_count.modifiedcount < 1)
                    //{
                    //    return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = "修改CompanyContract表内MaxContractNumberSatUse字段时报错！", ext1 = string.Empty };
                    //}

                    return new ModifyReplyForCreateOtherVM { success = true, modifiedcount = count, msg = "创建成功", ext1 = model.PersonContractId.ToString(), ext2 = model.PerContractNumber };
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = ex.Message, ext1 = model.PersonContractId.ToString(), ext2 = model.PerContractNumber };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdatePersonContractInfo(PersonContract model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.PersonContractId.ToString();

                    var modified_model = GetPersonContractByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    var pc_list = _context.PersonContract.Where(n => n.ContractId == model.ContractId && n.MemberPK == model.MemberPK && n.PCIsdelete == false).ToList();
                    if (pc_list.Count() > 0)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "同一个人不能添加两张一样类型的合同！This person already has a contract of the same type!" };
                    }

                    #region 修改子合同时，根据要求创建member服务内visitor并修改member表内memkindtype字段为CONFERENCE
                    var old_typeCode = string.Empty;
                    var pc_old_list = _context.PersonContract.Where(n => n.MemberPK == modified_model.MemberPK && n.PCIsdelete == false).ToList();
                    if (pc_old_list.Count > 1)
                    {
                        old_typeCode = "CONFERENCE";
                    }
                    
                    ModifyPCMQVM modifyPCMQVM = new ModifyPCMQVM();

                    modifyPCMQVM.OldMemberPK = modified_model.MemberPK.ToString();
                    modifyPCMQVM.NewMemberPK = model.MemberPK.ToString();

                    modifyPCMQVM.OldTypeCode = old_typeCode;
                    modifyPCMQVM.NewTypeCode = "CONFERENCE";

                    modifyPCMQVM.ContractId = model.PersonContractId.ToString();
                    modifyPCMQVM.ContractNumber = modified_model.PerContractNumber;
                    var ContractYear = _context.CompanyContract.Include(n => n.conferenceContract).FirstOrDefault(n => n.ContractId == modified_model.ContractId && n.CCIsdelete == false).conferenceContract.ContractYear;
                    modifyPCMQVM.Year = ContractYear;
                    modifyPCMQVM.Ower = modified_model.Ower;
                    modifyPCMQVM.Owerid = modified_model.Owerid;

                    var message = JsonConvert.SerializeObject(modifyPCMQVM);
                    var IsRMQ = _rabbitMQPublisher.Publish("UP", "update_pc_queue", message);
                    if (!IsRMQ)
                    {
                        return new ModifyReplyVM { success = IsRMQ, modifiedcount = 0, msg = "PersonContractId为" + modified_model.PersonContractId.ToString() + "插入队列失败" };
                    }
                    #endregion

                    #region 修改会员信息
                    modified_model.MemberPK = model.MemberPK;
                    modified_model.MemTranslation = model.MemTranslation;
                    modified_model.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.ModefieldBy = model.ModefieldBy;
                    #endregion

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        /// <summary>
        /// 更改个人合同号报道状态
        /// </summary>
        /// <param name="personContractId"></param>
        /// <returns></returns>
        public async Task<ModifyReplyVM> ModifyPersonContractByIsCheckIn(ModifyRequestVM model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = GetPersonContractByIdToUpdate(model.Id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.IsCheckIn = model.IsCheckIn;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyPersonContractIsCheckInByIdList(CheckInRequestVM model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var id in model.Ids)
                    {
                        var modified_model = GetPersonContractByIdToUpdate(id);

                        if (modified_model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "id=" + id + ",当前实例不存在" };
                        }

                        modified_model.IsCheckIn = model.IsCheckIn;
                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyPersonContractIsSendEmail(List<string> Pidlist)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in Pidlist)
                    {
                        var model = GetPersonContractByIdToUpdate(item.ToString());
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此" + item.ToString() + "的实例！" };
                        }

                        //修改IsSendEmail为true
                        model.IsSendEmail = true;
                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyPersonContractIsFianceRecord(List<PersonContractPCNoVM> list)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in list)
                    {
                        var model = _context.PersonContract.FirstOrDefault(n => n.PerContractNumber == item.PerContractNumber);
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此个人合同号：" + item.PerContractNumber + "的实例！" };
                        }

                        //修改IsFianceRecord为true
                        model.IsFianceRecord = item.Comfirmed;
                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> ModifyPersonContractIsPrintByOwerid(SearchInfo searchInfo)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var modified_model_list = _context.PersonContract.Where(n => n.Owerid == searchInfo.owerid && n.PCIsdelete == false).ToList();

                    if (modified_model_list.Count() < 1)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    foreach (var item in modified_model_list)
                    {
                        item.IsPrint = searchInfo.isPrint;
                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }


        public async Task<ModifyReplyVM> ModifyPersonContractIsPrintByids(string ids)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (list.Count > 0)
                    {
                        foreach (var str in list)
                        {
                            var modified_model = _context.PersonContract.FirstOrDefault(n => n.PersonContractId.ToString() == str);

                            if (modified_model == null)
                            {
                                return new ModifyReplyVM { success = false, modifiedcount = count, msg = "PersonContractId为" + str + "当前实例不存在" };
                            }

                            modified_model.IsPrint = true;
                        }
                    }

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }


        public async Task<ModifyReplyVM> DeletePersonContractById(string id)
        {
            var count = 0;

            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var pid = new Guid(id);
                    var model = GetPersonContractByIdToUpdate(id);
                    if (model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    //虚拟删除
                    model.PCIsdelete = true;

                    //同步删除PersonContractActivityMap表数据
                    var palist = await _context.PersonContractActivityMap.Where(n => n.PersonContractId == model.PersonContractId.ToString()).ToListAsync();
                    if (palist.Count() > 0)
                    {
                        _context.PersonContractActivityMap.RemoveRange(palist);
                    }

                    //同步删除ApplyConference表数据
                    var aplist = await _context.ApplyConference.Where(n => n.PersonContractId == model.PersonContractId.ToString()).ToListAsync();
                    if (aplist.Count() > 0)
                    {
                        _context.ApplyConference.RemoveRange(aplist);
                    }

                    //删除member.api服务内MemberContract表数据
                    MemberContractRequest memberContractRequest = new MemberContractRequest
                    {
                        MemberContract = model.PerContractNumber
                    };

                    var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                    if (cmcount < 1 && cmcount != -1)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除Member服务内MemberContract表数据失败，请联系管理人员！" };
                    }

                    //删除个人合同时，虚拟删除InviteCodeRecord表记录
                    var inviterecord_modle = _context.InviteCodeRecord.FirstOrDefault(n => n.InviteCodeId.ToString() == model.InviteCodeId && n.PersonContractId == model.PersonContractId.ToString());
                    if (inviterecord_modle != null)
                    {
                        inviterecord_modle.IsDelete = true;
                    }
                    

                    #region 使用队列，删除member服务内visitor并根据条件修改member表内memkindtype字段
                    var typeCode = string.Empty;
                    var pc_list = _context.PersonContract.Where(n => n.MemberPK == model.MemberPK && n.PCIsdelete == false).ToList();
                    if (pc_list.Count > 1)
                    {
                        typeCode = "CONFERENCE";
                    }

                    DeletePCMQVM deletePCMQVM = new DeletePCMQVM();
                    deletePCMQVM.MemberPK = model.MemberPK.ToString();
                    deletePCMQVM.ContractId = model.PersonContractId.ToString();
                    deletePCMQVM.ContractNumber = model.PerContractNumber;
                    var ContractYear = _context.CompanyContract.Include(n => n.conferenceContract).FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).conferenceContract.ContractYear;
                    deletePCMQVM.Year = ContractYear;
                    deletePCMQVM.TypeCode = typeCode;

                    var message = JsonConvert.SerializeObject(deletePCMQVM);
                    var IsRMQ = _rabbitMQPublisher.Publish("DP", "delete_pc_queue", message);
                    if (!IsRMQ)
                    {
                        return new ModifyReplyVM { success = IsRMQ, modifiedcount = 0, msg = "PersonContractId为" + model.PersonContractId.ToString() + "插入队列失败" };
                    }
                    #endregion

                    //如果ContractType表内IsSpeaker为true时，修改Conference服务内Participant表内IsDelete为true
                    //if (IsCheckSpeaker(model.ContractId))
                    //{
                    //    var cscount = ConferenceServiceClient.DeleteParticioantByPerContractNumber(model.PerContractNumber).Deletecount;

                    //    if (cscount < 0)
                    //    {
                    //        return new ModifyReplyVM { success = false, modifiedcount = cscount, msg = "虚拟删除数据到Conference服务内Participant表IsDelete字段时失败，请联系管理人员！" };
                    //    }
                    //}

                    count = await _context.SaveChangesAsync();

                    //修改CompanyContract表内MaxContractNumberSatUse字段
                    CompanyContract comodel = new CompanyContract
                    {
                        ContractId = model.ContractId.Value,
                        MaxContractNumber = _context.CompanyContract.FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).MaxContractNumber
                    };

                    var re_count = ModifyMaxContractNumberSatUse(comodel);

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeletePersonContractByList(List<string> Pidlist)
        {
            var count = 0;
            var all_count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in Pidlist)
                    {
                        var model = GetPersonContractByIdToUpdate(item.ToString());
                        if (model == null)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此" + item.ToString() + "的实例可以删除！" };
                        }

                        //虚拟删除
                        model.PCIsdelete = true;

                        //同步删除PersonContractActivityMap表数据
                        var palist = await _context.PersonContractActivityMap.Where(n => n.PersonContractId == model.PersonContractId.ToString()).ToListAsync();
                        if (palist.Count() > 0)
                        {
                            _context.PersonContractActivityMap.RemoveRange(palist);
                        }

                        //同步删除ApplyConference表数据
                        var aplist = await _context.ApplyConference.Where(n => n.PersonContractId == model.PersonContractId.ToString()).ToListAsync();
                        if (aplist.Count() > 0)
                        {
                            _context.ApplyConference.RemoveRange(aplist);
                        }

                        MemberContractRequest memberContractRequest = new MemberContractRequest
                        {
                            MemberContract = model.PerContractNumber
                        };
                        var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                        if (cmcount < 1 && cmcount != -1)
                        {
                            return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除合同号为" + model.PerContractNumber + "，在Member服务内MemberContract表数据失败，请联系管理人员！" };
                        }

                        //删除个人合同时，虚拟删除InviteCodeRecord表记录
                        var inviterecord_modle = _context.InviteCodeRecord.FirstOrDefault(n => n.InviteCodeId.ToString() == model.InviteCodeId && n.PersonContractId == model.PersonContractId.ToString());
                        if (inviterecord_modle != null)
                        {
                            inviterecord_modle.IsDelete = true;
                        }

                        #region 使用队列，删除member服务内visitor并根据条件修改member表内memkindtype字段
                        var typeCode = string.Empty;
                        var pc_list = _context.PersonContract.Where(n => n.MemberPK == model.MemberPK && n.PCIsdelete == false).ToList();
                        if (pc_list.Count > 1)
                        {
                            typeCode = "CONFERENCE";
                        }

                        DeletePCMQVM deletePCMQVM = new DeletePCMQVM();
                        deletePCMQVM.MemberPK = model.MemberPK.ToString();
                        deletePCMQVM.ContractId = model.PersonContractId.ToString();
                        deletePCMQVM.ContractNumber = model.PerContractNumber;
                        var ContractYear = _context.CompanyContract.Include(n => n.conferenceContract).FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).conferenceContract.ContractYear;
                        deletePCMQVM.Year = ContractYear;
                        deletePCMQVM.TypeCode = typeCode;

                        var message = JsonConvert.SerializeObject(deletePCMQVM);
                        var IsRMQ = _rabbitMQPublisher.Publish("DP", "delete_pc_queue", message);
                        if (!IsRMQ)
                        {
                            return new ModifyReplyVM { success = IsRMQ, modifiedcount = 0, msg = "PersonContractId为" + model.PersonContractId.ToString() + "插入队列失败" };
                        }
                        #endregion
                        //foreach (var item2 in model.personContract)
                        //{
                        //    item2.PCIsdelete = true;
                        //}

                        count = await _context.SaveChangesAsync();

                        //修改CompanyContract表内MaxContractNumberSatUse字段
                        CompanyContract comodel = new CompanyContract
                        {
                            ContractId = model.ContractId.Value,
                            MaxContractNumber = _context.CompanyContract.FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).MaxContractNumber
                        };

                        var re_count = ModifyMaxContractNumberSatUse(comodel);

                        all_count += count;
                    }


                    return new ModifyReplyVM { success = true, modifiedcount = all_count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeletePersonContractByIdForWeb(string id)
        {
            var count = 0;

            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var pid = new Guid(id);
                    var model = GetPersonContractByIdToUpdate(id);
                    if (model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    //虚拟删除
                    model.PCIsdelete = true;

                    //如二级合同MaxContractNumber为-1，把该条二级合同同时删除
                    var cc_model = await _context.CompanyContract.FirstOrDefaultAsync(n => n.ContractId == model.ContractId);
                    if (cc_model != null)
                    {
                        if (cc_model.MaxContractNumber == -1)
                        {
                            cc_model.CCIsdelete = true;
                        }
                    }

                    //删除PersonContractActivityMap表数据
                    var palist = await _context.PersonContractActivityMap.Where(n => n.PersonContractId == model.PersonContractId.ToString()).ToListAsync();
                    if (palist.Count() > 0)
                    {
                        _context.PersonContractActivityMap.RemoveRange(palist);
                    }

                    //同步删除ApplyConference表数据
                    var aplist = await _context.ApplyConference.Where(n => n.PersonContractId == model.PersonContractId.ToString()).ToListAsync();
                    if (aplist.Count() > 0)
                    {
                        _context.ApplyConference.RemoveRange(aplist);
                    }

                    //删除member.api服务内MemberContract表数据
                    MemberContractRequest memberContractRequest = new MemberContractRequest
                    {
                        MemberContract = model.PerContractNumber
                    };

                    var cmcount = DeleteMemContractByMemContract(memberContractRequest).ModifiedCount;
                    if (cmcount < 1 && cmcount != -1)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = cmcount, msg = "删除Member服务内MemberContract表数据失败，请联系管理人员！" };
                    }

                    //删除个人合同时，虚拟删除InviteCodeRecord表记录
                    var inviterecord_modle = _context.InviteCodeRecord.FirstOrDefault(n => n.InviteCodeId.ToString() == model.InviteCodeId && n.PersonContractId == model.PersonContractId.ToString());
                    if (inviterecord_modle != null)
                    {
                        inviterecord_modle.IsDelete = true;
                    }

                    #region 使用队列，删除member服务内visitor并根据条件修改member表内memkindtype字段
                    var typeCode = string.Empty;
                    var pc_list = _context.PersonContract.Where(n => n.MemberPK == model.MemberPK && n.PCIsdelete == false).ToList();
                    if (pc_list.Count > 1)
                    {
                        typeCode = "CONFERENCE";
                    }

                    DeletePCMQVM deletePCMQVM = new DeletePCMQVM();
                    deletePCMQVM.MemberPK = model.MemberPK.ToString();
                    deletePCMQVM.ContractId = model.PersonContractId.ToString();
                    deletePCMQVM.ContractNumber = model.PerContractNumber;
                    var ContractYear = _context.CompanyContract.Include(n => n.conferenceContract).FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).conferenceContract.ContractYear;
                    deletePCMQVM.Year = ContractYear;
                    deletePCMQVM.TypeCode = typeCode;

                    var message = JsonConvert.SerializeObject(deletePCMQVM);
                    var IsRMQ = _rabbitMQPublisher.Publish("DP", "delete_pc_queue", message);
                    if (!IsRMQ)
                    {
                        return new ModifyReplyVM { success = IsRMQ, modifiedcount = 0, msg = "PersonContractId为" + model.PersonContractId.ToString() + "插入队列失败" };
                    }
                    #endregion

                    //如果ContractType表内IsSpeaker为true时，修改Conference服务内Participant表内IsDelete为true
                    //if (IsCheckSpeaker(model.ContractId))
                    //{
                    //    var cscount = ConferenceServiceClient.DeleteParticioantByPerContractNumber(model.PerContractNumber).Deletecount;

                    //    if (cscount < 0)
                    //    {
                    //        return new ModifyReplyVM { success = false, modifiedcount = cscount, msg = "虚拟删除数据到Conference服务内Participant表IsDelete字段时失败，请联系管理人员！" };
                    //    }
                    //}

                    count = await _context.SaveChangesAsync();

                    //修改CompanyContract表内MaxContractNumberSatUse字段
                    CompanyContract comodel = new CompanyContract
                    {
                        ContractId = model.ContractId.Value,
                        MaxContractNumber = _context.CompanyContract.FirstOrDefault(n => n.ContractId == model.ContractId && n.CCIsdelete == false).MaxContractNumber
                    };

                    var re_count = ModifyMaxContractNumberSatUse(comodel);

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        private string PersonContractNumber(Guid? cid)
        {
            var perContractNmuber = string.Empty;
            try
            {
                var companyContract = _context.CompanyContract.Include(n => n.personContract).FirstOrDefault(n => n.ContractId == cid);

                if (companyContract != null)
                {
                    var comContractNumber = companyContract.ComContractNumber;

                    var count = companyContract.personContract.Count() + 1;

                    perContractNmuber = comContractNumber + count.ToString().PadLeft(3, '0');
                }

            }
            catch (Exception ex)
            {
                perContractNmuber = "wrong";
                LogHelper.Error(this, ex);
                return perContractNmuber;
            }

            return perContractNmuber;
        }

        private bool IsCheckSpeaker(Guid? Cid)
        {
            var CompanyContract = _context.CompanyContract.Include(n => n.companyServicePack).ThenInclude(n => n.contractType).FirstOrDefault(n => n.ContractId == Cid);
            var IsSpeaker = CompanyContract.companyServicePack.contractType.IsSpeaker;
            return IsSpeaker;
        }

        private List<ServicePackActivityMap> GetServicePackActivityMapList(Guid? ContractId)
        {
            try
            {
                List<ServicePackActivityMap> servicePackActivityMaps = new List<ServicePackActivityMap>();
                List<Guid?> ServicePackIdList = new List<Guid?>();
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var CompanyServicePackId = _context.CompanyContract.Find(ContractId).CompanyServicePackId;
                    var list = _context.CompanyServicePackMap.Where(n => n.CompanyServicePackId == CompanyServicePackId).ToList();
                    ServicePackIdList.AddRange(list.Select(n => n.ServicePackId).ToList());

                    foreach (var item in ServicePackIdList)
                    {
                        servicePackActivityMaps.AddRange(_context.ServicePackActivityMap.Where(n => n.ServicePackId == item));
                    }

                    return servicePackActivityMaps;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region ExtraService表操作

        public async Task<List<ExtraService>> GetExtraServiceList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ExtraService
                    .Where(n => (string.IsNullOrEmpty(searchInfo.extraContractNumber) || n.ExtraContractNumber.Contains(searchInfo.extraContractNumber))
                                && (
                                (string.IsNullOrEmpty(searchInfo.memTranslation))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN.Contains(searchInfo.memTranslation))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberEN.Contains(searchInfo.memTranslation))
                                )
                                && (n.IsDelete == searchInfo.IsDelete))
                    .OrderByDescending(n => n.CreatedOn)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetExtraServiceListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ExtraService.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                            .Where(n => (string.IsNullOrEmpty(searchInfo.extraContractNumber) || n.ExtraContractNumber.Contains(searchInfo.extraContractNumber))
                                    && (
                                    (string.IsNullOrEmpty(searchInfo.memTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN.Contains(searchInfo.memTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberEN.Contains(searchInfo.memTranslation))
                                    )
                                    && (n.IsDelete == searchInfo.IsDelete))
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ExtraServiceVM> GetExtraServiceById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid eid = new Guid(id);
                    var item = await _context.ExtraServicePackMap.Include(n => n.extraService)
                                                                   .Include(n => n.servicePack)
                                                                   .FirstOrDefaultAsync(n => n.extraService.ExtraServiceId == eid);
                    return new ExtraServiceVM
                    {
                        ExtraService = item.extraService,
                        ServicePackList = await _context.ExtraServicePackMap.Where(n => n.ExtraServiceId == item.ExtraServiceId).Select(g => g.servicePack).ToListAsync()
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public ExtraServiceVM GetExtraServiceByIdToUpdate(string id)
        {
            try
            {

                Guid eid = new Guid(id);
                var item = _context.ExtraServicePackMap.Include(n => n.extraService)
                                                               .Include(n => n.servicePack)
                                                               .FirstOrDefault(n => n.extraService.ExtraServiceId == eid);
                return new ExtraServiceVM
                {
                    ExtraService = item.extraService,
                    ServicePackList = _context.ExtraServicePackMap.Where(n => n.ExtraServiceId == item.ExtraServiceId).Select(g => g.servicePack).ToList()
                };

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateExtraServiceInfo(ExtraServiceVM model)
        {
            var count = 0;
            List<ExtraServicePackMap> esplist = new List<ExtraServicePackMap>();
            try
            {
                model.ExtraService.IsDelete = false;
                //合同号规则暂时未定，先拟定为空值
                model.ExtraService.ExtraContractNumber = string.Empty;

                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in model.ServicePackList)
                    {
                        ExtraServicePackMap modelmap = new ExtraServicePackMap
                        {
                            MapId = Guid.NewGuid(),
                            ExtraServiceId = model.ExtraService.ExtraServiceId,
                            ServicePackId = item.ServicePackId,
                        };
                        esplist.Add(modelmap);
                    }

                    await _context.ExtraService.AddAsync(model.ExtraService);
                    await _context.ExtraServicePackMap.AddRangeAsync(esplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateExtraServiceInfo(ExtraServiceVM model)
        {
            var count = 0;
            List<ExtraServicePackMap> esplist = new List<ExtraServicePackMap>();
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.ExtraService.ExtraServiceId.ToString();

                    var modified_model = GetExtraServiceByIdToUpdate(id);

                    if (modified_model.ExtraService == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.ExtraService.Ower = model.ExtraService.Ower;
                    modified_model.ExtraService.Owerid = model.ExtraService.Owerid;
                    modified_model.ExtraService.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                    modified_model.ExtraService.ModefieldBy = model.ExtraService.ModefieldBy;

                    var urm = await _context.ExtraServicePackMap.Where(n => n.ExtraServiceId == model.ExtraService.ExtraServiceId).ToListAsync();
                    _context.RemoveRange(urm);

                    foreach (var item in model.ServicePackList)
                    {
                        ExtraServicePackMap modelmap = new ExtraServicePackMap
                        {
                            MapId = Guid.NewGuid(),
                            ExtraServiceId = model.ExtraService.ExtraServiceId,
                            ServicePackId = item.ServicePackId,
                        };
                        esplist.Add(modelmap);
                    }
                    await _context.ExtraServicePackMap.AddRangeAsync(esplist);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM
                {
                    success = false,
                    modifiedcount = count,
                    msg = ex.Message
                };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteExtraServiceById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var model = await _context.ExtraService.Include(n => n.extraServicePackMap).FirstOrDefaultAsync(n => n.ExtraServiceId == gid);

                    if (model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }
                    //if (model.companyServicePackMap.Count() > 0)
                    //{
                    //    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "此服务包正在使用中不能删除！" };
                    //}

                    //_context.ExtraService.Remove(model);

                    //虚拟删除
                    model.IsDelete = true;

                    _context.ExtraServicePackMap.RemoveRange(model.extraServicePackMap);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region ContractStatusDic表操作
        public async Task<List<ContractStatusDic>> GetContractStatusDic()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ContractStatusDic.ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ContractStatusDic>> GetContractStatusDicList(int pageindex, int pagesize)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ContractStatusDic
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }
        public async Task<int> GetContractStatusDicListCount()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var total = await _context.ContractStatusDic.CountAsync();

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ContractStatusDic> GetContractStatusDicById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.ContractStatusDic.FindAsync(gid);

                    return item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public ContractStatusDic GetContractStatusDicByIdToUpdate(string id)
        {
            try
            {

                Guid gid = new Guid(id);
                var item = _context.ContractStatusDic.Find(gid);

                return item;

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateContractStatusDicInfo(ContractStatusDic model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.ContractStatusDic.AddAsync(model);
                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateContractStatusDicInfo(ContractStatusDic model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.Id.ToString();

                    var modified_model = GetContractStatusDicByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.StatusName = model.StatusName;
                    modified_model.StatusCode = model.StatusCode;
                    modified_model.ModefieldOn = model.ModefieldOn;
                    modified_model.ModefieldBy = model.ModefieldBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteContractStatusDicById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modle = GetContractStatusDicByIdToUpdate(id);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    _context.ContractStatusDic.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region RemarkDic表操作
        public async Task<List<RemarkDic>> GetRemarkDic()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.RemarkDic.ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<RemarkDic>> GetRemarkDicList(int pageindex, int pagesize)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.RemarkDic
                                             .Skip(((pageindex - 1) * pagesize))
                                             .Take(pagesize)
                                             .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }
        public async Task<int> GetRemarkDicListCount()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var total = await _context.RemarkDic.CountAsync();

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<RemarkDic> GetRemarkDicById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.RemarkDic.FindAsync(gid);

                    return item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public RemarkDic GetRemarkDicByIdToUpdate(string id)
        {
            try
            {

                Guid gid = new Guid(id);
                var item = _context.RemarkDic.Find(gid);

                return item;

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateRemarkDicInfo(RemarkDic model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.RemarkDic.AddAsync(model);
                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateRemarkDicInfo(RemarkDic model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    string id = model.Id.ToString();

                    var modified_model = GetRemarkDicByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.ContentCn = model.ContentCn;
                    modified_model.ContentEn = model.ContentEn;
                    modified_model.ContentJp = model.ContentJp;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteRemarkDicById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modle = GetRemarkDicByIdToUpdate(id);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    _context.RemarkDic.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region PersonContractActivityMap表操作

        public async Task<List<PersonContractActivityMap>> GetPersonContractActivityMapByMemberPKList(SearchInfo search)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContractActivityMap
                    .Where(n => (n.MemberPK == search.memberPK)
                                && (n.Year == search.year)
                                && (n.IsCheck == search.IsCheckIn)
                                && (n.IsConfirm == search.IsConfirm))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<PersonContractActivityMap>> GetPersonContractActivityMapByActivityIdList(SearchInfo search)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContractActivityMap
                    .Where(n => (n.ActivityId == search.activityId)
                                && (n.Year == search.year)
                                && (n.IsCheck == search.IsCheckIn)
                                && (n.IsConfirm == search.IsConfirm))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<PersonContractActivityMap>> GetPersonContractActivityMapByPersonContractNumberList(SearchInfo search)
        {
            try
            {
                List<PersonContractActivityMap> list = new List<PersonContractActivityMap>();
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var personContract = _context.PersonContract.FirstOrDefault(n => n.PerContractNumber == search.perContractNumber);


                    var memberPK = personContract == null ? string.Empty : personContract.MemberPK.ToString();

                    if (!string.IsNullOrEmpty(memberPK))
                    {
                        list = await _context.PersonContractActivityMap
                                    .Where(n => (n.MemberPK == memberPK)
                                                && (n.Year == search.year)
                                                && (n.IsCheck == search.IsCheckIn)
                                                && (n.IsConfirm == search.IsConfirm))
                                   .ToListAsync();
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreatePersonContractActivityMapInfo(List<PersonContractActivityMap> list)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var item in list)
                    {
                        item.Year = GetYear();
                    }

                    await _context.PersonContractActivityMap.AddRangeAsync(list);
                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdatePersonContractActivityMapInfo(string MemberPK, List<PersonContractActivityMap> list)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {

                    //修改时，先删除根据MemberPK查询出的list，再进行新增修改后的数据
                    var remove_model_list = _context.PersonContractActivityMap.Where(n => n.MemberPK == MemberPK).ToList();
                    if (remove_model_list.Count < 1)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在,不能删除！" };
                    }
                    //_context.RemoveRange(remove_model);

                    //if (list.Count > 0)
                    //{
                    //    foreach (var item in list)
                    //    {
                    //        item.Year = GetYear();
                    //    }

                    //    await _context.PersonContractActivityMap.AddRangeAsync(list);
                    //}

                    foreach (var item in remove_model_list)
                    {
                        foreach (var item2 in list)
                        {
                            if (item2.MapId == item.MapId)
                            {
                                item.IsCheck = item2.IsCheck;
                                item.IsConfirm = item2.IsConfirm;
                            }
                        }
                    }

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region ApplyConference表操作

        public async Task<List<ApplyConference>> GetApplyConferenceByPerContractIdList(SearchInfo search)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ApplyConference
                    .Where(n => (n.PersonContractId == search.id)
                                && (string.IsNullOrEmpty(search.year) || n.Year == search.year)
                                && (n.IsConfirm == true))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public List<string> GetApplyConferenceByPerContractIdListForImpl(string PersonContractId)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    List<string> sc_list = new List<string>();
                    var list = _context.ApplyConference
                                       .Where(n => n.PersonContractId == PersonContractId 
                                                && n.IsConfirm == true)
                                       .ToList();

                    foreach (var item in list)
                    {
                        sc_list.Add(item.SessionConferenceId);
                    }

                    return sc_list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ApplyConference>> GetApplyConferenceByCompanyIdList(string CompanyId)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ApplyConference
                    .Where(n => (n.CompanyId == CompanyId)
                                && (n.IsConfirm == true))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ApplyConference>> GetApplyConferenceBySessionConferenceIdList(string SessionConferenceId)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ApplyConference
                    .Where(n => (n.SessionConferenceId == SessionConferenceId)
                                && (n.IsConfirm == true))
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ApplyConference>> GetApplyConferenceByMemberPkAndYear(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ApplyConference
                                    .Where(n => (n.MemberPK == searchInfo.memberPK)
                                                && (n.Year == searchInfo.year))
                                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ApplyConference>> GetApplyConferenceBySessionConferenceIdListPagination(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ApplyConference
                     .Where(n => (searchInfo.sessionConferenceId.Contains(n.SessionConferenceId))
                                  && (n.IsConfirm == true))
                    .OrderBy(n => n.Id)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetApplyConferenceBySessionConferenceIdListPaginationCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ApplyConference.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                            .Where(n => (searchInfo.sessionConferenceId.Contains(n.SessionConferenceId))
                                        && (n.IsConfirm == true))
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<ApplyConference>> GetApplyConferenceBySessionConferenceIdAndTagTypeCodeList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    List<ApplyConference> applyConferences = new List<ApplyConference>();
                    var list = await _context.ApplyConference
                                                .Where(n => (string.IsNullOrEmpty(searchInfo.sessionConferenceId) || n.SessionConferenceId == searchInfo.sessionConferenceId)
                                                         && (string.IsNullOrEmpty(searchInfo.tagtypeCode) || n.TagTypeCodes.Contains(searchInfo.tagtypeCode))
                                                         && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                                         && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                                         && (n.IsConfirm == true)
                                                      )
                                                .GroupBy(c => c.PersonContractId)
                                                .Skip(((pageindex - 1) * pagesize))
                                                .Take(pagesize)
                                                .ToListAsync();

                    foreach (var item in list)
                    {
                        applyConferences.Add(item.First());
                    }

                    return applyConferences;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetApplyConferenceBySessionConferenceIdAndTagTypeCodeListCount(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {

                    var list = await _context.ApplyConference
                                             .Where(n => (string.IsNullOrEmpty(searchInfo.sessionConferenceId) || n.SessionConferenceId == searchInfo.sessionConferenceId)
                                                         && (string.IsNullOrEmpty(searchInfo.tagtypeCode) || n.TagTypeCodes.Contains(searchInfo.tagtypeCode))
                                                         && (string.IsNullOrEmpty(searchInfo.year) || n.Year == searchInfo.year)
                                                         && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                                         && (n.IsConfirm == true)
                                                      )
                                             .GroupBy(c => c.PersonContractId)
                                             .ToListAsync();


                    return list.Count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateOrUpdateApplyConferenceInfo(string PersonContractId, List<ApplyConference> list)
        {
            var count = 0;
            var str = "新增";
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {

                    //先根据PersonContractId查询出的list，如果count > 0时，先清空，再做增加,count = 0时，直接新增
                    var remove_model_list = _context.ApplyConference.Where(n => n.PersonContractId == PersonContractId).ToList();
                    if (remove_model_list.Count > 0)
                    {
                        _context.ApplyConference.RemoveRange(remove_model_list);
                        str = "修改";
                    }

                    await _context.ApplyConference.AddRangeAsync(list);

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = str + "成功!" + PersonContractId + "/" + list.Count };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region InviteLetter表操作

        public async Task<InviteLetter> GetInviteLetterById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.InviteLetter.FindAsync(gid);

                    return item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyForCreateOtherVM> CreateInviteLetterInfo(InviteLetter model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.InviteLetter.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyForCreateOtherVM { success = true, modifiedcount = count, msg = "创建成功", ext1 = model.Id.ToString(), ext2 = string.Empty };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = ex.Message, ext1 = string.Empty, ext2 = string.Empty };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region TagType表操作

        public async Task<List<TagType>> GetTagTypeDic()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.TagType.ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<TagType> GetTagTypeByCode(string code)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var item = await _context.TagType.FirstOrDefaultAsync(n => n.Code == code);
                    return item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateTagTypeInfo(TagType model)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    await _context.TagType.AddAsync(model);
                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateTagTypeInfo(TagType model)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var modified_model = _context.TagType.Find(model.Id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.NameTranslation = model.NameTranslation;

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功！" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteTagTypeById(string id)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var modle = _context.TagType.Find(gid);

                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    _context.TagType.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region YearConfig表操作

        public async Task<List<YearConfig>> GetYearConfigDic()
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.YearConfig.OrderByDescending(n => n.Year).ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public async Task<List<YearConfig>> GetYearConfigByIsUse(SearchInfo search)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.YearConfig.Where(n => n.IsUse == search.isUse).ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> CreateYearConfigInfo(YearConfig model)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    if (model.IsUse == true)
                    {
                        var item = await _context.YearConfig.FirstOrDefaultAsync(n => n.IsUse == true);
                        if (item != null)
                        {
                            item.IsUse = false;
                        }
                    }

                    await _context.YearConfig.AddAsync(model);
                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateYearConfigInfo(YearConfig model)
        {
            var count = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    if (model.IsUse == true)
                    {
                        var item = await _context.YearConfig.FirstOrDefaultAsync(n => n.IsUse == true);
                        if (item != null)
                        {
                            item.IsUse = false;
                        }
                    }

                    var modified_model = _context.YearConfig.Find(model.Id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.Year = model.Year;
                    modified_model.Date = model.Date;
                    modified_model.IsUse = model.IsUse;

                    count = await _context.SaveChangesAsync();
                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功！" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region ConferenceOnsite表操作

        public async Task<List<ConferenceOnsite>> GetConferenceOnsiteList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceOnsite
                    .Where(n => (string.IsNullOrEmpty(searchInfo.contractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.contractNumber.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.userName) || n.UserName.ToLower().Contains(searchInfo.userName.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.companyName) || n.CompanyName.ToLower().Contains(searchInfo.companyName.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.CompanyServicePackId == searchInfo.companyServicePackId)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                )
                    .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-CreatedOn" : searchInfo.orderings)
                    .Skip(((pageindex - 1) * pagesize))
                    .Take(pagesize)
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetConferenceOnsiteListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.ConferenceOnsite.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                             .Where(n => (string.IsNullOrEmpty(searchInfo.contractNumber) || n.ContractNumber.ToLower().Contains(searchInfo.contractNumber.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.userName) || n.UserName.ToLower().Contains(searchInfo.userName.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.companyName) || n.CompanyName.ToLower().Contains(searchInfo.companyName.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.CompanyServicePackId == searchInfo.companyServicePackId)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.ContractYear == searchInfo.year)
                                )
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ConferenceOnsite> GetConferenceOnsiteById(int id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var item = await _context.ConferenceOnsite
                                             .FirstOrDefaultAsync(n => n.Id == id);

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public ConferenceOnsite GetConferenceOnsiteByIdToUpdate(int id)
        {
            try
            {
                var item = _context.ConferenceOnsite
                                   .FirstOrDefault(n => n.Id == id);

                return item;


            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyForConferenceOnsiteVM> CreateConferenceOnsiteInfo(ConferenceOnsite model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var list = _context.ConferenceOnsite.ToList();
                    model.ContractYear = DateTime.Now.Year.ToString();
                    var number = list.Count > 0 ?
                        (_context.ConferenceOnsite.ToList().OrderByDescending(n => n.Id).FirstOrDefault().Id + 1).ToString().PadLeft(4, '0')
                    : (model.Id + 1).ToString().PadLeft(4, '0');
                    model.ContractNumber = model.ContractYear.Substring(2, 2)
                                                    + "SNEC"
                                                    + (model.Currency.Equals("RMB") ? "C" : "E")
                                                    + number
                                                    + "CW";

                    await _context.ConferenceOnsite.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyForConferenceOnsiteVM { success = true, modifiedcount = count, msg = "创建成功", conferenceOnsite = model };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyForConferenceOnsiteVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateConferenceOnsiteInfo(ConferenceOnsite model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = GetConferenceOnsiteByIdToUpdate(model.Id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.UserName = model.UserName;
                    modified_model.CompanyServicePackName = model.CompanyServicePackName;
                    modified_model.CompanyName = model.CompanyName;
                    modified_model.CompanyServicePackId = model.CompanyServicePackId;
                    modified_model.Currency = model.Currency;
                    modified_model.PayType = model.PayType;
                    modified_model.Credited = model.Credited;
                    modified_model.AddDate = model.AddDate;
                    modified_model.Cost = model.Cost;
                    modified_model.Remark = model.Remark;
                    modified_model.ModefieldOn = model.ModefieldOn;
                    modified_model.ModefieldBy = model.ModefieldBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteConferenceOnsiteById(int id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modle = GetConferenceOnsiteByIdToUpdate(id);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    _context.ConferenceOnsite.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region InviteCode表操作

        public async Task<List<InviteCodeCSPVM>> GetInviteCodeList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var query_list = await fsql.Select<InviteCode, CompanyServicePack>()
                                               .InnerJoin((a, b) => a.CompanyServicePackId == b.CompanyServicePackId.ToString())
                                               .Where((a, b) => b.IsDelete == false)
                                               .ToListAsync((a, b) => new InviteCodeCSPVM { inviteCode = a, companyServicePack = b });

                    query_list = query_list.Where(n => (string.IsNullOrEmpty(searchInfo.inviteCodeNumber) || n.inviteCode.InviteCodeNumber.ToLower().Contains(searchInfo.inviteCodeNumber.ToLower()))
                                                        && (string.IsNullOrEmpty(searchInfo.webSite) || n.inviteCode.WebSite.ToLower().Contains(searchInfo.webSite.ToLower()))
                                                        && (searchInfo.isNotFullUseInviteCode ? n.inviteCode.Count > _context.InviteCodeRecord.Where(m=>m.InviteCodeId == n.inviteCode.Id && m.IsDelete == false).Count() : 0 == 0)
                                                        && (searchInfo.isFullUseInviteCode ? n.inviteCode.Count == _context.InviteCodeRecord.Where(m => m.InviteCodeId == n.inviteCode.Id && m.IsDelete == false).Count() : 0 == 0)
                                                        && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.inviteCode.CompanyServicePackId == searchInfo.companyServicePackId)
                                                        && (string.IsNullOrEmpty(searchInfo.year) || n.inviteCode.Year == searchInfo.year)
                                                        )
                                          .OrderByDescending(n=>n.inviteCode.CreatedOn)
                                          .Skip(((pageindex - 1) * pagesize))
                                          .Take(pagesize)
                                          .ToList();

                    foreach (var item in query_list)
                    {
                        item.InviteRecordCount = _context.InviteCodeRecord.Where(n => n.InviteCodeId == item.inviteCode.Id && n.IsDelete == false).Count();
                    }

                    return query_list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetInviteCodeListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await fsql.Select<InviteCode, CompanyServicePack>()
                                               .InnerJoin((a, b) => a.CompanyServicePackId == b.CompanyServicePackId.ToString())
                                               .Where((a, b) => b.IsDelete == false)
                                               .ToListAsync((a, b) => new InviteCodeCSPVM { inviteCode = a, companyServicePack = b });
                    if (searchInfo != null)
                    {
                        total = list.Where(n => (string.IsNullOrEmpty(searchInfo.inviteCodeNumber) || n.inviteCode.InviteCodeNumber.ToLower().Contains(searchInfo.inviteCodeNumber.ToLower()))
                                                        && (string.IsNullOrEmpty(searchInfo.webSite) || n.inviteCode.WebSite.ToLower().Contains(searchInfo.webSite.ToLower()))
                                                        && (searchInfo.isNotFullUseInviteCode ? n.inviteCode.Count > _context.InviteCodeRecord.Where(m => m.InviteCodeId == n.inviteCode.Id && m.IsDelete == false).Count() : 0 == 0)
                                                        && (searchInfo.isFullUseInviteCode ? n.inviteCode.Count == _context.InviteCodeRecord.Where(m => m.InviteCodeId == n.inviteCode.Id && m.IsDelete == false).Count() : 0 == 0)
                                                        && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.inviteCode.CompanyServicePackId == searchInfo.companyServicePackId)
                                                        && (string.IsNullOrEmpty(searchInfo.year) || n.inviteCode.Year == searchInfo.year)
                                                        )
                                    .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<InviteCodeCSPVM> GetInviteCodeByInviteCodeNumber(SearchInfo search)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var item = await fsql.Select<InviteCode, CompanyServicePack>()
                                         .InnerJoin((a, b) => a.CompanyServicePackId == b.CompanyServicePackId.ToString())
                                         .Where((a, b) => b.IsDelete == false && a.InviteCodeNumber == search.inviteCodeNumber)
                                         .FirstAsync((a, b) => new InviteCodeCSPVM { inviteCode = a, companyServicePack = b });

                    if (item != null)
                    {
                        item.InviteRecordCount = _context.InviteCodeRecord.Where(n => n.InviteCodeId == item.inviteCode.Id && n.IsDelete == false).Count();
                    }

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<InviteCodeCSPVM> GetInviteCodeById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await fsql.Select<InviteCode, CompanyServicePack>()
                                         .InnerJoin((a, b) => a.CompanyServicePackId == b.CompanyServicePackId.ToString())
                                         .Where((a, b) => b.IsDelete == false)
                                         .FirstAsync((a, b) => new InviteCodeCSPVM { inviteCode = a, companyServicePack = b });

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public InviteCode GetInviteCodeByIdToUpdate(string id)
        {
            try
            {
                Guid gid = new Guid(id);
                var item = _context.InviteCode
                                   .FirstOrDefault(n => n.Id == gid);

                return item;


            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateInviteCodeInfo(InviteCode model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.InviteCode.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateInviteCodeInfo(InviteCode model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = GetInviteCodeByIdToUpdate(model.Id.ToString());

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.CompanyServicePackId = model.CompanyServicePackId;
                    modified_model.Count = model.Count;
                    modified_model.Year = model.Year;
                    modified_model.WebSite = model.WebSite;
                    modified_model.ModefieldOn = model.ModefieldOn;
                    modified_model.ModefieldBy = model.ModefieldBy;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteInviteCodeById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modle = GetInviteCodeByIdToUpdate(id);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    var record_list = _context.InviteCodeRecord.Where(n => n.InviteCodeId.ToString() == id).ToList();

                    _context.InviteCodeRecord.RemoveRange(record_list);

                    _context.InviteCode.Remove(modle);

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region InviteCodeRecord表操作

        public async Task<List<InviteCodeRecord>> GetInviteCodeRecordList(int pageindex, int pagesize, SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.InviteCodeRecord
                                             .Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber) || n.PersonContractNumber.ToLower().Contains(searchInfo.perContractNumber.ToLower()))
                                                      && (string.IsNullOrEmpty(searchInfo.memberPK) || n.MemberPK == searchInfo.memberPK)
                                                      && (string.IsNullOrEmpty(searchInfo.inviteCodeId) || n.InviteCodeId.ToString() == searchInfo.inviteCodeId)
                                                      && (
                                                           (string.IsNullOrEmpty(searchInfo.memTranslation))
                                                            ||
                                                           (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemberName).MemberCN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                                            ||
                                                           (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemberName).MemberEN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                                          )
                                                       && n.IsDelete == searchInfo.IsDelete
                                                   )
                                             .OrderByBatch(string.IsNullOrEmpty(searchInfo.orderings) ? "-UseDate" : searchInfo.orderings)
                                             .Skip(((pageindex - 1) * pagesize))
                                             .Take(pagesize)
                                             .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<int> GetInviteCodeRecordListCount(SearchInfo searchInfo)
        {
            var total = 0;
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.InviteCodeRecord.ToListAsync();
                    if (searchInfo != null)
                    {
                        total = list
                                .Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber) || n.PersonContractNumber.ToLower().Contains(searchInfo.perContractNumber.ToLower()))
                                                        && (string.IsNullOrEmpty(searchInfo.memberPK) || n.MemberPK == searchInfo.memberPK)
                                                        && (string.IsNullOrEmpty(searchInfo.inviteCodeId) || n.InviteCodeId.ToString() == searchInfo.inviteCodeId)
                                                        && (
                                                            (string.IsNullOrEmpty(searchInfo.memTranslation))
                                                            ||
                                                            (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemberName).MemberCN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                                            ||
                                                            (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemberName).MemberEN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                                           )
                                                        && n.IsDelete == searchInfo.IsDelete
                                        )
                            .Count();
                    }
                    else
                    {
                        total = list.Count();
                    }

                    return total;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<InviteCodeRecord> GetInviteCodeRecordById(string id)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    Guid gid = new Guid(id);
                    var item = await _context.InviteCodeRecord
                                       .FirstOrDefaultAsync(n => n.Id == gid);

                    return item;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public InviteCodeRecord GetInviteCodeRecordByIdToUpdate(string id)
        {
            try
            {
                Guid gid = new Guid(id);
                var item = _context.InviteCodeRecord
                                   .FirstOrDefault(n => n.Id == gid);

                return item;


            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<ModifyReplyVM> CreateInviteCodeRecordInfo(InviteCodeRecord model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    await _context.InviteCodeRecord.AddAsync(model);
                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "创建成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> UpdateInviteCodeRecordInfo(InviteCodeRecord model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = GetInviteCodeRecordByIdToUpdate(model.Id.ToString());

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.MemberPK = model.MemberPK;
                    modified_model.MemberName = model.MemberName;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyVM> DeleteInviteCodeRecordById(string id)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modle = GetInviteCodeRecordByIdToUpdate(id);
                    if (modle == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "数据库中没有此id的实例可以删除！" };
                    }

                    modle.IsDelete = true;

                    count = await _context.SaveChangesAsync();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "删除成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region Grpc服务调用方法

        private GrpcMembersService.ModifyReply CreateMemberContract(MemberContractStruct model)
        {
            try
            {
                var result = new GrpcMembersService.ModifyReply();
                MemberContractRequest memberContractRequest = new MemberContractRequest
                {
                    MemberContract = model.MemContract
                };

                var memberContract = MemberServiceClient.GetMemContractByMemContract(memberContractRequest);
                if (!string.IsNullOrEmpty(memberContract.MCPk))
                {
                    return new GrpcMembersService.ModifyReply { Success = false, ModifiedCount = 0, Msg = "在Member服务内MemberContract表内已存在当前合同号为" + memberContractRequest.MemberContract + "的数据，不允许再添加" };
                }

                if (string.IsNullOrEmpty(model.MemeberPK))
                {
                    //默认人员值
                    model.MemeberPK = "d7dc7259-aa32-44fb-8fdc-6134368e10cc";
                    //return new GrpcMembersService.ModifyReply { Success = true, ModifiedCount = 0, Msg = "不需要往Member服务MemberContract表内添加数据！" };
                }

                result = MemberServiceClient.CreateMemberContractInfo(model);

                result.Msg = "插入数据到Member服务内MemberContract表失败，请联系管理人员！";

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        private GrpcMembersService.ModifyReply DeleteMemContractByMemContract(MemberContractRequest model)
        {
            try
            {
                var result = MemberServiceClient.DeleteMemContractByMemContract(model);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        private MemberStruct GetMemberById(string id)
        {
            try
            {
                var result = MemberServiceClient.GetMemberById(id);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        private CompanyStruct GetCompanyById(string id)
        {
            try
            {
                var result = MemberServiceClient.GetCompanyById(id);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        public async Task<CreateInfoVM> CreateParticipantInfo(PersonContract model)
        {
            var result = new CreateInfoVM();
            try
            {
                var CompanyContract = await _context.CompanyContract.Include(n => n.companyServicePack).ThenInclude(n => n.contractType).FirstOrDefaultAsync(n => n.ContractId == model.ContractId);
                var IsSpeaker = CompanyContract.companyServicePack.IsSpeaker;
                if (IsSpeaker)
                {
                    var personContract = await _context.PersonContract.FindAsync(model.PersonContractId);
                    var memberInfo = GetMemberById(personContract.MemberPK.ToString());
                    var companyInfo = GetCompanyById(CompanyContract.CompanyId.ToString());
                    var year = _context.YearConfig.FirstOrDefault(n => n.IsUse == true).Year;

                    result = CreateParticipantToGrpc(model, memberInfo, companyInfo, year);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

            return result;
        }

        private CreateInfoVM CreateParticipantToGrpc(PersonContract personContract, MemberStruct member, CompanyStruct company, string year)
        {
            try
            {
                ParticipantStruct participantStruct = new ParticipantStruct
                {
                    MemberPK = personContract.MemberPK.ToString(),
                    ParticipantNameTranslation = string.Format("{{ \"CN\": \"{0}\", \"EN\": \"{1}\", \"JP\": \"{2}\" }}", member.MemNameCn, member.MemNameEn, string.Empty),
                    CompanyId = company.CompanyPK,
                    CompanyTranslation = string.Format("{{ \"CN\": \"{0}\", \"EN\": \"{1}\", \"JP\": \"{2}\" }}", company.ComNameCn, company.ComNameEn, string.Empty),
                    CountryTranslation = string.Format("{{ \"CN\": \"{0}\", \"EN\": \"{1}\", \"JP\": \"{2}\" }}", member.MemCountryNameCn, member.MemCountryNameEn, string.Empty),
                    JobTranslation = string.Format("{{ \"CN\": \"{0}\", \"EN\": \"{1}\", \"JP\": \"{2}\" }}", member.MemPosition, string.Empty, string.Empty),
                    Email = member.MemEmail,
                    Mobile = member.MemMobile,
                    PersonContractID = personContract.PersonContractId.ToString(),
                    PerContractNumber = personContract.PerContractNumber,
                    Owerid = personContract.Owerid,
                    Ower = personContract.Ower,
                    AppellationTranslation = member.MemTitle,
                    ShowOnFont = 0,
                    Year = year
                };

                var result = ConferenceServiceClient.CreateParticipant(participantStruct);

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        #endregion

        #region 其他接口

        public async Task<List<ContractStatistics>> GetContractStatisticsList(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    //var ss = IsMaxContractNumberEqualsPCCountByCompanyPKAndYear(searchInfo);
                    var list = await _context.CompanyContract.Include(n => n.conferenceContract).Include(n => n.companyServicePack).Include(n => n.personContract).ToListAsync();
                    var cs_list = list.Where(n => n.CCIsdelete == false
                                                && n.companyServicePack.IsGive == searchInfo.isGive
                                                && n.companyServicePack.IsDelete == false
                                                && n.conferenceContract.ContractYear == searchInfo.year
                                                && n.conferenceContract.IsDelete == false)
                                 .GroupBy(c => c.CompanyServicePackId)
                                 .Select(g => new ContractStatistics
                                 {
                                     CompanyServicePackId = g.Key.ToString(),
                                     CompanyServicePackName = _context.CompanyServicePack.Find(g.Key).Translation,
                                     PersonCount = g.Sum(x => x.personContract.Where(n => n.PCIsdelete == false).Count()).ToString(),
                                     MaxContractNumberSum = g.Sum(x => x.MaxContractNumberSatUse).ToString()
                                 }
                                 )
                                 .ToList();

                    return cs_list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<BoolReplyVM> IsMaxContractNumberEqualsPCCountByCompanyPKAndYear(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var result = false;
                    var list = await _context.CompanyContract.Include(n => n.conferenceContract).Include(n => n.personContract).ToListAsync();

                    var cs_list2 = list.Where(n => n.CCIsdelete == false
                                       && n.CompanyId == new Guid(searchInfo.companyId)
                                       && n.MaxContractNumber == -1
                                       && n.conferenceContract.ContractYear == searchInfo.year
                                       && n.conferenceContract.IsDelete == false)
                                       .ToList();

                    if (cs_list2.Count > 0)
                    {
                        result = true;
                    }


                    var cs_list = list.Where(n => n.CCIsdelete == false
                                                && n.CompanyId == new Guid(searchInfo.companyId)
                                                && n.MaxContractNumber > 0
                                                && n.conferenceContract.ContractYear == searchInfo.year
                                                && n.conferenceContract.IsDelete == false)
                                      .GroupBy(c => c.CompanyId)
                                      .Select(g => new ContractStatistics
                                      {
                                          PersonCount = g.Sum(x => x.personContract.Where(n => n.PCIsdelete == false).Count()).ToString(),
                                          MaxContractNumberSum = g.Sum(x => x.MaxContractNumber).ToString()
                                      }
                                      )
                                      .ToList();



                    if (cs_list.Count > 0)
                    {
                        var pccount = Convert.ToInt32(cs_list[0].PersonCount);
                        var maxContractNumberSum = Convert.ToInt32(cs_list[0].MaxContractNumberSum);

                        result = maxContractNumberSum == pccount;
                    }


                    return new BoolReplyVM { result = result };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

        }

        public async Task<List<PersonContractAndSessionConferenceIdListVM>> GetPersonContractListForLunch(int pageindex, int pagesize, SearchInfo searchInfo)
        {

            try
            {
                List<PersonContractAndSessionConferenceIdListVM> return_list = new List<PersonContractAndSessionConferenceIdListVM>();
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var query_list = await fsql.Select<PersonContract, ApplyConference, CompanyContract>()
                                               .InnerJoin((a, b, c) => a.PersonContractId.ToString() == b.PersonContractId)
                                               .InnerJoin((a, b, c) => a.ContractId == c.ContractId)
                                               .Where((a, b, c) => a.PCIsdelete == false
                                                             && (string.IsNullOrEmpty(searchInfo.year) || b.Year == searchInfo.year)
                                                             && (searchInfo.sessionConferenceId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(b.SessionConferenceId))
                                                             )
                                               .ToListAsync((a, b, c) => new PersonContractAndApplyConferenceVM { PersonContract = a, SessionConferenceId = b.SessionConferenceId, CompanyContract = c });


                    if (query_list.Count > 0)
                    {
                        var pids_list = query_list.GroupBy(n => n.PersonContract.PersonContractId).ToList();

                        foreach (var item in pids_list)
                        {
                            PersonContractAndSessionConferenceIdListVM model = new PersonContractAndSessionConferenceIdListVM();
                            model.CompanyContract = item.Select(n => n.CompanyContract).FirstOrDefault();
                            model.PersonContract = item.Select(n => n.PersonContract).FirstOrDefault();
                            model.SessionConferenceIds = item.Select(n => n.SessionConferenceId).ToList();
                            var compackname = _context.CompanyServicePack.FirstOrDefault(n => n.CompanyServicePackId == model.CompanyContract.CompanyServicePackId).Translation;
                            model.CompanyServicePackName = string.IsNullOrEmpty(compackname) ? "" : JsonConvert.DeserializeObject<TranslationVM>(compackname).CN;
                            return_list.Add(model);
                        }


                        return_list = return_list.Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber.ToLower()) || n.PersonContract.PerContractNumber.ToLower() == searchInfo.perContractNumber.ToLower())
                                && (
                                    (string.IsNullOrEmpty(searchInfo.memTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.PersonContract.MemTranslation).MemberCN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.PersonContract.MemTranslation).MemberEN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                    )
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.PersonContract.Owerid == searchInfo.owerid)
                                && (
                                    (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.CompanyContract.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.CompanyContract.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    )
                                && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.CompanyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId))
                                && (searchInfo.IsCheckIn ? n.PersonContract.IsCheckIn == true : 0 == 0)
                        )
                        .Skip(((pageindex - 1) * pagesize))
                        .Take(pagesize)
                        .ToList();

                    }

                    return return_list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public async Task<int> GetPersonContractListForLunchCount(SearchInfo searchInfo)
        {

            try
            {
                var count = 0;
                List<PersonContractAndSessionConferenceIdListVM> return_list = new List<PersonContractAndSessionConferenceIdListVM>();
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var query_list = await fsql.Select<PersonContract, ApplyConference, CompanyContract>()
                                               .InnerJoin((a, b, c) => a.PersonContractId.ToString() == b.PersonContractId)
                                               .InnerJoin((a, b, c) => a.ContractId == c.ContractId)
                                               .Where((a, b, c) => a.PCIsdelete == false
                                                             && (searchInfo.sessionConferenceId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(b.SessionConferenceId))
                                                             )
                                               .ToListAsync((a, b, c) => new PersonContractAndApplyConferenceVM { PersonContract = a, SessionConferenceId = b.SessionConferenceId, CompanyContract = c });


                    if (query_list.Count > 0)
                    {
                        var pids_list = query_list.GroupBy(n => n.PersonContract.PersonContractId).ToList();

                        foreach (var item in pids_list)
                        {
                            PersonContractAndSessionConferenceIdListVM model = new PersonContractAndSessionConferenceIdListVM();
                            model.CompanyContract = item.Select(n => n.CompanyContract).FirstOrDefault();
                            model.PersonContract = item.Select(n => n.PersonContract).FirstOrDefault();
                            model.SessionConferenceIds = item.Select(n => n.SessionConferenceId).ToList();
                            return_list.Add(model);
                        }


                        return_list = return_list.Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber.ToLower()) || n.PersonContract.PerContractNumber.ToLower() == searchInfo.perContractNumber.ToLower())
                                && (
                                    (string.IsNullOrEmpty(searchInfo.memTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.PersonContract.MemTranslation).MemberCN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                    ||
                                    (JsonConvert.DeserializeObject<MemTranslationVM>(n.PersonContract.MemTranslation).MemberEN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                    )
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.PersonContract.Owerid == searchInfo.owerid)
                                && (
                                    (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.CompanyContract.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                    (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.CompanyContract.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    )
                                && (string.IsNullOrEmpty(searchInfo.companyServicePackId) || n.CompanyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId))
                                && (searchInfo.IsCheckIn ? n.PersonContract.IsCheckIn == true : 0 == 0)
                        )
                        .ToList();

                        count = return_list.Count;
                    }

                    return count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public async Task<List<PersonContract>> GetPersonContractListAndApplyConference(int pageindex, int pagesize, SearchInfo searchInfo)
        {

            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    List<PersonContract> pc_list = new List<PersonContract>();
                    var query_list = await fsql.Select<PersonContract, ApplyConference>()
                                               .InnerJoin((a, b) => a.PersonContractId.ToString() == b.PersonContractId)
                                               .Where((a, b) => (a.PCIsdelete == false)
                                                                && (string.IsNullOrEmpty(searchInfo.sessionConferenceId) || b.SessionConferenceId == searchInfo.sessionConferenceId)
                                                                && (string.IsNullOrEmpty(searchInfo.tagtypeCode) || b.TagTypeCodes.Contains(searchInfo.tagtypeCode))
                                                                && (string.IsNullOrEmpty(searchInfo.year) || b.Year == searchInfo.year)
                                                             )
                                               .ToListAsync();

                    var pcid_list = query_list.GroupBy(n => n.PersonContractId).Select(n => n.Key).ToList();

                    foreach (var str in pcid_list)
                    {
                        var model = _context.PersonContract.Find(str);
                        pc_list.Add(model);
                    }

                    pc_list = pc_list.Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList();

                    return pc_list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public async Task<int> GetPersonContractListAndApplyConferenceCount(SearchInfo searchInfo)
        {

            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    List<PersonContract> pc_list = new List<PersonContract>();
                    var query_list = await fsql.Select<PersonContract, ApplyConference>()
                                               .InnerJoin((a, b) => a.PersonContractId.ToString() == b.PersonContractId)
                                               .Where((a, b) => (a.PCIsdelete == false)
                                                                && (string.IsNullOrEmpty(searchInfo.sessionConferenceId) || b.SessionConferenceId == searchInfo.sessionConferenceId)
                                                                && (string.IsNullOrEmpty(searchInfo.tagtypeCode) || b.TagTypeCodes.Contains(searchInfo.tagtypeCode))
                                                                && (string.IsNullOrEmpty(searchInfo.year) || b.Year == searchInfo.year)
                                                             )
                                               .ToListAsync();

                    var pcid_list = query_list.GroupBy(n => n.PersonContractId).Select(n => n.Key).ToList();

                    foreach (var str in pcid_list)
                    {
                        var model = _context.PersonContract.Find(str);
                        pc_list.Add(model);
                    }

                    return pc_list.Count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public async Task<ModifyReplyForCreateOtherVM> CreatePersonContractActivityMapImport()
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var query_list = _context.PersonContract.Where(n => n.PCIsdelete == false).ToList();

                    foreach (var model in query_list)
                    {
                        var servicePackActivityMaps = GetServicePackActivityMapList(model.ContractId);
                        if (servicePackActivityMaps.Count < 1)
                        {
                            return new ModifyReplyForCreateOtherVM { success = false, modifiedcount = count, msg = "创建失败/" + model.ContractId, ext1 = string.Empty, ext2 = string.Empty };
                        }
                        //把数据拼齐并插入PersonContractActivityMap表内
                        List<PersonContractActivityMap> plist = new List<PersonContractActivityMap>();
                        foreach (var item in servicePackActivityMaps)
                        {
                            PersonContractActivityMap pmodel = new PersonContractActivityMap
                            {
                                MapId = Guid.NewGuid(),
                                PersonContractId = model.PersonContractId.ToString(),
                                MemberPK = model.MemberPK.ToString(),
                                ActivityId = item.ActivityId.ToString(),
                                ActivityName = item.ActivityName,
                                SessionConferenceID = item.SessionConferenceID.ToString(),
                                SessionConferenceName = item.SessionConferenceName,
                                IsConfirm = false,
                                IsCheck = false,
                                Year = GetConferenceContractById(_context.CompanyContract.FirstOrDefault(n => n.ContractId == model.ContractId).ConferenceContractId.ToString()).Result.ContractYear
                            };
                            plist.Add(pmodel);
                        }
                        await _context.PersonContractActivityMap.AddRangeAsync(plist);
                    }

                    count = await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }

            return new ModifyReplyForCreateOtherVM { success = true, modifiedcount = count, msg = "创建成功", ext1 = string.Empty, ext2 = string.Empty };
        }

        public async Task<List<PersonContract>> ExportPersonContractList(SearchInfo searchInfo)
        {
            try
            {
                using (var _context = new ConCDBContext(_options.Options))
                {
                    var list = await _context.PersonContract.Include(n => n.companyContract)
                    .Where(n => (string.IsNullOrEmpty(searchInfo.perContractNumber) || n.PerContractNumber.ToLower().Contains(searchInfo.perContractNumber.ToLower()))
                                &&
                                (
                                (string.IsNullOrEmpty(searchInfo.memTranslation))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                ||
                                (JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberEN.ToLower().Contains(searchInfo.memTranslation.ToLower()))
                                )
                                &&
                                (
                                (string.IsNullOrEmpty(searchInfo.comNameTranslation))
                                ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyCN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                    ||
                                (JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyEN.ToLower().Contains(searchInfo.comNameTranslation.ToLower()))
                                )
                                &&
                                (string.IsNullOrEmpty(searchInfo.companyServicePackId) ||
                                (searchInfo.companyServicePackId.Split(new char[] { ',' }).Length > 0 ?
                                searchInfo.companyServicePackId.Contains(n.companyContract.CompanyServicePackId.ToString())
                                :
                                n.companyContract.CompanyServicePackId == new Guid(searchInfo.companyServicePackId)))
                                && (string.IsNullOrEmpty(searchInfo.owerid) || n.Owerid == searchInfo.owerid)
                                && (string.IsNullOrEmpty(searchInfo.cTypeCode) ||
                                    (searchInfo.cTypeCode.Split(new char[] { ',' }).Length > 0 ?
                                    searchInfo.cTypeCode.Contains(n.CTypeCode)
                                    :
                                    n.CTypeCode == searchInfo.cTypeCode))
                                && (string.IsNullOrEmpty(searchInfo.conferenceId) || n.ConferenceId == searchInfo.conferenceId)
                                && (string.IsNullOrEmpty(searchInfo.memberPK) || n.MemberPK == new Guid(searchInfo.memberPK))
                                && (string.IsNullOrEmpty(searchInfo.exTypeCode.ToLower()) || n.PerContractNumber.ToLower().Contains(searchInfo.exTypeCode.ToLower()))
                                && (string.IsNullOrEmpty(searchInfo.contractCode.ToLower()) || n.PerContractNumber.ToLower().Contains(searchInfo.contractCode.ToLower()))
                                && (n.PCIsdelete == false)
                                && (string.IsNullOrEmpty(searchInfo.year) || n.companyContract.conferenceContract.ContractYear == searchInfo.year)
                                && (string.IsNullOrEmpty(searchInfo.companyId) || n.companyContract.CompanyId == new Guid(searchInfo.companyId))
                                && (n.IsPrint == false)
                                )
                    .OrderBy(n => CommonHelper.GetFirstPinyin(JsonConvert.DeserializeObject<MemTranslationVM>(n.MemTranslation).MemberCN))
                    .OrderBy(n => JsonConvert.DeserializeObject<ComNameTranslationVM>(n.companyContract.ComNameTranslation).CompanyCN)                  
                    .ToListAsync();

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        #endregion

        #region 消费队列使用方法
        public ModifyReplyVM ModifyPersonContractIsCommitAbstractByPCNumber(PCIsCommitAbstractVm model)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    var modified_model = _context.PersonContract.FirstOrDefault(n => n.PerContractNumber == model.PerContractNumber);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "PerContractNumber = " + model.PerContractNumber + ",当前实例不存在" };
                    }

                    modified_model.IsCommitAbstract = model.IsCommitAbstract;


                    count = _context.SaveChanges();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }

        public ModifyReplyVM ModifyPersonContractPaidAmountByPCNumber(List<PCPaidAmountVm> list)
        {
            var count = 0;
            try
            {
                using (_context = new ConCDBContext(_options.Options))
                {
                    foreach (var model in list)
                    {
                        var modified_model = _context.PersonContract.FirstOrDefault(n => n.PerContractNumber == model.PerContractNumber);

                        if (modified_model == null)
                        {
                            LogHelper.Info(this, "PerContractNumber = " + model.PerContractNumber + ",当前实例不存在");
                            return new ModifyReplyVM { success = false, modifiedcount = count, msg = "PerContractNumber = " + model.PerContractNumber + ",当前实例不存在" };
                        }

                        modified_model.PaidAmount = model.PaidAmount;
                    }

                    count = _context.SaveChanges();

                    return new ModifyReplyVM { success = true, modifiedcount = count, msg = "修改成功" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                return new ModifyReplyVM { success = false, modifiedcount = count, msg = ex.Message };
                throw new Exception("异常", ex);
            }
        }
        #endregion
    }
}
