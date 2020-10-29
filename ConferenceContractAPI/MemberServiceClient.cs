using ConferenceContractAPI.Common;
using Grpc.Core;
using GrpcMembersService;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceContractAPI
{
    public static class MemberServiceClient
    {
        private static Channel _channel;
        private static MemberServiceToGrpc.MemberServiceToGrpcClient _client;
        private static string _channel_address = ContextConnect.GrpcChannelConnstrContent("47.56.246.214:3207", "member.api: 40001");

        static MemberServiceClient()
        {
            _channel = new Channel(_channel_address, ChannelCredentials.Insecure);
            //_channel = new Channel("memberapi:40001", ChannelCredentials.Insecure);
            _client = new MemberServiceToGrpc.MemberServiceToGrpcClient(_channel);
        }

        //单点登录
        public static LoginInfoReply LoginToMember(MemberRequest model)
        {
            return _client.LoginToMember(model);
        }

        public static LoginInfoReply LoginToMemContract(MemContractRequest model)
        {
            return _client.LoginToMemContract(model);
        }

        #region MemContract

        public static ModifyReply CreateMemberContractInfo(MemberContractStruct model)
        {
            return _client.CreateMemberContractInfo(model);
        }

        public static MemberContractStruct GetMemContractByMemContract(MemberContractRequest model)
        {
            return _client.GetMemContractByMemContract(model);
        }

        public static ModifyReply DeleteMemContractByMemContract(MemberContractRequest model)
        {
            return _client.DeleteMemContractByMemContract(model);
        }

        #endregion

        #region Member

        public static MemberList GetMemberList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetMemberList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static MemberStruct GetMemberById(string id)
        {
            return _client.GetMemberById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReplyForCreateMember CreateMemberInfo(MemberStruct member)
        {
            return _client.CreateMemberInfo(member);
        }

        public static ModifyReplyForCreateMember CreateMemberAccountInfo(MemberAccountStruct member)
        {
            return _client.CreateMemberAccountInfo(member);
        }

        public static ModifyReply UpdateMemberInfo(MemberStruct member)
        {
            return _client.UpdateMemberInfo(member);
        }

        public static ModifyReply DeleteMemberById(string id)
        {
            return _client.DeleteMemberById(new IdRequest
            {
                Id = id
            });
        }

        #endregion

        #region Company

        public static CompanyList GetCompanyList(int pageindex, int pagesize, SearchStruct search)
        {
            return _client.GetCompanyList(new PaginationRequestSearch
            {
                Offset = pageindex,
                Limit = pagesize,
                Search = search
            });
        }

        public static CompanyStruct GetCompanyById(string id)
        {
            return _client.GetCompanyById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply UpdateCompanyInfo(CompanyStruct model)
        {
            return _client.UpdateCompanyInfo(model);
        }

        public static ModifyReply DeleteCompanyById(string id)
        {
            return _client.DeleteCompanyById(new IdRequest
            {
                Id = id
            });
        }

        public static ModifyReply VerifyComAccount(string id)
        {
            return _client.VerifyComAccount(new IdRequest
            {
                Id = id
            });
        }

        #endregion

    }
}
