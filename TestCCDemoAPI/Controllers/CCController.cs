using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcConferenceContractService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestCCDemoAPI.Controllers
{
    [Route("api/cc")]
    [ApiController]
    public class CCController : ControllerBase
    {

        [Route("getcomcontractlist")]
        [HttpGet]
        public IActionResult GetComContractList(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct()
            {
                //ComContractNumber= "CF2019SNEC0006CC",
                //ComNameTranslation = "testgaopeng",
                //ConferenceId = "a8e5ce7a-3543-460d-891f-118d5c4edfb3",
                //ContractTypeId= "ee00dc29-8556-4635-8fe3-80f2428f5b1e",
                //CcIsdelete = false
                //IsDiscount = false,
                //IsFillPC = false,
                //IsGive = false,
                //Year = "2020"
                //IsContractTypeWithECode = false,
                //CTypeCode = "AS,E"
                //ContractStatusCode="S1,W",
                //CompanyId= "60cec3ac-08ed-4dd9-bcd7-d35de69d2c43",
                //CompanyServicePackId= "7750eb23-2ba8-4762-b3c8-3ec38d68c5ae",
                //ComPrice="0",
                //Owerid= "6b43b32c-d12e-4617-b157-912d74daf4d7"
                //SessionConferenceId = "c7bdbb1b-888d-414a-a009-4a71a6f78435"
                //InviteCodeId = "43d68534-bc4e-435b-9e01-18272307fb3c"
                //InviteCodeNumber = "11111"
                ComContractNumber = "CF2020SNEC0303CS",
                Year = "2020"
            };
            //var result = CCServiceClient.GetDSPDList(pageindex, pagesize, search);
            //var result = CCServiceClient.GetContractStatusDic();
            //var result3 = CCServiceClient.GetDSPDFCCList(pageindex, pagesize, search);
            //var result = CCServiceClient.GetInviteCodeList(pageindex, pagesize, search); 
            var result = CCServiceClient.GetCompanyContractList(pageindex, pagesize, search);
            //var result = CCServiceClient.GetConferenceContractListByIsGiveWithAllContractStatusCode(pageindex, pagesize, search);
            //var result = CCServiceClient.GetInviteCodeByInviteCodeNumber(search);
            //var result = CCServiceClient.GetInviteCodeRecordList(pageindex, pagesize, search);
            //var result2 = CCServiceClient.GetConferenceContractListByIsGive(pageindex, pagesize, search);
            return Ok(result);
        }

        [Route("getcomcontractbyconferencecontractidlist/{id}")]
        [HttpGet]
        public IActionResult GetComContractByConferenceContractIdList(string id)
        {
            var result = CCServiceClient.GetCompanyContractByConferenceContractIdList(id);

            return Ok(result);
        }

        [Route("getcomcontractbycompanyidlist/{id}")]
        [HttpGet]
        public IActionResult GetComContractByCompanyIdList(string id)
        {
            var result = CCServiceClient.GetCompanyContractByCompanyIdList(id);

            return Ok(result);
        }

        [Route("getcomcontract/{id}")]
        [HttpGet]
        public IActionResult GetComContractById(string id)
        {
            //var result = CCServiceClient.GetDSPDById(id);
            var result = CCServiceClient.GetCompanyContractById(id);

            //SearchStruct search = new SearchStruct()
            //{
            //    PerContractNumber = "CF2019SNEC0001CC009"
            //};

            //var result = CCServiceClient.GetCompanyServicePackVMByPersonContractNumber(search);

            return Ok(result);
        }

        [Route("getcomservicepacklist")]
        [HttpGet]
        public IActionResult GetComSPList(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct
            {
                //Translation = "",
                //ConferenceId= "a8e5ce7a-3543-460d-891f-118d5c4edfb3",
                Year="2020"
            };

            var result = CCServiceClient.GetCompanyServicePackList(pageindex, pagesize, search);
            return Ok(result);
        }

        [Route("getcomservicepack/{id}")]
        [HttpGet]
        public IActionResult GetComSPById(string id)
        {
            var result = CCServiceClient.GetCompanyServicePackById(id);

            return Ok(result);
        }

        [Route("createccnumberconig")]
        [HttpPost]
        public IActionResult CreateCCNumberConfig([FromBody] CCNumberConfigStruct model)
        {
            var result = CCServiceClient.CreateCCNumberConfigInfo(model);
            return Ok(result);
        }

        [Route("createcomservicepack")]
        [HttpPost]
        public IActionResult CreateComSP([FromBody] CompanyServicePackVMStruct company)
        {
            var result = CCServiceClient.CreateCompanyServicePackInfo(company);
            return Ok(result);
        }

        [Route("updatecomservicepack")]
        [HttpPost]
        public IActionResult UpdateComSP([FromBody] CompanyServicePackVMStruct user)
        {
            var result = CCServiceClient.UpdateCompanyServicePackInfo(user);
            return Ok(result);
        }

        [Route("deletecomservicepackbyid/{id}")]
        [HttpDelete]
        public IActionResult DeleteComSP(string id)
        {
            //var result = RoleServiceClient.DeleteRoleById(id);
            var result = CCServiceClient.DeleteCompanyServicePackById(id);
            return Ok(result);
        }


        [Route("createcomcontract")]
        [HttpPost]
        public IActionResult CreateComContract([FromBody] CompanyContractStruct company)
        {
            var result = CCServiceClient.CreateCompanyContractInfo(company);
            return Ok(result);
        }

        [Route("updatecomcontract")]
        [HttpPost]
        public IActionResult UpdateComContract([FromBody] CompanyContractStruct company)
        {
            var result = CCServiceClient.UpdateCompanyContractInfo(company);
            return Ok(result);
        }

        // DELETE api/values/5
        [Route("deletecomcontractbyid/{id}")]
        [HttpDelete]
        public IActionResult DeleteComContract(string id)
        {
            var result = CCServiceClient.DeleteCompanyContractById(id);
            return Ok(result);
        }

        [Route("deletecomcontractbylist")]
        [HttpPost]
        public IActionResult DeleteComContractByList([FromBody] CompanyContractCidList list)
        {
            var result = CCServiceClient.DeleteCompanyContractByList(list);
            return Ok(result);
        }

        [Route("getcclist")]
        [HttpGet]
        public IActionResult Get(int pageindex, int pagesize)
        {
            var result = CCServiceClient.GetContractTypeList(pageindex, pagesize);

            return Ok(result);
        }

        [Route("getcc/{id}")]
        [HttpGet]
        public IActionResult GetCCById(string id)
        {
            var result = CCServiceClient.GetContractTypeById(id);

            return Ok(result);
        }

        [Route("createcc")]
        [HttpPost]
        public IActionResult CreateCC([FromBody] ContractTypeStruct company)
        {
            var result = CCServiceClient.CreateContractTypeInfo(company);
            return Ok(result);
        }

        [Route("getservicepackbycid/{id}")]
        [HttpGet]
        public IActionResult GetSPByCidList(string id)
        {
            var result = CCServiceClient.GetServicePackByConferenceIdList(id);

            return Ok(result);
        }

        [Route("getservicepacklist")]
        [HttpGet]
        public IActionResult GetSPList(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct();
            //{
            //    Translation = ""
            //};

            var result = CCServiceClient.GetServicePackList(pageindex, pagesize, search);
            var result2 = CCServiceClient.GetServicePackListAll();
            return Ok(result);
        }

        [Route("getservicepack/{id}")]
        [HttpGet]
        public IActionResult GetSPById(string id)
        {
            var result = CCServiceClient.GetServicePackById(id);

            return Ok(result);
        }

        [Route("createservicepackinfo")]
        [HttpPost]
        public IActionResult CreateSP([FromBody] ServicePackVMStruct company)
        {
            var result = CCServiceClient.CreateServicePackInfo(company);
            return Ok(result);
        }

        [Route("updateservicepack")]
        [HttpPost]
        public IActionResult UpdateSP([FromBody] ServicePackVMStruct user)
        {
            var result = CCServiceClient.UpdateServicePackInfo(user);
            return Ok(result);
        }

        [Route("deleteservicepackbyid/{id}")]
        [HttpDelete]
        public IActionResult DeleteSP(string id)
        {
            var result = CCServiceClient.DeleteServicePackById(id);
            return Ok(result);
        }

        [Route("iscandeleteacitvity/{id}")]
        [HttpGet]
        public IActionResult IsCanDeleteAcitvity(string id)
        {
            var result = CCServiceClient.IsCanDeleteAcitvity(id);

            return Ok(result);
        }

        [Route("createdspd")]
        [HttpPost]
        public IActionResult CreateDSPD([FromBody] DelegateServicePackDiscountStruct company)
        {
            var result = CCServiceClient.CreateDSPDInfo(company);
            return Ok(result);
        }

        [Route("updatedspd")]
        [HttpPost]
        public IActionResult UpdateDSPD([FromBody] DelegateServicePackDiscountStruct user)
        {
            var result = CCServiceClient.UpdateDSPDInfo(user);
            return Ok(result);
        }

        [Route("deletedspdbyid/{id}")]
        [HttpDelete]
        public IActionResult DeleteDSPD(string id)
        {
            var result = CCServiceClient.DeleteDSPDById(id);
            return Ok(result);
        }


        [Route("getpclist")]
        [HttpGet]
        public IActionResult GetPCList(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct()
            {
                //MemTranslation= "张冰洁",
                //MemberPK = "9485c410-a742-477e-8291-3600abf0e8e5",
                //CompanyServicePackId = "7410f4cb-d1b0-4e66-a8f4-67ac0914e678,37d9fc6f-870a-4102-9d04-7229330edaa3,37d9fc6f-870a-4102-9d04-7229330edaa3,b4cd9af4-db28-4ee6-b3b3-c4d54cd50dfb,7b7f2ec2-8cbd-4f30-9580-e8470375f456,b5eeeaac-b692-4ce9-ab99-a4a5bcb05daf,a1400684-6169-47e5-8006-91a1d94106b2",
                //Year = "2020"               
                //PcIsdelete = true,
                //CompanyId= "99e21994-0b7a-48f8-a2ef-012b0ec6be32",
                //MemTranslation="ning165"
                //IsGive = false
                //ExTypeCode = "esh"
                //SessionConferenceId = "c9b3aa72-e6b8-4fd3-83ee-baf72c293a45," +
                //"5154ed2f-445a-4cc6-94de-addb8efa1040," +
                //"8e7f585a-6a4a-41c8-8637-bfea82535642," +
                //"e6793a90-961c-4c9e-870a-70db4aff264f," +
                //"18b48377-98f3-4cd3-a9a9-6c4bb91c3ddd," +
                //"43a83692-440c-4588-8f00-500bd6acc615"
                //SessionConferenceId = "c9b3aa72-e6b8-4fd3-83ee-baf72c293a45"
            };
            //search.ContractId = "de0bcd31-ada3-4fb0-9daf-754721d7afa3";
            var result = CCServiceClient.GetPersonContractList(pageindex, pagesize, search);
            //var result = CCServiceClient.GetApplyConferenceBySessionConferenceIdAndTagTypeCodeList(pageindex, pagesize, search);
            //var result = CCServiceClient.GetPersonContractByNewList(pageindex, pagesize, search);
            //var result2 = CCServiceClient.GetContractStatisticsList(search);
            //var result3 = CCServiceClient.GetPersonContractByMemberPKListWithNoPagination(search);
            //var result = CCServiceClient.GetPersonContractByContractIdList(pageindex, pagesize, search);
            //search.MemberPK = "42f52dc6-9ded-47ed-aa13-9a10927a9995";
            //var result = CCServiceClient.GetPersonContractByMemberPKList(pageindex, pagesize, search);
            return Ok(result);
        }

        [Route("getPersonContractListForLunch")]
        [HttpGet]
        public IActionResult GetPersonContractListForLunch(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct()
            {
                //MemTranslation= "张冰洁",
                //MemberPK = "9485c410-a742-477e-8291-3600abf0e8e5",
                //CompanyServicePackId = "7410f4cb-d1b0-4e66-a8f4-67ac0914e678,37d9fc6f-870a-4102-9d04-7229330edaa3,37d9fc6f-870a-4102-9d04-7229330edaa3,b4cd9af4-db28-4ee6-b3b3-c4d54cd50dfb,7b7f2ec2-8cbd-4f30-9580-e8470375f456,b5eeeaac-b692-4ce9-ab99-a4a5bcb05daf,a1400684-6169-47e5-8006-91a1d94106b2",
                //Year = "2020",
                //PcIsdelete = true,
                //CompanyId= "99e21994-0b7a-48f8-a2ef-012b0ec6be32",
                //MemTranslation="ning165"
                //IsGive = false
                //ExTypeCode = "esh"
                //SessionConferenceId = "c9b3aa72-e6b8-4fd3-83ee-baf72c293a45," +
                //"5154ed2f-445a-4cc6-94de-addb8efa1040," +
                //"8e7f585a-6a4a-41c8-8637-bfea82535642," +
                //"e6793a90-961c-4c9e-870a-70db4aff264f," +
                //"18b48377-98f3-4cd3-a9a9-6c4bb91c3ddd," +
                //"43a83692-440c-4588-8f00-500bd6acc615,"
                 SessionConferenceId = "b1afb83a-ee44-4350-bd1d-08cdfeeb5567," +
                "d635507b-f3b8-4989-a92f-82ff75766f5a," +
                "302632b2-bd95-4cb5-8e6a-f3c0da8f1629," +
                "1a3dc0d2-c36d-4584-8638-e21313058ae4," 
                //SessionConferenceId = "c9b3aa72-e6b8-4fd3-83ee-baf72c293a45"
                //SessionConferenceId = "c9b3aa72-e6b8-4fd3-83ee-baf72c293a45," +
                //"5154ed2f-445a-4cc6-94de-addb8efa1040,"+
                //"8e7f585a-6a4a-41c8-8637-bfea82535642,"
            };
            var result = CCServiceClient.GetPersonContractListForLunch(pageindex, pagesize, search);

            return Ok(result);
        }

        [Route("getpc/{id}")]
        [HttpGet]
        public IActionResult GetPC(string id)
        {
            var result = CCServiceClient.GetPersonContractById(id);

            return Ok(result);
        }


        [Route("createpc")]
        [HttpPost]
        public IActionResult CreatePC([FromBody] PersonContractStruct company)
        {
            var result = CCServiceClient.CreatePersonContractInfo(company);
            return Ok(result);
        }

        [Route("createConferenceOnsiteInfo")]
        [HttpPost]
        public IActionResult CreateConferenceOnsiteInfo([FromBody] ConferenceOnsiteStruct model)
        {
            var result = CCServiceClient.CreateConferenceOnsiteInfo(model);
            return Ok(result);
        }

        [Route("updatepc")]
        [HttpPost]
        public IActionResult UpdatePC([FromBody] PersonContractStruct user)
        {            
            var result = CCServiceClient.UpdatePersonContractInfo(user);
            return Ok(result);
        }

        [Route("updateConferenceOnsiteInfo")]
        [HttpPost]
        public IActionResult UpdateConferenceOnsiteInfo([FromBody] ConferenceOnsiteStruct user)
        {
            var result = CCServiceClient.UpdateConferenceOnsiteInfo(user);

            return Ok(result);
        }

        [Route("deletepcbyid/{id}")]
        [HttpDelete]
        public IActionResult DeletePC(string id)
        {
            var result = CCServiceClient.DeletePersonContractById(id);
            return Ok(result);
        }

        [Route("geteslist")]
        [HttpGet]
        public IActionResult GetESList(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct();
            //{
            //    Translation = ""
            //};
            var result = CCServiceClient.GetExtraServiceList(pageindex, pagesize, search);
            return Ok(result);
        }

        [Route("getesbyid/{id}")]
        [HttpGet]
        public IActionResult GetESById(string id)
        {
            var result = CCServiceClient.GetExtraServiceById(id);

            return Ok(result);
        }

        [Route("createes")]
        [HttpPost]
        public IActionResult CreateES([FromBody] ExtraServiceVMStruct company)
        {
            var result = CCServiceClient.CreateExtraServiceInfo(company);
            return Ok(result);
        }

        [Route("updatees")]
        [HttpPost]
        public IActionResult UpdateES([FromBody] ExtraServiceVMStruct user)
        {
            var result = CCServiceClient.UpdateExtraServiceInfo(user);
            return Ok(result);
        }

        [Route("deleteesbyid/{id}")]
        [HttpDelete]
        public IActionResult DeleteES(string id)
        {
            var result = CCServiceClient.DeleteExtraServiceById(id);
            return Ok(result);
        }

        [Route("getPersonContractListAndApplyConference")]
        [HttpGet]
        public IActionResult GetPersonContractListAndApplyConference(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct()
            {
                //MemTranslation= "张冰洁",
                //MemberPK = "9485c410-a742-477e-8291-3600abf0e8e5",
                //CompanyServicePackId = "7410f4cb-d1b0-4e66-a8f4-67ac0914e678,37d9fc6f-870a-4102-9d04-7229330edaa3,37d9fc6f-870a-4102-9d04-7229330edaa3,b4cd9af4-db28-4ee6-b3b3-c4d54cd50dfb,7b7f2ec2-8cbd-4f30-9580-e8470375f456,b5eeeaac-b692-4ce9-ab99-a4a5bcb05daf,a1400684-6169-47e5-8006-91a1d94106b2",
                //Year = "2020",
                //PcIsdelete = true,
                //CompanyId= "99e21994-0b7a-48f8-a2ef-012b0ec6be32",
                //MemTranslation="ning165"
                //IsGive = false
                //ExTypeCode = "esh"
                //SessionConferenceId = "c9b3aa72-e6b8-4fd3-83ee-baf72c293a45," +
                //"5154ed2f-445a-4cc6-94de-addb8efa1040," +
                //"8e7f585a-6a4a-41c8-8637-bfea82535642," +
                //"e6793a90-961c-4c9e-870a-70db4aff264f," +
                //"18b48377-98f3-4cd3-a9a9-6c4bb91c3ddd," +
                //"43a83692-440c-4588-8f00-500bd6acc615"
                //SessionConferenceId = "c9b3aa72-e6b8-4fd3-83ee-baf72c293a45"
            };

            var result = CCServiceClient.GetPersonContractListAndApplyConference(pageindex, pagesize, search);

            return Ok(result);
        }

        [Route("modifyPersonContractIsCheckInByIdList")]
        [HttpPost]
        public IActionResult ModifyPersonContractIsCheckInByIdList([FromBody] CheckInRequest model)
        {
            var result = CCServiceClient.ModifyPersonContractIsCheckInByIdList(model);
            return Ok(result);
        }

        //[Route("createPersonContractActivityMapImport")]
        //[HttpPost]
        //public IActionResult CreatePersonContractActivityMapImport([FromBody] Empty model)
        //{
        //    var result = CCServiceClient.CreatePersonContractActivityMapImport();
        //    return Ok(result);
        //}

        [Route("exportPersonContractList")]
        [HttpGet]
        public IActionResult ExportPersonContractList()
        {
            SearchStruct search = new SearchStruct()
            {
                //ComContractNumber= "CF2019SNEC0006CC",
                //ComNameTranslation = "testgaopeng",
                //ConferenceId = "a8e5ce7a-3543-460d-891f-118d5c4edfb3",
                //ContractTypeId= "ee00dc29-8556-4635-8fe3-80f2428f5b1e",
                //CcIsdelete = false
                //IsDiscount = false,
                //IsFillPC = false,
                //IsGive = false,
                Year = "2020",
                //IsContractTypeWithECode = false,
                //CTypeCode = "AS,E"
                //ContractStatusCode="S1,W",
                //CompanyId= "60cec3ac-08ed-4dd9-bcd7-d35de69d2c43",
                //CompanyServicePackId= "7750eb23-2ba8-4762-b3c8-3ec38d68c5ae",
                //ComPrice="0",
                Owerid= "65dddd36-971f-4f75-ac93-7830e3ce4c3b"
                //SessionConferenceId = "c7bdbb1b-888d-414a-a009-4a71a6f78435"
                //InviteCodeId = "43d68534-bc4e-435b-9e01-18272307fb3c"
                //InviteCodeNumber = "11111"

            };

            var result = CCServiceClient.ExportPersonContractList(search);

            return Ok(result);
        }

        #region ConferenceContract

        [Route("getconferencecontractlist")]
        [HttpGet]
        public IActionResult GetConferenceContractList(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct()
            {
                //ComContractNumber= "CF2019SNEC0006CC",
                //ComNameTranslation = "testgaopeng",
                //ConferenceId = "a8e5ce7a-3543-460d-891f-118d5c4edfb3",
                //ContractTypeId= "0dc7655a-bc5e-4079-a85e-d0b9b6dcde99",
                //CcIsdelete = false
                //IsDiscount = false
                //Year = "2020"
            };

            var result = CCServiceClient.GetConferenceContractList(pageindex, pagesize, search);
            //var result = CCServiceClient.GetConferenceContractListByIsGive(pageindex, pagesize, search);
            return Ok(result);
        }

        [Route("getconferencecontractbycompanyidlist/{id}")]
        [HttpGet]
        public IActionResult GetConferenceContractByCompanyIdList(string id)
        {
            var result = CCServiceClient.GetConferenceContractByCompanyIdList(id);

            return Ok(result);
        }

        [Route("getconferencecontract/{id}")]
        [HttpGet]
        public IActionResult GetConferenceContractById(string id)
        {
            var result = CCServiceClient.GetConferenceContractById(id);

            return Ok(result);
        }


        [Route("createconferencecontract")]
        [HttpPost]
        public IActionResult CreateConferenceContract([FromBody] ConferenceContractStruct company)
        {
            var result = CCServiceClient.CreateConferenceContractInfo(company);
            return Ok(result);
        }

        [Route("updateconferencecontract")]
        [HttpPost]
        public IActionResult UpdateConferenceContract([FromBody] ConferenceContractStruct company)
        {
            var result = CCServiceClient.UpdateConferenceContractInfo(company);
            return Ok(result);
        }

        [Route("deleteconferencecontractbyid/{id}")]
        [HttpDelete]
        public IActionResult DeleteConferenceContract(string id)
        {
            var result = CCServiceClient.DeleteConferenceContractById(id);
            return Ok(result);
        }

        [Route("deleteconferencecontractbylist")]
        [HttpPost]
        public IActionResult DeleteConferenceContractByList([FromBody] ConferenceContractCidList list)
        {
            var result = CCServiceClient.DeleteConferenceContractByList(list);
            return Ok(result);
        }

        [Route("getPersonContractActivityMapByMemberPKList")]
        [HttpGet]
        public IActionResult GetPersonContractActivityMapByMemberPKList()
        {
            SearchStruct search = new SearchStruct()
            {
                MemberPK = "a34af51d-185f-40a3-8739-136ead41954a",
                Year = "2020"
            };
            var result = CCServiceClient.GetPersonContractActivityMapByMemberPKList(search);

            return Ok(result);
        }

        [Route("getPersonContractActivityMapByActivityIdList")]
        [HttpGet]
        public IActionResult GetPersonContractActivityMapByActivityIdList()
        {
            SearchStruct search = new SearchStruct()
            {
                ActivityId = "4cadc79b-258f-43a6-b177-a80e2c632f80",
                PerContractNumber = "CF2019SNEC0001CW02001",
                Year = "2019"
            };
            var result = CCServiceClient.GetPersonContractActivityMapByActivityIdList(search);
            var result2 = CCServiceClient.GetPersonContractActivityMapByPersonContractNumberList(search);
            return Ok(result);
        }

        [Route("createPersonContractActivityMapInfo")]
        [HttpPost]
        public IActionResult CreatePersonContractActivityMapInfo([FromBody] PersonContractActivityMapList list)
        {
            var result = CCServiceClient.CreatePersonContractActivityMapInfo(list);
            return Ok(result);
        }

        [Route("updatePersonContractActivityMapInfo")]
        [HttpPost]
        public IActionResult UpdatePersonContractActivityMapInfo([FromBody] PersonContractActivityMapListToUpdate list)
        {
            var result = CCServiceClient.UpdatePersonContractActivityMapInfo(list);
            return Ok(result);
        }

        //[Route("getApplyConferenceByPerContractIdList/{id}")]
        //[HttpGet]
        //public IActionResult GetApplyConferenceByPerContractIdList(string id)
        //{
        //    var result = CCServiceClient.GetApplyConferenceByPerContractIdList(id);

        //    return Ok(result);
        //}

        [Route("getApplyConferenceBySessionConferenceIdList/{id}")]
        [HttpGet]
        public IActionResult GetApplyConferenceBySessionConferenceIdList(string id)
        {
            var result = CCServiceClient.GetApplyConferenceBySessionConferenceIdList(id);

            return Ok(result);
        }

        [Route("getApplyConferenceBySessionConferenceIdListPagination")]
        [HttpGet]
        public IActionResult GetApplyConferenceBySessionConferenceIdListPagination(int pageindex, int pagesize)
        {
            SearchStruct search = new SearchStruct()
            {
                //ComContractNumber= "CF2019SNEC0006CC",
                //ComNameTranslation = "乐密",
                //ConferenceId = "a8e5ce7a-3543-460d-891f-118d5c4edfb3",
                //ContractTypeId= "ee00dc29-8556-4635-8fe3-80f2428f5b1e",
                //CcIsdelete = false
                //IsDiscount = false
                //IsFillPC = true
                SessionConferenceId = "e5618b19-c2ce-4f74-b2ca-4998d32591a2,9e5f4bea-9603-47b9-8940-9bce798af6d3,a8ec27e7-447f-49ab-a545-7aed9f5865ff"
            };

            var result = CCServiceClient.GetApplyConferenceBySessionConferenceIdListPagination(pageindex, pagesize, search);
            return Ok(result);
        }

        [Route("createOrUpdateApplyConferenceInfo")]
        [HttpPost]
        public IActionResult CreateOrUpdateApplyConferenceInfo([FromBody] ApplyConferenceListToCreateOrUpdate list)
        {
            var result = CCServiceClient.CreateOrUpdateApplyConferenceInfo(list);
            return Ok(result);
        }

        [Route("copyPackInfoByYear")]
        [HttpPost]
        public IActionResult CopyPackInfoByYear([FromBody] SearchStruct model)
        {
            var result = CCServiceClient.CopyPackInfoByYear(model);
            return Ok(result);
        }

        //[Route("copyPackInfoByYearForESH")]
        //[HttpPost]
        //public IActionResult CopyPackInfoByYearForESH([FromBody] SearchStruct model)
        //{
        //    var result = CCServiceClient.CopyPackInfoByYearForESH(model);
        //    return Ok(result);
        //}

        [Route("createCompanyServicePackMap")]
        [HttpPost]
        public IActionResult CreateCompanyServicePackMap([FromBody] CompanyServicePackMapList list)
        {
            var result = CCServiceClient.CreateCompanyServicePackMap(list);
            return Ok(result);
        }

        [Route("modifyPersonContractIsFianceRecord")]
        [HttpPost]
        public IActionResult ModifyPersonContractIsFianceRecord([FromBody] PersonContractPCNoRequest model)
        {
            var result = CCServiceClient.ModifyPersonContractIsFianceRecord(model);
            return Ok(result);
        }

        #endregion
    }
}
