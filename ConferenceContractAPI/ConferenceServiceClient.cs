using ConferenceContractAPI.Common;
using Grpc.Core;
using GrpcConferenceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI
{
    public class ConferenceServiceClient
    {
        private static Channel _channel;
        private static ConferenceServiceToGrpc.ConferenceServiceToGrpcClient _client;
        private static string _channel_address = ContextConnect.GrpcChannelConnstrContent("47.56.246.214:3208", "conference.api: 40001");
        static ConferenceServiceClient()
        {
            //latest,stable环境使用
            //_channel = new Channel("conference.api: 40001", ChannelCredentials.Insecure);
            //调试服务器上接口使用
            _channel = new Channel(_channel_address, ChannelCredentials.Insecure);
            _client = new ConferenceServiceToGrpc.ConferenceServiceToGrpcClient(_channel);
        }

        public static CreateInfoVM CreateParticipant(ParticipantStruct S)
        {
            return _client.CreateParticipant(S);
        }

        public static DeleteInfoVM DeleteParticioantByPerContractNumber(string percontractnumber)
        {
            return _client.DeleteParticioantByPerContractNumber(new PerContractNumberID
            {
                PerContractNumberID_ = percontractnumber
            });
        }

    }
}
