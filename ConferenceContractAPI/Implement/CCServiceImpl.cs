using AutoMapper;
using ConferenceContractAPI.CCDBContext;
using ConferenceContractAPI.Common;
using ConferenceContractAPI.ConferenceContractService;
using ConferenceContractAPI.DBModels;
using ConferenceContractAPI.ViewModel;
using Grpc.Core;
using GrpcConferenceContractService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.Implement
{
    public class CCServiceImpl : ConferenceContractServiceToGrpc.ConferenceContractServiceToGrpcBase
    {
        private string _sql = ContextConnect.ReadConnstrContent();
        private CCService _service;


        //public CCServiceImpl()
        //{
        //    var optionsBulider = new DbContextOptionsBuilder<ConCDBContext>();
        //    optionsBulider.UseNpgsql(_sql);

        //    _context = new ConCDBContext(optionsBulider.Options);

        //    _service = new CCService(_context);

        //}


        public CCServiceImpl()
        {
            var optionsBulider = new DbContextOptionsBuilder<ConCDBContext>();
            _service = new CCService(optionsBulider, _sql);

        }

        #region ConferenceContract表操作

        public override async Task<ConferenceContractList> GetConferenceContractList(PaginationRequestSearch request, ServerCallContext context)
        {
            ConferenceContractList CList = new ConferenceContractList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetConferenceContractList(request.Offset, request.Limit, search);

                var ccList = Mapper.Map<List<ConferenceContract>, List<ConferenceContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var dsList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(item2.companyContract.ToList());
                        dsList = dsList.Where(n => n.CCIsdelete == false).ToList();
                        var dsList2 = Mapper.Map<List<DelegateServicePackDiscountForConferenceContract>, List<DelegateServicePackDiscountForConferenceContractStruct>>(item2.delegateServicePackDiscountForConferenceContract.ToList());
                        if (item2.ConferenceContractId.ToString() == item.ConferenceContractId)
                        {
                            item.Clistdata.AddRange(dsList);
                            item.Dlistdata.AddRange(dsList2);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
                CList.Total = await _service.GetConferenceContractListCount(search);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ConferenceContractList> GetConferenceContractListByIsGive(PaginationRequestSearch request, ServerCallContext context)
        {
            ConferenceContractList CList = new ConferenceContractList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetConferenceContractListByIsGive(request.Offset, request.Limit, search);

                var ccList = Mapper.Map<List<ConferenceContract>, List<ConferenceContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var dsList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(item2.companyContract.ToList());
                        dsList = dsList.Where(n => n.CCIsdelete == false).ToList();
                        var dsList2 = Mapper.Map<List<DelegateServicePackDiscountForConferenceContract>, List<DelegateServicePackDiscountForConferenceContractStruct>>(item2.delegateServicePackDiscountForConferenceContract.ToList());
                        if (item2.ConferenceContractId.ToString() == item.ConferenceContractId)
                        {
                            item.Clistdata.AddRange(dsList);
                            item.Dlistdata.AddRange(dsList2);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
                CList.Total = await _service.GetConferenceContractListByIsGiveCount(search);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ConferenceContractList> GetConferenceContractListByIsGiveWithAllContractStatusCode(PaginationRequestSearch request, ServerCallContext context)
        {
            ConferenceContractList CList = new ConferenceContractList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetConferenceContractListByIsGiveWithAllContractStatusCode(request.Offset, request.Limit, search);

                var ccList = Mapper.Map<List<ConferenceContract>, List<ConferenceContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var dsList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(item2.companyContract.ToList());
                        dsList = dsList.Where(n => n.CCIsdelete == false).ToList();
                        var dsList2 = Mapper.Map<List<DelegateServicePackDiscountForConferenceContract>, List<DelegateServicePackDiscountForConferenceContractStruct>>(item2.delegateServicePackDiscountForConferenceContract.ToList());
                        if (item2.ConferenceContractId.ToString() == item.ConferenceContractId)
                        {
                            item.Clistdata.AddRange(dsList);
                            item.Dlistdata.AddRange(dsList2);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
                CList.Total = await _service.GetConferenceContractListByIsGiveWithAllContractStatusCodeCount(search);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }
        public override async Task<ConferenceContractList> GetConferenceContractByCompanyIdList(IdRequest request, ServerCallContext context)
        {
            ConferenceContractList CList = new ConferenceContractList();
            try
            {
                var list = await _service.GetConferenceContractByCompanyIdList(request.Id);

                var ccList = Mapper.Map<List<ConferenceContract>, List<ConferenceContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var dsList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(item2.companyContract.ToList());
                        dsList = dsList.Where(n => n.CCIsdelete == false).ToList();
                        var dsList2 = Mapper.Map<List<DelegateServicePackDiscountForConferenceContract>, List<DelegateServicePackDiscountForConferenceContractStruct>>(item2.delegateServicePackDiscountForConferenceContract.ToList());
                        if (item2.ConferenceContractId.ToString() == item.ConferenceContractId)
                        {
                            item.Clistdata.AddRange(dsList);
                            item.Dlistdata.AddRange(dsList2);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ConferenceContractList> GetConferenceContractByCompanyIdAndYearList(SearchStruct request, ServerCallContext context)
        {
            ConferenceContractList CList = new ConferenceContractList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var list = await _service.GetConferenceContractByCompanyIdAndYearList(search);

                var ccList = Mapper.Map<List<ConferenceContract>, List<ConferenceContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var dsList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(item2.companyContract.ToList());
                        dsList = dsList.Where(n => n.CCIsdelete == false).ToList();
                        var dsList2 = Mapper.Map<List<DelegateServicePackDiscountForConferenceContract>, List<DelegateServicePackDiscountForConferenceContractStruct>>(item2.delegateServicePackDiscountForConferenceContract.ToList());
                        if (item2.ConferenceContractId.ToString() == item.ConferenceContractId)
                        {
                            item.Clistdata.AddRange(dsList);
                            item.Dlistdata.AddRange(dsList2);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ConferenceContractStruct> GetConferenceContractById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetConferenceContractById(request.Id);
            var dsList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(item.companyContract.ToList());
            dsList = dsList.Where(n => n.CCIsdelete == false).ToList();
            var dsList2 = Mapper.Map<List<DelegateServicePackDiscountForConferenceContract>, List<DelegateServicePackDiscountForConferenceContractStruct>>(item.delegateServicePackDiscountForConferenceContract.ToList());
            var Struct = Mapper.Map<ConferenceContract, ConferenceContractStruct>(item);
            Struct.Clistdata.AddRange(dsList);
            Struct.Dlistdata.AddRange(dsList2);
            return Struct;
        }

        public override async Task<ModifyReplyForCreateOther> CreateConferenceContractInfo(ConferenceContractStruct request, ServerCallContext context)
        {
            List<DelegateServicePackDiscountForConferenceContract> delegateServicePackDiscountsforconferenceontract_list = new List<DelegateServicePackDiscountForConferenceContract>();
            request.ConferenceContractId = Guid.NewGuid().ToString();
            request.ModifyPermission = "0";
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            //如果在生成合同时，有折扣信息，把折扣信息存入delegateServicePackDiscounts_list内
            var dlistdata = request.Dlistdata;
            if (dlistdata.Count > 0)
            {
                foreach (var item in dlistdata)
                {
                    DelegateServicePackDiscountForConferenceContract dmodel = new DelegateServicePackDiscountForConferenceContract();
                    dmodel.DiscountId = Guid.NewGuid();
                    dmodel.ConferenceContractId = new Guid(request.ConferenceContractId);
                    dmodel.PriceAfterDiscountUSD = item.PriceAfterDiscountUSD;
                    dmodel.PriceAfterDiscountRMB = item.PriceAfterDiscountRMB;
                    dmodel.CreatedOn = DateTime.UtcNow.ToUniversalTime();
                    dmodel.CreatedBy = item.CreatedBy;
                    dmodel.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                    dmodel.ModefieldBy = item.ModefieldBy;
                    delegateServicePackDiscountsforconferenceontract_list.Add(dmodel);
                }
            }

            var model = Mapper.Map<ConferenceContractStruct, ConferenceContract>(request);

            var result = await _service.CreateConferenceContractInfo(model, delegateServicePackDiscountsforconferenceontract_list);
            var modifyReply = Mapper.Map<ModifyReplyForCreateOtherVM, ModifyReplyForCreateOther>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateConferenceContractInfo(ConferenceContractStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<ConferenceContractStruct, ConferenceContract>(request);
            var result = await _service.UpdateConferenceContractInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyConferenceContractPaymentStatusCode(ConferenceContractStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<ConferenceContractStruct, ConferenceContract>(request);
            var result = await _service.ModifyConferenceContractPaymentStatusCode(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyConferenceContractIsSendEmail(ConferenceContractCidList request, ServerCallContext context)
        {
            List<string> idlist = new List<string>();
            idlist.AddRange(request.Cidlistdata);

            var result = await _service.ModifyConferenceContractIsSendEmail(idlist);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyConferenceContractByOwer(ConferenceContractStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<ConferenceContractStruct, ConferenceContract>(request);
            var result = await _service.ModifyConferenceContractByOwer(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyModifyPermissionById(SearchStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<SearchStruct, SearchInfo>(request);
            var result = await _service.ModifyModifyPermissionById(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteConferenceContractById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteConferenceContractById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteConferenceContractByList(ConferenceContractCidList request, ServerCallContext context)
        {
            List<string> idlist = new List<string>();
            idlist.AddRange(request.Cidlistdata);

            var result = await _service.DeleteConferenceContractByList(idlist);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteConCAndCCAndPCByConIdList(ConferenceContractCidList request, ServerCallContext context)
        {
            List<string> idlist = new List<string>();
            idlist.AddRange(request.Cidlistdata);

            var result = await _service.DeleteConCAndCCAndPCByConIdList(idlist);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region CompanyContract表操作

        public override async Task<CompanyContractList> GetCompanyContractList(PaginationRequestSearch request, ServerCallContext context)
        {
            CompanyContractList CList = new CompanyContractList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetCompanyContractList(request.Offset, request.Limit, search);

                var ccList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var dsList = Mapper.Map<List<DelegateServicePackDiscount>, List<DelegateServicePackDiscountStruct>>(item2.delegateServicePackDiscount.ToList());
                        if (item2.ContractId.ToString() == item.ContractId)
                        {
                            item.Dlistdata.AddRange(dsList);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
                CList.Total = await _service.GetCompanyContractListCount(search);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<CompanyContractList> GetCompanyContractByCompanyIdList(IdRequest request, ServerCallContext context)
        {
            CompanyContractList CList = new CompanyContractList();
            try
            {
                var list = await _service.GetCompanyContractByCompanyIdList(request.Id);

                var ccList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var perList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(item2.personContract.ToList());
                        perList = perList.Where(n => n.PCIsdelete == false).ToList();
                        var dsList = Mapper.Map<List<DelegateServicePackDiscount>, List<DelegateServicePackDiscountStruct>>(item2.delegateServicePackDiscount.ToList());
                        if (item2.ContractId.ToString() == item.ContractId)
                        {
                            item.Plistdata.AddRange(perList);
                            item.Dlistdata.AddRange(dsList);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<CompanyContractList> GetCompanyContractByConferenceContractIdList(IdRequest request, ServerCallContext context)
        {
            CompanyContractList CList = new CompanyContractList();
            try
            {
                var list = await _service.GetCompanyContractByConferenceContractIdList(request.Id);

                var ccList = Mapper.Map<List<CompanyContract>, List<CompanyContractStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        var perList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(item2.personContract.ToList());
                        perList = perList.Where(n => n.PCIsdelete == false).ToList();
                        var dsList = Mapper.Map<List<DelegateServicePackDiscount>, List<DelegateServicePackDiscountStruct>>(item2.delegateServicePackDiscount.ToList());
                        if (item2.ContractId.ToString() == item.ContractId)
                        {
                            item.Plistdata.AddRange(perList);
                            item.Dlistdata.AddRange(dsList);
                        }
                    }
                }

                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<CompanyContractStruct> GetCompanyContractById(IdRequest request, ServerCallContext context)
        {
            CompanyContractStruct Struct = new CompanyContractStruct();
            try
            {
                var item = await _service.GetCompanyContractById(request.Id);
                var perList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(item.personContract.Where(n => n.PCIsdelete == false).ToList());
                var dsList = Mapper.Map<List<DelegateServicePackDiscount>, List<DelegateServicePackDiscountStruct>>(item.delegateServicePackDiscount.ToList());
                Struct = Mapper.Map<CompanyContract, CompanyContractStruct>(item);
                Struct.Dlistdata.AddRange(dsList);
                Struct.Plistdata.AddRange(perList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }
            return Struct;
        }

        public override async Task<ModifyReplyForCreateOther> CreateCompanyContractInfo(CompanyContractStruct request, ServerCallContext context)
        {
            List<DelegateServicePackDiscount> delegateServicePackDiscounts_list = new List<DelegateServicePackDiscount>();
            var modifyReply = new ModifyReplyForCreateOther();
            try
            {
                request.ContractId = Guid.NewGuid().ToString();
                request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

                //如果在生成合同时，有折扣信息，把折扣信息存入delegateServicePackDiscounts_list内
                var dlistdata = request.Dlistdata;
                if (dlistdata.Count > 0)
                {
                    foreach (var item in dlistdata)
                    {
                        DelegateServicePackDiscount dmodel = new DelegateServicePackDiscount();
                        dmodel.DiscountId = Guid.NewGuid();
                        dmodel.ContractId = new Guid(request.ContractId);
                        dmodel.PriceAfterDiscountUSD = item.PriceAfterDiscountUSD;
                        dmodel.PriceAfterDiscountRMB = item.PriceAfterDiscountRMB;
                        dmodel.Year = item.Year;
                        dmodel.CreatedOn = DateTime.UtcNow.ToUniversalTime();
                        dmodel.CreatedBy = item.CreatedBy;
                        dmodel.ModefieldOn = DateTime.UtcNow.ToUniversalTime();
                        dmodel.ModefieldBy = item.ModefieldBy;
                        delegateServicePackDiscounts_list.Add(dmodel);
                    }
                }

                var model = Mapper.Map<CompanyContractStruct, CompanyContract>(request);

                var result = await _service.CreateCompanyContractInfo(model, delegateServicePackDiscounts_list);
                modifyReply = Mapper.Map<ModifyReplyForCreateOtherVM, ModifyReplyForCreateOther>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateCompanyContractInfo(CompanyContractStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<CompanyContractStruct, CompanyContract>(request);
            var result = await _service.UpdateCompanyContractInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyMaxContractNumber(CompanyContractStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<CompanyContractStruct, CompanyContract>(request);
            var result = await _service.ModifyMaxContractNumber(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyCCPCOwer(ModifyCCPCOwerInfoStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<ModifyCCPCOwerInfoStruct, ModifyCCPCOwerInfo>(request);
            var result = await _service.ModifyCCPCOwer(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteCompanyContractById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteCompanyContractById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReplyForCreateOther> RemoveCompanyContractIfPersonContractEmpty(IdRequest request, ServerCallContext context)
        {
            var result = await _service.RemoveCompanyContractIfPersonContractEmpty(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyForCreateOtherVM, ModifyReplyForCreateOther>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteCompanyContractByList(CompanyContractCidList request, ServerCallContext context)
        {
            List<string> idlist = new List<string>();
            idlist.AddRange(request.Cidlistdata);

            var result = await _service.DeleteCompanyContractByList(idlist);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteCCAndPCByCidList(CompanyContractCidList request, ServerCallContext context)
        {
            List<string> idlist = new List<string>();
            idlist.AddRange(request.Cidlistdata);

            var result = await _service.DeleteCCAndPCByCidList(idlist);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region CCNumberConfig表操作

        public override async Task<CCNumberConfigList> GetCCNumberConfigDic(Empty request, ServerCallContext context)
        {
            var list = await _service.GetCCNumberConfigDic();
            CCNumberConfigList CCList = new CCNumberConfigList();

            var structList = Mapper.Map<List<CCNumberConfig>, List<CCNumberConfigStruct>>(list);
            CCList.Listdata.AddRange(structList);

            return CCList;

        }

        public override async Task<CCNumberConfigList> GetCCNumberConfigList(PaginationRequest request, ServerCallContext context)
        {
            CCNumberConfigList CCList = new CCNumberConfigList();

            var list = await _service.GetCCNumberConfigList(request.Offset, request.Limit);
            var structList = Mapper.Map<List<CCNumberConfig>, List<CCNumberConfigStruct>>(list);
            CCList.Listdata.AddRange(structList);
            CCList.Total = await _service.GetCCNumberConfigListCount();

            return CCList;
        }

        public override async Task<CCNumberConfigStruct> GetCCNumberConfigById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetCCNumberConfigById(request.Id);
            var Struct = Mapper.Map<CCNumberConfig, CCNumberConfigStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateCCNumberConfigInfo(CCNumberConfigStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();

            var model = Mapper.Map<CCNumberConfigStruct, CCNumberConfig>(request);

            var result = await _service.CreateCCNumberConfigInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateCCNumberConfigInfo(CCNumberConfigStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<CCNumberConfigStruct, CCNumberConfig>(request);
            var result = await _service.UpdateCCNumberConfigInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteCCNumberConfigById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteCCNumberConfigById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }


        #endregion

        #region ContractType表操作

        public override async Task<ContractTypeList> GetContractTypeDic(Empty request, ServerCallContext context)
        {
            var list = await _service.GetContractTypeDic();
            ContractTypeList diclist = new ContractTypeList();

            var mapperlist = Mapper.Map<List<ContractType>, List<ContractTypeStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            diclist.Total = await _service.GetContractTypeListCount();
            return diclist;

        }
        public override async Task<ContractTypeList> GetContractTypeList(PaginationRequest request, ServerCallContext context)
        {
            var list = await _service.GetContractTypeList(request.Offset, request.Limit);
            ContractTypeList diclist = new ContractTypeList();

            var mapperlist = Mapper.Map<List<ContractType>, List<ContractTypeStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            diclist.Total = await _service.GetContractTypeListCount();
            return diclist;
        }

        public override async Task<ContractTypeStruct> GetContractTypeById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetContractTypeById(request.Id);
            var Struct = Mapper.Map<ContractType, ContractTypeStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateContractTypeInfo(ContractTypeStruct request, ServerCallContext context)
        {
            request.ContractTypeId = Guid.NewGuid().ToString();
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<ContractTypeStruct, ContractType>(request);

            var result = await _service.CreateContractTypeInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateContractTypeInfo(ContractTypeStruct request, ServerCallContext context)
        {
            request.ModefieldOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var Struct = Mapper.Map<ContractTypeStruct, ContractType>(request);
            var result = await _service.UpdateContractTypeInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteContractTypeById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteContractTypeById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region CompanyServicePack表操作

        public override async Task<CompanyServicePackList> GetCompanyServicePackDic(Empty request, ServerCallContext context)
        {
            var list = await _service.GetCompanyServicePackDic();
            CompanyServicePackList CList = new CompanyServicePackList();

            var ccList = Mapper.Map<List<CompanyServicePack>, List<CompanyServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);
            CList.Total = await _service.GetServicePackListCount(null);

            return CList;

        }

        public override async Task<CompanyServicePackList> GetCompanyServicePackDicByYear(SearchStruct request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var list = await _service.GetCompanyServicePackDicByYear(search);
            CompanyServicePackList CList = new CompanyServicePackList();

            var ccList = Mapper.Map<List<CompanyServicePack>, List<CompanyServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);
            CList.Total = await _service.GetServicePackListCount(null);

            return CList;

        }

        public override async Task<CompanyServicePackList> GetCompanyServicePackListByIsShownOnFront(SearchStruct request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var list = await _service.GetCompanyServicePackListByIsShownOnFront(search);
            CompanyServicePackList CList = new CompanyServicePackList();

            var ccList = Mapper.Map<List<CompanyServicePack>, List<CompanyServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);

            return CList;

        }

        public override async Task<CompanyServicePackList> GetCompanyServicePackList(PaginationRequestSearch request, ServerCallContext context)
        {
            CompanyServicePackList CList = new CompanyServicePackList();

            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetCompanyServicePackList(request.Offset, request.Limit, search);

                var ccList = Mapper.Map<List<CompanyServicePack>, List<CompanyServicePackStruct>>(list);
                CList.Listdata.AddRange(ccList);
                CList.Total = await _service.GetCompanyServicePackListCount(search);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

            return CList;
        }

        public override async Task<CompanyServicePackList> GetCompanyServicePackListByContractTypeId(IdRequest request, ServerCallContext context)
        {
            CompanyServicePackList CList = new CompanyServicePackList();
            var list = await _service.GetCompanyServicePackListByContractTypeId(request.Id);

            var ccList = Mapper.Map<List<CompanyServicePack>, List<CompanyServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);

            return CList;
        }

        public override async Task<CompanyServicePackList> GetCompanyServicePackListForLunchOrDinner(SearchStruct request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var list = await _service.GetCompanyServicePackListForLunchOrDinner(search);
            CompanyServicePackList CList = new CompanyServicePackList();

            var ccList = Mapper.Map<List<CompanyServicePack>, List<CompanyServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);

            return CList;

        }

        public override async Task<CompanyServicePackVMStruct> GetCompanyServicePackById(IdRequest request, ServerCallContext context)
        {
            CompanyServicePackVMStruct cspVMStrcut = new CompanyServicePackVMStruct();
            List<ServicePackStruct> spStrcutList = new List<ServicePackStruct>();

            try
            {
                var model = await _service.GetCompanyServicePackById(request.Id);

                spStrcutList = Mapper.Map<List<ServicePack>, List<ServicePackStruct>>(model.ServicePackList);

                cspVMStrcut = model.CompanyServicePack.companyServicePackMap.Count == 0
                    ? cspVMStrcut : Mapper.Map<CompanyServicePackVM, CompanyServicePackVMStruct>(model);
                cspVMStrcut.ServicepackListdata.AddRange(spStrcutList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return cspVMStrcut;
        }

        public override async Task<ModifyReply> CreateCompanyServicePackInfo(CompanyServicePackVMStruct request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            List<ServicePack> csplist = new List<ServicePack>();
            try
            {
                CompanyServicePackVM CspVm = new CompanyServicePackVM();
                CspVm.CompanyServicePack = new CompanyServicePack();
                CspVm.ServicePackList = new List<ServicePack>();

                request.CompanyServicePack.CompanyServicePackId = Guid.NewGuid().ToString();
                request.CompanyServicePack.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                CspVm.CompanyServicePack = Mapper.Map<CompanyServicePackStruct, CompanyServicePack>(request.CompanyServicePack);

                foreach (var item in request.ServicepackListdata)
                {
                    var servicepack = Mapper.Map<ServicePackStruct, ServicePack>(item);
                    csplist.Add(servicepack);
                }

                CspVm.ServicePackList.AddRange(csplist);

                var result = await _service.CreateCompanyServicePackInfo(CspVm);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateCompanyServicePackInfo(CompanyServicePackVMStruct request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            List<ServicePack> csplist = new List<ServicePack>();
            try
            {
                CompanyServicePackVM CspVm = new CompanyServicePackVM();
                CspVm.CompanyServicePack = new CompanyServicePack();
                CspVm.ServicePackList = new List<ServicePack>();

                CspVm.CompanyServicePack = Mapper.Map<CompanyServicePackStruct, CompanyServicePack>(request.CompanyServicePack);

                foreach (var item in request.ServicepackListdata)
                {
                    var servicepack = Mapper.Map<ServicePackStruct, ServicePack>(item);
                    csplist.Add(servicepack);
                }

                CspVm.ServicePackList.AddRange(csplist);

                var result = await _service.UpdateCompanyServicePackInfo(CspVm);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteCompanyServicePackById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteCompanyServicePackById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<CompanyServicePackVMStruct> GetCompanyServicePackVMByPersonContractNumber(SearchStruct request, ServerCallContext context)
        {
            CompanyServicePackVMStruct cspVMStrcut = new CompanyServicePackVMStruct();
            List<ServicePackStruct> spStrcutList = new List<ServicePackStruct>();

            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var model = await _service.GetCompanyServicePackVMByPersonContractNumber(search);

                spStrcutList = Mapper.Map<List<ServicePack>, List<ServicePackStruct>>(model.ServicePackList);

                cspVMStrcut = model.CompanyServicePack.companyServicePackMap.Count == 0
                    ? cspVMStrcut : Mapper.Map<CompanyServicePackVM, CompanyServicePackVMStruct>(model);
                cspVMStrcut.ServicepackListdata.AddRange(spStrcutList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return cspVMStrcut;
        }

        #endregion

        #region ServicePack表操作

        public override async Task<ServicePackList> GetServicePackListAll(Empty request, ServerCallContext context)
        {
            var list = await _service.GetServicePackListAll();
            ServicePackList CList = new ServicePackList();

            var ccList = Mapper.Map<List<ServicePack>, List<ServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);
            CList.Total = await _service.GetServicePackListCount(null);

            return CList;

        }

        public override async Task<ServicePackList> GetServicePackByConferenceIdList(IdRequest request, ServerCallContext context)
        {
            ServicePackList CList = new ServicePackList();
            var list = await _service.GetServicePackByConferenceIdList(request.Id);

            var ccList = Mapper.Map<List<ServicePack>, List<ServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);

            return CList;
        }

        public override async Task<ServicePackList> GetServicePackList(PaginationRequestSearch request, ServerCallContext context)
        {
            ServicePackList CList = new ServicePackList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetServicePackList(request.Offset, request.Limit, search);

            var ccList = Mapper.Map<List<ServicePack>, List<ServicePackStruct>>(list);
            CList.Listdata.AddRange(ccList);
            CList.Total = await _service.GetServicePackListCount(search);

            return CList;
        }

        public override async Task<ServicePackVMStruct> GetServicePackById(IdRequest request, ServerCallContext context)
        {
            ServicePackVMStruct cspVMStrcut = new ServicePackVMStruct();
            List<ActivityStruct> acStrcutList = new List<ActivityStruct>();

            try
            {
                var model = await _service.GetServicePackById(request.Id);

                acStrcutList = Mapper.Map<List<ActivityVM>, List<ActivityStruct>>(model.ActivityList);

                cspVMStrcut = Mapper.Map<ServicePackVM, ServicePackVMStruct>(model);
                cspVMStrcut.ActivityListdata.AddRange(acStrcutList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return cspVMStrcut;
        }

        public override async Task<ModifyReply> CreateServicePackInfo(ServicePackVMStruct request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            List<ActivityVM> aclist = new List<ActivityVM>();
            try
            {
                ServicePackVM CspVm = new ServicePackVM();
                CspVm.ServicePack = new ServicePack();
                CspVm.ActivityList = new List<ActivityVM>();

                request.ServicePack.ServicePackId = Guid.NewGuid().ToString();
                request.ServicePack.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                CspVm.ServicePack = Mapper.Map<ServicePackStruct, ServicePack>(request.ServicePack);

                foreach (var item in request.ActivityListdata)
                {
                    var activity = Mapper.Map<ActivityStruct, ActivityVM>(item);
                    aclist.Add(activity);
                }

                CspVm.ActivityList.AddRange(aclist);

                var result = await _service.CreateServicePackInfo(CspVm);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateServicePackInfo(ServicePackVMStruct request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            List<ActivityVM> aclist = new List<ActivityVM>();
            try
            {
                ServicePackVM CspVm = new ServicePackVM();
                CspVm.ServicePack = new ServicePack();
                CspVm.ActivityList = new List<ActivityVM>();

                CspVm.ServicePack = Mapper.Map<ServicePackStruct, ServicePack>(request.ServicePack);

                foreach (var item in request.ActivityListdata)
                {
                    var activity = Mapper.Map<ActivityStruct, ActivityVM>(item);
                    aclist.Add(activity);
                }

                CspVm.ActivityList.AddRange(aclist);

                var result = await _service.UpdateServicePackInfo(CspVm);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteServicePackById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteServicePackById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ResultReply> IsCanDeleteAcitvity(IdRequest request, ServerCallContext context)
        {
            var result = await _service.IsCanDeleteAcitvity(request.Id);
            ResultReply resultReply = new ResultReply();
            resultReply.Result = result;
            return resultReply;
        }

        #endregion

        #region ServicePackActivityMap表操作

        public override async Task<ResultReply> IsExistSessionConferencdId(SearchStruct request, ServerCallContext context)
        {
            ResultReply resultReply = new ResultReply();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var result = await _service.IsExistSessionConferencdId(search);

                resultReply.Result = result;

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return resultReply;
        }

        public override async Task<ModifyReply> RemoveSCBySessionConferencdId(SearchStruct request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var result = await _service.RemoveSCBySessionConferencdId(search);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region DelegateServicePackDiscount表操作

        public override async Task<DelegateServicePackDiscountList> GetDSPDList(PaginationRequestSearch request, ServerCallContext context)
        {
            DelegateServicePackDiscountList CList = new DelegateServicePackDiscountList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetDSPDList(request.Offset, request.Limit, search);

            var ccList = Mapper.Map<List<DelegateServicePackDiscount>, List<DelegateServicePackDiscountListStruct>>(list);
            CList.Listdata.AddRange(ccList);
            CList.Total = await _service.GetDSPDListCount(search);

            return CList;
        }

        public override async Task<DelegateServicePackDiscountListStruct> GetDSPDById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetDSPDById(request.Id);
            var Struct = Mapper.Map<DelegateServicePackDiscount, DelegateServicePackDiscountListStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateDSPDInfo(DelegateServicePackDiscountStruct request, ServerCallContext context)
        {
            request.DiscountId = Guid.NewGuid().ToString();
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<DelegateServicePackDiscountStruct, DelegateServicePackDiscount>(request);

            var result = await _service.CreateDSPDInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateDSPDInfo(DelegateServicePackDiscountStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<DelegateServicePackDiscountStruct, DelegateServicePackDiscount>(request);
            var result = await _service.UpdateDSPDInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteDSPDById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteDSPDById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region DelegateServicePackDiscountForConferenceContract表操作

        public override async Task<DelegateServicePackDiscountForConferenceContractList> GetDSPDFCCList(PaginationRequestSearch request, ServerCallContext context)
        {
            DelegateServicePackDiscountForConferenceContractList CList = new DelegateServicePackDiscountForConferenceContractList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetDSPDFCCList(request.Offset, request.Limit, search);

            var ccList = Mapper.Map<List<DelegateServicePackDiscountForConferenceContract>, List<DelegateServicePackDiscountForConferenceContractListStruct>>(list);
            CList.Listdata.AddRange(ccList);
            CList.Total = await _service.GetDSPDFCCListCount(search);

            return CList;
        }

        public override async Task<DelegateServicePackDiscountForConferenceContractListStruct> GetDSPDFCCById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetDSPDFCCById(request.Id);
            var Struct = Mapper.Map<DelegateServicePackDiscountForConferenceContract, DelegateServicePackDiscountForConferenceContractListStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateDSPDFCCInfo(DelegateServicePackDiscountForConferenceContractStruct request, ServerCallContext context)
        {
            request.DiscountId = Guid.NewGuid().ToString();
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<DelegateServicePackDiscountForConferenceContractStruct, DelegateServicePackDiscountForConferenceContract>(request);

            var result = await _service.CreateDSPDFCCInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateDSPDFCCInfo(DelegateServicePackDiscountForConferenceContractStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<DelegateServicePackDiscountForConferenceContractStruct, DelegateServicePackDiscountForConferenceContract>(request);
            var result = await _service.UpdateDSPDFCCInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteDSPDFCCById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteDSPDFCCById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region PersonContract表操作

        public override async Task<PersonContractList> GetPersonContractList(PaginationRequestSearch request, ServerCallContext context)
        {
            PersonContractList PList = new PersonContractList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetPersonContractList(request.Offset, request.Limit, search);

            var pcList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(list);
            PList.Listdata.AddRange(pcList);
            PList.Total = await _service.GetPersonContractListCount(search);

            return PList;
        }

        public override async Task<PersonContractList> GetPersonContractByNewList(PaginationRequestSearch request, ServerCallContext context)
        {
            PersonContractList PList = new PersonContractList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetPersonContractByNewList(request.Offset, request.Limit, search);

            var pcList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(list);
            PList.Listdata.AddRange(pcList);
            PList.Total = await _service.GetPersonContractByNewListCount(search);

            return PList;
        }

        public override async Task<PersonContractList> GetPersonContractByContractIdList(PaginationRequestSearch request, ServerCallContext context)
        {
            PersonContractList PList = new PersonContractList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetPersonContractByContractIdList(request.Offset, request.Limit, search);

            var pcList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(list);
            PList.Listdata.AddRange(pcList);
            PList.Total = await _service.GetPersonContractByContractIdListCount(search);

            return PList;
        }

        public override async Task<PersonContractList> GetPersonContractByMemberPKList(PaginationRequestSearch request, ServerCallContext context)
        {
            PersonContractList PList = new PersonContractList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetPersonContractByMemberPKList(request.Offset, request.Limit, search);

            var pcList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(list);
            PList.Listdata.AddRange(pcList);
            PList.Total = await _service.GetPersonContractByMemberPKListCount(search);

            return PList;
        }

        public override async Task<PersonContractList> GetPersonContractByMemberPKListWithNoPagination(SearchStruct request, ServerCallContext context)
        {
            PersonContractList PList = new PersonContractList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var list = await _service.GetPersonContractByMemberPKListWithNoPagination(search);

            var pcList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(list);
            PList.Listdata.AddRange(pcList);

            return PList;
        }

        public override async Task<PersonContractStruct> GetPersonContractById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetPersonContractById(request.Id);

            var Struct = Mapper.Map<PersonContract, PersonContractStruct>(item);

            return Struct;
        }

        public override async Task<PersonContractStruct> GetPersonContractByPersonContractNumber(SearchStruct request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var item = await _service.GetPersonContractByPersonContractNumber(search);
            var Struct = Mapper.Map<PersonContract, PersonContractStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReplyForCreateOther> CreatePersonContractInfo(PersonContractStruct request, ServerCallContext context)
        {
            request.PersonContractId = Guid.NewGuid().ToString();
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<PersonContractStruct, PersonContract>(request);

            var result = await _service.CreatePersonContractInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyForCreateOtherVM, ModifyReplyForCreateOther>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdatePersonContractInfo(PersonContractStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<PersonContractStruct, PersonContract>(request);
            var result = await _service.UpdatePersonContractInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyPersonContractByIsCheckIn(ModifyRequest request, ServerCallContext context)
        {
            var modifyRequestVM = Mapper.Map<ModifyRequest, ModifyRequestVM>(request);
            var result = await _service.ModifyPersonContractByIsCheckIn(modifyRequestVM);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyPersonContractIsCheckInByIdList(CheckInRequest request, ServerCallContext context)
        {
            var Struct = Mapper.Map<CheckInRequest, CheckInRequestVM>(request);
            var result = await _service.ModifyPersonContractIsCheckInByIdList(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyPersonContractIsSendEmail(PersonContractPidList request, ServerCallContext context)
        {
            List<string> idlist = new List<string>();
            idlist.AddRange(request.Pidlistdata);

            var result = await _service.ModifyPersonContractIsSendEmail(idlist);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }


        public override async Task<ModifyReply> ModifyPersonContractIsFianceRecord(PersonContractPCNoRequest request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            List<PersonContractPCNoVM> personContractPCNoVMs = new List<PersonContractPCNoVM>();
            try
            {
                foreach (var item in request.Listdata)
                {
                    var model = Mapper.Map<PersonContractPCNoStruct, PersonContractPCNoVM>(item);
                    personContractPCNoVMs.Add(model);
                }

                var result = await _service.ModifyPersonContractIsFianceRecord(personContractPCNoVMs);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyPersonContractIsPrintByOwerid(SearchStruct request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var result = await _service.ModifyPersonContractIsPrintByOwerid(search);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> ModifyPersonContractIsPrintByids(IdsRequest request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            try
            {
                var result = await _service.ModifyPersonContractIsPrintByids(request.Ids);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> DeletePersonContractById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeletePersonContractById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeletePersonContractByList(PersonContractPidList request, ServerCallContext context)
        {
            List<string> idlist = new List<string>();
            idlist.AddRange(request.Pidlistdata);

            var result = await _service.DeletePersonContractByList(idlist);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeletePersonContractByIdForWeb(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeletePersonContractByIdForWeb(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region ContractStatusDic表操作

        public override async Task<ContractStatusDicForDicList> GetContractStatusDic(Empty request, ServerCallContext context)
        {
            var list = await _service.GetContractStatusDic();
            ContractStatusDicForDicList diclist = new ContractStatusDicForDicList();

            var mapperlist = Mapper.Map<List<ContractStatusDic>, List<ContractStatusDicStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            return diclist;

        }
        public override async Task<ContractStatusDicList> GetContractStatusDicList(PaginationRequest request, ServerCallContext context)
        {
            ContractStatusDicList IntypeList = new ContractStatusDicList();

            var list = await _service.GetContractStatusDicList(request.Offset, request.Limit);
            var structList = Mapper.Map<List<ContractStatusDic>, List<ContractStatusDicStruct>>(list);
            IntypeList.Listdata.AddRange(structList);
            IntypeList.Total = await _service.GetContractStatusDicListCount();

            return IntypeList;
        }

        public override async Task<ContractStatusDicStruct> GetContractStatusDicById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetContractStatusDicById(request.Id);
            var Struct = Mapper.Map<ContractStatusDic, ContractStatusDicStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateContractStatusDicInfo(ContractStatusDicStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<ContractStatusDicStruct, ContractStatusDic>(request);

            var result = await _service.CreateContractStatusDicInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateContractStatusDicInfo(ContractStatusDicStruct request, ServerCallContext context)
        {
            request.ModefieldOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var Struct = Mapper.Map<ContractStatusDicStruct, ContractStatusDic>(request);
            var result = await _service.UpdateContractStatusDicInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteContractStatusDicById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteContractStatusDicById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region RemarkDic表操作

        public override async Task<RemarkDicForDicList> GetRemarkDic(Empty request, ServerCallContext context)
        {
            var list = await _service.GetRemarkDic();
            RemarkDicForDicList diclist = new RemarkDicForDicList();

            var mapperlist = Mapper.Map<List<RemarkDic>, List<RemarkDicStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            return diclist;

        }
        public override async Task<RemarkDicList> GetRemarkDicList(PaginationRequest request, ServerCallContext context)
        {
            RemarkDicList IntypeList = new RemarkDicList();

            var list = await _service.GetRemarkDicList(request.Offset, request.Limit);
            var structList = Mapper.Map<List<RemarkDic>, List<RemarkDicStruct>>(list);
            IntypeList.Listdata.AddRange(structList);
            IntypeList.Total = await _service.GetRemarkDicListCount();

            return IntypeList;
        }

        public override async Task<RemarkDicStruct> GetRemarkDicById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetRemarkDicById(request.Id);
            var Struct = Mapper.Map<RemarkDic, RemarkDicStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateRemarkDicInfo(RemarkDicStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();

            var model = Mapper.Map<RemarkDicStruct, RemarkDic>(request);

            var result = await _service.CreateRemarkDicInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateRemarkDicInfo(RemarkDicStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<RemarkDicStruct, RemarkDic>(request);
            var result = await _service.UpdateRemarkDicInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteRemarkDicById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteRemarkDicById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region ExtraService表操作

        public override async Task<ExtraServiceList> GetExtraServiceList(PaginationRequestSearch request, ServerCallContext context)
        {
            ExtraServiceList PList = new ExtraServiceList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetExtraServiceList(request.Offset, request.Limit, search);

            var pcList = Mapper.Map<List<ExtraService>, List<ExtraServiceStruct>>(list);
            PList.Listdata.AddRange(pcList);
            PList.Total = await _service.GetExtraServiceListCount(search);

            return PList;
        }

        public override async Task<ExtraServiceVMStruct> GetExtraServiceById(IdRequest request, ServerCallContext context)
        {
            ExtraServiceVMStruct esVMStrcut = new ExtraServiceVMStruct();
            List<ServicePackStruct> spStrcutList = new List<ServicePackStruct>();

            try
            {
                var model = await _service.GetExtraServiceById(request.Id);

                spStrcutList = Mapper.Map<List<ServicePack>, List<ServicePackStruct>>(model.ServicePackList);

                esVMStrcut = Mapper.Map<ExtraServiceVM, ExtraServiceVMStruct>(model);
                esVMStrcut.ServicepackListdata.AddRange(spStrcutList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return esVMStrcut;
        }

        public override async Task<ModifyReply> CreateExtraServiceInfo(ExtraServiceVMStruct request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            List<ServicePack> csplist = new List<ServicePack>();
            try
            {
                ExtraServiceVM EsVm = new ExtraServiceVM();
                EsVm.ExtraService = new ExtraService();
                EsVm.ServicePackList = new List<ServicePack>();

                request.ExtraService.ExtraServiceId = Guid.NewGuid().ToString();
                request.ExtraService.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                EsVm.ExtraService = Mapper.Map<ExtraServiceStruct, ExtraService>(request.ExtraService);

                foreach (var item in request.ServicepackListdata)
                {
                    var servicepack = Mapper.Map<ServicePackStruct, ServicePack>(item);
                    csplist.Add(servicepack);
                }

                EsVm.ServicePackList.AddRange(csplist);

                var result = await _service.CreateExtraServiceInfo(EsVm);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateExtraServiceInfo(ExtraServiceVMStruct request, ServerCallContext context)
        {
            ModifyReply modifyReply = new ModifyReply();
            List<ServicePack> csplist = new List<ServicePack>();
            try
            {
                ExtraServiceVM EsVm = new ExtraServiceVM();
                EsVm.ExtraService = new ExtraService();
                EsVm.ServicePackList = new List<ServicePack>();

                EsVm.ExtraService = Mapper.Map<ExtraServiceStruct, ExtraService>(request.ExtraService);

                foreach (var item in request.ServicepackListdata)
                {
                    var servicepack = Mapper.Map<ServicePackStruct, ServicePack>(item);
                    csplist.Add(servicepack);
                }

                EsVm.ServicePackList.AddRange(csplist);

                var result = await _service.UpdateExtraServiceInfo(EsVm);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }
            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteExtraServiceById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteExtraServiceById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region PersonContractActivityMap表操作

        public override async Task<PersonContractActivityMapList> GetPersonContractActivityMapByMemberPKList(SearchStruct request, ServerCallContext context)
        {
            PersonContractActivityMapList CList = new PersonContractActivityMapList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var list = await _service.GetPersonContractActivityMapByMemberPKList(search);

                var ccList = Mapper.Map<List<PersonContractActivityMap>, List<PersonContractActivityMapStruct>>(list);
                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<PersonContractActivityMapList> GetPersonContractActivityMapByActivityIdList(SearchStruct request, ServerCallContext context)
        {
            PersonContractActivityMapList CList = new PersonContractActivityMapList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var list = await _service.GetPersonContractActivityMapByActivityIdList(search);

                var ccList = Mapper.Map<List<PersonContractActivityMap>, List<PersonContractActivityMapStruct>>(list);
                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<PersonContractActivityMapList> GetPersonContractActivityMapByPersonContractNumberList(SearchStruct request, ServerCallContext context)
        {
            PersonContractActivityMapList CList = new PersonContractActivityMapList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var list = await _service.GetPersonContractActivityMapByPersonContractNumberList(search);

                var ccList = Mapper.Map<List<PersonContractActivityMap>, List<PersonContractActivityMapStruct>>(list);
                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ModifyReply> CreatePersonContractActivityMapInfo(PersonContractActivityMapList request, ServerCallContext context)
        {
            List<PersonContractActivityMap> personContractActivityMapslist = new List<PersonContractActivityMap>();
            ModifyReply modifyReply = new ModifyReply();
            try
            {
                foreach (var item in request.Listdata)
                {
                    item.MapId = Guid.NewGuid().ToString();
                    var list = Mapper.Map<PersonContractActivityMapStruct, PersonContractActivityMap>(item);
                    personContractActivityMapslist.Add(list);
                }
                var result = await _service.CreatePersonContractActivityMapInfo(personContractActivityMapslist);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

            return modifyReply;
        }

        public override async Task<ModifyReply> UpdatePersonContractActivityMapInfo(PersonContractActivityMapListToUpdate request, ServerCallContext context)
        {
            List<PersonContractActivityMap> personContractActivityMapslist = new List<PersonContractActivityMap>();
            ModifyReply modifyReply = new ModifyReply();
            var count = request.Listdata.Count;
            try
            {
                if (count > 0)
                {
                    foreach (var item in request.Listdata)
                    {
                        item.MapId = Guid.NewGuid().ToString();
                        var list = Mapper.Map<PersonContractActivityMapStruct, PersonContractActivityMap>(item);
                        personContractActivityMapslist.Add(list);
                    }
                }

                var result = await _service.UpdatePersonContractActivityMapInfo(request.MemberPK, personContractActivityMapslist);

                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

            return modifyReply;
        }

        #endregion

        #region ApplyConference表操作

        public override async Task<ApplyConferenceList> GetApplyConferenceBySessionConferenceIdListPagination(PaginationRequestSearch request, ServerCallContext context)
        {
            ApplyConferenceList PList = new ApplyConferenceList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetApplyConferenceBySessionConferenceIdListPagination(request.Offset, request.Limit, search);

            var pcList = Mapper.Map<List<ApplyConference>, List<ApplyConferenceStruct>>(list);
            PList.Listdata.AddRange(pcList);
            PList.Total = await _service.GetApplyConferenceBySessionConferenceIdListPaginationCount(search);

            return PList;
        }

        public override async Task<ApplyConferenceList> GetApplyConferenceByMemberPkAndYear(SearchStruct request, ServerCallContext context)
        {
            ApplyConferenceList PList = new ApplyConferenceList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var list = await _service.GetApplyConferenceByMemberPkAndYear(search);

            var pcList = Mapper.Map<List<ApplyConference>, List<ApplyConferenceStruct>>(list);
            PList.Listdata.AddRange(pcList);

            return PList;
        }

        public override async Task<ApplyConferenceList> GetApplyConferenceBySessionConferenceIdAndTagTypeCodeList(PaginationRequestSearch request, ServerCallContext context)
        {
            ApplyConferenceList PList = new ApplyConferenceList();
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetApplyConferenceBySessionConferenceIdAndTagTypeCodeList(request.Offset, request.Limit, search);

            var pcList = Mapper.Map<List<ApplyConference>, List<ApplyConferenceStruct>>(list);
            foreach (var item in pcList)
            {
                item.SessionConferenceIds.AddRange(_service.GetApplyConferenceByPerContractIdListForImpl(item.PersonContractId));
            }

            PList.Listdata.AddRange(pcList);
            PList.Total = await _service.GetApplyConferenceBySessionConferenceIdAndTagTypeCodeListCount(search);

            return PList;
        }

        public override async Task<ApplyConferenceList> GetApplyConferenceByPerContractIdList(SearchStruct request, ServerCallContext context)
        {
            ApplyConferenceList CList = new ApplyConferenceList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var list =  await _service.GetApplyConferenceByPerContractIdList(search);

                var ccList = Mapper.Map<List<ApplyConference>, List<ApplyConferenceStruct>>(list);
                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ApplyConferenceList> GetApplyConferenceByCompanyIdList(IdRequest request, ServerCallContext context)
        {
            ApplyConferenceList CList = new ApplyConferenceList();
            try
            {
                var list = await _service.GetApplyConferenceByCompanyIdList(request.Id);

                var ccList = Mapper.Map<List<ApplyConference>, List<ApplyConferenceStruct>>(list);
                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ApplyConferenceList> GetApplyConferenceBySessionConferenceIdList(IdRequest request, ServerCallContext context)
        {
            ApplyConferenceList CList = new ApplyConferenceList();
            try
            {
                var list = await _service.GetApplyConferenceBySessionConferenceIdList(request.Id);

                var ccList = Mapper.Map<List<ApplyConference>, List<ApplyConferenceStruct>>(list);
                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<ModifyReply> CreateOrUpdateApplyConferenceInfo(ApplyConferenceListToCreateOrUpdate request, ServerCallContext context)
        {
            List<ApplyConference> applyConferencelist = new List<ApplyConference>();
            ModifyReply modifyReply = new ModifyReply();
            var count = request.Listdata.Count;
            try
            {
                if (count > 0)
                {
                    foreach (var item in request.Listdata)
                    {
                        item.Id = Guid.NewGuid().ToString();
                        var list = Mapper.Map<ApplyConferenceStruct, ApplyConference>(item);
                        applyConferencelist.Add(list);
                    }
                }

                var result = await _service.CreateOrUpdateApplyConferenceInfo(request.PersonContractId, applyConferencelist);

                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

            return modifyReply;
        }

        #endregion

        #region InviteLetter表操作

        public override async Task<InviteLetterStruct> GetInviteLetterById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetInviteLetterById(request.Id);
            var Struct = Mapper.Map<InviteLetter, InviteLetterStruct>(item);

            return Struct;
        }


        public override async Task<ModifyReplyForCreateOther> CreateInviteLetterInfo(InviteLetterStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<InviteLetterStruct, InviteLetter>(request);

            var result = await _service.CreateInviteLetterInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyForCreateOtherVM, ModifyReplyForCreateOther>(result);
            return modifyReply;
        }

        #endregion

        #region TagType表操作

        public override async Task<TagTypeForDicList> GetTagTypeDic(Empty request, ServerCallContext context)
        {
            var list = await _service.GetTagTypeDic();
            TagTypeForDicList diclist = new TagTypeForDicList();

            var mapperlist = Mapper.Map<List<TagType>, List<TagTypeStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            return diclist;

        }

        public override async Task<TagTypeStruct> GetTagTypeByCode(CodeRequest request, ServerCallContext context)
        {
            var item = await _service.GetTagTypeByCode(request.Code);
            var Struct = Mapper.Map<TagType, TagTypeStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateTagTypeInfo(TagTypeStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();

            var model = Mapper.Map<TagTypeStruct, TagType>(request);

            var result = await _service.CreateTagTypeInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateTagTypeInfo(TagTypeStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<TagTypeStruct, TagType>(request);
            var result = await _service.UpdateTagTypeInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteTagTypeById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteTagTypeById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region YearConfig表操作

        public override async Task<YearConfigForDicList> GetYearConfigDic(Empty request, ServerCallContext context)
        {
            var list = await _service.GetYearConfigDic();
            YearConfigForDicList diclist = new YearConfigForDicList();

            var mapperlist = Mapper.Map<List<YearConfig>, List<YearConfigStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            return diclist;

        }

        public override async Task<YearConfigForDicList> GetYearConfigByIsUse(SearchStruct request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var list = await _service.GetYearConfigByIsUse(search);
            YearConfigForDicList diclist = new YearConfigForDicList();

            var mapperlist = Mapper.Map<List<YearConfig>, List<YearConfigStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            return diclist;

        }

        public override async Task<ModifyReply> CreateYearConfigInfo(YearConfigStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();

            var model = Mapper.Map<YearConfigStruct, YearConfig>(request);

            var result = await _service.CreateYearConfigInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateYearConfigInfo(YearConfigStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<YearConfigStruct, YearConfig>(request);
            var result = await _service.UpdateYearConfigInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region ConferenceOnsite表操作

        public override async Task<ConferenceOnsiteList> GetConferenceOnsiteList(PaginationRequestSearch request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
            var list = await _service.GetConferenceOnsiteList(request.Offset, request.Limit, search);
            ConferenceOnsiteList diclist = new ConferenceOnsiteList();

            var mapperlist = Mapper.Map<List<ConferenceOnsite>, List<ConferenceOnsiteStruct>>(list);
            diclist.Listdata.AddRange(mapperlist);
            diclist.Total = await _service.GetConferenceOnsiteListCount(search);
            return diclist;
        }

        public override async Task<ConferenceOnsiteStruct> GetConferenceOnsiteById(IdIntRequest request, ServerCallContext context)
        {
            var item = await _service.GetConferenceOnsiteById(request.Id);
            var Struct = Mapper.Map<ConferenceOnsite, ConferenceOnsiteStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReplyForConferenceOnsite> CreateConferenceOnsiteInfo(ConferenceOnsiteStruct request, ServerCallContext context)
        {
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<ConferenceOnsiteStruct, ConferenceOnsite>(request);

            var result = await _service.CreateConferenceOnsiteInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyForConferenceOnsiteVM, ModifyReplyForConferenceOnsite>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateConferenceOnsiteInfo(ConferenceOnsiteStruct request, ServerCallContext context)
        {
            request.ModefieldOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var Struct = Mapper.Map<ConferenceOnsiteStruct, ConferenceOnsite>(request);
            var result = await _service.UpdateConferenceOnsiteInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteConferenceOnsiteById(IdIntRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteConferenceOnsiteById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region InviteCode表操作

        public override async Task<InviteCodeList> GetInviteCodeList(PaginationRequestSearch request, ServerCallContext context)
        {
            InviteCodeList diclist = new InviteCodeList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetInviteCodeList(request.Offset, request.Limit, search);

                var mapperlist = Mapper.Map<List<InviteCodeCSPVM>, List<InviteCodeCSPVMStruct>>(list);
                diclist.Listdata.AddRange(mapperlist);
                diclist.Total = await _service.GetInviteCodeListCount(search);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }
            return diclist;
        }

        public override async Task<InviteCodeCSPVMStruct> GetInviteCodeById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetInviteCodeById(request.Id);
            var Struct = Mapper.Map<InviteCodeCSPVM, InviteCodeCSPVMStruct>(item);

            return Struct;
        }

        public override async Task<InviteCodeCSPVMStruct> GetInviteCodeByInviteCodeNumber(SearchStruct request, ServerCallContext context)
        {
            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var item = await _service.GetInviteCodeByInviteCodeNumber(search);
            var Struct = item == null ? new InviteCodeCSPVMStruct() : Mapper.Map<InviteCodeCSPVM, InviteCodeCSPVMStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateInviteCodeInfo(InviteCodeStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();
            request.CreatedOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var model = Mapper.Map<InviteCodeStruct, InviteCode>(request);

            var result = await _service.CreateInviteCodeInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateInviteCodeInfo(InviteCodeStruct request, ServerCallContext context)
        {
            request.ModefieldOn = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            var Struct = Mapper.Map<InviteCodeStruct, InviteCode>(request);
            var result = await _service.UpdateInviteCodeInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteInviteCodeById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteInviteCodeById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region InviteCodeRecord表操作

        public override async Task<InviteCodeRecordList> GetInviteCodeRecordList(PaginationRequestSearch request, ServerCallContext context)
        {
            InviteCodeRecordList diclist = new InviteCodeRecordList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetInviteCodeRecordList(request.Offset, request.Limit, search);

                var mapperlist = Mapper.Map<List<InviteCodeRecord>, List<InviteCodeRecordStruct>>(list);
                diclist.Listdata.AddRange(mapperlist);
                diclist.Total = await _service.GetInviteCodeRecordListCount(search);
            }

            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }
            return diclist;
        }

        public override async Task<InviteCodeRecordStruct> GetInviteCodeRecordById(IdRequest request, ServerCallContext context)
        {
            var item = await _service.GetInviteCodeRecordById(request.Id);
            var Struct = Mapper.Map<InviteCodeRecord, InviteCodeRecordStruct>(item);

            return Struct;
        }

        public override async Task<ModifyReply> CreateInviteCodeRecordInfo(InviteCodeRecordStruct request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();
            request.UseDate = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            var model = Mapper.Map<InviteCodeRecordStruct, InviteCodeRecord>(request);

            var result = await _service.CreateInviteCodeRecordInfo(model);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        public override async Task<ModifyReply> UpdateInviteCodeRecordInfo(InviteCodeRecordStruct request, ServerCallContext context)
        {
            var Struct = Mapper.Map<InviteCodeRecordStruct, InviteCodeRecord>(request);
            var result = await _service.UpdateInviteCodeRecordInfo(Struct);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        public override async Task<ModifyReply> DeleteInviteCodeRecordById(IdRequest request, ServerCallContext context)
        {
            var result = await _service.DeleteInviteCodeRecordById(request.Id);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);

            return modifyReply;
        }

        #endregion

        #region 其他接口

        public override async Task<ModifyReply> CopyPackInfoByYear(SearchStruct request, ServerCallContext context)
        {

            var search = Mapper.Map<SearchStruct, SearchInfo>(request);
            var result = await _service.CopyPackInfoByYear(search);
            var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            return modifyReply;
        }

        //public override async Task<ModifyReply> CopyPackInfoByYearForESH(SearchStruct request, ServerCallContext context)
        //{

        //    var search = Mapper.Map<SearchStruct, SearchInfo>(request);

        //    var result = await _service.CopyPackInfoByYearForESH(search);
        //    var modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
        //    return modifyReply;
        //}

        public override async Task<ModifyReply> CreateCompanyServicePackMap(CompanyServicePackMapList request, ServerCallContext context)
        {
            List<CompanyServicePackMapVM> companyServicePackMapslist = new List<CompanyServicePackMapVM>();
            ModifyReply modifyReply = new ModifyReply();
            try
            {
                foreach (var item in request.Listdata)
                {
                    item.MapId = Guid.NewGuid().ToString();
                    var model = Mapper.Map<CompanyServicePackMapVMStruct, CompanyServicePackMapVM>(item);
                    companyServicePackMapslist.Add(model);
                }
                var result = await _service.CreateCompanyServicePackMap(companyServicePackMapslist);
                modifyReply = Mapper.Map<ModifyReplyVM, ModifyReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw ex;
            }

            return modifyReply;
        }

        public override async Task<ContractStatisticsList> GetContractStatisticsList(SearchStruct request, ServerCallContext context)
        {
            ContractStatisticsList CList = new ContractStatisticsList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var list = await _service.GetContractStatisticsList(search);

                var ccList = Mapper.Map<List<ContractStatistics>, List<ContractStatisticsStruct>>(list);
                CList.Listdata.AddRange(ccList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return CList;
        }

        public override async Task<BoolReply> IsMaxContractNumberEqualsPCCountByCompanyPKAndYear(SearchStruct request, ServerCallContext context)
        {
            BoolReply boolReply = new BoolReply();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var result = await _service.IsMaxContractNumberEqualsPCCountByCompanyPKAndYear(search);

                boolReply = Mapper.Map<BoolReplyVM, BoolReply>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return boolReply;
        }

        public override async Task<PersonContractAndSessionConferenceIdList> GetPersonContractListForLunch(PaginationRequestSearch request, ServerCallContext context)
        {
            PersonContractAndSessionConferenceIdList re_list = new PersonContractAndSessionConferenceIdList();
            List<string> SessionConferenceIds = new List<string>();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetPersonContractListForLunch(request.Offset, request.Limit, search);

                var ccList = Mapper.Map<List<PersonContractAndSessionConferenceIdListVM>, List<PersonContractAndSessionConferenceIdsStruct>>(list);

                foreach (var item in ccList)
                {
                    foreach (var item2 in list)
                    {
                        if (item.PersonContract.PersonContractId == item2.PersonContract.PersonContractId.ToString())
                        {
                            item.SessionConferenceIds.AddRange(item2.SessionConferenceIds);
                        }

                    }
                }

                re_list.Listdata.AddRange(ccList);
                re_list.Total = await _service.GetPersonContractListForLunchCount(search);
                search.IsCheckIn = false;
                re_list.AllTotal = await _service.GetPersonContractListForLunchCount(search);

            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return re_list;
        }

        public override async Task<PersonContractList> GetPersonContractListAndApplyConference(PaginationRequestSearch request, ServerCallContext context)
        {
            PersonContractList PList = new PersonContractList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request.Search);
                var list = await _service.GetPersonContractListAndApplyConference(request.Offset, request.Limit, search);

                var pcList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(list);

                PList.Listdata.AddRange(pcList);
                PList.Total = await _service.GetPersonContractListAndApplyConferenceCount(search);
            }

            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return PList;
        }

        public override async Task<PersonContractList> ExportPersonContractList(SearchStruct request, ServerCallContext context)
        {
            PersonContractList PList = new PersonContractList();
            try
            {
                var search = Mapper.Map<SearchStruct, SearchInfo>(request);
                var list = await _service.ExportPersonContractList(search);

                var pcList = Mapper.Map<List<PersonContract>, List<PersonContractStruct>>(list);
                PList.Listdata.AddRange(pcList);
                PList.Total = pcList.Count;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }

            return PList;
        }

        //public override async Task<ModifyReplyForCreateOther> CreatePersonContractActivityMapImport(Empty request, ServerCallContext context)
        //{
        //    ModifyReplyForCreateOther result = new ModifyReplyForCreateOther();
        //    try
        //    {
        //        var item = await _service.CreatePersonContractActivityMapImport();

        //        result = Mapper.Map<ModifyReplyForCreateOtherVM, ModifyReplyForCreateOther>(item);
        //    }

        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(this, ex);
        //    }

        //    return result;
        //}
        #endregion

        #region 消费队列使用方法
        public int ModifyPersonContractIsCommitAbstractByPCNumberRMQ(PCIsCommitAbstractVm model)
        {
            var count = 0;
            try
            {
                var result = _service.ModifyPersonContractIsCommitAbstractByPCNumber(model);
                count = result.modifiedcount;
                return count;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        public int ModifyPersonContractPaidAmountByPCNumberRMQ(List<PCPaidAmountVm> list)
        {
            var count = 0;
            try
            {
                var result = _service.ModifyPersonContractPaidAmountByPCNumber(list);
                count = result.modifiedcount;
                return count;
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new Exception("异常", ex);
            }
        }

        #endregion
    }
}
