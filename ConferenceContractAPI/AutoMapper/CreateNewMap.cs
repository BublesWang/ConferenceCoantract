using AutoMapper;
using ConferenceContractAPI.DBModels;
using GrpcConferenceContractService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.AutoMapper
{
    public class CreateNewMap
    {
        public static void CreateMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<new_ServicePackStruct, CompanyContract>().ReverseMap();

            config.CreateMap<new_ServicePackStruct, ConferenceContract>();

        }
    }
}
