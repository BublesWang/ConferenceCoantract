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
            _options = new DbContextOptionsBuilder<ConCDBContext>();
            _options.UseNpgsql(_sql);
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
                var expression = new_GetServicePackExpression(request);
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var list = dbContext.CompanyServicePack.AsNoTracking()
                        .Where(expression).OrderBy(x => x.Sort)
                        .Select(x => new new_GetServicePackStruct
                        {
                            Code = x.Code ?? string.Empty,
                            CompanyServicePackId = x.CompanyServicePackId.ToString(),
                            IsGive = x.IsGive,
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
            bool? isGive = request?.IsGive;
            #region 拼接条件
            Expression<Func<CompanyServicePack, bool>> expression = x => x.Year == year && x.IsDelete == false && x.ConferenceId == conferenceId;
            if (!string.IsNullOrEmpty(contractTypeId))
            {
                expression = expression.And(x => x.ContractTypeId.ToString() == contractTypeId);
            }
            if (isGive != null)
            {
                expression = expression.And(x => x.IsGive == isGive);
            }
            #endregion
            return expression;
        }



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
                        comContract.ComContractNumber= conferenceContract.ContractNumber+ companyContractCount.ToString().PadLeft(2, '0');
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
                            comContract.ComContractNumber = model.ContractNumber+ companyContractCount.ToString().PadLeft(2, '0');
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

        //public override async Task<modifyResponse> new_AddServicePackDiscount(new_ServicePackStructDiscount request, ServerCallContext context)
        //{
        //    //return base.new_AddServicePackDiscount(request, context);
        //}
    }
}
