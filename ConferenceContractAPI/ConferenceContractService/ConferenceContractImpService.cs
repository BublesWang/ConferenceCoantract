using AutoMapper;
using ConferenceContractAPI.CCDBContext;
using ConferenceContractAPI.Common;
using ConferenceContractAPI.Config;
using ConferenceContractAPI.DBModels;
using ConferenceContractAPI.RabbitServices;
using ConferenceContractAPI.ViewModel;
using Extensions.Repository;
using Grpc.Core;
using GrpcConferenceContractServiceNew;
using MemberAPI.DBModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static GrpcConferenceContractServiceNew.ConferenceContractServiceNew;

namespace ConferenceContractAPI.ConferenceContractService
{
    public partial class ConferenceContractImpService : ConferenceContractServiceNewBase
    {
        private string _sql = ContextConnect.ReadConnstrContent();
        private DbContextOptionsBuilder<ConCDBContext> _options;

        private DbContextOptions<MemberDBContext> _member_options;
        private DbContextOptions<RoleDBContext> _role_options;
        public ConferenceContractImpService()
        {
            try
            {
                _options = new DbContextOptionsBuilder<ConCDBContext>();
                _options.UseNpgsql(_sql);

                _member_options = DatabaseConfig.GetMemberDbOptions();
                _role_options = DatabaseConfig.GetRoleDbOptions();
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        /// <summary>
        /// 根据年份，大会id，上级分类 id，是否赠送得到套餐列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<newGetServicePackResponse> newGetServicePack(newGetServicePackRequest request, ServerCallContext context)
        {
            try
            {

                newGetServicePackResponse response = new newGetServicePackResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var companyServicePackQueryanble = dbContext.CompanyServicePack as IQueryable<CompanyServicePack>;
                    companyServicePackQueryanble = GetServicePackIquerable(request, companyServicePackQueryanble);

                    var list = companyServicePackQueryanble.AsNoTracking().OrderBy(x => x.Sort)
                        .Select(x => new newGetServicePackStruct
                        {
                            Code = x.Code ?? string.Empty,
                            CompanyServicePackId = x.CompanyServicePackId.ToString(),
                            IsGive = x.IsGive.ObjToBool(),
                            PriceJP = x.PriceJP ?? "0",
                            PriceRMB = x.PriceRMB ?? "0",
                            PriceUSD = x.PriceUSD ?? "0",
                            Translation = x.Translation ?? string.Empty,
                            Year = x.Year ?? string.Empty,
                            ContractTypeId = x.ContractTypeId.ToString(),
                            CTypeCode = x.CTypeCode ?? string.Empty,
                            Sort = x.Sort ?? 0,
                            ConferenceId = x.ConferenceId ?? string.Empty,
                            ConferenceName = x.ConferenceName ?? string.Empty,
                            IsShownOnFront = x.IsShownOnFront ?? false,
                            RemarkTranslation = x.RemarkTranslation ?? string.Empty,
                            RemarkCode = x.RemarkCode ?? string.Empty,
                            IsSpeaker = x.IsSpeaker,
                            IsDelete = x.IsDelete ?? false,
                            CreatedOn = x.CreatedOn.ToString() ?? string.Empty,
                            CreatedBy = x.CreatedBy ?? string.Empty,
                            ModefieldOn = x.ModefieldOn.ToString() ?? string.Empty,
                            ModefieldBy = x.ModefieldBy ?? string.Empty
                        }).ToList();
                    response.Listdata.AddRange(list);
                    return Task.FromResult(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        public IQueryable<CompanyServicePack> GetServicePackIquerable(newGetServicePackRequest request, IQueryable<CompanyServicePack> companyServiceQueryable)
        {
            if (string.IsNullOrEmpty(request?.Year))
            {
                throw new ArgumentNullException($"{nameof(request.Year)}不能为空");
            }
            if (string.IsNullOrEmpty(request?.ConferenceId))
            {
                throw new ArgumentNullException($"{nameof(request.ConferenceId)}不能为空");
            }
            var conferenceId = request?.ConferenceId;
            var contractTypeId = request?.ContractTypeId;
            var year = request.Year;
            var isGive = request?.IsGive;
            #region 拼接条件
            if (!string.IsNullOrEmpty(conferenceId))
            {
                companyServiceQueryable = companyServiceQueryable.Where(x => x.ConferenceId == conferenceId);
            }
            if (!string.IsNullOrEmpty(contractTypeId))
            {
                companyServiceQueryable = companyServiceQueryable.Where(x => x.ContractTypeId.ObjToString() == contractTypeId);
            }
            if (!string.IsNullOrEmpty(year))
            {
                companyServiceQueryable = companyServiceQueryable.Where(x => x.Year == year);
            }
            if (!string.IsNullOrEmpty(isGive))
            {
                companyServiceQueryable = companyServiceQueryable.Where(x => x.IsGive == false);
            }
            #endregion
            return companyServiceQueryable;
        }


        /// <summary>
        /// 判断两个参数是否为相互全部包含
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsSubSet(string[] a, string[] b)
        {
            bool rt = true;
            for (int i = 0; i < b.Length; i++)//循环待测bai试的字符长度
            {
                for (int j = 0; j < a.Length; j++)//被测试的字符串
                {
                    if (b[i].Equals(a[j]))
                    {
                        rt = true;
                        break;
                    }
                    else
                    {
                        rt = false;
                    }
                }
            }
            return rt;
        }

        public bool IsEqual(string OtherOwnerId1, string OtherOwnerId2)
        {

            string[] a = OtherOwnerId1.Split(",");
            string[] b = OtherOwnerId2.Split(",");
            bool i = IsSubSet(a, b);

            bool j = IsSubSet(b, a);
            if (i == true && j == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CompanyContract GetCompanyContractByIdToUpdate(string id)
        {
            try
            {
                Guid gid = new Guid(id);
                var dbContext = new ConCDBContext(_options.Options);
                var item = dbContext.CompanyContract
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


        public ModifyReplyVM ModifyMaxContractNumberSatUse(CompanyContract model)
        {
            var count = 0;
            try
            {
                string id = model.ContractId.ToString();
                var dbContext = new ConCDBContext(_options.Options);
                var modified_model = GetCompanyContractByIdToUpdate(id);

                if (modified_model == null)
                {
                    return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                }

                modified_model.MaxContractNumberSatUse = modified_model.MaxContractNumber == -1 ?
                    modified_model.personContract.Where(n => n.PCIsdelete == false).Count()
                    :
                    model.MaxContractNumber;

                count = dbContext.SaveChanges();

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
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    string id = model.ContractId.ToString();

                    var modified_model = GetCompanyContractByIdToUpdate(id);

                    if (modified_model == null)
                    {
                        return new ModifyReplyVM { success = false, modifiedcount = count, msg = "当前实例不存在" };
                    }

                    modified_model.MaxContractNumber = model.MaxContractNumber;

                    count = await dbContext.SaveChangesAsync();

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
        /// 当存在付费、赠送二级合同时，需要进行合并（最大子合同数进行合并）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<modifyResponse> newMergeCompanyContract(newMergeCompanyContractRequest request, ServerCallContext context)
        {
            try
            {
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    modifyResponse response = new modifyResponse();
                    var db_CompanyContract = dbContext.CompanyContract;
                    var originalConferenceContract = db_CompanyContract.FirstOrDefault(x => x.ContractId.ToString() == request.CompanyContractId); ;
                    dbContext.Set<CompanyContract>().Attach(originalConferenceContract);
                    if (request.MaxContractNumber != -1 && originalConferenceContract.MaxContractNumber != -1)
                    {
                        originalConferenceContract.MaxContractNumber = originalConferenceContract.MaxContractNumber + request.MaxContractNumber;
                        originalConferenceContract.MaxContractNumberSatUse = originalConferenceContract.MaxContractNumber == -1 ? 0 : originalConferenceContract.MaxContractNumber;
                        dbContext.Entry(originalConferenceContract).State = EntityState.Modified;
                        var conferenceContract = dbContext.ConferenceContract.Where(x => x.ConferenceContractId == originalConferenceContract.ConferenceContractId).FirstOrDefault();
                        var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(originalConferenceContract.ComPrice) * request.MaxContractNumber).ToString();
                        conferenceContract.TotalPrice = totalPrice;
                        if (conferenceContract.IsOpPayStatudCode == false)
                        {
                            conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                        }
                        dbContext.Entry(conferenceContract).State = EntityState.Modified;
                        var result = await dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            response.Msg = "合并成功";
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "合并失败";
                        }
                    }

                    if (request.MaxContractNumber == -1 || originalConferenceContract.MaxContractNumber == -1)
                    {
                        originalConferenceContract.MaxContractNumber = -1;
                        dbContext.Entry(originalConferenceContract).State = EntityState.Modified;
                        var result = await dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            response.Msg = "合并成功";
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "合并失败";
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }
        /// <summary>
        /// 当存在相应的二级折扣合同时，进行数据合并（最大子合同数）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<modifyResponse> newMergeCompanyContractDiscount(newMergeCompanyContractRequest request, ServerCallContext context)
        {
            try
            {
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    modifyResponse response = new modifyResponse();
                    var db_CompanyContract = dbContext.CompanyContract;
                    var originalConferenceContract = db_CompanyContract.FirstOrDefault(x => x.ContractId.ToString() == request.CompanyContractId); ;
                    dbContext.Set<CompanyContract>().Attach(originalConferenceContract);
                    if (request.MaxContractNumber != -1 && originalConferenceContract.MaxContractNumber != -1)
                    {
                        originalConferenceContract.MaxContractNumber = originalConferenceContract.MaxContractNumber + request.MaxContractNumber;
                        originalConferenceContract.MaxContractNumberSatUse = originalConferenceContract.MaxContractNumber == -1 ? 0 : originalConferenceContract.MaxContractNumber;
                        var totalPrice = (double.Parse(originalConferenceContract.ComPrice) + double.Parse(originalConferenceContract.ComPrice) * request.MaxContractNumber).ToString();
                        var conferenceContract = dbContext.ConferenceContract.Where(x => x.ConferenceContractId == originalConferenceContract.ConferenceContractId).FirstOrDefault();
                        conferenceContract.TotalPrice = totalPrice;
                        if (conferenceContract.IsOpPayStatudCode == false)
                        {
                            conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                            dbContext.Entry(conferenceContract).State = EntityState.Modified;
                        }
                        dbContext.Entry(originalConferenceContract).State = EntityState.Modified;
                        var result = await dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            response.Msg = "合并成功";
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "合并失败";
                        }
                    }

                    if (request.MaxContractNumber == -1 || originalConferenceContract.MaxContractNumber == -1)
                    {
                        originalConferenceContract.MaxContractNumber = -1;
                        dbContext.Entry(originalConferenceContract).State = EntityState.Modified;
                        var result = await dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            response.Msg = "合并成功";
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "合并失败";
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }
        /// <summary>
        /// 添加二级合同（付费、赠送）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<modifyResponseNew> newAddCompanyContract(newServicePackStruct request, ServerCallContext context)
        {
            try
            {
                Trans t = new Trans();
                request.ComNameTranslation = JsonConvert.SerializeObject(t);
                request.AddressTranslation = JsonConvert.SerializeObject(t);

                request = InitCompanyContract(request);
                var comContract = Mapper.Map<CompanyContract>(request);

                modifyResponseNew response = new modifyResponseNew();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;

                    var originalConferenceContract = db_CompanyContract.Where(x =>
                                                             x.CompanyId == comContract.CompanyId
                                                          && x.ConferenceId == comContract.ConferenceId
                                                          //&& x.CompanyServicePackId == comContract.CompanyServicePackId
                                                          && x.CompanyServicePackCode == comContract.CompanyServicePackCode
                                                          && x.Owerid == comContract.Owerid
                                                          && x.ContractStatusCode == comContract.ContractStatusCode
                                                          && x.ComPrice == comContract.ComPrice
                                                          && x.Year == comContract.Year
                                                          && IsEqual(x.OtherOwnerId, comContract.OtherOwnerId)).ToList();
                    //已经有了该二级合同
                    if (originalConferenceContract != null && originalConferenceContract.Count > 0)
                    {
                        int companyContractCount = originalConferenceContract.Count();
                        ////已经有了该合同，只需要更新子合同数（原数量加上新增的数量）
                        //originalConferenceContract.First().MaxContractNumber = originalConferenceContract.First().MaxContractNumber + comContract.MaxContractNumber;
                        //dbContext.CompanyContract.Update(originalConferenceContract.First());
                        //var result = await dbContext.SaveChangesAsync();
                        //if (result > 0)
                        //{
                        //response.IsSuccess = true;
                        if (companyContractCount > 0)
                        {
                            response.Msg = "存在该二级合同";
                            response.Total = companyContractCount;
                            if (companyContractCount == 1)
                            {
                                response.CompanyContractId = originalConferenceContract.FirstOrDefault().ContractId.ToString();
                            }

                        }
                        //}
                        //else
                        //{
                        //response.IsSuccess = false;
                        //response.Msg = "存在该二级合同";
                        //response.Total = companyContractCount;
                        //}
                    }
                    else
                    {
                        //1.根据公司Id、conferenceId和年份去公司一级合同中查询是否存在一级合同
                        //2.若已经存在，则返回一级合同的合同号
                        //2.1添加二级合同
                        //3.若不存在，添加一级合同，后将一级合同的合同号返回出来
                        //3.1添加二级合同
                        var companyContractCount = 1;
                        string conferenceContratctNumber = "";
                        var conferenceContract = db_ConferenceContract.AsNoTracking().FirstOrDefault(x => x.ConferenceId == request.ConferenceId
                                                                                                 && x.CompanyId == request.CompanyId
                                                                                                 && x.ContractYear == request.Year);
                        if (conferenceContract != null)
                        {
                            companyContractCount = db_CompanyContract.Count(x => x.ConferenceContractId == conferenceContract.ConferenceContractId) + 1;
                        }
                        else
                        {
                            //添加一级合同
                            var config = db_CCNumberConfig.AsNoTracking().FirstOrDefault(n => n.ConferenceId == request.ConferenceId
                                                                                             && n.Status == 1
                                                                                             && n.IsDelete == false
                                                                                             && n.Year == request.Year);
                            if (config != null)
                            {

                                conferenceContract = CreateConferenceContract(config, comContract, request.Year);
                                await db_ConferenceContract.AddAsync(conferenceContract);
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Msg = $"CCNumberConfig配置表中不存在当前{request.Year}年份,ConferenceId为{request.ConferenceId}的配置项";
                                response.Total = 0;
                                return response;
                            }
                        }

                        comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                        comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                        await db_CompanyContract.AddAsync(comContract);
                        //当二级合同的最大子合同数>0时
                        if (comContract.MaxContractNumber > 0)
                        {
                            var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(comContract.ComPrice) * comContract.MaxContractNumber).ToString();
                            conferenceContract.TotalPrice = totalPrice;
                            if (conferenceContract.IsOpPayStatudCode == false)
                            {
                                conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                            }
                            dbContext.Entry(conferenceContract).State = EntityState.Modified;

                        }
                        var result = await dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            response.IsSuccess = true;
                            response.Msg = "添加成功";
                            response.Total = 0;

                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "添加失败";
                            response.Total = 0;

                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 添加二级合同（折扣）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<modifyResponseNew> newAddCompanyContractDiscount(newServicePackStructDiscount request, ServerCallContext context)
        {
            try
            {
                request.Model = InitCompanyContract(request.Model);
                var comContract = Mapper.Map<CompanyContract>(request.Model);

                modifyResponseNew response = new modifyResponseNew();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;

                    var originalConferenceContract = db_CompanyContract.Where(x =>
                                                             x.CompanyId == comContract.CompanyId
                                                          && x.ConferenceId == comContract.ConferenceId
                                                          //&& x.CompanyServicePackId == comContract.CompanyServicePackId
                                                          && x.CompanyServicePackCode == comContract.CompanyServicePackCode
                                                          && x.Owerid == comContract.Owerid
                                                          && x.ContractStatusCode == comContract.ContractStatusCode
                                                          && x.ComPrice == comContract.ComPrice
                                                          && x.Year == comContract.Year
                                                          && IsEqual(x.OtherOwnerId, comContract.OtherOwnerId)).ToList();
                    if (originalConferenceContract != null && originalConferenceContract.Count > 0)
                    {
                        var companyContractCount = originalConferenceContract.Count();
                        if (companyContractCount > 0)
                        {

                            response.Msg = "存在该二级合同";
                            response.Total = companyContractCount;
                            if (companyContractCount == 1)
                            {
                                response.CompanyContractId = originalConferenceContract.FirstOrDefault().ContractId.ToString();
                            }
                        }
                        ////已经有了该合同，只需要更新子合同数（原数量加上新增的数量）
                        //originalConferenceContract.First().MaxContractNumber = originalConferenceContract.First().MaxContractNumber + comContract.MaxContractNumber;
                        //dbContext.CompanyContract.Update(originalConferenceContract.First());
                        //var result = await dbContext.SaveChangesAsync();
                        //if (result > 0)
                        //{
                        //    response.IsSuccess = true;
                        //    response.Msg = "添加成功";
                        //    response.Total = companyContractCount;
                        //}
                        //else
                        //{
                        //    response.IsSuccess = false;
                        //    response.Msg = "添加失败";
                        //    response.Total = companyContractCount;
                        //}
                    }
                    else
                    {
                        //1.根据公司Id、conferenceId和年份去公司一级合同中查询是否存在一级合同
                        //2.若已经存在，则返回一级合同的合同号
                        //2.1添加二级合同
                        //3.若不存在，添加一级合同，后将一级合同的合同号返回出来
                        //3.1添加二级合同
                        var companyContractCount = 1;
                        var conferenceContract = db_ConferenceContract.AsNoTracking().FirstOrDefault(x => x.ConferenceId == comContract.ConferenceId
                                                                                                 && x.ContractYear == comContract.Year
                                                                                                 && x.CompanyId == comContract.CompanyId.ObjToString());
                        if (conferenceContract != null)
                        {
                            companyContractCount = db_CompanyContract.Count(x => x.ConferenceContractId == conferenceContract.ConferenceContractId) + 1;
                        }
                        else
                        {
                            //添加一级合同
                            var config = db_CCNumberConfig.AsNoTracking().FirstOrDefault(n => n.ConferenceId == comContract.ConferenceId
                                                                                             && n.Status == 1
                                                                                             && n.IsDelete == false
                                                                                             && n.Year == comContract.Year);
                            if (config != null)
                            {

                                conferenceContract = CreateConferenceContract(config, comContract, comContract.Year);
                                await db_ConferenceContract.AddAsync(conferenceContract);
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Msg = $"CCNumberConfig配置表中不存在当前{comContract.Year}年份,ConferenceId为{comContract.ConferenceId}的配置项";
                                response.Total = 0;
                                return response;
                            }
                        }

                        comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                        comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                        await db_CompanyContract.AddAsync(comContract);

                        DelegateServicePackDiscount discountContract = new DelegateServicePackDiscount();
                        discountContract.Year = comContract.Year;
                        discountContract.DiscountId = Guid.NewGuid();
                        discountContract.ContractId = comContract.ContractId;
                        discountContract.PriceAfterDiscountRMB = request.PriceAfterDiscountRMB;
                        discountContract.PriceAfterDiscountUSD = request.PriceAfterDiscountUSD;
                        discountContract.CreatedOn = comContract.CreatedOn;
                        discountContract.CreatedBy = comContract.CreatedBy;
                        discountContract.ModefieldOn = comContract.ModifiedOn;
                        discountContract.ModefieldBy = comContract.ModifiedBy;
                        await dbContext.DelegateServicePackDiscount.AddAsync(discountContract);

                        //当二级合同的最大子合同数>0时
                        if (comContract.MaxContractNumber > 0)
                        {
                            var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(comContract.ComPrice) * comContract.MaxContractNumber).ToString();
                            conferenceContract.TotalPrice = totalPrice;
                            if (conferenceContract.IsOpPayStatudCode == false)
                            {
                                conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                            }

                            dbContext.Entry(conferenceContract).State = EntityState.Modified;
                        }

                        var result = await dbContext.SaveChangesAsync();
                        if (result > 0)
                        {
                            response.IsSuccess = true;
                            response.Msg = "添加成功";
                            response.Total = 0;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "添加失败";
                            response.Total = 0;
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }


        /// <summary>
        /// 初始化添加对象
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public newServicePackStruct InitCompanyContract(newServicePackStruct request)
        {
            request.ContractId = Guid.NewGuid().ToString();
            request.ConferenceContractId = Guid.Empty.ToString();
            request.CreatedOn = DateTime.UtcNow.ToString();
            request.ModifiedOn = new DateTime().ToUniversalTime().ToString();
            request.CCIsdelete = false;
            request.IsVerify = false;
            request.IsCheckIn = false;
            return request;
        }


        /// <summary>
        /// 创建一级合同
        /// </summary>
        /// <param name="config"></param>
        /// <param name="companyContract"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ConferenceContract CreateConferenceContract(CCNumberConfig config, CompanyContract companyContract, string year)
        {
            ConferenceContract model = new ConferenceContract();
            model.ConferenceContractId = Guid.NewGuid();
            var count = config.Count + 1;
            //合同规则为：21SNECC0001CS
            model.ContractNumber = config.Year.Substring(2) + config.CNano + companyContract.ContractCode.Substring(0, 1) + count.ToString().PadLeft(4, '0') + companyContract.ContractCode;
            model.ConferenceId = companyContract.ConferenceId;
            model.ContractCode = companyContract.ContractCode;
            model.ComNameTranslation = companyContract.ComNameTranslation;
            model.CompanyId = companyContract.CompanyId.ToString();
            model.ContractYear = year;
            model.CreatedBy = companyContract.CreatedBy;
            model.CreatedOn = companyContract.CreatedOn;
            model.ModifiedBy = companyContract.ModifiedBy;
            model.ModifiedOn = companyContract.ModifiedOn;
            model.Ower = companyContract.Ower;
            model.Owerid = companyContract.Owerid;
            model.IsDelete = false;
            model.IsModify = false;
            model.IsSendEmail = false;
            model.ContractStatusCode = "W";
            model.ModifyPermission = "0";
            model.PaymentStatusCode = "N";
            model.OtherOwner = companyContract.OtherOwner;
            model.OtherOwnerId = companyContract.OtherOwnerId;
            model.EnterpriseType = companyContract.EnterpriseType;
            model.TotalPrice = string.Empty;
            model.TotalPaid = string.Empty;
            return model;
        }

        /// <summary>
        /// 创建三级（个人）合同：不确定是否有一级、二级合同用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<modifyResponse> newAddPersonContract(newAddPersonContractRequest request, ServerCallContext context)
        {
            try
            {
                request.Person.PersonContractId = Guid.NewGuid().ToString();
                request.Person.ContractId = Guid.Empty.ToString();
                request.Person.CreatedOn = DateTime.UtcNow.ToString();
                request.Person.ModifiedOn = new DateTime().ToUniversalTime().ToString();
                Trans t = new Trans();
                request.Person.MemTranslation = JsonConvert.SerializeObject(t);
                request.CompanyContractModel = InitCompanyContract(request.CompanyContractModel);
                var comContract = Mapper.Map<CompanyContract>(request.CompanyContractModel);

                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;


                    //0先根据公司Id、套餐Id、业务员Id、合同状态、套餐价格、年份查看是否存在二级合同
                    //0.1若存在，则直接创建个人合同
                    //0.2若不存在，则创建二级合同后再创建三级合同

                    //根据公司Id、套餐Id、业务员Id、合同状态、套餐价格、年份查看是否存在二级合同
                    var companyContract = db_CompanyContract.AsNoTracking().Include(x => x.personContract).FirstOrDefault(x => x.CompanyId == comContract.CompanyId
                                                                           //&& x.CompanyServicePackId == comContract.CompanyServicePackId
                                                                           && x.CompanyServicePackCode == comContract.CompanyServicePackCode
                                                                           && x.Owerid == comContract.Owerid
                                                                           && x.ComPrice == comContract.ComPrice
                                                                           && x.Year == comContract.Year
                                                                           && x.ContractStatusCode == comContract.ContractStatusCode);
                    if (companyContract != null)
                    {
                        //说明已经存在了二级合同，直接创建个人合同
                        PersonContract person = CreatePersonContract(request.Person, companyContract);

                        var comContractNumber = companyContract.ComContractNumber;
                        var count = companyContract.personContract.Count() + 1;
                        person.PerContractNumber = comContractNumber + count.ToString().PadLeft(3, '0');

                        await dbContext.PersonContract.AddAsync(person);
                        if (companyContract.MaxContractNumber == -1)
                        {
                            var conferenceContract = dbContext.ConferenceContract.Where(x => x.ConferenceContractId == comContract.ConferenceContractId).FirstOrDefault();
                            var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(person.PerPrice)).ToString();
                            conferenceContract.TotalPrice = totalPrice;
                            if (conferenceContract.IsOpPayStatudCode == false)
                            {
                                conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                            }
                            dbContext.Entry(conferenceContract).State = EntityState.Modified;
                        }
                        var result = await dbContext.SaveChangesAsync();

                        if (result > 0)
                        {
                            response.IsSuccess = true;
                            response.Msg = "添加成功";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "添加失败";
                        }
                        return response;
                    }
                    else
                    {

                        //1.根据公司Id、conferenceId和年份去公司一级合同中查询是否存在一级合同
                        //2.若已经存在，则返回一级合同的合同号
                        //2.1添加二级合同
                        //3.若不存在，添加一级合同，后将一级合同的合同号返回出来
                        //3.1添加二级合同

                        var companyContractCount = 1;
                        var conferenceContract = db_ConferenceContract.AsNoTracking().FirstOrDefault(x => x.ConferenceId == comContract.ConferenceId
                                                                                                 && x.ContractYear == comContract.Year
                                                                                                 && x.CompanyId == comContract.CompanyId.ObjToString());
                        if (conferenceContract != null)
                        {
                            companyContractCount = db_CompanyContract.Count(x => x.ConferenceContractId == conferenceContract.ConferenceContractId) + 1;
                        }
                        else
                        {
                            //添加一级合同
                            var config = db_CCNumberConfig.AsNoTracking().FirstOrDefault(n => n.ConferenceId == comContract.ConferenceId
                                                                                             && n.Status == 1
                                                                                             && n.IsDelete == false
                                                                                             && n.Year == comContract.Year);
                            if (config != null)
                            {

                                conferenceContract = CreateConferenceContract(config, comContract, comContract.Year);
                                await db_ConferenceContract.AddAsync(conferenceContract);
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Msg = $"CCNumberConfig配置表中不存在当前{comContract.Year}年份,ConferenceId为{comContract.ConferenceId}的配置项";
                                return response;
                            }
                        }

                        comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                        comContract.ConferenceContractId = conferenceContract.ConferenceContractId;


                        await dbContext.CompanyContract.AddAsync(comContract);
                        if (comContract.MaxContractNumber > 0)
                        {
                            var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(comContract.ComPrice) * comContract.MaxContractNumber).ToString();
                            conferenceContract.TotalPrice = totalPrice;
                            if (conferenceContract.IsOpPayStatudCode == false)
                            {
                                conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                            }
                            dbContext.Entry(conferenceContract).State = EntityState.Modified;
                        }


                        PersonContract person = CreatePersonContract(request.Person, comContract);
                        var count = 1;
                        person.PerContractNumber = comContract.ComContractNumber + count.ToString().PadLeft(3, '0');
                        await dbContext.PersonContract.AddAsync(person);

                        if (comContract.MaxContractNumber == -1)
                        {
                            var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(person.PerPrice)).ToString();
                            conferenceContract.TotalPrice = totalPrice;
                            if (conferenceContract.IsOpPayStatudCode == false)
                            {
                                conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                            }
                            dbContext.Entry(conferenceContract).State = EntityState.Modified;
                        }

                        var result = await dbContext.SaveChangesAsync();

                        if (result > 0)
                        {
                            response.IsSuccess = true;
                            response.Msg = "添加成功";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "添加失败";
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }
        /// <summary>
        /// 根据应付费和已付费来自动计算付款状态
        /// </summary>
        /// <param name="totalPrice"></param>
        /// <param name="totalPaid"></param>
        /// <returns></returns>

        public string AutoCalculatePaymentStatus(string totalPrice, string totalPaid)
        {
            var price = double.Parse(totalPrice);
            var paid = double.Parse(totalPaid);
            string paymentStatus = "N";
            var result = paid / price;
            if (paid == 0)
            {
                paymentStatus = "N";
            }

            else if (result > 0 && result < 0.5)
            {
                paymentStatus = "P";
            }
            else if (result >= 0.5 && result < 1)
            {
                paymentStatus = "H";
            }
            else if (result == 1)
            {
                paymentStatus = "F";
            }

            return paymentStatus;
        }

        /// <summary>
        /// 创建个人合同
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private PersonContract CreatePersonContract(newPersonContractStruct request, CompanyContract companyContract)
        {
            //说明已经存在了二级合同，直接创建个人合同
            var pRequest = request;
            PersonContract person = new PersonContract();

            person.ConferenceId = pRequest.ConferenceId;
            person.PerPrice = companyContract.ComPrice;
            person.CompanyServicePackId = companyContract.CompanyServicePackId;
            person.CompanyServicePackCode = companyContract.CompanyServicePackCode;
            person.ContractId = companyContract.ContractId;
            person.ConferenceContractId = companyContract.ConferenceContractId;
            person.OtherOwner = companyContract.OtherOwner;
            person.OtherOwnerId = companyContract.OtherOwnerId;
            person.Ower = companyContract.Ower;
            person.Owerid = companyContract.Owerid;

            person.CreatedBy = pRequest.CreatedBy;
            person.CreatedOn = DateTime.UtcNow;
            person.ModefieldBy = pRequest.ModefieldBy;
            person.PaidAmount = pRequest.PaidAmount;
            person.PCIsdelete = false;
            person.CTypeCode = pRequest.CTypeCode;
            person.InviteCodeId = pRequest.InviteCodeId;
            person.IsInviteCode = pRequest.IsInviteCode;
            person.IsCheckIn = false;
            person.IsCommitAbstract = pRequest.IsCommitAbstract;
            person.IsFianceRecord = pRequest.IsFianceRecord;
            person.IsModify = pRequest.IsModify;
            person.IsPrint = pRequest.IsPrint;
            person.IsSendEmail = pRequest.IsSendEmail;
            person.MemberPK = pRequest.MemberPK.ObjToGuid();
            person.MemTranslation = pRequest.MemTranslation;
            person.ModefieldBy = pRequest.ModefieldBy;
            person.ModefieldOn = new DateTime().ToUniversalTime();
            person.PersonContractId = Guid.NewGuid();
            person.Year = pRequest.Year;
            return person;
        }


        /// <summary>
        /// 当已经存在二级合同时，在二级合同的基础上创建个人合同
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<modifyResponse> newAddPersonContractUnderCompanyContract(newAddPersonContractUnderCompanyContractRequest request, ServerCallContext context)
        {
            try
            {
                request.Person.PersonContractId = Guid.NewGuid().ToString();
                request.Person.ContractId = Guid.Empty.ToString();
                request.Person.CreatedOn = DateTime.UtcNow.ToString();
                request.Person.ModifiedOn = new DateTime().ToUniversalTime().ToString();
                Trans t = new Trans();
                request.Person.MemTranslation = JsonConvert.SerializeObject(t);

                //request.CompanyContractModel = InitCompanyContract(request.CompanyContractModel);
                //var comContract = Mapper.Map<CompanyContract>(request.CompanyContractModel);

                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;



                    CompanyContract comContract = db_CompanyContract.FirstOrDefault(x => x.ComContractNumber == request.CompanyContractNumber);
                    if (comContract != null)
                    {

                        PersonContract person = CreatePersonContract(request.Person, comContract);


                        var count = dbContext.PersonContract.Where(x => x.PerContractNumber.Contains(request.CompanyContractNumber)).Count() + 1;
                        person.PerContractNumber = comContract.ComContractNumber + count.ToString().PadLeft(3, '0');
                        await dbContext.PersonContract.AddAsync(person);

                        var result = await dbContext.SaveChangesAsync();

                        if (result > 0)
                        {
                            response.IsSuccess = true;
                            response.Msg = "添加成功";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = "添加失败";
                        }
                        return response;
                    }

                    else
                    {
                        response.Msg = "请确认该公司合同号是否正确";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }


        public override async Task<newGetConferenceInfoResponse> newGetConferenceInfo(newGetConferenceInfoRequest request, ServerCallContext context)
        {
            try
            {
                newGetConferenceInfoResponse response = new newGetConferenceInfoResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var CCNumberConfig = dbContext.CCNumberConfig;
                    var conference = dbContext.Conference;

                    var list = (from cc in CCNumberConfig
                                join con in conference
                                on cc.ConferenceId equals con.ConferenceID.ToString()
                                select new newGetConferenceInfoStruct
                                {
                                    ConferenceId = cc.ConferenceId,
                                    ConferenceName = cc.ConferenceName,
                                    Prefix = cc.Prefix,
                                    Year = cc.Year,
                                    CNano = cc.CNano,
                                    Count = cc.Count ?? 0,
                                    Status = cc.Status ?? 0,
                                    IsDelete = cc.IsDelete ?? false,
                                    Abbreviation = con.Abbreviation
                                }).Where(x => x.Status == 1 && x.Year == request.Year);
                    int count = list.Count();
                    response.Listdata.AddRange(list);
                    return await Task.FromResult(response);

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }



        public IQueryable<ConferenceContract> GetConferenceContratcIquerable(newGetOwnerByConfIdAndComIdAndYearRequest request, IQueryable<ConferenceContract> conferenceContractQueryable)
        {
            if (string.IsNullOrEmpty(request?.Year))
            {
                throw new ArgumentNullException($"{nameof(request.Year)}不能为空");
            }

            var year = request.Year;

            #region 拼接条件

            if (!string.IsNullOrEmpty(year))
            {
                conferenceContractQueryable = conferenceContractQueryable.Where(x => x.ContractYear == year);
            }

            #endregion
            return conferenceContractQueryable;
        }


        public override async Task<newGetOwnerByConfIdAndComIdAndYearResponse> newGetOwnerByConfIdAndComIdAndYear(newGetOwnerByConfIdAndComIdAndYearRequest request, ServerCallContext context)
        {
            try
            {
                newGetOwnerByConfIdAndComIdAndYearResponse response = new newGetOwnerByConfIdAndComIdAndYearResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var conferenceContratcQueryanble = dbContext.ConferenceContract as IQueryable<ConferenceContract>;
                    if (string.IsNullOrEmpty(request?.Year))
                    {
                        throw new ArgumentNullException($"{nameof(request.Year)}不能为空");
                    }

                    var list = dbContext.ConferenceContract
                        .Where(x => x.ContractYear == request.Year
                        && x.CompanyId == request.CompanyId
                        && x.ConferenceId == request.ConferenceID).Select(x => new newGetOwnerByConfIdAndComIdAndYearStruct
                        {
                            OtherOwner = x.OtherOwner,
                            OtherOwnerId = x.OtherOwnerId,
                            Owner = x.Ower,
                            OwnerId = x.Owerid,
                        });
                    response.Listdata.AddRange(list);

                    return await Task.FromResult(response);

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }



        public override async Task<newGetConferenceContractListResponse> newGetConferenceContractList(newGetConferenceContractListRequest request, ServerCallContext context)
        {
            try
            {
                newGetConferenceContractListResponse response = new newGetConferenceContractListResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {

                    var model = dbContext.ConferenceContract;
                    //搜索条件
                    var conferencContratcQueryable = dbContext.ConferenceContract as IQueryable<ConferenceContract>;
                    var contractStatusDic = dbContext.ContractStatusDic;
                    var companyName = request.Search.CompanyName.Trim();
                    var conferenceId = request.Search.ConferenceId.Trim();
                    var contractNumber = request.Search.ContractNumber.Trim();
                    var year = request.Search.Year.Trim();
                    var ownerId = request.Search.OwnerId.Trim();

                    if (!string.IsNullOrEmpty(companyName) && companyName != "string")
                    {
                        conferencContratcQueryable = conferencContratcQueryable.Where(x => x.ComNameTranslation.ToLower().Contains(companyName.ToLower()));
                    }
                    if (!string.IsNullOrEmpty(conferenceId) && conferenceId != "string")
                    {
                        conferencContratcQueryable = conferencContratcQueryable.Where(x => x.ConferenceId.ObjToString() == conferenceId);
                    }
                    if (!string.IsNullOrEmpty(year) && year != "string")
                    {
                        conferencContratcQueryable = conferencContratcQueryable.Where(x => x.ContractYear == year);
                    }
                    if (!string.IsNullOrEmpty(ownerId) && ownerId != "string")
                    {
                        conferencContratcQueryable = conferencContratcQueryable.Where(x => (x.Owerid.Contains(ownerId) || x.OtherOwnerId.Contains(ownerId)));
                    }
                    if (!string.IsNullOrEmpty(contractNumber) && contractNumber != "string")
                    {
                        conferencContratcQueryable = conferencContratcQueryable.Where(x => x.ContractNumber == contractNumber);
                    }
                    var listAll = (from con in conferencContratcQueryable
                                   join CSD in contractStatusDic
                                   on con.ContractStatusCode equals CSD.StatusCode
                                   select new newGetConferenceContractStruct
                                   {
                                       ComNameTranslation = con.ComNameTranslation ?? string.Empty,
                                       CompanyId = con.CompanyId ?? string.Empty,
                                       ConferenceContractId = con.ConferenceContractId.ToString(),
                                       ConferenceId = con.ConferenceId ?? string.Empty,
                                       ContractCode = con.ContractCode ?? string.Empty,
                                       ContractNumber = con.ContractNumber ?? string.Empty,
                                       ContractStatusCode = con.ContractStatusCode ?? string.Empty,
                                       ContractYear = con.ContractYear ?? string.Empty,
                                       CreatedBy = con.CreatedBy ?? string.Empty,
                                       CreatedOn = con.CreatedOn.ObjToString(),
                                       EnterpriseType = con.EnterpriseType ?? 0,
                                       IsDelete = con.IsDelete ?? false,
                                       IsModify = con.IsModify ?? false,
                                       IsOpPayStatudCode = con.IsOpPayStatudCode ?? false,
                                       IsSendEmail = con.IsSendEmail ?? false,
                                       ModifiedBy = con.ModifiedBy ?? string.Empty,
                                       ModifiedOn = con.ModifiedOn.ToString(),
                                       ModifyPermission = con.ModifyPermission ?? string.Empty,
                                       OtherOwner = con.OtherOwner ?? string.Empty,
                                       OtherOwnerId = con.OtherOwnerId ?? string.Empty,
                                       Ower = con.Ower ?? string.Empty,
                                       Owerid = con.Owerid ?? string.Empty,
                                       PaymentStatusCode = con.PaymentStatusCode ?? string.Empty,
                                       TotalPaid = con.TotalPaid ?? string.Empty,
                                       TotalPrice = con.TotalPrice ?? string.Empty,
                                       ContractStatusName = CSD.StatusName ?? string.Empty
                                   })
                        .OrderByBatch((string.IsNullOrEmpty(request.Search.Orderings.Trim()) || request.Search.Orderings.Trim() == "string") ? "-CreatedOn" : request.Search.Orderings.Trim())
                      ;
                    //var s = (string.IsNullOrEmpty(request.Search.Orderings) || request.Search.Orderings == "string") ? "-CreatedOn" : request.Search.Orderings;
                    var list = listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize);
                    var count = listAll.Count();
                    response.Listdata.AddRange(list);
                    response.Total = count;
                    return await Task.FromResult(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw (ex);
            }
        }

        /// <summary>
        /// 根据二级合同的合同号查询该合同下三级合同的个数
        /// </summary>
        /// <param name="comContractId"></param>
        /// <returns></returns>
        public int GetPersonContractCountByComContratcId(Guid? comContractId)
        {
            try
            {
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var PersonContract = dbContext.PersonContract;

                    int count = PersonContract.Where(x => x.ContractId == comContractId).Count();
                    return count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw (ex);
            }

        }
        public override async Task<newGetCompanyContractListResponse> newGetCompanyContractList(newGetCompanyContractListRequest request, ServerCallContext context)
        {

            try
            {
                newGetCompanyContractListResponse response = new newGetCompanyContractListResponse();
                //var comContract = Mapper.Map<CompanyContract>(response);
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var companyContract = dbContext.CompanyContract;
                    var compServicePack = dbContext.CompanyServicePack;
                    var conferenceContract = dbContext.ConferenceContract;
                    var contratcStatus = dbContext.ContractStatusDic;
                    var delegatePack = dbContext.DelegateServicePackDiscount;
                    //搜索条件
                    var companyContratcQueryable = dbContext.CompanyContract as IQueryable<CompanyContract>;

                    var companyName = request.Search.CompanyName.Trim();
                    var conferenceId = request.Search.ConferenceId.Trim();
                    var year = request.Search.Year.Trim();
                    var ownerId = request.Search.OwnerId.Trim();
                    var contractNumber = request.Search.ContractNumber.Trim();
                    var companyServicePackId = request.Search.CompanyServicePackId.Trim();

                    if (!string.IsNullOrEmpty(companyName) && companyName != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.ComNameTranslation.ToLower().Contains(companyName.ToLower()));
                    }
                    if (!string.IsNullOrEmpty(conferenceId) && conferenceId != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.ConferenceId.ToString() == conferenceId);
                    }
                    if (!string.IsNullOrEmpty(year) && year != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.Year == year);
                    }
                    if (!string.IsNullOrEmpty(ownerId) && ownerId != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => (x.Owerid.Contains(ownerId) || x.OtherOwnerId.Contains(ownerId)));
                    }
                    if (!string.IsNullOrEmpty(contractNumber) && contractNumber != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.ComContractNumber == contractNumber);
                    }

                    if (!string.IsNullOrEmpty(companyServicePackId) && companyServicePackId != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.CompanyServicePackId.ToString() == companyServicePackId);
                    }



                    var listAll = (from com in companyContratcQueryable
                                   join CSP in compServicePack
                                   on com.CompanyServicePackId.ObjToString() equals CSP.CompanyServicePackId.ObjToString()
                                   join CS in contratcStatus
                                   on com.ContractStatusCode equals CS.StatusCode into newCS
                                   from NCS in newCS.DefaultIfEmpty()
                                   join DP in delegatePack
                                   on com.ContractId equals DP.ContractId into newDP
                                   from NDP in newDP.DefaultIfEmpty()
                                   select new newGetCompanyContractListStruct
                                   {
                                       ContractNumber = com.ComContractNumber ?? string.Empty,
                                       CompanyNameTranslation = com.ComNameTranslation ?? string.Empty,
                                       EnterpriseType = com.EnterpriseType == 0 ? "内资" : "外资" ?? string.Empty,
                                       Pay = (int.Parse(com.ComPrice ?? "0") * com.MaxContractNumber ?? 1).ObjToString() ?? string.Empty,
                                       ComServicePackTranslation = CSP.Translation ?? string.Empty,
                                       Year = com.Year ?? string.Empty,
                                       MaxContractNumber = com.MaxContractNumber ?? 0,
                                       Owner = com.Ower ?? string.Empty,
                                       OwnerId = com.Owerid ?? string.Empty,
                                       OtherOwner = com.OtherOwner ?? string.Empty,
                                       OtherOwnerId = com.OtherOwnerId ?? string.Empty,
                                       IsGive = CSP.IsGive ?? false,
                                       FilledContractCount = GetPersonContractCountByComContratcId(com.ContractId).ToString() ?? string.Empty,
                                       ContractStatus = com.ContractStatusCode ?? string.Empty,
                                       CreatedBy = com.CreatedBy ?? string.Empty,
                                       CreatedOn = com.CreatedOn.ToString() ?? string.Empty,
                                       ModifiyBy = com.ModifiedBy ?? string.Empty,
                                       ModifyOn = com.ModifiedOn.ToString() ?? string.Empty,
                                       ComPrice = com.EnterpriseType == 0 ? CSP.PriceRMB : CSP.PriceUSD ?? string.Empty,
                                       ComapnyId = com.CompanyId.ToString() ?? string.Empty,
                                       OriginPrice = com.EnterpriseType == 0 ? CSP.PriceRMB : CSP.PriceUSD ?? string.Empty,
                                       ContractStateName = NCS.StatusName ?? string.Empty,
                                       DiscountId = NDP.DiscountId.ToString() ?? string.Empty,
                                       ContractId = NDP.ContractId.ToString() ?? string.Empty,
                                       PriceAfterDiscountUSD = NDP.PriceAfterDiscountUSD ?? string.Empty,
                                       PriceAfterDiscountRMB = NDP.PriceAfterDiscountRMB ?? string.Empty

                                   }).Where(x => request.Search.IsGive ? x.IsGive == request.Search.IsGive : 1 == 1)
                                  .OrderByBatch((string.IsNullOrEmpty(request.Search.Orderings.Trim()) || request.Search.Orderings.Trim() == "string") ? "-CreatedOn" : request.Search.Orderings.Trim())
                                  ;
                    var list = listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize);
                    int count = listAll.Count();
                    response.Listdata.AddRange(list);
                    response.Total = count;
                    return await Task.FromResult(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw (ex);
            }

        }



        public override async Task<newGetCompanyContractListResponse> newGetCompanyContractListByIsNotFullPerson(newBoolRequest request, ServerCallContext context)
        {
            try
            {
                newGetCompanyContractListResponse response = new newGetCompanyContractListResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var companyContract = dbContext.CompanyContract;
                    var compServicePack = dbContext.CompanyServicePack;
                    //搜索条件
                    var companyContratcQueryable = dbContext.CompanyContract as IQueryable<CompanyContract>;
                    var year = request.Search.Year.Trim();
                    var check = request.Search.Check;
                    var ownerId = request.Search.OwnerId.Trim();
                    var listAll = (from com in dbContext.CompanyContract
                                   join CSP in compServicePack
                                   on com.CompanyServicePackId equals CSP.CompanyServicePackId
                                   select new newGetCompanyContractListStruct
                                   {
                                       ContractNumber = com.ComContractNumber ?? string.Empty,
                                       CompanyNameTranslation = com.ComNameTranslation ?? string.Empty,
                                       EnterpriseType = com.EnterpriseType == 0 ? "内资" : "外资" ?? string.Empty,
                                       Pay = (double.Parse(com.ComPrice) * com.MaxContractNumber).ToString() ?? string.Empty,
                                       ComServicePackTranslation = CSP.Translation ?? string.Empty,
                                       Year = com.Year ?? string.Empty,
                                       MaxContractNumber = com.MaxContractNumber ?? 0,
                                       Owner = com.Ower ?? string.Empty,
                                       FilledContractCount = GetPersonContractCountByComContratcId(com.ContractId).ToString() ?? string.Empty,
                                       ContractStatus = com.ContractCode ?? string.Empty,
                                       IsGive = CSP.IsGive ?? false,
                                       CreatedBy = com.CreatedBy ?? string.Empty,
                                       CreatedOn = com.CreatedOn.ToString() ?? string.Empty,
                                       ModifiyBy = com.ModifiedBy ?? string.Empty,
                                       ModifyOn = com.ModifiedOn.ToString() ?? string.Empty,
                                       ComPrice = com.EnterpriseType == 0 ? CSP.PriceRMB : CSP.PriceUSD ?? string.Empty
                                   }).Where(x => x.Year == request.Search.Year.Trim()
                                   && (request.Search.Check ? x.MaxContractNumber > int.Parse(x.FilledContractCount) : 0 == 0)
                                   && (request.Search.IsGive ? x.IsGive == request.Search.IsGive : 1 == 1)
                                   && (!string.IsNullOrEmpty(ownerId) ? (x.OwnerId.Contains(ownerId) || x.OtherOwnerId.Contains(ownerId)) : 1 == 1)
                                   )
                                .OrderByBatch((string.IsNullOrEmpty(request.Search.Orderings.Trim()) || request.Search.Orderings.Trim() == "string") ? "-CreatedOn" : request.Search.Orderings.Trim())
                     ;
                    var list = listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize);
                    var count = listAll.Count();
                    //response.Listdata.Add(list);
                    response.Listdata.AddRange(list);
                    response.Total = count;
                    return await Task.FromResult(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw (ex);
            }

        }

        public override async Task<newGetCompanyContractListResponse> newGetCompanyContratcListByIsHaveDiscount(newBoolRequest request, ServerCallContext context)
        {
            try
            {
                newGetCompanyContractListResponse response = new newGetCompanyContractListResponse();
                //var comContract = Mapper.Map<CompanyContract>(response);
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var companyContract = dbContext.CompanyContract;
                    var compServicePack = dbContext.CompanyServicePack;
                    //搜索条件
                    var companyContratcQueryable = dbContext.CompanyContract as IQueryable<CompanyContract>;
                    var year = request.Search.Year.Trim();
                    var check = request.Search.Check;
                    var ownerId = request.Search.OwnerId.Trim();
                    if (!string.IsNullOrEmpty(year) && year != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.Year == year);
                    }
                    if (check == true)
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.delegateServicePackDiscount.Count == 0);
                    }
                    if (!string.IsNullOrEmpty(ownerId) && ownerId != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => (x.Owerid.Contains(ownerId) || x.OtherOwnerId.Contains(ownerId)));
                    }
                    var listAll = (from com in companyContratcQueryable
                                   join CSP in compServicePack
                                   on com.CompanyServicePackId equals CSP.CompanyServicePackId
                                   select new newGetCompanyContractListStruct
                                   {
                                       ContractNumber = com.ComContractNumber ?? string.Empty,
                                       CompanyNameTranslation = com.ComNameTranslation ?? string.Empty,
                                       EnterpriseType = com.EnterpriseType == 0 ? "内资" : "外资" ?? string.Empty,
                                       Pay = (int.Parse(com.ComPrice) * com.MaxContractNumber).ToString() ?? string.Empty,
                                       ComServicePackTranslation = CSP.Translation ?? string.Empty,
                                       Year = com.Year ?? string.Empty,
                                       MaxContractNumber = com.MaxContractNumber ?? 0,
                                       Owner = com.Ower ?? string.Empty,
                                       //FilledContractCount = "2",
                                       FilledContractCount = GetPersonContractCountByComContratcId(com.ContractId).ToString() ?? string.Empty,
                                       ContractStatus = com.ContractCode ?? string.Empty,
                                       CreatedBy = com.CreatedBy ?? string.Empty,
                                       CreatedOn = com.CreatedOn.ToString() ?? string.Empty,
                                       ModifiyBy = com.ModifiedBy ?? string.Empty,
                                       ModifyOn = com.ModifiedOn.ToString() ?? string.Empty,
                                       ComPrice = com.EnterpriseType == 0 ? CSP.PriceRMB : CSP.PriceUSD ?? string.Empty
                                   })
                                .OrderByBatch((string.IsNullOrEmpty(request.Search.Orderings.Trim()) || request.Search.Orderings.Trim() == "string") ? "-CreatedOn" : request.Search.Orderings.Trim())
                    ;
                    var list = listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize);
                    response.Total = listAll.Count();
                    response.Listdata.AddRange(list);
                    return await Task.FromResult(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw (ex);
            }
        }


        public override async Task<newGetCompanyContractListDiscountResponse> newGetCompanyContractListDiscount(newGetCompanyContractListDiscountRequest request, ServerCallContext context)
        {
            try
            {
                newGetCompanyContractListDiscountResponse response = new newGetCompanyContractListDiscountResponse();

                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var companyContract = dbContext.CompanyContract;
                    var compServicePack = dbContext.CompanyServicePack;
                    var delegateServicePackDiscount = dbContext.DelegateServicePackDiscount;
                    //搜索条件
                    var companyContratcQueryable = dbContext.CompanyContract as IQueryable<CompanyContract>;
                    var year = request.Search.Year.Trim();
                    var CompanyTranslation = request.Search.CompanyTranslation.Trim();
                    var ConferenceId = request.Search.ConferenceId.Trim();
                    var ContractNumber = request.Search.ContractNumber.Trim();
                    if (!string.IsNullOrEmpty(year) && year != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.Year == year);
                    }
                    if (!string.IsNullOrEmpty(CompanyTranslation) && CompanyTranslation != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.ComNameTranslation.Contains(CompanyTranslation));
                    }
                    if (!string.IsNullOrEmpty(ConferenceId) && ConferenceId != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.ConferenceId == ConferenceId);
                    }
                    if (!string.IsNullOrEmpty(ContractNumber) && ContractNumber != "string")
                    {
                        companyContratcQueryable = companyContratcQueryable.Where(x => x.ComContractNumber == ContractNumber);
                    }


                    var listAll = (from com in companyContratcQueryable
                                   join CSP in compServicePack
                                   on com.CompanyServicePackId equals CSP.CompanyServicePackId
                                   join dSPD in delegateServicePackDiscount
                                   on com.ContractId equals dSPD.ContractId
                                   select new newGetCompanyContractListDiscountStruct
                                   {
                                       ContractNumber = com.ComContractNumber ?? string.Empty,
                                       CompanyTranslation = com.ComNameTranslation ?? string.Empty,
                                       Price = com.ComPrice ?? string.Empty,
                                       PriceDiscount = com.EnterpriseType == 0 ? dSPD.PriceAfterDiscountRMB : dSPD.PriceAfterDiscountUSD ?? string.Empty,
                                   });
                    var list = listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize);
                    response.Total = listAll.Count();
                    response.Listdata.AddRange(list);
                    return await Task.FromResult(response);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw (ex);
            }
        }
        public override async Task<newGetPersonContractListResponse> newGetPersonContractList(newGetPersonContractListRequest request, ServerCallContext context)
        {
            try
            {
                newGetPersonContractListResponse response = new newGetPersonContractListResponse();
                using (var memberdbContext = new MemberDBContext(_member_options))
                {
                    using (var dbContext = new ConCDBContext(_options.Options))
                    {
                        var companyContractQueryable = dbContext.CompanyContract as IQueryable<CompanyContract>;
                        //var compServicePack = dbContext.CompanyServicePack;
                        var conferenceContract = dbContext.ConferenceContract;
                        //搜索条件
                        var personContratcQueryable = dbContext.PersonContract as IQueryable<PersonContract>;
                        var memberQuerable = memberdbContext.Member as IQueryable<Member>;
                        var compServicePackQueryable = dbContext.CompanyServicePack as IQueryable<CompanyServicePack>;

                        var year = request.Search.Year.Trim();
                        var conferenceId = request.Search.ConferenceId.Trim();
                        var servicePackId = request.Search.ServicePackId.Trim();
                        var companyName = request.Search.CompanyName.Trim();
                        var contractNumber = request.Search.ContractNumber.Trim();
                        var ownerId = request.Search.OwnerId.Trim();
                        var name = request.Search.Name.Trim();
                        string email = request.Search.Email.Trim();
                        //0:非赠送（付费、折扣） 1：赠送 2：全部
                        var isGive = request.Search.IsGive;

                        if (!string.IsNullOrEmpty(year) && year != "string")
                        {
                            personContratcQueryable = personContratcQueryable.Where(x => x.Year == year);
                        }
                        if (!string.IsNullOrEmpty(conferenceId) && conferenceId != "string")
                        {
                            personContratcQueryable = personContratcQueryable.Where(x => x.ConferenceId.ToString() == conferenceId);
                        }
                        if (!string.IsNullOrEmpty(servicePackId) && servicePackId != "string")
                        {
                            personContratcQueryable = personContratcQueryable.Where(x => x.CompanyServicePackId.ToString() == servicePackId);
                        }
                        if (!string.IsNullOrEmpty(contractNumber) && contractNumber != "string")
                        {
                            personContratcQueryable = personContratcQueryable.Where(x => x.PerContractNumber == contractNumber);
                        }

                        if (!string.IsNullOrEmpty(ownerId) && ownerId != "string")
                        {
                            personContratcQueryable = personContratcQueryable.Where(x => (x.Owerid.Contains(ownerId) || x.OtherOwnerId.Contains(ownerId)));
                        }
                        if (!string.IsNullOrEmpty(name) && name != "string")
                        {
                            personContratcQueryable = personContratcQueryable.Where(x => x.MemTranslation.ToLower().Contains(name.ToLower()));
                        }
                        if (!string.IsNullOrEmpty(companyName))
                        {
                            companyContractQueryable = companyContractQueryable.Where(x => x.ComNameTranslation.ToLower().Contains(companyName));
                        }
                        //非赠送的个人合同
                        if (isGive == 0)
                        {
                            compServicePackQueryable = compServicePackQueryable.Where(x => x.IsGive == false);
                        }
                        //筛选赠送的个人合同
                        else if (isGive == 1)
                        {
                            compServicePackQueryable = compServicePackQueryable.Where(x => x.IsGive == true);
                        }
                        //全部
                        else if (isGive == 2)
                        {
                            compServicePackQueryable = compServicePackQueryable.Where(x => 1 == 1);
                        }

                        var isMemberMain = false;
                        if (!string.IsNullOrEmpty(email))
                        {
                            isMemberMain = true;
                            memberQuerable = memberQuerable.Where(x => x.MemEmail.ToLower().Contains(email.ToLower()));
                        }

                        List<newGetPersonContractListStruct> resultList = new List<newGetPersonContractListStruct>();

                        #region 按照邮箱查询时，以会员表为主表进行查询
                        if (isMemberMain)
                        {
                            var memberList = await memberQuerable.Select(x => new
                            {
                                MemberPK = x.MemberPK.ObjToString(),
                                MemPosition = x.MemPosition ?? string.Empty,
                                MemEmail = x.MemEmail ?? string.Empty,
                                MemMobile = x.MemMobile ?? string.Empty
                            }).ToListAsync();
                            var memberPKList = memberList.Select(x => x.MemberPK.ObjToString()).ToList();

                            personContratcQueryable = personContratcQueryable.Where(x => memberPKList.Contains(x.MemberPK.ObjToString()));

                            var listAll = (from per in personContratcQueryable
                                           join CSP in compServicePackQueryable
                                           on per.CompanyServicePackId equals CSP.CompanyServicePackId
                                           join com in companyContractQueryable
                                           on per.ContractId equals com.ContractId
                                           select new newGetPersonContractListStruct
                                           {
                                               ContractNumber = per.PerContractNumber ?? string.Empty,
                                               MemNameTranslation = per.MemTranslation ?? string.Empty,
                                               CompanyNameTranslation = com.ComNameTranslation ?? string.Empty,
                                               Position = memberList.FirstOrDefault(x => x.MemberPK == per.MemberPK.ObjToString()).MemPosition,
                                               Email = memberList.FirstOrDefault(x => x.MemberPK == per.MemberPK.ObjToString()).MemEmail,
                                               Mobile = memberList.FirstOrDefault(x => x.MemberPK == per.MemberPK.ObjToString()).MemMobile,
                                               ComServicePackTranslation = CSP.Translation ?? string.Empty,
                                               Pay = per.PerPrice ?? string.Empty,
                                               Paid = per.PaidAmount ?? string.Empty,
                                               Owner = per.Ower ?? string.Empty,
                                               IsSendEmail = per.IsSendEmail ?? false,
                                               IsPrint = per.IsPrint ?? false,
                                               CreatedBy = per.CreatedBy ?? string.Empty,
                                               CreatedOn = per.CreatedOn.ToString() ?? string.Empty,
                                               ModifiyBy = per.ModefieldBy ?? string.Empty,
                                               ModifyOn = per.ModefieldOn.ToString() ?? string.Empty,
                                               MemberPK = per.MemberPK.ToString() ?? string.Empty
                                           }).OrderByBatch((string.IsNullOrEmpty(request.Search.Orderings.Trim()) || request.Search.Orderings.Trim() == "string") ? "-CreatedOn" : request.Search.Orderings.Trim());

                            resultList = await listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize).ToListAsync();
                            response.Total = listAll.Count();
                            response.Listdata.AddRange(resultList);
                        }
                        else
                        {

                            var listAll = (from per in personContratcQueryable
                                           join CSP in compServicePackQueryable
                                           on per.CompanyServicePackId equals CSP.CompanyServicePackId
                                           join com in companyContractQueryable
                                           on per.ContractId equals com.ContractId
                                           select new newGetPersonContractListStruct
                                           {
                                               ContractNumber = per.PerContractNumber ?? string.Empty,
                                               MemNameTranslation = per.MemTranslation ?? string.Empty,
                                               CompanyNameTranslation = com.ComNameTranslation ?? string.Empty,
                                               Position = "" ?? string.Empty,
                                               Email = "" ?? string.Empty,
                                               Mobile = "" ?? string.Empty,
                                               ComServicePackTranslation = CSP.Translation ?? string.Empty,
                                               Pay = per.PerPrice ?? string.Empty,
                                               Paid = per.PaidAmount ?? string.Empty,
                                               Owner = per.Ower ?? string.Empty,
                                               IsSendEmail = per.IsSendEmail ?? false,
                                               IsPrint = per.IsPrint ?? false,
                                               CreatedBy = per.CreatedBy ?? string.Empty,
                                               CreatedOn = per.CreatedOn.ToString() ?? string.Empty,
                                               ModifiyBy = per.ModefieldBy ?? string.Empty,
                                               ModifyOn = per.ModefieldOn.ToString() ?? string.Empty,
                                               MemberPK = per.MemberPK.ToString() ?? string.Empty
                                           }).OrderByBatch((string.IsNullOrEmpty(request.Search.Orderings.Trim()) || request.Search.Orderings.Trim() == "string") ? "-CreatedOn" : request.Search.Orderings.Trim());

                            var list = await listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize).ToListAsync();

                            var memberPKList = list.Select(a => a.MemberPK).ToList();
                            var memberList = await memberdbContext.Member.Where(x => memberPKList.Contains(x.MemberPK.ObjToString())).ToListAsync();
                            foreach (var item in list)
                            {
                                item.Position = memberList.FirstOrDefault(x => x.MemberPK.ObjToString() == item.MemberPK.ObjToString()).MemPosition;
                                item.Email = memberList.FirstOrDefault(x => x.MemberPK.ObjToString() == item.MemberPK.ObjToString()).MemEmail;
                                item.Mobile = memberList.FirstOrDefault(x => x.MemberPK.ObjToString() == item.MemberPK.ObjToString()).MemMobile;
                            }
                            response.Total = listAll.Count();
                            response.Listdata.AddRange(list);
                        }
                        #endregion
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw (ex);
            }
        }



        public override async Task<modifyResponse> newCreatePersonContractByCompanyContractInviteCode(newCreatePersonContractByCompanyContractInviteCodeRequest request, ServerCallContext context)
        {
            try
            {
                request.Person.PersonContractId = Guid.NewGuid().ToString();
                //request.Person.ContractId = Guid.Empty.ToString();
                request.Person.CreatedOn = DateTime.UtcNow.ToString();
                request.Person.ModifiedOn = new DateTime().ToUniversalTime().ToString();

                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_PersonContract = dbContext.PersonContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;
                    var s = request.Person.ContractId;
                    //1.先去二级合同表中查询是否存在该邀请码（二级合同Id）
                    CompanyContract inviteCode = db_CompanyContract.FirstOrDefault(x => x.ContractId.ToString() == request.Person.ContractId);

                    //有该邀请码
                    if (inviteCode != null)
                    {
                        //1.2若存在：在根据公司Id、年份、大会、非删除状态查询是否匹配
                        var comContract = db_CompanyContract.FirstOrDefault(x => x.CompanyId.ToString() == request.CompanyId && x.Year == request.Person.Year && x.ConferenceId == request.Person.ConferenceId && x.CCIsdelete == false);
                        //匹配
                        if (comContract != null)
                        {
                            //1.2.2若匹配：查询该套餐的参会人员是否已经填满了
                            var maxContractNumber = comContract.MaxContractNumber;
                            var personCount = db_PersonContract.Where(x => x.ContractId == comContract.ContractId).Count();
                            //1.2.2.2若未填满：在该套餐下添加一个个人合同
                            if (maxContractNumber > personCount)
                            {

                                PersonContract person = CreatePersonContract(request.Person, comContract);
                                person.InviteCodeId = inviteCode.ContractId.ToString();
                                person.IsInviteCode = true;
                                await dbContext.PersonContract.AddAsync(person);

                                var result = await dbContext.SaveChangesAsync();

                                if (result > 0)
                                {
                                    response.IsSuccess = true;
                                    response.Msg = "添加成功";
                                }
                                else
                                {
                                    response.IsSuccess = false;
                                    response.Msg = "添加失败";
                                }
                                return response;
                            }
                            else
                            {
                                //1.2.2.1若已经填满：提示参会人员已满
                                response.IsSuccess = false;
                                response.Msg = "该邀请码下参会人员已满";
                            }
                        }
                        else
                        {
                            //1.2.1若不匹配：提示无效的邀请码
                            response.IsSuccess = false;
                            response.Msg = "无效的邀请码";
                        }

                    }
                    //1.1若不存在：提示该邀请码不存在
                    else
                    {
                        response.IsSuccess = false;
                        response.Msg = "该邀请码不存在";
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

        }

        public override async Task<modifyResponse> newCreatePersonContractByInviteCode(newCreatePersonContractByInviteCodeRequest request, ServerCallContext context)
        {
            try
            {

                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_InviteCode = dbContext.InviteCode;
                    var db_InviteCodeRecord = dbContext.InviteCodeRecord;
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;
                    var comContract = Mapper.Map<CompanyContract>(request.CompanyContractModel);

                    //1.先根据邀请码、年份和大会去邀请码表中查询该邀请码是否存在
                    var inviteCode = db_InviteCode.FirstOrDefault(x => x.InviteCodeNumber == request.InviteCode && x.Year == request.Person.Year && x.WebSite == request.WebSite);

                    if (inviteCode != null)
                    {
                        //1.2若存在：统计该邀请码下已经存在几个个人合同，同时将该邀请码对应的套餐Id查询出来
                        var inviteCodeRecord = db_InviteCodeRecord.Where(x => x.InviteCodeId == inviteCode.Id);
                        int inviteCodeRecordCount = inviteCodeRecord.Count();
                        var servicePackId = inviteCode.CompanyServicePackId;
                        //1.2.2若小于可使用次数
                        if (inviteCode.Count > inviteCodeRecordCount)
                        {
                            //1.2.2.1根据companyId，年份、大会查询是否存在一级合同：
                            var conferenceContract = db_ConferenceContract.FirstOrDefault(x => x.CompanyId == comContract.CompanyId.ToString() && x.ContractYear == request.Person.Year && x.ConferenceId == request.Person.ConferenceId);
                            if (conferenceContract != null)
                            {
                                //1.2.2.2存在一级合同：
                                //在根据套餐Id，公司Id,年份,大会判断是否存在二级合同
                                var companyContract = db_CompanyContract.FirstOrDefault(x => x.CompanyServicePackId.ToString() == servicePackId && x.CompanyId == comContract.CompanyId && x.ConferenceId == request.Person.ConferenceId);
                                if (companyContract != null)
                                {
                                    //1.2.2.2.2若存在该套餐对应的二级合同，则直接创建三级合同（个人合同）

                                    PersonContract person = CreatePersonContract(request.Person, companyContract);
                                    person.InviteCodeId = inviteCode.Id.ToString();
                                    person.IsInviteCode = true;
                                    var comContractNumber = companyContract.ComContractNumber;
                                    var count = companyContract.personContract.Count() + 1;
                                    person.PerContractNumber = comContractNumber + count.ToString().PadLeft(3, '0');

                                    await dbContext.PersonContract.AddAsync(person);
                                    if (companyContract.MaxContractNumber == -1)
                                    {
                                        var coneContract = dbContext.ConferenceContract.Where(x => x.ConferenceContractId == comContract.ConferenceContractId).FirstOrDefault();
                                        var totalPrice = (double.Parse(coneContract.TotalPrice) + double.Parse(person.PerPrice)).ToString();
                                        coneContract.TotalPrice = totalPrice;
                                        if (conferenceContract.IsOpPayStatudCode == false)
                                        {
                                            conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                                        }
                                        dbContext.Entry(conferenceContract).State = EntityState.Modified;
                                    }
                                    //插入InviteCodeRecord表
                                    InviteCodeRecord record = new InviteCodeRecord();
                                    record.PersonContractId = person.InviteCodeId;
                                    record.Id = Guid.NewGuid();
                                    record.IsDelete = false;
                                    record.MemberName = person.MemTranslation;
                                    record.MemberPK = person.MemberPK.ToString();
                                    record.PersonContractId = person.PersonContractId.ToString();
                                    record.PersonContractNumber = person.PerContractNumber;
                                    record.UseDate = DateTime.UtcNow.ToString();
                                    await dbContext.InviteCodeRecord.AddAsync(record);

                                    var result = await dbContext.SaveChangesAsync();

                                    if (result > 0)
                                    {
                                        response.IsSuccess = true;
                                        response.Msg = "添加成功";
                                    }
                                    else
                                    {
                                        response.IsSuccess = false;
                                        response.Msg = "添加失败";
                                    }
                                    return response;
                                }
                                else
                                {
                                    //1.2.2.2.1若不存在该套餐对应的二级合同：创建该套餐对应的二级合同后，在创建三级（个人）合同
                                    comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                                    comContract.MaxContractNumber = -1;
                                    await dbContext.CompanyContract.AddAsync(comContract);


                                    PersonContract person = CreatePersonContract(request.Person, comContract);
                                    var count = 1;
                                    person.PerContractNumber = comContract.ComContractNumber + count.ToString().PadLeft(3, '0');
                                    await dbContext.PersonContract.AddAsync(person);

                                    if (comContract.MaxContractNumber == -1)
                                    {
                                        var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(person.PerPrice)).ToString();
                                        conferenceContract.TotalPrice = totalPrice;
                                        if (conferenceContract.IsOpPayStatudCode == false)
                                        {
                                            conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                                        }
                                        dbContext.Entry(conferenceContract).State = EntityState.Modified;
                                    }
                                    //插入到InviteRecord表中
                                    InviteCodeRecord record = new InviteCodeRecord();
                                    record.PersonContractId = person.InviteCodeId;
                                    record.Id = Guid.NewGuid();
                                    record.IsDelete = false;
                                    record.MemberName = person.MemTranslation;
                                    record.MemberPK = person.MemberPK.ToString();
                                    record.PersonContractId = person.PersonContractId.ToString();
                                    record.PersonContractNumber = person.PerContractNumber;
                                    record.UseDate = DateTime.UtcNow.ToString();
                                    await dbContext.InviteCodeRecord.AddAsync(record);
                                    var result = await dbContext.SaveChangesAsync();

                                    if (result > 0)
                                    {
                                        response.IsSuccess = true;
                                        response.Msg = "添加成功";
                                    }
                                    else
                                    {
                                        response.IsSuccess = false;
                                        response.Msg = "添加失败";
                                    }
                                    return response;


                                }
                            }
                            else
                            {
                                // 若不存在：则先创建一级合同，在根据套餐Id创建相应的二级合同，在创建三级（个人）合同
                                var config = db_CCNumberConfig.AsNoTracking().FirstOrDefault(n => n.ConferenceId == comContract.ConferenceId
                                                                                          && n.Status == 1
                                                                                          && n.IsDelete == false
                                                                                          && n.Year == comContract.Year);
                                var companyContractCount = 1;
                                if (config != null)
                                {

                                    conferenceContract = CreateConferenceContract(config, comContract, comContract.Year);
                                    await db_ConferenceContract.AddAsync(conferenceContract);


                                }
                                else
                                {
                                    response.IsSuccess = false;
                                    response.Msg = $"CCNumberConfig配置表中不存在当前{comContract.Year}年份,ConferenceId为{comContract.ConferenceId}的配置项";
                                    return response;
                                }
                                comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                                comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                                comContract.MaxContractNumber = -1;

                                await dbContext.CompanyContract.AddAsync(comContract);



                                PersonContract person = CreatePersonContract(request.Person, comContract);
                                var count = 1;
                                person.PerContractNumber = comContract.ComContractNumber + count.ToString().PadLeft(3, '0');
                                await dbContext.PersonContract.AddAsync(person);

                                if (comContract.MaxContractNumber == -1)
                                {
                                    var totalPrice = (double.Parse(conferenceContract.TotalPrice) + double.Parse(person.PerPrice)).ToString();
                                    conferenceContract.TotalPrice = totalPrice;
                                    if (conferenceContract.IsOpPayStatudCode == false)
                                    {
                                        conferenceContract.PaymentStatusCode = AutoCalculatePaymentStatus(totalPrice, conferenceContract.TotalPaid);
                                    }
                                    dbContext.Entry(conferenceContract).State = EntityState.Modified;
                                }
                                //插入到InviteCodeRecord表中
                                InviteCodeRecord record = new InviteCodeRecord();
                                record.PersonContractId = person.InviteCodeId;
                                record.Id = Guid.NewGuid();
                                record.IsDelete = false;
                                record.MemberName = person.MemTranslation;
                                record.MemberPK = person.MemberPK.ToString();
                                record.PersonContractId = person.PersonContractId.ToString();
                                record.PersonContractNumber = person.PerContractNumber;
                                record.UseDate = DateTime.UtcNow.ToString();
                                await dbContext.InviteCodeRecord.AddAsync(record);
                                var result = await dbContext.SaveChangesAsync();

                                if (result > 0)
                                {
                                    response.IsSuccess = true;
                                    response.Msg = "添加成功";
                                }
                                else
                                {
                                    response.IsSuccess = false;
                                    response.Msg = "添加失败";
                                }
                            }
                        }
                        else
                        {
                            //1.2.1若已经和可使用次数相同：提示：该邀请码已经没有可使用次数了
                            response.Msg = "该邀请码已经没有可使用次数了";
                            response.IsSuccess = false;

                        }
                        return response;
                    }
                    else
                    {
                        //1.1若不存在：提示该邀请码不存在
                        response.IsSuccess = false;
                        response.Msg = "该邀请码不存在";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }


        }

        public override async Task<modifyResponse> newModifyOwnerByConferenceContractId(newModifyOwnerByConferenceContractIdRequest request, ServerCallContext context)
        {
            try
            {
                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var conferenceContract = dbContext.ConferenceContract.FirstOrDefault(x => x.ConferenceContractId.ToString() == request.ConferenceContractId);

                    conferenceContract.Ower = request.Owner;
                    conferenceContract.Owerid = request.OwnerId;
                    conferenceContract.OtherOwner = request.OtherOwner;
                    conferenceContract.OtherOwnerId = request.OtherOwnerId;
                    conferenceContract.ModifiedBy = request.ModifiedBy;
                    conferenceContract.ModifiedOn = DateTime.UtcNow;
                    dbContext.Entry(conferenceContract).State = EntityState.Modified;
                    var count = await dbContext.SaveChangesAsync();
                    if (count > 0)
                    {
                        response.Msg = "修改成功";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Msg = "修改失败";
                        response.IsSuccess = false;
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }



        public override async Task<newGetMySpeakersListResponse> newGetMySpeakersList(newGetMySpeakersListRequest request, ServerCallContext context)
        {
            try
            {
                newGetMySpeakersListResponse response = new newGetMySpeakersListResponse();
                using (var memberdbContext = new MemberDBContext(_member_options))
                {
                    using (var dbContext = new ConCDBContext(_options.Options))
                    {
                        var year = request.Search.Year.Trim();
                        var conferenceId = request.Search.ConferenceId.Trim();
                        //公司名：个人合同中的公司名
                        var companyName = request.Search.CompanyName.Trim();
                        //合同号
                        var contractNumber = request.Search.ContractNumber.Trim();
                        //姓名：个人合同中的姓名
                        var name = request.Search.Name.Trim();
                        //业务员
                        var ownerId = request.Search.OwnerId.Trim();

                        var personContractQueryable = dbContext.PersonContract as IQueryable<PersonContract>;
                        var memberQueryable = memberdbContext.Member as IQueryable<Member>;
                        var abstractDraft = dbContext.AbstractDraft.Where(x => x.IsDelete == false);
                        var abstractParticipant = dbContext.AbstractParticipant.Where(x => x.IsDelete == false);
                        var companyServicePack = dbContext.CompanyServicePack;
                        var company = memberdbContext.Company;


                        if (!string.IsNullOrEmpty(year) && year != "string")
                        {
                            personContractQueryable = personContractQueryable.Where(x => x.Year == year);
                        }
                        if (!string.IsNullOrEmpty(conferenceId) && conferenceId != "string")
                        {
                            personContractQueryable = personContractQueryable.Where(x => x.ConferenceId == conferenceId);
                        }
                        if (!string.IsNullOrEmpty(companyName) && companyName != "string")
                        {
                            personContractQueryable = personContractQueryable.Where(x => x.companyContract.ComNameTranslation.Contains(companyName));
                        }
                        if (!string.IsNullOrEmpty(contractNumber) && contractNumber != "string")
                        {
                            personContractQueryable = personContractQueryable.Where(x => x.PerContractNumber.Contains(contractNumber));
                        }
                        if (!string.IsNullOrEmpty(name) && name != "string")
                        {
                            personContractQueryable = personContractQueryable.Where(x => x.MemTranslation.Contains(name));
                        }
                        if (!string.IsNullOrEmpty(ownerId) && ownerId != "string")
                        {
                            personContractQueryable = personContractQueryable.Where(x => (x.Owerid.Contains(ownerId) || x.OtherOwnerId.Contains(ownerId)));
                        }

                        var draftList = await (from ad in abstractDraft
                                               join ap in abstractParticipant
                                               on ad.AbstractParticipantID equals ap.AbstractParticipantID into apT
                                               from ap1 in apT.DefaultIfEmpty()
                                               select new
                                               {
                                                   PerContractNumber = ad.perContractNumber,
                                                   Name = ap1.NameTranslation ?? string.Empty,
                                                   CompanyTranslation = ap1.CompanyTranslation ?? string.Empty,
                                                   Email = ap1.Email ?? string.Empty,
                                                   Mobile = ap1.Mobile ?? string.Empty,
                                                   Position = ap1.JobTranslation ?? string.Empty,
                                               }).ToListAsync();



                        var personCon = (from person in personContractQueryable
                                         join csp in companyServicePack
                                         on person.CompanyServicePackId equals csp.CompanyServicePackId
                                         select new
                                         {
                                             MemTranslation = person.MemTranslation ?? string.Empty,
                                             PerContractNumber = person.PerContractNumber ?? string.Empty,
                                             Translation = csp.Translation ?? string.Empty,
                                             IsSpeaker = csp.IsSpeaker,
                                             MemberPK = person.MemberPK.ToString() ?? string.Empty,
                                             ComNameTranslation = person.companyContract.ComNameTranslation ?? string.Empty,
                                             EnterpriseType = person.companyContract.EnterpriseType ?? 0,
                                             Ower = person.Ower ?? string.Empty,
                                             Owerid = person.Owerid ?? string.Empty,
                                             OtherOwner = person.OtherOwner ?? string.Empty,
                                             OtherOwnerId = person.OtherOwnerId ?? string.Empty,
                                             CreatedOn = person.CreatedOn.ToString() ?? string.Empty,
                                             CreatedBy = person.CreatedBy ?? string.Empty,
                                             ModefieldBy = person.ModefieldBy ?? string.Empty,
                                             ConferenceId = person.ConferenceId ?? string.Empty,
                                             ModefieldOn = person.ModefieldOn.ToString() ?? string.Empty
                                         }
                                  ).Where(x => x.IsSpeaker == true)
                                   .OrderByBatch((string.IsNullOrEmpty(request.Search.Orderings.Trim()) || request.Search.Orderings.Trim() == "string") ? "-CreatedOn" : request.Search.Orderings.Trim());



                        var listAll = from per in personCon
                                      select new newGetMySpeakersListStruct
                                      {
                                          ContractNumber = per.PerContractNumber ?? string.Empty,
                                          PerContractName = per.MemTranslation ?? string.Empty,
                                          PerParticipantTypeTranslation = per.Translation ?? string.Empty,

                                          Name = draftList.FirstOrDefault(x => x.PerContractNumber == per.PerContractNumber).Name ?? string.Empty,
                                          CompanyTranslation = draftList.FirstOrDefault(x => x.PerContractNumber == per.PerContractNumber).CompanyTranslation ?? string.Empty,
                                          Email = draftList.FirstOrDefault(x => x.PerContractNumber == per.PerContractNumber).Email ?? string.Empty,
                                          Mobile = draftList.FirstOrDefault(x => x.PerContractNumber == per.PerContractNumber).Mobile ?? string.Empty,
                                          Position = draftList.FirstOrDefault(x => x.PerContractNumber == per.PerContractNumber).Position ?? string.Empty,
                                          ConferenceId = per.ConferenceId ?? string.Empty,
                                          Owner = per.Ower ?? string.Empty,
                                          OwnerId = per.Owerid ?? string.Empty,
                                          OtherOwner = per.OtherOwner ?? string.Empty,
                                          OtherOwnerId = per.OtherOwnerId ?? string.Empty,
                                          CreatedOn = per.CreatedOn.ToString() ?? string.Empty,
                                          CreatedBy = per.CreatedBy ?? string.Empty,
                                          ModifiyBy = per.ModefieldBy ?? string.Empty,
                                          ModifyOn = per.ModefieldOn.ToString() ?? string.Empty,
                                          MemberPK = per.MemberPK.ToString(),
                                          EnterpriseType = per.EnterpriseType,
                                          IsAdd = draftList.FirstOrDefault(x => x.PerContractNumber == per.PerContractNumber) != null
                                      };

                        var list = await listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize).ToListAsync();
                        var memberPKList = list.Select(x => x.MemberPK).ToList();

                        var memberList = await memberQueryable.Where(x => memberPKList.Contains(x.MemberPK.ToString())).ToListAsync();
                        var memlist = (from mem in memberQueryable
                                       join com in company
                                       on mem.CompanyPK equals com.CompanyPK.ToString() into MC
                                       from NMC in MC.DefaultIfEmpty()
                                       select new
                                       {
                                           MemberPK = mem.MemberPK,
                                           MemNameCn = mem.MemNameCn ?? string.Empty,
                                           MemNameEn = mem.MemNameEn ?? string.Empty,
                                           ComNameCn = NMC.ComNameCn ?? string.Empty,
                                           ComNameEn = NMC.ComNameEn ?? string.Empty,
                                           MemEmail = mem.MemEmail ?? string.Empty,
                                           MemMobile = mem.MemMobile ?? string.Empty,
                                           MemPosition = mem.MemPosition ?? string.Empty
                                       });

                        foreach (var item in list)
                        {
                            var model = memlist.FirstOrDefault(x => x.MemberPK == item.MemberPK.ObjToGuid());
                            var enterpriseType = item.EnterpriseType;
                            Trans nameTrans = new Trans();
                            if (string.IsNullOrEmpty(item.Name))
                            {
                                nameTrans.CN = model.MemNameCn;
                                nameTrans.EN = model.MemNameEn;
                            }
                            Trans companyTrans = new Trans();
                            if (string.IsNullOrEmpty(item.CompanyTranslation))
                            {
                                companyTrans.CN = model.ComNameCn;
                                companyTrans.EN = model.ComNameEn;
                            }
                            item.Name = string.IsNullOrEmpty(item.Name) ? JsonConvert.SerializeObject(nameTrans) : item.Name;
                            item.CompanyTranslation = string.IsNullOrEmpty(item.CompanyTranslation) ? JsonConvert.SerializeObject(companyTrans) : item.CompanyTranslation;
                            item.Email = string.IsNullOrEmpty(item.Email) ? model.MemEmail : item.Email;
                            item.Mobile = string.IsNullOrEmpty(item.Mobile) ? model.MemMobile : item.Mobile;
                            item.Position = string.IsNullOrEmpty(item.Position) ? model.MemPosition : item.Position;
                        }

                        response.Listdata.AddRange(list);
                        response.Total = await listAll.CountAsync();
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 根据合同号更改已付费金额和付款状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int UpdatePaiedAndStatusByContractNumber(ConferenceContractQueueVM model)
        {
            try
            {
                int count = 0;
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var conferenceContract = dbContext.ConferenceContract.FirstOrDefault(x => x.ContractNumber == model.ContractNumber);
                    conferenceContract.TotalPaid = model.TotalPaid + " " + model.TotalPaidUnit;
                    if (conferenceContract.IsOpPayStatudCode == false)
                    {
                        conferenceContract.PaymentStatusCode = model.PaymentStatusCode;
                    }
                    count = dbContext.SaveChanges();

                    return count;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }


        public override async Task<newGetPersonContractListResponse> newGetPersonContractListByComContratcId(newGetPersonContractListByComContratcIdRequest request, ServerCallContext context)
        {
            try
            {
                newGetPersonContractListResponse response = new newGetPersonContractListResponse();
                using (var memberdbContext = new MemberDBContext(_member_options))
                {
                    using (var dbContext = new ConCDBContext(_options.Options))
                    {
                        var companyContractQueryable = dbContext.CompanyContract as IQueryable<CompanyContract>;
                        //var compServicePack = dbContext.CompanyServicePack;
                        var conferenceContract = dbContext.ConferenceContract;
                        //搜索条件
                        var personContratcQueryable = dbContext.PersonContract as IQueryable<PersonContract>;
                        var memberQuerable = memberdbContext.Member as IQueryable<Member>;
                        var compServicePackQueryable = dbContext.CompanyServicePack as IQueryable<CompanyServicePack>;


                        var comContractId = request.Search.ComContractId.Trim();

                        if (!string.IsNullOrEmpty(comContractId) && comContractId != "string")
                        {
                            personContratcQueryable = personContratcQueryable.Where(x => x.ContractId.ToString() == comContractId);
                        }
                        List<newGetPersonContractListStruct> resultList = new List<newGetPersonContractListStruct>();


                        var listAll = (from per in personContratcQueryable
                                       join CSP in compServicePackQueryable
                                       on per.CompanyServicePackId equals CSP.CompanyServicePackId
                                       join com in companyContractQueryable
                                       on per.ContractId equals com.ContractId
                                       select new newGetPersonContractListStruct
                                       {
                                           ContractNumber = per.PerContractNumber ?? string.Empty,
                                           MemNameTranslation = per.MemTranslation ?? string.Empty,
                                           CompanyNameTranslation = com.ComNameTranslation ?? string.Empty,
                                           Position = "" ?? string.Empty,
                                           Email = "" ?? string.Empty,
                                           Mobile = "" ?? string.Empty,
                                           ComServicePackTranslation = CSP.Translation ?? string.Empty,
                                           Pay = per.PerPrice ?? string.Empty,
                                           Paid = per.PaidAmount ?? string.Empty,
                                           Owner = per.Ower ?? string.Empty,
                                           IsSendEmail = per.IsSendEmail ?? false,
                                           IsPrint = per.IsPrint ?? false,
                                           CreatedBy = per.CreatedBy ?? string.Empty,
                                           CreatedOn = per.CreatedOn.ToString() ?? string.Empty,
                                           ModifiyBy = per.ModefieldBy ?? string.Empty,
                                           ModifyOn = per.ModefieldOn.ToString() ?? string.Empty,
                                           MemberPK = per.MemberPK.ToString() ?? string.Empty
                                       });

                        var list = await listAll.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize).ToListAsync();

                        var memberPKList = list.Select(a => a.MemberPK).ToList();
                        var memberList = await memberdbContext.Member.Where(x => memberPKList.Contains(x.MemberPK.ObjToString())).ToListAsync();
                        foreach (var item in list)
                        {
                            item.Position = memberList.FirstOrDefault(x => x.MemberPK.ObjToString() == item.MemberPK.ObjToString()).MemPosition;
                            item.Email = memberList.FirstOrDefault(x => x.MemberPK.ObjToString() == item.MemberPK.ObjToString()).MemEmail;
                            item.Mobile = memberList.FirstOrDefault(x => x.MemberPK.ObjToString() == item.MemberPK.ObjToString()).MemMobile;
                        }
                        response.Total = listAll.Count();
                        response.Listdata.AddRange(list);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        public override async Task<modifyResponse> newModifyParticipantByAbstractParticipantPerContractNumber(PerContractNumberRequest request, ServerCallContext context)
        {
            try
            {
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    modifyResponse response = new modifyResponse();
                    //根据个人合同号找到abstractDraftId
                    var abstractParticipantId = dbContext.AbstractDraft.FirstOrDefault(x => x.perContractNumber == request.PerContractNumber).AbstractParticipantID;
                    var abstractParticipate = dbContext.AbstractParticipant;
                    var participate = dbContext.Participant;
                    if (string.IsNullOrEmpty(request.PerContractNumber.Trim()) || request.PerContractNumber.Trim() == "string")
                    {
                        response.Msg = "合同号为必传项";
                        response.IsSuccess = false;
                    }
                    else
                    {
                        if (abstractParticipantId == null)
                        {
                            response.Msg = "该参会人员未提交摘要";
                            response.IsSuccess = false;
                        }
                        else
                        {
                            //根据abstractParticipateId得到AbstractParticipate信息
                            var abstractParticipateInfo = abstractParticipate.FirstOrDefault(x => x.AbstractParticipantID == abstractParticipantId);
                            if (abstractParticipateInfo == null)
                            {
                                response.Msg = "没有演讲人信息";
                                response.IsSuccess = false;
                            }
                            else
                            {
                                var participateInfo = participate.FirstOrDefault(x => x.PerContractNumber == request.PerContractNumber);
                                if (participateInfo == null)
                                {
                                    response.Msg = "没有嘉宾信息";
                                    response.IsSuccess = false;
                                }
                                else
                                {
                                    participateInfo.ParticipantNameTranslation = abstractParticipateInfo.NameTranslation;
                                    participateInfo.IMGSRC = abstractParticipateInfo.IMGSRC;
                                    participateInfo.CompanyTranslation = abstractParticipateInfo.CompanyTranslation;
                                    participateInfo.JobTranslation = abstractParticipateInfo.JobTranslation;
                                    participateInfo.CountryTranslation = abstractParticipateInfo.CountryTranslantion;
                                    participateInfo.Email = abstractParticipateInfo.Email;
                                    participateInfo.Mobile = abstractParticipateInfo.Mobile;
                                    participateInfo.IntroduceTranslation = abstractParticipateInfo.IntroduceTranslation;
                                    participateInfo.ModifiedBy = abstractParticipateInfo.ModifiedBy;
                                    participateInfo.ModifiedOn = abstractParticipateInfo.ModifiedOn;
                                    participateInfo.AppellationTranslation = abstractParticipateInfo.JobTitleTranslation;
                                    var count = await dbContext.SaveChangesAsync();
                                    if (count > 0)
                                    {
                                        response.IsSuccess = true;
                                        response.Msg = "更新成功";
                                    }
                                }
                            }
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        //public override async Task<newGetReportNoticResponse> newGetPersonReportNotice(newGetReportNoticeRequest request, ServerCallContext context)
        //{
        //    try
        //    {
        //        using (var dbContext = new ConCDBContext(_options.Options))
        //        {
        //            using (var memberdbContext = new MemberDBContext(_member_options))
        //            {
        //                using (var roledbContext = new RoleDBContext(_role_options))
        //                {
        //                    newGetReportNoticResponse response = new newGetReportNoticResponse();
        //                    var dbMember = memberdbContext.Member;
        //                    var dbUser = roledbContext.User;
        //                    var dbPersonContract = dbContext.PersonContract;
        //                    var dbCompanyServicePack = dbContext.CompanyServicePack;
        //                    var dbCompanyContract = dbContext.CompanyContract;
        //                    var dbCompanyServicePackMap = dbContext.CompanyServicePackMap;
        //                    var dbServicePackActivityMap = dbContext.ServicePackActivityMap;
        //                    var dbServicePack = dbContext.ServicePack;
        //                    var dbConference = dbContext.Conference;
        //                    var dbTagType = dbContext.TagType;
        //                    var dbApplyConference = dbContext.ApplyConference;
        //                    var dbActivity = dbContext.Activity;
        //                    var dbTalk = dbContext.Talk;
        //                    var dbParticipate = dbContext.Participant;
        //                    var dbParticipateType = dbContext.ParticipantType;
        //                    var dbActivityParticipantMap = dbContext.ActivityParticipantMap;
        //                    var dbTalkParticipantMap = dbContext.TalkParticipantMap;

        //                    var perContractNumer = request.ContractNumber.Trim();
        //                    var conferenceId = request.ConferenceId.Trim();
        //                    var year = request.Year.Trim();
        //                    var memberPK = request.MemberPK.Trim();


        //                    #region 获得个人信息
        //                    var personContract = await dbPersonContract.FirstOrDefaultAsync(x => x.PerContractNumber == perContractNumer);

        //                    var memberEmail = (await dbMember.FirstOrDefaultAsync(x => x.MemberPK == personContract.MemberPK.ObjToGuid()))?.MemEmail;
        //                    var companyNameTranslation = (await dbCompanyContract.FirstOrDefaultAsync(n => n.ContractId == personContract.ContractId))?.ComNameTranslation;
        //                    var comServicePackTranslation = (await dbCompanyServicePack.FirstOrDefaultAsync(n => n.CompanyServicePackId == personContract.CompanyServicePackId))?.Translation;
        //                    var personInfo = new NewPersonContractInfoStruct
        //                    {
        //                        PersonContractId = personContract.PersonContractId.ToString(),
        //                        PerContractNumber = personContract.PerContractNumber,
        //                        MemberPK = personContract.MemberPK.ToString(),
        //                        MemTranslation = personContract.MemTranslation,
        //                        MemEmail = memberEmail,
        //                        CompanyNameTranslation = companyNameTranslation,
        //                        ComServicePackTranslation = comServicePackTranslation,
        //                        PerPrice = personContract.PerPrice
        //                    };
        //                    #endregion

        //                    #region 获得业务员信息
        //                    var owner = await dbUser.Where(x => x.UserPK.ToString() == personContract.Owerid)
        //                                           .Select(x => new OwnerInfo
        //                                           {
        //                                               Tel = x.UserTel,
        //                                               Name = string.Empty,
        //                                               Email = x.UserEmail,
        //                                               Address = string.Empty,
        //                                           }).FirstOrDefaultAsync();

        //                    var model = await dbUser.FirstOrDefaultAsync(x => x.UserPK.ToString() == personContract.Owerid);
        //                    TransFull nameTrans = new TransFull();
        //                    nameTrans.CN = model.UserRealNameCn;
        //                    nameTrans.EN = model.UserRealNameEn;
        //                    nameTrans.JP = model.UserRealNameJp;
        //                    TransFull addressTrans = new TransFull();
        //                    addressTrans.CN = model.UserAddresseCn;
        //                    addressTrans.EN = model.UserAddresseEn;
        //                    addressTrans.JP = model.UserAddresseJp;
        //                    owner.Name = JsonConvert.SerializeObject(nameTrans);
        //                    owner.Address = JsonConvert.SerializeObject(addressTrans);
        //                    #endregion

        //                    var tagTypeList = await dbTagType.ToListAsync();

        //                    //勾选的活动
        //                    var applyList = await dbApplyConference.Where(x => x.PersonContractId == personContract.PersonContractId.ToString()).ToListAsync();

        //                    var conferenceAllList = new List<NewCalendarStruct>();

        //                    var companyServicePackMap = await dbCompanyServicePackMap.FirstOrDefaultAsync(x => x.CompanyServicePackId == personContract.CompanyServicePackId);
        //                    if (companyServicePackMap != null)
        //                    {
        //                        var servicePack = dbServicePack.Where(x => x.ServicePackId == companyServicePackMap.ServicePackId);
        //                        var servicePackActivityMap = dbServicePackActivityMap.Where(x => x.ServicePackId == companyServicePackMap.ServicePackId);

        //                        //套餐包含的活动
        //                        var listInServicePack = await (from SP in servicePack
        //                                                       join SPAM in servicePackActivityMap
        //                                                       on SP.ServicePackId equals SPAM.ServicePackId
        //                                                       select new
        //                                                       {
        //                                                           ConferenceId = SPAM.SessionConferenceID,
        //                                                           SP.SessionConferenceName,
        //                                                           SP.PriceRMB,
        //                                                           SP.PriceUSD,
        //                                                           SP.ServicePackId,
        //                                                           SP.SessionAddress,
        //                                                           SP.SessionDate,
        //                                                           SP.SessionStartTime,
        //                                                           SP.ThirdSessionConferenceId,
        //                                                           SP.ThirdSessionConferenceName,
        //                                                           SP.Translation,
        //                                                           SP.Year,
        //                                                           SP.CreatedBy,
        //                                                           SP.CreatedOn,
        //                                                           SP.ModefieldBy,
        //                                                           SP.ModefieldOn,
        //                                                           SPAM.ActivityId,
        //                                                           SPAM.ActivityName
        //                                                       }).ToListAsync();

        //                        //所有会议活动
        //                        conferenceAllList = await dbConference.Where(x => x.Year == year && x.ParentID == conferenceId)
        //                             .Select(x => new NewCalendarStruct
        //                             {
        //                                 SessionConferenceId = x.ConferenceID.ToString(),
        //                                 SessionConferenceName = x.Translation ?? string.Empty,
        //                                 RemarkTranslation = string.Empty,
        //                                 TagTypeCodes = string.Empty,
        //                                 IsParticularConf = false,
        //                                 IsConfirm = false,
        //                                 TagTypeName = string.Empty,
        //                                 Date = string.Empty,
        //                                 Time = string.Empty,
        //                                 Address = string.Empty,
        //                                 //是否包含在套餐内（true-标黄底  false--白底）
        //                                 IsInsidePackage = false,
        //                                 //是否勾选（true--打勾日程 false--未打钩）
        //                                 IsChecked = false,


        //                             })
        //                             .ToListAsync();

        //                        foreach (var item in conferenceAllList)
        //                        {
        //                            var servicePackModel = listInServicePack.FirstOrDefault(x => x.ConferenceId == item.SessionConferenceId);
        //                            var applyModel = applyList.FirstOrDefault(x => x.SessionConferenceId == item.SessionConferenceId);

        //                            if (applyModel != null)
        //                            {

        //                                item.RemarkTranslation = applyModel.RemarkTranslation;
        //                                item.TagTypeCodes = applyModel.TagTypeCodes;
        //                                item.TagTypeName = JsonConvert.SerializeObject(GetTagTypeNamesByCodes(applyModel.TagTypeCodes, tagTypeList));
        //                                item.IsParticularConf = true;
        //                                item.IsChecked = true;
        //                            }
        //                            if (servicePackModel != null)
        //                            {
        //                                //说明包含在25条数据内
        //                                item.IsInsidePackage = true;
        //                            }


        //                            //根据conferenceId找到该大会下所有的Activity
        //                            var activityList = await dbActivity.Where(x => x.ConferenceID.ToString() == item.SessionConferenceId).ToListAsync();
        //                            //根据MemberPK找到participateId
        //                            var participate =await dbParticipate.Where(x => x.MemberPK==request.MemberPK).ToListAsync();
        //                            //根据activityId和ParticipateId得到该人
        //                            //根据ActivityId找到Activity下所有的Talk    


        //                            foreach (var activity in activityList)
        //                            {
        //                                var talk = dbTalk.Where(x=>x.ActivityID==activity.ActivityID);
        //                            }
        //                        }



        //                    }

        //                    response.Person = personInfo;
        //                    response.Calendar.AddRange(conferenceAllList);
        //                    response.Owner = owner;

        //                    return response;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(this, ex);
        //        throw ex;
        //    }
        //}

        public override async Task<newGetReportNoticResponse> newGetPersonReportNotice(newGetReportNoticeRequest request, ServerCallContext context)
        {
            newGetReportNoticResponse response = new newGetReportNoticResponse();
            response=await GetPersonNoticeReportAsync(request);
            return response;
        }
        public TransFull GetTagTypeNamesByCodes(string tagTypeCodes, List<TagType> list)
        {
            try
            {
                string nameCN = string.Empty;
                string nameEN = string.Empty;
                string nameJP = string.Empty;

                TransFull nameFull = new TransFull();
                if (!string.IsNullOrEmpty(tagTypeCodes))
                {
                    string[] codes = tagTypeCodes.Split(",");
                    foreach (var item in codes)
                    {
                        var model = list.FirstOrDefault(x => x.Code == item);
                        if (model != null)
                        {
                            var tagName = model.NameTranslation;
                            TransFull fullName = JsonConvert.DeserializeObject<TransFull>(tagName);
                            nameCN += fullName.CN + ",";
                            nameEN += fullName.EN + ",";
                            nameJP += fullName.JP + ",";
                        }
                    }
                    nameFull.CN = nameCN.TrimEnd(',');
                    nameFull.EN = nameEN.TrimEnd(',');
                    nameFull.JP = nameJP.TrimEnd(',');
                }
                return nameFull;

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        public override async Task<newGetComapnyServicePackTypeResponse> newGetComapnyServicePackType(newGetComapnyServicePackTypeRequest request, ServerCallContext context)
        {
            try
            {
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    newGetComapnyServicePackTypeResponse response = new newGetComapnyServicePackTypeResponse();
                    var dbCompanyServicePack = dbContext.CompanyServicePack.Where(x => x.ConferenceId == request.ConferenceId && x.Year == request.Year);
                    var dbPersonContract = dbContext.PersonContract.Where(x => x.MemberPK.ToString() == request.MemberPK);
                    var list = await (from person in dbPersonContract
                                      join com in dbCompanyServicePack
                                      on person.CompanyServicePackId equals com.CompanyServicePackId
                                      select new
                                      {
                                          com.IsSpeaker,
                                          person.MemTranslation,
                                          person.PerContractNumber,
                                          com.Translation
                                      }).FirstOrDefaultAsync();
                    response.IsSpeaker = list.IsSpeaker;
                    response.ComServicePackName = list.Translation;
                    response.Name = list.MemTranslation;
                    response.ContractNumer = list.PerContractNumber;
                    return response;

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }
    }


    public class Trans
    {
        public string CN { get; set; }

        public string EN { get; set; }

        public Trans()
        {
            this.CN = string.Empty;
            this.EN = string.Empty;
        }
    }

    public class TransFull
    {
        public string CN { get; set; }

        public string EN { get; set; }
        public string JP { get; set; }


        public TransFull()
        {
            this.CN = string.Empty;
            this.EN = string.Empty;
            this.JP = string.Empty;
        }
    }
}

