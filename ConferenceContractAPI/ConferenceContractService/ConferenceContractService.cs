using AutoMapper;
using ConferenceContractAPI.CCDBContext;
using ConferenceContractAPI.Common;
using ConferenceContractAPI.DBModels;
using Grpc.Core;
using GrpcServer.Web.ConferenceContract.Protos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static GrpcServer.Web.ConferenceContract.Protos.NewConferenceContractService;

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
                LogHelper.Error(this, ex.Message);
                throw new Exception(ex.Message);
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

        

        public override Task<modifyResponse> new_AddServicePack(new_ServicePackStruct request, ServerCallContext context)
        {
            try
            {
                modifyResponse response = new modifyResponse();
                using (var dbContext = new ConCDBContext(_options.Options))
                {
                    var CompanyContractModel = Mapper.Map<CompanyContract>(request);
                    var ConferenceContract = Mapper.Map<ConferenceContract>(request);
                    //1.根据公司Id、conferenceId和年份去公司一级合同中查询是否存在一级合同
                    //2.若已经存在，则返回一级合同的合同号
                    //2.1添加二级合同
                    //3.若不存在，添加一级合同，后将一级合同的合同号返回出来
                    //3.1添加二级合同
                    var conferenceContract = dbContext.ConferenceContract.FirstOrDefault(x => x.ConferenceId == request.ConferenceId && x.ContractYear == request.ContractYear && x.CompanyId == request.CompanyId);
                    if (conferenceContract != null)
                    {
                        CompanyContractModel.ComContractNumber = conferenceContract.ContractNumber;
                        CompanyContractModel.CCIsdelete = false;
                        CompanyContractModel.CreatedOn = DateTime.UtcNow;
                        CompanyContractModel.ModefieldOn = DateTime.UtcNow;
                        dbContext.CompanyContract.Add(CompanyContractModel);
                    }
                    else
                    {
                        //添加一级合同
                        var config = dbContext.CCNumberConfig.FirstOrDefault(n => (n.ConferenceId == request.ConferenceId) && (n.Status == 1) && (n.IsDelete == false) && (n.Year == request.ContractYear));
                        if (config != null)
                        {
                            var count = config.Count + 1;
                            //合同规则为：21SNECC0001CS
                            ConferenceContract.ContractNumber = config.Year.Substring(2) + config.CNano + request.ContractCode.Substring(0, 1) + count.ToString().PadLeft(4, '0') + request.ContractCode;
                        }
                    }

                    //dbContext.Add(request);
                    var result = dbContext.SaveChanges();
                    modifyResponse modify = new modifyResponse();
                    if (result > 0)
                    {
                        modify.IsSuccess = true;
                        modify.Msg = "添加成功";
                    }
                    else
                    {
                        modify.IsSuccess = false;
                        modify.Msg = "添加失败";
                    }
                    return Task.FromResult(modify);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
