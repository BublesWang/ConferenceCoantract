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

        public Expression<Func<CompanyServicePack, bool>> new_GetServicePackExpression(new_GetServicePackRequest request)
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
            Expression<Func<CompanyServicePack, bool>> expression = x => x.Year == year && x.IsDelete == false && x.ConferenceId == conferenceId;
            if (!string.IsNullOrEmpty(contractTypeId))
            {
                expression = expression.And(x => x.ContractTypeId.ToString() == contractTypeId);
            }
            //if (!string.IsNullOrEmpty(isGive))
            //{
            //    expression = expression.And(x => x.IsGive == false);
            //}
            #endregion
            return expression;
        }


        /// <summary>
        /// 添加二级合同（付费、赠送）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<modifyResponse> new_AddServicePack(new_ServicePackStruct request, ServerCallContext context)
        {
            try
            {
                request.ContractId = Guid.NewGuid().ToString();
                request.ConferenceContractId = Guid.Empty.ToString();
                request.CreatedOn = DateTime.UtcNow.ToString();
                request.ModifiedOn = new DateTime().ToUniversalTime().ToString();

                var comContract = Mapper.Map<CompanyContract>(request);
                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    //1.根据公司Id、conferenceId和年份去公司一级合同中查询是否存在一级合同
                    //2.若已经存在，则返回一级合同的合同号
                    //2.1添加二级合同
                    //3.若不存在，添加一级合同，后将一级合同的合同号返回出来
                    //3.1添加二级合同
                    var conferenceContract = dbContext.ConferenceContract.FirstOrDefault(x => x.ConferenceId == request.ConferenceId && x.ContractYear == request.Year && x.CompanyId == request.CompanyId);
                    if (conferenceContract != null)
                    {
                        var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == conferenceContract.ConferenceContractId) + 1;
                        comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                        comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                    }
                    else
                    {
                        //添加一级合同
                        var config = dbContext.CCNumberConfig.FirstOrDefault(n => (n.ConferenceId == request.ConferenceId) && (n.Status == 1) && (n.IsDelete == false) && (n.Year == request.Year));
                        if (config != null)
                        {
                            ConferenceContract model = new ConferenceContract();
                            model.ConferenceContractId = Guid.NewGuid();
                            comContract.ConferenceContractId = model.ConferenceContractId;
                            var count = config.Count + 1;
                            //合同规则为：21SNECC0001CS
                            model.ContractNumber = config.Year.Substring(2) + config.CNano + request.ContractCode.Substring(0, 1) + count.ToString().PadLeft(4, '0') + request.ContractCode;
                            var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == model.ConferenceContractId) + 1;
                            comContract.ComContractNumber = model.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                            model.ConferenceId = comContract.ConferenceId;
                            model.ContractStatusCode = "W";
                            model.ModifyPermission = "0";
                            model.ContractCode = comContract.ContractCode;
                            model.ComNameTranslation = request.ComNameTranslation;
                            model.CompanyId = request.CompanyId;
                            model.ContractYear = request.Year;
                            model.CreatedBy = request.CreatedBy;
                            model.CreatedOn = comContract.CreatedOn;
                            model.IsDelete = false;
                            model.IsModify = false;
                            model.IsSendEmail = false;
                            model.ModifiedBy = request.ModifiedBy;
                            model.ModifiedOn = comContract.ModifiedOn;
                            model.Ower = request.Ower;
                            model.Owerid = request.Owerid;
                            model.PaymentStatusCode = "N";
                            await dbContext.ConferenceContract.AddAsync(model);
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Msg = $"CCNumberConfig配置表中不存在当前{request.Year}年份,ConferenceId为{request.ConferenceId}的配置项";
                            return response;
                        }
                    }
                    comContract.CCIsdelete = false;
                    comContract.IsVerify = false;
                    comContract.IsCheckIn = false;
                    await dbContext.CompanyContract.AddAsync(comContract);
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
        public override async Task<modifyResponse> new_AddServicePackDiscount(new_ServicePackStructDiscount request, ServerCallContext context)
        {
            try
            {
                request.Model.ContractId = Guid.NewGuid().ToString();
                request.Model.ConferenceContractId = Guid.Empty.ToString();
                request.Model.CreatedOn = DateTime.UtcNow.ToString();
                request.Model.ModifiedOn = new DateTime().ToUniversalTime().ToString();
                var comContract = Mapper.Map<CompanyContract>(request.Model);
                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {

                    //先判断是否已经存在该折扣合同了：根据公司Id、套餐Id、业务员Id、合同状态、套餐价格、年份这六个条件查询二级合同中是否存在数据
                    var conferenceContractDiscount = dbContext.CompanyContract.FirstOrDefault(x => x.CompanyId == Guid.Parse(request.Model.CompanyId) && x.CompanyServicePackId == Guid.Parse(request.Model.CompanyServicePackId) && x.Owerid == request.Model.Owerid && x.ContractStatusCode == request.Model.ContractStatusCode && x.ComPrice == request.Model.ComPrice);
                    if (conferenceContractDiscount != null)
                    {
                        //已经有了该折扣合同，只需要更新子合同数（原数量加上新增的数量）
                        conferenceContractDiscount.MaxContractNumber = conferenceContractDiscount.MaxContractNumber + comContract.MaxContractNumber;
                        dbContext.CompanyContract.Update(conferenceContractDiscount);
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
                        var conferenceContract = dbContext.ConferenceContract.FirstOrDefault(x => x.ConferenceId == comContract.ConferenceId && x.ContractYear == comContract.Year && x.CompanyId == comContract.CompanyId.ObjToString());
                        if (conferenceContract != null)
                        {
                            var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == conferenceContract.ConferenceContractId) + 1;
                            comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                            comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                        }
                        else
                        {
                            //添加一级合同
                            var config = dbContext.CCNumberConfig.AsNoTracking().FirstOrDefault(n => (n.ConferenceId == comContract.ConferenceId) && (n.Status == 1) && (n.IsDelete == false) && (n.Year == comContract.Year));
                            if (config != null)
                            {
                                ConferenceContract model = new ConferenceContract();
                                model.ConferenceContractId = Guid.NewGuid();
                                comContract.ConferenceContractId = model.ConferenceContractId;
                                var count = config.Count + 1;
                                //合同规则为：21SNECC0001CS
                                model.ContractNumber = config.Year.Substring(2) + config.CNano + request.Model.ContractCode.Substring(0, 1) + count.ToString().PadLeft(4, '0') + request.Model.ContractCode;
                                var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == model.ConferenceContractId) + 1;
                                comContract.ComContractNumber = model.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                                model.ConferenceId = comContract.ConferenceId;
                                model.ContractStatusCode = "W";
                                model.ModifyPermission = "0";
                                model.ContractCode = comContract.ContractCode;
                                model.ComNameTranslation = comContract.ComNameTranslation;
                                model.CompanyId = comContract.CompanyId.ObjToString();
                                model.ContractYear = comContract.Year;
                                model.CreatedBy = comContract.CreatedBy;
                                model.CreatedOn = comContract.CreatedOn;
                                model.IsDelete = false;
                                model.IsModify = false;
                                model.IsSendEmail = false;
                                model.ModifiedBy = comContract.ModifiedBy;
                                model.ModifiedOn = comContract.ModifiedOn;
                                model.Ower = comContract.Ower;
                                model.Owerid = comContract.Owerid;
                                model.PaymentStatusCode = "N";
                                await dbContext.ConferenceContract.AddAsync(model);
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Msg = $"CCNumberConfig配置表中不存在当前{comContract.Year}年份,ConferenceId为{comContract.ConferenceId}的配置项";
                                return response;
                            }
                        }
                        comContract.CCIsdelete = false;
                        comContract.IsVerify = false;
                        comContract.IsCheckIn = false;
                        comContract.ComPrice = request.Discount.ToString(); ;
                        await dbContext.CompanyContract.AddAsync(comContract);
                        await dbContext.SaveChangesAsync();
                        #region

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
                        #endregion
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

        private string PersonContractNumber(Guid? cid)
        {
            using (var dbContext = new ConCDBContext(_options.Options))
            {
                var perContractNmuber = string.Empty;
                try
                {
                    var companyContract = dbContext.CompanyContract.Include(n => n.personContract).FirstOrDefault(n => n.ContractId == cid);

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
        }


        public override async Task<modifyResponse> new_AddPersonContract(new_AddPersonContractRequest request, ServerCallContext context)
        {
            try
            {
                request.Person.PersonContractId = Guid.NewGuid().ToString();
                request.Person.ContractId = Guid.Empty.ToString();
                request.Person.CreatedOn = DateTime.UtcNow.ToString();
                request.Person.ModifiedOn = new DateTime().ToUniversalTime().ToString();

                var comContract = Mapper.Map<CompanyContract>(request);

                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {

                    //0先根据公司Id、套餐Id、业务员Id、合同状态、套餐价格、年份查看是否存在二级合同
                    //0.1若存在，则直接创建个人合同
                    //0.2若不存在，则创建二级合同后再创建三级合同
                    var companyContract = dbContext.CompanyContract.FirstOrDefault(x => x.CompanyId == Guid.Parse(request.CompanyId)
                                                                         && x.CompanyServicePackId == Guid.Parse(request.Person.CompanyServicePackId)
                                                                         && x.Owerid == request.Person.Owerid
                                                                         && x.ComPrice == request.ComPrice
                                                                         && x.Year == request.Person.Year
                                                                         && x.ContractStatusCode == request.ContractStatusCode);
                    if (companyContract != null)
                    {
                        //说明已经存在了二级合同，直接创建个人合同
                        PersonContract person = new PersonContract();
                        person.ConferenceId = request.Person.ConferenceId;
                        person.ContractId = Guid.Parse(request.Person.ContractId);
                        person.CreatedBy = request.Person.CreatedBy;
                        person.CreatedOn = DateTime.UtcNow;
                        person.ModefieldBy = request.Person.ModefieldBy;
                        person.OtherOwner = request.Person.OtherOwner;
                        person.OtherownerId = request.Person.OtherOwnerId;
                        person.Ower = request.Person.Ower;
                        person.Owerid = request.Person.Owerid;
                        person.PaidAmount = request.Person.PaidAmount;
                        person.PCIsdelete = false;
                        person.PerContractNumber = PersonContractNumber(Guid.Parse(request.Person.ContractId));
                        person.CTypeCode = request.Person.CTypeCode;
                        person.InviteCodeId = request.Person.InviteCodeId;
                        person.IsInviteCode = request.Person.IsInviteCode;
                        person.IsCheckIn = false;
                        person.IsCommitAbstract = request.Person.IsCommitAbstract;
                        person.IsFianceRecord = request.Person.IsFianceRecord;
                        person.IsModify = request.Person.IsModify;
                        person.IsPrint = request.Person.IsPrint;
                        person.IsSendEmail = request.Person.IsSendEmail;
                        person.MemberPK = Guid.Parse(request.Person.MemberPK);
                        person.MemTranslation = request.Person.MemTranslation;
                        person.ModefieldBy = request.Person.ModefieldBy;
                        person.ModefieldOn = new DateTime().ToUniversalTime();
                        person.PersonContractId = Guid.Parse(request.Person.PersonContractId);
                        person.Year = request.Person.Year;
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
                        var conferenceContract = dbContext.ConferenceContract.FirstOrDefault(x => x.ConferenceId == request.CompanyContractModel.ConferenceId && x.ContractYear == request.CompanyContractModel.Year && x.CompanyId == request.CompanyId);
                        if (conferenceContract != null)
                        {
                            var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == conferenceContract.ConferenceContractId) + 1;
                            comContract.ComContractNumber = conferenceContract.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                            comContract.ConferenceContractId = conferenceContract.ConferenceContractId;
                        }
                        else
                        {
                            //添加一级合同
                            var config = dbContext.CCNumberConfig.FirstOrDefault(n => (n.ConferenceId == request.Person.ConferenceId) && (n.Status == 1) && (n.IsDelete == false) && (n.Year == request.Person.Year));
                            if (config != null)
                            {
                                ConferenceContract model = new ConferenceContract();
                                model.ConferenceContractId = Guid.NewGuid();
                                comContract.ConferenceContractId = model.ConferenceContractId;
                                var count = config.Count + 1;
                                //合同规则为：21SNECC0001CS
                                model.ContractNumber = config.Year.Substring(2) + config.CNano + request.ContractCode.Substring(0, 1) + count.ToString().PadLeft(4, '0') + request.ContractCode;
                                var companyContractCount = dbContext.CompanyContract.Count(x => x.ConferenceContractId == model.ConferenceContractId) + 1;
                                comContract.ComContractNumber = model.ContractNumber + companyContractCount.ToString().PadLeft(2, '0');
                                model.ConferenceId = comContract.ConferenceId;
                                model.ContractStatusCode = "W";
                                model.ModifyPermission = "0";
                                model.ContractCode = comContract.ContractCode;
                                model.ComNameTranslation = request.CompanyContractModel.ComNameTranslation;
                                model.CompanyId = request.CompanyId;
                                model.ContractYear = request.Person.Year;
                                model.CreatedBy = request.Person.CreatedBy;
                                model.CreatedOn = comContract.CreatedOn;
                                model.IsDelete = false;
                                model.IsModify = false;
                                model.IsSendEmail = false;
                                model.ModifiedBy = request.CompanyContractModel.ModifiedBy;
                                model.ModifiedOn = comContract.ModifiedOn;
                                model.Ower = request.CompanyContractModel.Ower;
                                model.Owerid = request.CompanyContractModel.Owerid;
                                model.PaymentStatusCode = "N";
                                await dbContext.ConferenceContract.AddAsync(model);
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Msg = $"CCNumberConfig配置表中不存在当前{request.CompanyContractModel.Year}年份,ConferenceId为{request.CompanyContractModel.ConferenceId}的配置项";
                                return response;
                            }
                        }
                        comContract.CCIsdelete = false;
                        comContract.IsVerify = false;
                        comContract.IsCheckIn = false;
                        await dbContext.CompanyContract.AddAsync(comContract);
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

    }
}
