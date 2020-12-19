using Grpc.Core;
using GrpcConferenceContractServiceNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCCDemoAPI
{
    public class CCServiceClient
    {
        private static Channel _channel;
        private static ConferenceContractServiceToGrpc.ConferenceContractServiceToGrpcClient _client;

        static CCServiceClient()
        {
            _channel = new Channel("conferencecontractserver:40001", ChannelCredentials.Insecure);
            _client = new ConferenceContractServiceToGrpc.ConferenceContractServiceToGrpcClient(_channel);
        }

        public static CompanyContractList GetCompanyContractList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetCompanyContractList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static CompanyContractList GetCompanyContractByConferenceContractIdList(string id)
        {
            return _client.GetCompanyContractByConferenceContractIdList(new IdRequest
            {
                Id = id
            });
        }


        public static InviteCodeCSPVMStruct GetInviteCodeByInviteCodeNumber(SearchStruct search)
        {
            return _client.GetInviteCodeByInviteCodeNumber(search);
        }

        public static CompanyContractStruct GetCompanyContractById(string id)
        {
            return _client.GetCompanyContractById(new IdRequest
            {
                Id = id
            });
        }

        public static CompanyServicePackList GetCompanyServicePackList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetCompanyServicePackList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static InviteCodeRecordList GetInviteCodeRecordList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetInviteCodeRecordList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static InviteCodeList GetInviteCodeList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetInviteCodeList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static CompanyServicePackVMStruct GetCompanyServicePackById(string id)
        {
            return _client.GetCompanyServicePackById(new IdRequest
            {
                Id = id
            });
        }

        public static CompanyServicePackVMStruct GetCompanyServicePackVMByPersonContractNumber(SearchStruct search)
        {
            return _client.GetCompanyServicePackVMByPersonContractNumber(search);
        }

        public static PersonContractList GetPersonContractByMemberPKListWithNoPagination(SearchStruct search)
        {
            return _client.GetPersonContractByMemberPKListWithNoPagination(search);
        }

        public static ModifyReply CreateCompanyServicePackInfo(CompanyServicePackVMStruct model)
        {
            return _client.CreateCompanyServicePackInfo(model);
        }

        public static ModifyReply UpdateCompanyServicePackInfo(CompanyServicePackVMStruct model)
        {
            return _client.UpdateCompanyServicePackInfo(model);
        }

        public static ModifyReply UpdateConferenceOnsiteInfo(ConferenceOnsiteStruct model)
        {
            return _client.UpdateConferenceOnsiteInfo(model);
        }

        public static ModifyReply DeleteCompanyServicePackById(string id)
        {
            return _client.DeleteCompanyServicePackById(new IdRequest
            {
                Id = id
            });
        }


     

        public static ModifyReplyForCreateOther CreateCompanyContractInfo(CompanyContractStruct model)
        {
            return _client.CreateCompanyContractInfo(model);
        }

        public static ModifyReply UpdateCompanyContractInfo(CompanyContractStruct model)
        {
            return _client.UpdateCompanyContractInfo(model);
        }

        public static ModifyReply DeleteCompanyContractById(string id)
        {
            return _client.DeleteCompanyContractById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply DeleteCompanyContractByList(CompanyContractCidList list)
        {
            return _client.DeleteCompanyContractByList(list);
        }

        public static CompanyContractList GetCompanyContractByCompanyIdList(string id)
        {
            return _client.GetCompanyContractByCompanyIdList(new IdRequest
            {
                Id = id
            });
        }

        public static ContractTypeList GetContractTypeList(int pageindex, int pagesize)
        {
            return _client.GetContractTypeList(new PaginationRequest
            {
                Offset = pageindex,
                Limit = pagesize
            });
        }

        public static ContractTypeStruct GetContractTypeById(string id)
        {
            return _client.GetContractTypeById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply CreateContractTypeInfo(ContractTypeStruct model)
        {
            return _client.CreateContractTypeInfo(model);
        }

        public static ServicePackList GetServicePackListAll()
        {
            return _client.GetServicePackListAll(new Empty());
        }

        public static ServicePackList GetServicePackByConferenceIdList(string id)
        {
            return _client.GetServicePackByConferenceIdList(new IdRequest
            {
                Id = id
            });
        }

        public static ServicePackList GetServicePackList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetServicePackList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static ServicePackVMStruct GetServicePackById(string id)
        {
            return _client.GetServicePackById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply CreateServicePackInfo(ServicePackVMStruct model)
        {
            return _client.CreateServicePackInfo(model);
        }

        public static ModifyReply UpdateServicePackInfo(ServicePackVMStruct model)
        {
            return _client.UpdateServicePackInfo(model);
        }

        public static ModifyReply DeleteServicePackById(string id)
        {
            return _client.DeleteServicePackById(new IdRequest
            {
                Id = id
            });
        }

        public static ResultReply IsCanDeleteAcitvity(string id)
        {
            return _client.IsCanDeleteAcitvity(new IdRequest
            {
                Id = id
            });
        }

        public static DelegateServicePackDiscountList GetDSPDList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetDSPDList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static DelegateServicePackDiscountListStruct GetDSPDById(string id)
        {
            return _client.GetDSPDById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply CreateDSPDInfo(DelegateServicePackDiscountStruct model)
        {
            return _client.CreateDSPDInfo(model);
        }

        public static ModifyReply UpdateDSPDInfo(DelegateServicePackDiscountStruct model)
        {
            return _client.UpdateDSPDInfo(model);
        }

        public static ModifyReply DeleteDSPDById(string id)
        {
            return _client.DeleteDSPDById(new IdRequest
            {
                Id = id
            });
        }

        public static PersonContractList GetPersonContractList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetPersonContractList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static PersonContractList GetPersonContractByNewList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetPersonContractByNewList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static PersonContractList GetPersonContractByContractIdList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetPersonContractByContractIdList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static PersonContractList GetPersonContractByMemberPKList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetPersonContractByMemberPKList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static PersonContractStruct GetPersonContractById(string id)
        {
            return _client.GetPersonContractById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReplyForCreateOther CreatePersonContractInfo(PersonContractStruct model)
        {
            return _client.CreatePersonContractInfo(model);
        }

        public static ModifyReply UpdatePersonContractInfo(PersonContractStruct model)
        {
            return _client.UpdatePersonContractInfo(model);
        }

        public static ModifyReply DeletePersonContractById(string id)
        {
            return _client.DeletePersonContractById(new IdRequest
            {
                Id = id
            });
        }

        public static ExtraServiceList GetExtraServiceList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetExtraServiceList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static ExtraServiceVMStruct GetExtraServiceById(string id)
        {
            return _client.GetExtraServiceById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply CreateExtraServiceInfo(ExtraServiceVMStruct model)
        {
            return _client.CreateExtraServiceInfo(model);
        }

        public static ModifyReply UpdateExtraServiceInfo(ExtraServiceVMStruct model)
        {
            return _client.UpdateExtraServiceInfo(model);
        }

        public static ModifyReply DeleteExtraServiceById(string id)
        {
            return _client.DeleteExtraServiceById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply CreateCCNumberConfigInfo(CCNumberConfigStruct model)
        {
            return _client.CreateCCNumberConfigInfo(model);
        }

        public static ContractStatusDicForDicList GetContractStatusDic()
        {
            return _client.GetContractStatusDic(new Empty
            {

            });
        }

        public static PersonContractAndSessionConferenceIdList GetPersonContractListForLunch(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetPersonContractListForLunch(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static PersonContractList GetPersonContractListAndApplyConference(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetPersonContractListAndApplyConference(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        #region ConferenceContract表操作
        public static ConferenceContractList GetConferenceContractList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetConferenceContractList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static ConferenceContractList GetConferenceContractListByIsGive(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetConferenceContractListByIsGive(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static ConferenceContractList GetConferenceContractByCompanyIdList(string id)
        {
            return _client.GetConferenceContractByCompanyIdList(new IdRequest
            {
                Id = id
            });
        }

        public static ConferenceContractStruct GetConferenceContractById(string id)
        {
            return _client.GetConferenceContractById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReplyForCreateOther CreateConferenceContractInfo(ConferenceContractStruct model)
        {
            return _client.CreateConferenceContractInfo(model);
        }

        public static ModifyReply UpdateConferenceContractInfo(ConferenceContractStruct model)
        {
            return _client.UpdateConferenceContractInfo(model);
        }

        public static ModifyReply DeleteConferenceContractById(string id)
        {
            return _client.DeleteConferenceContractById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply DeleteConferenceContractByList(ConferenceContractCidList list)
        {
            return _client.DeleteConferenceContractByList(list);
        }

        public static PersonContractActivityMapList GetPersonContractActivityMapByMemberPKList(SearchStruct search)
        {
            return _client.GetPersonContractActivityMapByMemberPKList(search);
        }

        public static PersonContractActivityMapList GetPersonContractActivityMapByActivityIdList(SearchStruct search)
        {
            return _client.GetPersonContractActivityMapByActivityIdList(search);
        }

        public static PersonContractActivityMapList GetPersonContractActivityMapByPersonContractNumberList(SearchStruct search)
        {
            return _client.GetPersonContractActivityMapByPersonContractNumberList(search);
        }

        public static ModifyReply CreatePersonContractActivityMapInfo(PersonContractActivityMapList list)
        {
            return _client.CreatePersonContractActivityMapInfo(list);
        }

        public static ModifyReply UpdatePersonContractActivityMapInfo(PersonContractActivityMapListToUpdate model)
        {
            return _client.UpdatePersonContractActivityMapInfo(model);
        }

        public static ApplyConferenceList GetApplyConferenceByPerContractIdList(SearchStruct search)
        {
            return _client.GetApplyConferenceByPerContractIdList(search);
        }

        public static ApplyConferenceList GetApplyConferenceBySessionConferenceIdList(string id)
        {
            return _client.GetApplyConferenceBySessionConferenceIdList(new IdRequest
            {
                Id = id
            });
        }

        public static ApplyConferenceList GetApplyConferenceBySessionConferenceIdListPagination(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetApplyConferenceBySessionConferenceIdListPagination(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static ModifyReply CreateOrUpdateApplyConferenceInfo(ApplyConferenceListToCreateOrUpdate model)
        {
            return _client.CreateOrUpdateApplyConferenceInfo(model);
        }

        public static DelegateServicePackDiscountForConferenceContractList GetDSPDFCCList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetDSPDFCCList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static DelegateServicePackDiscountForConferenceContractListStruct GetDSPDFCCById(string id)
        {
            return _client.GetDSPDFCCById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply CreateDSPDFCCInfo(DelegateServicePackDiscountForConferenceContractStruct model)
        {
            return _client.CreateDSPDFCCInfo(model);
        }

        public static ModifyReply UpdateDSPDFCCInfo(DelegateServicePackDiscountForConferenceContractStruct model)
        {
            return _client.UpdateDSPDFCCInfo(model);
        }

        public static ModifyReply DeleteDSPDFCCById(string id)
        {
            return _client.DeleteDSPDFCCById(new IdRequest
            {
                Id = id
            });
        }

        public static ContractStatisticsList GetContractStatisticsList(SearchStruct search)
        {
            return _client.GetContractStatisticsList(search);
        }

        public static ModifyReply CopyPackInfoByYear(SearchStruct model)
        {
            return _client.CopyPackInfoByYear(model);
        }

        //public static ModifyReply CopyPackInfoByYearForESH(SearchStruct model)
        //{
        //    return _client.CopyPackInfoByYearForESH(model);
        //}

        public static ModifyReply CreateCompanyServicePackMap(CompanyServicePackMapList list)
        {
            return _client.CreateCompanyServicePackMap(list);
        }

        public static ModifyReply ModifyPersonContractIsFianceRecord(PersonContractPCNoRequest model)
        {
            return _client.ModifyPersonContractIsFianceRecord(model);
        }

        #endregion

        public static ModifyReplyForConferenceOnsite CreateConferenceOnsiteInfo(ConferenceOnsiteStruct model)
        {
            return _client.CreateConferenceOnsiteInfo(model);
        }

        public static ModifyReply ModifyPersonContractIsCheckInByIdList(CheckInRequest model)
        {
            return _client.ModifyPersonContractIsCheckInByIdList(model);
        }

        public static PersonContractList ExportPersonContractList(SearchStruct search)
        {
            return _client.ExportPersonContractList(search);
        }

        public static ConferenceContractList GetConferenceContractListByIsGiveWithAllContractStatusCode(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetConferenceContractListByIsGiveWithAllContractStatusCode(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static ApplyConferenceList GetApplyConferenceBySessionConferenceIdAndTagTypeCodeList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetApplyConferenceBySessionConferenceIdAndTagTypeCodeList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        //public static ModifyReplyForCreateOther CreatePersonContractActivityMapImport()
        //{
        //    return _client.CreatePersonContractActivityMapImport(new Empty
        //    {

        //    });
        //}
    }
}
