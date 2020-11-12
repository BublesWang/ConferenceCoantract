using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcConferenceContractService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GrpcConferenceContractService.GrpcConferenceContractServiceNew;

namespace TestCCDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceContractController : ControllerBase
    {
        private Channel _channel;
        private GrpcConferenceContractServiceNewClient _client;


        public ConferenceContractController()
        {
            //_channel = new Channel("127.0.0.1:40001", ChannelCredentials.Insecure);
            _channel = new Channel("conferencecontractserver:40001", ChannelCredentials.Insecure);
            _client = new GrpcConferenceContractServiceNewClient(_channel);
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

        [HttpPost(nameof(new_AddServicePack))]
        public async Task<IActionResult> new_AddServicePack(new_ServicePackStruct request)
        {
            var result = await _client.new_AddServicePackAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(new_AddServicePackDiscount))]
        public async Task<IActionResult> new_AddServicePackDiscount(new_ServicePackStructDiscount request)
        {
            var result = await _client.new_AddServicePackDiscountAsync(request);

            return Ok(result);
        }
       


        [HttpPost(nameof(new_AddPersonContract))]
        public async Task<IActionResult> new_AddPersonContract(new_AddPersonContractRequest request)
        {
            var result = await _client.new_AddPersonContractAsync(request);

            return Ok(result);
        }
    }
}