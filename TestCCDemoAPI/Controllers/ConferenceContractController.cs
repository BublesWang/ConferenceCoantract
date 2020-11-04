using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServer.Web.ConferenceContract.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GrpcServer.Web.ConferenceContract.Protos.NewConferenceContractService;

namespace TestCCDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceContractController : ControllerBase
    {
        private Channel _channel;
        private NewConferenceContractServiceClient _client;


        public ConferenceContractController()
        {
            //_channel = new Channel("127.0.0.1:40001", ChannelCredentials.Insecure);
            _channel = new Channel("conferencecontractapi:40001", ChannelCredentials.Insecure);
            _client = new NewConferenceContractServiceClient(_channel);
        }

        [HttpPost(nameof(new_GetCompanyContractList))]
        public async Task<IActionResult> new_GetCompanyContractList(new_GetCompanyContractListRequest request)
        {
            var result = await _client.new_GetCompanyContractListAsync(request);
            return Ok(result);
        }

        [HttpPost(nameof(new_AddSchedule))]
        public async Task<IActionResult> new_AddSchedule(new_AddScheduleRequest request)
        {
            var result = await _client.new_AddScheduleAsync(request);
           
            return Ok(result);
        }

        [HttpPost(nameof(new_GetServicePack))]
        public async Task<IActionResult> new_GetServicePack(new_GetServicePackRequest request)
        {
            var result = await _client.new_GetServicePackAsync(request);

            return Ok(result);
        }
        
    }
}