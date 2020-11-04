using AutoMapper;
using ConferenceContractAPI.DBModels;
using ConferenceContractAPI.ViewModel;
using GrpcConferenceContractService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.AutoMapper
{
    public class Mappings
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(config =>
            {
                CreateNewMap.CreateMapper(config);
                config.CreateMap<CCNumberConfig, CCNumberConfigStruct>();
                config.CreateMap<CCNumberConfigStruct, CCNumberConfig>();

                config.CreateMap<ModifyRequestVM, ModifyRequest>();
                config.CreateMap<ModifyRequest, ModifyRequestVM>();

                config.CreateMap<ContractType, ContractTypeStruct>()
                     .ForMember(dest => dest.Translation, opt => opt.MapFrom(dto => new TranslationStruct()
                     {
                         CN = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).CN,
                         EN = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).EN,
                         JP = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).JP
                     }));

                config.CreateMap<ContractTypeStruct, ContractType>()
                      .BeforeMap(((src, dest) => dest.Translation = JsonConvert.SerializeObject(src.Translation)));


                config.CreateMap<CompanyContract, CompanyContractStruct>()
                .ForMember(dest => dest.ComNameTranslation, opt => opt.MapFrom(dto => new ComNameTranslationStruct()
                {
                    CompanyCN = JsonConvert.DeserializeObject<ComNameTranslationStruct>(dto.ComNameTranslation).CompanyCN,
                    CompanyEN = JsonConvert.DeserializeObject<ComNameTranslationStruct>(dto.ComNameTranslation).CompanyEN
                }))
                .ForMember(dest => dest.AddressTranslation, opt => opt.MapFrom(dto => new AddressTranslationStruct()
                {
                    AddressCN = JsonConvert.DeserializeObject<AddressTranslationStruct>(dto.AddressTranslation).AddressCN,
                    AddressEN = JsonConvert.DeserializeObject<AddressTranslationStruct>(dto.AddressTranslation).AddressEN
                }));

                config.CreateMap<CompanyContractStruct, CompanyContract>()
                      .BeforeMap(((src, dest) => dest.ComNameTranslation = JsonConvert.SerializeObject(src.ComNameTranslation)))
                      .BeforeMap(((src, dest) => dest.AddressTranslation = JsonConvert.SerializeObject(src.AddressTranslation)));


                config.CreateMap<ConferenceContract, ConferenceContractStruct>()
                        .ForMember(dest => dest.ComNameTranslation, opt => opt.MapFrom(dto => new ComNameTranslationStruct()
                        {
                            CompanyCN = JsonConvert.DeserializeObject<ComNameTranslationStruct>(dto.ComNameTranslation).CompanyCN,
                            CompanyEN = JsonConvert.DeserializeObject<ComNameTranslationStruct>(dto.ComNameTranslation).CompanyEN
                        }));

                config.CreateMap<ConferenceContractStruct, ConferenceContract>()
                      .BeforeMap(((src, dest) => dest.ComNameTranslation = JsonConvert.SerializeObject(src.ComNameTranslation)));

                //                config.CreateMap<CompanyContract, CompanyContractStructWithOther>()
                //.ForMember(dest => dest.ComNameTranslation, opt => opt.MapFrom(dto => new ComNameTranslationStruct()
                //{
                //    CompanyCN = JsonConvert.DeserializeObject<ComNameTranslationStruct>(dto.ComNameTranslation).CompanyCN,
                //    CompanyEN = JsonConvert.DeserializeObject<ComNameTranslationStruct>(dto.ComNameTranslation).CompanyEN
                //}))
                //.ForMember(dest => dest.AddressTranslation, opt => opt.MapFrom(dto => new AddressTranslationStruct()
                //{
                //    AddressCN = JsonConvert.DeserializeObject<AddressTranslationStruct>(dto.AddressTranslation).AddressCN,
                //    AddressEN = JsonConvert.DeserializeObject<AddressTranslationStruct>(dto.AddressTranslation).AddressEN
                //}));

                //                config.CreateMap<CompanyContractStructWithOther, CompanyContract>()
                //                      .BeforeMap(((src, dest) => dest.ComNameTranslation = JsonConvert.SerializeObject(src.ComNameTranslation)))
                //                      .BeforeMap(((src, dest) => dest.AddressTranslation = JsonConvert.SerializeObject(src.AddressTranslation)));


                config.CreateMap<CompanyServicePack, CompanyServicePackStruct>()
                     .ForMember(dest => dest.Translation, opt => opt.MapFrom(dto => new TranslationStruct()
                     {
                         CN = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).CN,
                         EN = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).EN,
                         JP = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).JP
                     }))
                     .ForMember(dest => dest.RemarkTranslation, opt => opt.MapFrom(dto => new TranslationStruct()
                     {
                         CN = JsonConvert.DeserializeObject<TranslationStruct>(dto.RemarkTranslation).CN,
                         EN = JsonConvert.DeserializeObject<TranslationStruct>(dto.RemarkTranslation).EN,
                         JP = JsonConvert.DeserializeObject<TranslationStruct>(dto.RemarkTranslation).JP
                     }));

                config.CreateMap<CompanyServicePackStruct, CompanyServicePack>()
                      .BeforeMap(((src, dest) => dest.Translation = JsonConvert.SerializeObject(src.Translation)));

                config.CreateMap<ServicePack, ServicePackStruct>()
                     .ForMember(dest => dest.Translation, opt => opt.MapFrom(dto => new TranslationStruct()
                     {
                         CN = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).CN,
                         EN = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).EN,
                         JP = JsonConvert.DeserializeObject<TranslationStruct>(dto.Translation).JP
                     }));

                config.CreateMap<ServicePackStruct, ServicePack>()
                      .BeforeMap(((src, dest) => dest.Translation = JsonConvert.SerializeObject(src.Translation)));

                config.CreateMap<CompanyServicePackVM, CompanyServicePackVMStruct>();
                config.CreateMap<CompanyServicePackVMStruct, CompanyServicePackVM>();

                config.CreateMap<ServicePackVM, ServicePackVMStruct>();
                config.CreateMap<ServicePackVMStruct, ServicePackVM>();

                config.CreateMap<ActivityVM, ActivityStruct>()
                     .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(dto => new TranslationStruct()
                     {
                         CN = JsonConvert.DeserializeObject<TranslationStruct>(dto.ActivityName).CN,
                         EN = JsonConvert.DeserializeObject<TranslationStruct>(dto.ActivityName).EN,
                         JP = JsonConvert.DeserializeObject<TranslationStruct>(dto.ActivityName).JP
                     }))
                     .ForMember(dest => dest.SessionConferenceName, opt => opt.MapFrom(dto => new TranslationStruct()
                     {
                         CN = JsonConvert.DeserializeObject<TranslationStruct>(dto.SessionConferenceName).CN,
                         EN = JsonConvert.DeserializeObject<TranslationStruct>(dto.SessionConferenceName).EN,
                         JP = JsonConvert.DeserializeObject<TranslationStruct>(dto.SessionConferenceName).JP
                     }));

                config.CreateMap<ActivityStruct, ActivityVM>()
                      .BeforeMap(((src, dest) => dest.ActivityName = JsonConvert.SerializeObject(src.ActivityName)));

                //config.CreateMap<ActivityVM, ActivityStruct>();
                //config.CreateMap<ActivityStruct, ActivityVM>();

                config.CreateMap<DelegateServicePackDiscount, DelegateServicePackDiscountListStruct>();
                config.CreateMap<DelegateServicePackDiscountListStruct, DelegateServicePackDiscount>();

                config.CreateMap<DelegateServicePackDiscount, DelegateServicePackDiscountStruct>();
                config.CreateMap<DelegateServicePackDiscountStruct, DelegateServicePackDiscount>();

                config.CreateMap<DelegateServicePackDiscountForConferenceContract, DelegateServicePackDiscountForConferenceContractListStruct>();
                config.CreateMap<DelegateServicePackDiscountForConferenceContractListStruct, DelegateServicePackDiscountForConferenceContract>();

                config.CreateMap<DelegateServicePackDiscountForConferenceContract, DelegateServicePackDiscountForConferenceContractStruct>();
                config.CreateMap<DelegateServicePackDiscountForConferenceContractStruct, DelegateServicePackDiscountForConferenceContract>();

                config.CreateMap<PersonContract, PersonContractStruct>()
                    .ForMember(dest => dest.MemTranslation, opt => opt.MapFrom(dto => new MemTranslationStruct()
                    {
                        MemberCN = JsonConvert.DeserializeObject<MemTranslationStruct>(dto.MemTranslation).MemberCN,
                        MemberEN = JsonConvert.DeserializeObject<MemTranslationStruct>(dto.MemTranslation).MemberEN
                    }));

                config.CreateMap<PersonContractStruct, PersonContract>()
                      .BeforeMap(((src, dest) => dest.MemTranslation = JsonConvert.SerializeObject(src.MemTranslation)));

                config.CreateMap<ExtraService, ExtraServiceStruct>()
                                    .ForMember(dest => dest.MemTranslation, opt => opt.MapFrom(dto => new MemTranslationStruct()
                                    {
                                        MemberCN = JsonConvert.DeserializeObject<MemTranslationStruct>(dto.MemTranslation).MemberCN,
                                        MemberEN = JsonConvert.DeserializeObject<MemTranslationStruct>(dto.MemTranslation).MemberEN
                                    }));
                config.CreateMap<ExtraServiceStruct, ExtraService>()
                                      .BeforeMap(((src, dest) => dest.MemTranslation = JsonConvert.SerializeObject(src.MemTranslation)));

                config.CreateMap<ExtraServiceVM, ExtraServiceVMStruct>();
                config.CreateMap<ExtraServiceVMStruct, ExtraServiceVM>();

                config.CreateMap<ContractStatusDic, ContractStatusDicStruct>();
                config.CreateMap<ContractStatusDicStruct, ContractStatusDic>();

                config.CreateMap<ModifyReplyForCreateOther, ModifyReplyForCreateOtherVM>();
                config.CreateMap<ModifyReplyForCreateOtherVM, ModifyReplyForCreateOther>();

                config.CreateMap<ApplyConference, ApplyConferenceStruct>();
                config.CreateMap<ApplyConferenceStruct, ApplyConference>();

                config.CreateMap<PersonContractActivityMap, PersonContractActivityMapStruct>();
                config.CreateMap<PersonContractActivityMapStruct, PersonContractActivityMap>();

                config.CreateMap<CompanyServicePackMap, CompanyServicePackMapStruct>();
                config.CreateMap<CompanyServicePackMapStruct, CompanyServicePackMap>();

                config.CreateMap<InviteLetter, InviteLetterStruct>();
                config.CreateMap<InviteLetterStruct, InviteLetter>();

                config.CreateMap<TagType, TagTypeStruct>();
                config.CreateMap<TagTypeStruct, TagType>();

                config.CreateMap<YearConfig, YearConfigStruct>();
                config.CreateMap<YearConfigStruct, YearConfig>();

                config.CreateMap<ContractStatistics, ContractStatisticsStruct>();
                config.CreateMap<ContractStatisticsStruct, ContractStatistics>();

                config.CreateMap<ModifyCCPCOwerInfo, ModifyCCPCOwerInfoStruct>();
                config.CreateMap<ModifyCCPCOwerInfoStruct, ModifyCCPCOwerInfo>();

                config.CreateMap<BoolReply, BoolReplyVM>();
                config.CreateMap<BoolReplyVM, BoolReply>();

                config.CreateMap<PersonContractPCNoStruct, PersonContractPCNoVM>();
                config.CreateMap<PersonContractPCNoVM, PersonContractPCNoStruct>();

                config.CreateMap<ConferenceOnsite, ConferenceOnsiteStruct>();
                config.CreateMap<ConferenceOnsiteStruct, ConferenceOnsite>();

                config.CreateMap<PersonContractAndSessionConferenceIdListVM, PersonContractAndSessionConferenceIdsStruct>();
                config.CreateMap<PersonContractAndSessionConferenceIdsStruct, PersonContractAndSessionConferenceIdListVM>();

                config.CreateMap<CheckInRequest, CheckInRequestVM>();
                config.CreateMap<CheckInRequestVM, CheckInRequest>();

                config.CreateMap<InviteCode, InviteCodeStruct>();
                config.CreateMap<InviteCodeStruct, InviteCode>();

                config.CreateMap<InviteCodeRecord, InviteCodeRecordStruct>();
                config.CreateMap<InviteCodeRecordStruct, InviteCodeRecord>();

                config.CreateMap<InviteCodeCSPVM, InviteCodeCSPVMStruct>();
                config.CreateMap<InviteCodeCSPVMStruct, InviteCodeCSPVM>();

                config.CreateMap<RemarkDic, RemarkDicStruct>();
                config.CreateMap<RemarkDicStruct, RemarkDic>();
            });


        }
    }
}
