using AutoMapper;
using ConferenceContractAPI.DBModels;
using GrpcConferenceContractServiceNew;
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
            config.CreateMap<newServicePackStruct, CompanyContract>().ReverseMap();

            config.CreateMap<newServicePackStruct, ConferenceContract>().ReverseMap();

        }
    }
}
