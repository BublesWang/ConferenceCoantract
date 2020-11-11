using AutoMapper;
using ConferenceContractAPI.CCDBContext;
using ConferenceContractAPI.Common;
using ConferenceContractAPI.DBModels;
using Grpc.Core;
using GrpcConferenceContractServiceNew;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static GrpcConferenceContractServiceNew.NewConferenceContractService;

namespace ConferenceContractAPI.ConferenceContractService
{
    public class ConferenceContractService : NewConferenceContractServiceBase
    {
        private string _sql = ContextConnect.ReadConnstrContent();
        private DbContextOptionsBuilder<ConCDBContext> _options;
        public ConferenceContractService()
        {
            try
            {
                _options = new DbContextOptionsBuilder<ConCDBContext>();
                _options.UseNpgsql(_sql);
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
        public override Task<new_GetServicePackResponse> new_GetServicePack(new_GetServicePackRequest request, ServerCallContext context)
        {
            try
            {
                new_GetServicePackResponse response = new new_GetServicePackResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var companyServicePackQueryanble = dbContext.CompanyServicePack as IQueryable<CompanyServicePack>;
                    companyServicePackQueryanble = GetServicePackIquerable(request, companyServicePackQueryanble);

                    var list = companyServicePackQueryanble.AsNoTracking().OrderBy(x => x.Sort)
                        .Select(x => new new_GetServicePackStruct
                        {
                            Code = x.Code ?? string.Empty,
                            CompanyServicePackId = x.CompanyServicePackId.ToString(),
                            IsGive = x.IsGive.ObjToBool(),
                            PriceJP = x.PriceJP ?? "0",
                            PriceRMB = x.PriceRMB ?? "0",
                            PriceUSD = x.PriceUSD ?? "0",
                            Translation = x.Translation ?? string.Empty,
                            Year = x.Year ?? string.Empty
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

        public IQueryable<CompanyServicePack> GetServicePackIquerable(new_GetServicePackRequest request, IQueryable<CompanyServicePack> companyServiceQueryable)
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
            if (!string.IsNullOrEmpty(contractTypeId))
            {
                companyServiceQueryable = companyServiceQueryable.Where(x => x.ContractTypeId.ToString() == contractTypeId);
            }
            if (!string.IsNullOrEmpty(isGive))
            {
                companyServiceQueryable = companyServiceQueryable.Where(x => x.IsGive == false);
            }
            #endregion
            return companyServiceQueryable;
        }

        ///// <summary>
        ///// 添加二级合同（付费、赠送）
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public override async Task<modifyResponse> new_AddServicePack(new_ServicePackStruct request, ServerCallContext context)
        //{
        //    try
        //    {
        //        request.ContractId = Guid.NewGuid().ToString();
        //        request.ConferenceContractId = Guid.Empty.ToString();
        //        request.CreatedOn = DateTime.UtcNow.ToString();
        //        request.ModifiedOn = new DateTime().ToUniversalTime().ToString();

        //        var comContract = Mapper.Map<CompanyContract>(request);
        //        modifyResponse response = new modifyResponse();
        //        using (var dbContext = new ConCDBContext(_options.Options))
        //        {
        //            //1.根据公司Id、conferenceId和年份去公司一级合同中查询是否存在一级合同
        //            //2.若已经存在，则返回一级合同的合同号
        //            //2.1添加二级合同
        //            //3.若不存在，添加一级合同，后将一级合同的合同号返回出来
        //            //3.1添加二级合同
        //            var conferenceContract = dbContext.ConferenceContract.FirstOrDefault(x => x.ConferenceId == request.ConferenceId && x.ContractYear == request.Year && x.CompanyId == request.CompanyId);
        //            if (conferenceContract != null)
        //            {
        //                var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == conferenceContract.ConferenceContractId) + 1;
        //                comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
        //                comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
        //            }
        //            else
        //            {
        //                //添加一级合同
        //                var config = dbContext.CCNumberConfig.FirstOrDefault(n => (n.ConferenceId == request.ConferenceId) && (n.Status == 1) && (n.IsDelete == false) && (n.Year == request.Year));
        //                if (config != null)
        //                {
        //                    ConferenceContract model = new ConferenceContract();
        //                    model.ConferenceContractId = Guid.NewGuid();
        //                    comContract.ConferenceContractId = model.ConferenceContractId;
        //                    var count = config.Count + 1;
        //                    //合同规则为：21SNECC0001CS
        //                    model.ContractNumber = config.Year.Substring(2) + config.CNano + request.ContractCode.Substring(0, 1) + count.ToString().PadLeft(4, '0') + request.ContractCode;
        //                    var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == model.ConferenceContractId) + 1;
        //                    comContract.ComContractNumber = model.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
        //                    model.ConferenceId = comContract.ConferenceId;
        //                    model.ContractStatusCode = "W";
        //                    model.ModifyPermission = "0";
        //                    model.ContractCode = comContract.ContractCode;
        //                    model.ComNameTranslation = request.ComNameTranslation;
        //                    model.CompanyId = request.CompanyId;
        //                    model.ContractYear = request.Year;
        //                    model.CreatedBy = request.CreatedBy;
        //                    model.CreatedOn = comContract.CreatedOn;
        //                    model.IsDelete = false;
        //                    model.IsModify = false;
        //                    model.IsSendEmail = false;
        //                    model.ModifiedBy = request.ModifiedBy;
        //                    model.ModifiedOn = comContract.ModifiedOn;
        //                    model.Ower = request.Ower;
        //                    model.Owerid = request.Owerid;
        //                    model.PaymentStatusCode = "N";
        //                    await dbContext.ConferenceContract.AddAsync(model);
        //                }
        //                else
        //                {
        //                    response.IsSuccess = false;
        //                    response.Msg = $"CCNumberConfig配置表中不存在当前{request.Year}年份,ConferenceId为{request.ConferenceId}的配置项";
        //                    return response;
        //                }
        //            }
        //            comContract.CCIsdelete = false;
        //            comContract.IsVerify = false;
        //            comContract.IsCheckIn = false;
        //            await dbContext.CompanyContract.AddAsync(comContract);
        //            var result = await dbContext.SaveChangesAsync();

        //            if (result > 0)
        //            {
        //                response.IsSuccess = true;
        //                response.Msg = "添加成功";
        //            }
        //            else
        //            {
        //                response.IsSuccess = false;
        //                response.Msg = "添加失败";
        //            }
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(this, ex);
        //        throw ex;
        //    }
        //}
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

        public override async Task<modifyResponse> new_AddServicePack(new_ServicePackStruct request, ServerCallContext context)
        {
            try
            {
                request = InitCompanyContract(request);
                var comContract = Mapper.Map<CompanyContract>(request);

                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;

                    var originalConferenceContract = db_CompanyContract.FirstOrDefault(x =>
                                                             x.CompanyId == comContract.CompanyId
                                                          && x.ConferenceId == comContract.ConferenceId
                                                          && x.CompanyServicePackId == comContract.CompanyServicePackId
                                                          && x.Owerid == comContract.Owerid
                                                          && x.ContractStatusCode == comContract.ContractStatusCode
                                                          && x.ComPrice == comContract.ComPrice
                                                          && x.Year == comContract.Year
                                                          && IsEqual(x.OtherOwnerId, comContract.OtherOwnerId));
                    if (originalConferenceContract != null)
                    {
                        //已经有了该折扣合同，只需要更新子合同数（原数量加上新增的数量）
                        originalConferenceContract.MaxContractNumber = originalConferenceContract.MaxContractNumber + comContract.MaxContractNumber;
                        dbContext.CompanyContract.Update(originalConferenceContract);
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
                    else
                    {
                        //1.根据公司Id、conferenceId和年份去公司一级合同中查询是否存在一级合同
                        //2.若已经存在，则返回一级合同的合同号
                        //2.1添加二级合同
                        //3.若不存在，添加一级合同，后将一级合同的合同号返回出来
                        //3.1添加二级合同
                        var companyContractCount = 1;
                        var conferenceContract = db_ConferenceContract.AsNoTracking().FirstOrDefault(x => x.ConferenceId == request.ConferenceId
                                                                                                 && x.ContractYear == request.Year
                                                                                                 && x.CompanyId == request.CompanyId);
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
                                return response;
                            }
                        }

                        comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                        comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                        await db_CompanyContract.AddAsync(comContract);
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
        public new_ServicePackStruct InitCompanyContract(new_ServicePackStruct request)
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

        public override async Task<modifyResponse> new_AddServicePackDiscount(new_ServicePackStructDiscount request, ServerCallContext context)
        {
            try
            {
                request.Model = InitCompanyContract(request.Model);
                var comContract = Mapper.Map<CompanyContract>(request.Model);

                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var db_ConferenceContract = dbContext.ConferenceContract;
                    var db_CompanyContract = dbContext.CompanyContract;
                    var db_CCNumberConfig = dbContext.CCNumberConfig;

                    var originalConferenceContract = db_CompanyContract.FirstOrDefault(x =>
                                                             x.CompanyId == comContract.CompanyId
                                                          && x.ConferenceId == comContract.ConferenceId
                                                          && x.CompanyServicePackId == comContract.CompanyServicePackId
                                                          && x.Owerid == comContract.Owerid
                                                          && x.ContractStatusCode == comContract.ContractStatusCode
                                                          && x.ComPrice == comContract.ComPrice
                                                          && x.Year == comContract.Year
                                                          && IsEqual(x.OtherOwnerId, comContract.OtherOwnerId));
                    if (originalConferenceContract != null)
                    {
                        //已经有了该合同，只需要更新子合同数（原数量加上新增的数量）
                        originalConferenceContract.MaxContractNumber = originalConferenceContract.MaxContractNumber + comContract.MaxContractNumber;
                        dbContext.CompanyContract.Update(originalConferenceContract);
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
                        await db_CompanyContract.AddAsync(comContract);
                        await dbContext.SaveChangesAsync();

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
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        public override async Task<modifyResponse> new_AddPersonContract(new_AddPersonContractRequest request, ServerCallContext context)
        {
            try
            {
                request.Person.PersonContractId = Guid.NewGuid().ToString();
                request.Person.ContractId = Guid.Empty.ToString();
                request.Person.CreatedOn = DateTime.UtcNow.ToString();
                request.Person.ModifiedOn = new DateTime().ToUniversalTime().ToString();

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
                    var companyContract = db_CompanyContract.AsNoTracking().Include(x=>x.personContract).FirstOrDefault(x => x.CompanyId == comContract.CompanyId
                                                                         && x.CompanyServicePackId == comContract.CompanyServicePackId
                                                                         && x.Owerid == comContract.Owerid
                                                                         && x.ComPrice == comContract.ComPrice
                                                                         && x.Year == comContract.Year
                                                                         && x.ContractStatusCode == comContract.ContractStatusCode);
                    if (companyContract != null)
                    {
                        //说明已经存在了二级合同，直接创建个人合同
                        PersonContract person = CreatePersonContract(request, companyContract);

                        var comContractNumber = companyContract.ComContractNumber;
                        var count = companyContract.personContract.Count() + 1;
                        person.PerContractNumber = comContractNumber + count.ToString().PadLeft(3, '0');

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

                        PersonContract person = CreatePersonContract(request, comContract);
                        var count = 1;
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
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 创建个人合同
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private PersonContract CreatePersonContract(new_AddPersonContractRequest request,CompanyContract companyContract)
        {
            //说明已经存在了二级合同，直接创建个人合同
            var pRequest = request.Person;
            PersonContract person = new PersonContract();

            person.ConferenceId = pRequest.ConferenceId;
            person.PerPrice = companyContract.ComPrice;
            person.CompanyServicePackId = companyContract.CompanyServicePackId;
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
            person.PersonContractId =Guid.NewGuid();
            person.Year = pRequest.Year;
            return person;
        }

    }
}
