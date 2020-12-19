using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcConferenceContractServiceNew;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GrpcConferenceContractServiceNew.ConferenceContractServiceNew;

namespace TestCCDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceContractController : ControllerBase
    {
        private Channel _channel;
        private ConferenceContractServiceNewClient _client;


        public ConferenceContractController()
        {
            //_channel = new Channel("127.0.0.1:40001", ChannelCredentials.Insecure);
            _channel = new Channel("conferencecontractserver:40001", ChannelCredentials.Insecure);
            _client = new ConferenceContractServiceNewClient(_channel);
        }

        //[HttpPost(nameof(new_GetCompanyContractList1))]
        //public async Task<IActionResult> new_GetCompanyContractList1(new_GetCompanyContractList1Request request)
        //{
        //    var result = await _client.new_GetCompanyContractList1Async(request);
        //    return Ok(result);
        //}

        [HttpPost(nameof(newAddSchedule))]
        public async Task<IActionResult> newAddSchedule(newAddScheduleRequest request)
        {
            var result = await _client.newAddScheduleAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newGetServicePack))]
        public async Task<IActionResult> newGetServicePack(newGetServicePackRequest request)
        {
            var result = await _client.newGetServicePackAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newAddCompanyContract))]
        public async Task<IActionResult> newAddCompanyContract(newServicePackStruct request)
        {
            var result = await _client.newAddCompanyContractAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newAddCompanyContractDiscount))]
        public async Task<IActionResult> newAddCompanyContractDiscount(newServicePackStructDiscount request)
        {
            var result = await _client.newAddCompanyContractDiscountAsync(request);

            return Ok(result);
        }



        [HttpPost(nameof(newAddPersonContract))]
        public async Task<IActionResult> newAddPersonContract(newAddPersonContractRequest request)
        {
            var result = await _client.newAddPersonContractAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newAddPersonContractUnderCompanyContract))]
        public async Task<IActionResult> newAddPersonContractUnderCompanyContract(newAddPersonContractUnderCompanyContractRequest request)
        {
            var result = await _client.newAddPersonContractUnderCompanyContractAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newMergeCompanyContract))]
        public async Task<IActionResult> newMergeCompanyContract(newMergeCompanyContractRequest request)
        {
            var result = await _client.newMergeCompanyContractAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newMergeCompanyContractDiscount))]
        public async Task<IActionResult> newMergeCompanyContractDiscount(newMergeCompanyContractRequest request)
        {
            var result = await _client.newMergeCompanyContractDiscountAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newGetConferenceInfo))]
        public async Task<IActionResult> newGetConferenceInfo(newGetConferenceInfoRequest request)
        {
            var result = await _client.newGetConferenceInfoAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newGetOwnerByConfIdAndComIdAndYear))]
        public async Task<IActionResult> newGetOwnerByConfIdAndComIdAndYear(newGetOwnerByConfIdAndComIdAndYearRequest request)
        {
            var result = await _client.newGetOwnerByConfIdAndComIdAndYearAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newGetConferenceContractList))]
        public async Task<IActionResult> newGetConferenceContractList(newGetConferenceContractListRequest request)
        {
            var result = await _client.newGetConferenceContractListAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newGetCompanyContractList))]
        public async Task<IActionResult> newGetCompanyContractList(newGetCompanyContractListRequest request)
        {
            var result = await _client.newGetCompanyContractListAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newGetCompanyContractListByIsNotFullPerson))]
        public async Task<IActionResult> newGetCompanyContractListByIsNotFullPerson(newBoolRequest request)
        {
            var result = await _client.newGetCompanyContractListByIsNotFullPersonAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newGetCompanyContratcListByIsHaveDiscount))]
        public async Task<IActionResult> newGetCompanyContratcListByIsHaveDiscount(newBoolRequest request)
        {
            var result = await _client.newGetCompanyContratcListByIsHaveDiscountAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newGetCompanyContractListDiscount))]
        public async Task<IActionResult> newGetCompanyContractListDiscount(newGetCompanyContractListDiscountRequest request)
        {
            var result = await _client.newGetCompanyContractListDiscountAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newCreatePersonContractByCompanyContractInviteCode))]
        public async Task<IActionResult> newCreatePersonContractByCompanyContractInviteCode(newCreatePersonContractByCompanyContractInviteCodeRequest request)
        {
            var result = await _client.newCreatePersonContractByCompanyContractInviteCodeAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newCreatePersonContractByInviteCode))]
        public async Task<IActionResult> newCreatePersonContractByInviteCode(newCreatePersonContractByInviteCodeRequest request)
        {
            var result = await _client.newCreatePersonContractByInviteCodeAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newModifyOwnerByConferenceContractId))]
        public async Task<IActionResult> newModifyOwnerByConferenceContractId(newModifyOwnerByConferenceContractIdRequest request)
        {
            var result = await _client.newModifyOwnerByConferenceContractIdAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newGetPersonContractList))]
        public async Task<IActionResult> newGetPersonContractList(newGetPersonContractListRequest request)
        {
            var result = await _client.newGetPersonContractListAsync(request);

            return Ok(result);
        }

        [HttpPost(nameof(newGetMySpeakersList))]
        public async Task<IActionResult> newGetMySpeakersList(newGetMySpeakersListRequest request)
        {
            var result = await _client.newGetMySpeakersListAsync(request);

            return Ok(result);
        }



        [HttpPost(nameof(newGetPersonContractListByComContratcId))]
        public async Task<IActionResult> newGetPersonContractListByComContratcId(newGetPersonContractListByComContratcIdRequest request)
        {
            var result = await _client.newGetPersonContractListByComContratcIdAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newModifyParticipantByAbstractParticipantPerContractNumber))]
        public async Task<IActionResult> newModifyParticipantByAbstractParticipantPerContractNumber(PerContractNumberRequest request)
        {
            var result = await _client.newModifyParticipantByAbstractParticipantPerContractNumberAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newGetPersonReportNotice))]
        public async Task<IActionResult> newGetPersonReportNotice(newGetReportNoticeRequest request)
        {
            var result = await _client.newGetPersonReportNoticeAsync(request);

            return Ok(result);
        }


        [HttpPost(nameof(newGetComapnyServicePackType))]
        public async Task<IActionResult> newGetComapnyServicePackType(newGetComapnyServicePackTypeRequest request)
        {
            var result = await _client.newGetComapnyServicePackTypeAsync(request);

            return Ok(result);
        }
    }
}