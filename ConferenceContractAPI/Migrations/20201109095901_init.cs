using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConferenceContractAPI.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyConference",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonContractId = table.Column<string>(maxLength: 150, nullable: true),
                    CompanyId = table.Column<string>(maxLength: 150, nullable: true, defaultValue: ""),
                    MemberPK = table.Column<string>(maxLength: 150, nullable: true),
                    SessionConferenceId = table.Column<string>(maxLength: 150, nullable: true),
                    IsConfirm = table.Column<bool>(nullable: true),
                    IsParticularConf = table.Column<bool>(nullable: true, defaultValue: false),
                    TagTypeCodes = table.Column<string>(maxLength: 40000, nullable: true, defaultValue: ""),
                    RemarkTranslation = table.Column<string>(maxLength: 40000, nullable: true, defaultValue: ""),
                    Year = table.Column<string>(maxLength: 150, nullable: true),
                    Owerid = table.Column<string>(maxLength: 500, nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyConference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CCNumberConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConferenceId = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    ConferenceName = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    Prefix = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    CNano = table.Column<string>(maxLength: 50, nullable: true),
                    Count = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CCNumberConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceContract",
                columns: table => new
                {
                    ConferenceContractId = table.Column<Guid>(nullable: false),
                    ConferenceId = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    CompanyId = table.Column<string>(maxLength: 500, nullable: true),
                    ComNameTranslation = table.Column<string>(maxLength: 500, nullable: true),
                    ContractNumber = table.Column<string>(maxLength: 150, nullable: true),
                    ContractYear = table.Column<string>(maxLength: 150, nullable: true),
                    ContractCode = table.Column<string>(maxLength: 50, nullable: true),
                    ContractStatusCode = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    PaymentStatusCode = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    ModifyPermission = table.Column<string>(maxLength: 150, nullable: true, defaultValue: "0"),
                    IsDelete = table.Column<bool>(nullable: true),
                    IsSendEmail = table.Column<bool>(nullable: true, defaultValue: false),
                    IsModify = table.Column<bool>(nullable: true, defaultValue: true),
                    Owerid = table.Column<string>(maxLength: 500, nullable: true),
                    Ower = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    OtherOwner = table.Column<string>(maxLength: 3000, nullable: true),
                    OtherOwnerId = table.Column<string>(maxLength: 3000, nullable: true),
                    TotalPrice = table.Column<string>(maxLength: 150, nullable: true),
                    TotalPaid = table.Column<string>(maxLength: 150, nullable: true),
                    IsOpPayStatudCode = table.Column<bool>(nullable: true),
                    EnterpriseType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceContract", x => x.ConferenceContractId);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceOnsite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ContractNumber = table.Column<string>(maxLength: 150, nullable: true),
                    UserName = table.Column<string>(maxLength: 150, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 150, nullable: true),
                    CompanyServicePackId = table.Column<string>(maxLength: 1500, nullable: true),
                    CompanyServicePackName = table.Column<string>(maxLength: 1500, nullable: true),
                    Currency = table.Column<string>(maxLength: 150, nullable: true),
                    PayType = table.Column<string>(maxLength: 150, nullable: true),
                    Credited = table.Column<string>(maxLength: 150, nullable: true),
                    AddDate = table.Column<string>(maxLength: 150, nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    ContractYear = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceOnsite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractStatusDic",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StatusName = table.Column<string>(maxLength: 50, nullable: true),
                    StatusCode = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractStatusDic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractType",
                columns: table => new
                {
                    ContractTypeId = table.Column<Guid>(nullable: false),
                    Sort = table.Column<int>(nullable: true),
                    Translation = table.Column<string>(maxLength: 500, nullable: true),
                    CTypeCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsSpeaker = table.Column<bool>(nullable: false, defaultValue: false),
                    IsGive = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractType", x => x.ContractTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ExtraService",
                columns: table => new
                {
                    ExtraServiceId = table.Column<Guid>(nullable: false),
                    ExtraContractNumber = table.Column<string>(maxLength: 150, nullable: true),
                    MemberPK = table.Column<Guid>(nullable: true),
                    MemTranslation = table.Column<string>(maxLength: 500, nullable: true),
                    IsDelete = table.Column<bool>(nullable: true),
                    Owerid = table.Column<string>(maxLength: 500, nullable: true),
                    Ower = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraService", x => x.ExtraServiceId);
                });

            migrationBuilder.CreateTable(
                name: "InviteCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InviteCodeNumber = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyServicePackId = table.Column<string>(maxLength: 500, nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    WebSite = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InviteLetter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Company = table.Column<string>(maxLength: 500, nullable: true),
                    EHall = table.Column<string>(maxLength: 500, nullable: true),
                    ENo = table.Column<string>(maxLength: 500, nullable: true),
                    Profile = table.Column<string>(maxLength: 1000, nullable: true),
                    EAcitvity = table.Column<string>(maxLength: 1000, nullable: true),
                    Language = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteLetter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperateRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContractNumber = table.Column<string>(maxLength: 500, nullable: true),
                    OperateContent = table.Column<string>(type: "text", nullable: true, defaultValue: ""),
                    OperateTime = table.Column<string>(maxLength: 50, nullable: true),
                    Operator = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperateRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonContractActivityMap",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    PersonContractId = table.Column<string>(maxLength: 150, nullable: true, defaultValue: ""),
                    MemberPK = table.Column<string>(maxLength: 150, nullable: true),
                    ActivityId = table.Column<string>(maxLength: 150, nullable: true),
                    ActivityName = table.Column<string>(maxLength: 1000, nullable: true),
                    SessionConferenceID = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    SessionConferenceName = table.Column<string>(maxLength: 1000, nullable: true, defaultValue: ""),
                    IsConfirm = table.Column<bool>(nullable: true, defaultValue: false),
                    IsCheck = table.Column<bool>(nullable: true, defaultValue: false),
                    Year = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonContractActivityMap", x => x.MapId);
                });

            migrationBuilder.CreateTable(
                name: "RemarkDic",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentCn = table.Column<string>(type: "text", nullable: true),
                    ContentEn = table.Column<string>(type: "text", nullable: true),
                    ContentJp = table.Column<string>(type: "text", nullable: true),
                    ContentCode = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemarkDic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicePack",
                columns: table => new
                {
                    ServicePackId = table.Column<Guid>(nullable: false),
                    ConferenceId = table.Column<string>(maxLength: 500, nullable: true),
                    ConferenceName = table.Column<string>(maxLength: 500, nullable: true),
                    SessionConferenceId = table.Column<string>(maxLength: 500, nullable: true),
                    SessionConferenceName = table.Column<string>(maxLength: 500, nullable: true),
                    SessionAddress = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    ThirdSessionConferenceId = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    ThirdSessionConferenceName = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    SessionDate = table.Column<string>(maxLength: 500, nullable: true),
                    SessionStartTime = table.Column<string>(maxLength: 500, nullable: true),
                    Translation = table.Column<string>(maxLength: 500, nullable: true),
                    PriceRMB = table.Column<string>(maxLength: 50, nullable: true),
                    PriceUSD = table.Column<string>(maxLength: 50, nullable: true),
                    PriceJP = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 150, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePack", x => x.ServicePackId);
                });

            migrationBuilder.CreateTable(
                name: "TagType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NameTranslation = table.Column<string>(maxLength: 500, nullable: true),
                    Code = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YearConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    IsUse = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DelegateServicePackDiscountForConferenceContract",
                columns: table => new
                {
                    DiscountId = table.Column<Guid>(nullable: false),
                    ConferenceContractId = table.Column<Guid>(nullable: true),
                    PriceAfterDiscountRMB = table.Column<string>(maxLength: 50, nullable: true),
                    PriceAfterDiscountUSD = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateServicePackDiscountForConferenceContract", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_DelegateServicePackDiscountForConferenceContract_Conference~",
                        column: x => x.ConferenceContractId,
                        principalTable: "ConferenceContract",
                        principalColumn: "ConferenceContractId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServicePack",
                columns: table => new
                {
                    CompanyServicePackId = table.Column<Guid>(nullable: false),
                    ContractTypeId = table.Column<Guid>(nullable: true),
                    CTypeCode = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    Translation = table.Column<string>(maxLength: 500, nullable: true),
                    Sort = table.Column<int>(nullable: true),
                    ConferenceId = table.Column<string>(maxLength: 500, nullable: true),
                    ConferenceName = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    PriceRMB = table.Column<string>(maxLength: 50, nullable: true),
                    PriceUSD = table.Column<string>(maxLength: 50, nullable: true),
                    PriceJP = table.Column<string>(maxLength: 50, nullable: true),
                    IsShownOnFront = table.Column<bool>(nullable: true),
                    RemarkTranslation = table.Column<string>(maxLength: 1000, nullable: true),
                    RemarkCode = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    IsSpeaker = table.Column<bool>(nullable: false, defaultValue: false),
                    IsDelete = table.Column<bool>(nullable: true),
                    IsGive = table.Column<bool>(nullable: true, defaultValue: false),
                    Year = table.Column<string>(maxLength: 150, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServicePack", x => x.CompanyServicePackId);
                    table.ForeignKey(
                        name: "FK_CompanyServicePack_ContractType_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "ContractType",
                        principalColumn: "ContractTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InviteCodeRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InviteCodeId = table.Column<Guid>(nullable: true),
                    MemberPK = table.Column<string>(maxLength: 500, nullable: true),
                    MemberName = table.Column<string>(maxLength: 500, nullable: true),
                    UseDate = table.Column<string>(maxLength: 500, nullable: true),
                    PersonContractId = table.Column<string>(maxLength: 500, nullable: true),
                    PersonContractNumber = table.Column<string>(maxLength: 1000, nullable: true),
                    IsDelete = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteCodeRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InviteCodeRecord_InviteCode_InviteCodeId",
                        column: x => x.InviteCodeId,
                        principalTable: "InviteCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExtraServicePackMap",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    ExtraServiceId = table.Column<Guid>(nullable: true),
                    ServicePackId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraServicePackMap", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_ExtraServicePackMap_ExtraService_ExtraServiceId",
                        column: x => x.ExtraServiceId,
                        principalTable: "ExtraService",
                        principalColumn: "ExtraServiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExtraServicePackMap_ServicePack_ServicePackId",
                        column: x => x.ServicePackId,
                        principalTable: "ServicePack",
                        principalColumn: "ServicePackId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackActivityMap",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    ServicePackId = table.Column<Guid>(nullable: true),
                    SessionIDs = table.Column<string>(maxLength: 10000, nullable: true, defaultValue: ""),
                    ActivityId = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    ActivityName = table.Column<string>(maxLength: 1000, nullable: true),
                    SessionConferenceID = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    SessionConferenceName = table.Column<string>(maxLength: 1000, nullable: true, defaultValue: ""),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackActivityMap", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_ServicePackActivityMap_ServicePack_ServicePackId",
                        column: x => x.ServicePackId,
                        principalTable: "ServicePack",
                        principalColumn: "ServicePackId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyContract",
                columns: table => new
                {
                    ContractId = table.Column<Guid>(nullable: false),
                    ConferenceContractId = table.Column<Guid>(nullable: true),
                    CompanyServicePackId = table.Column<Guid>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: true),
                    ConferenceId = table.Column<string>(maxLength: 500, nullable: true),
                    ConferenceName = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    ComNameTranslation = table.Column<string>(maxLength: 500, nullable: true),
                    ComContractNumber = table.Column<string>(maxLength: 150, nullable: true),
                    Country = table.Column<string>(maxLength: 50, nullable: true),
                    AddressTranslation = table.Column<string>(maxLength: 500, nullable: true),
                    ComPrice = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    MaxContractNumber = table.Column<int>(nullable: true),
                    MaxContractNumberSatUse = table.Column<int>(nullable: true, defaultValue: 0),
                    CCIsdelete = table.Column<bool>(nullable: true),
                    EnterpriseType = table.Column<int>(nullable: true),
                    IsCheckIn = table.Column<bool>(nullable: true),
                    IsVerify = table.Column<bool>(nullable: true),
                    PPTUrl = table.Column<string>(maxLength: 5000, nullable: true),
                    ContractStatusCode = table.Column<string>(maxLength: 50, nullable: true),
                    Owerid = table.Column<string>(maxLength: 500, nullable: true),
                    Ower = table.Column<string>(maxLength: 50, nullable: true),
                    ContractCode = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    OtherOwnerId = table.Column<string>(maxLength: 3000, nullable: true),
                    OtherOwner = table.Column<string>(maxLength: 3000, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyContract", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_CompanyContract_CompanyServicePack_CompanyServicePackId",
                        column: x => x.CompanyServicePackId,
                        principalTable: "CompanyServicePack",
                        principalColumn: "CompanyServicePackId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyContract_ConferenceContract_ConferenceContractId",
                        column: x => x.ConferenceContractId,
                        principalTable: "ConferenceContract",
                        principalColumn: "ConferenceContractId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyServicePackMap",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    CompanyServicePackId = table.Column<Guid>(nullable: true),
                    ServicePackId = table.Column<Guid>(nullable: true),
                    SPCCode = table.Column<string>(maxLength: 150, nullable: true),
                    CSPCCode = table.Column<string>(maxLength: 150, nullable: true),
                    Year = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyServicePackMap", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_CompanyServicePackMap_CompanyServicePack_CompanyServicePack~",
                        column: x => x.CompanyServicePackId,
                        principalTable: "CompanyServicePack",
                        principalColumn: "CompanyServicePackId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyServicePackMap_ServicePack_ServicePackId",
                        column: x => x.ServicePackId,
                        principalTable: "ServicePack",
                        principalColumn: "ServicePackId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DelegateServicePackDiscount",
                columns: table => new
                {
                    DiscountId = table.Column<Guid>(nullable: false),
                    ContractId = table.Column<Guid>(nullable: true),
                    PriceAfterDiscountRMB = table.Column<string>(maxLength: 50, nullable: true),
                    PriceAfterDiscountUSD = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateServicePackDiscount", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_DelegateServicePackDiscount_CompanyContract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "CompanyContract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonContract",
                columns: table => new
                {
                    PersonContractId = table.Column<Guid>(nullable: false),
                    ConferenceId = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    ContractId = table.Column<Guid>(nullable: true),
                    PerContractNumber = table.Column<string>(maxLength: 150, nullable: true),
                    MemberPK = table.Column<Guid>(nullable: true),
                    MemTranslation = table.Column<string>(maxLength: 500, nullable: true),
                    PCIsdelete = table.Column<bool>(nullable: true),
                    CTypeCode = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    InviteCodeId = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    Owerid = table.Column<string>(maxLength: 500, nullable: true, defaultValue: ""),
                    Ower = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    PaidAmount = table.Column<string>(maxLength: 150, nullable: true, defaultValue: ""),
                    IsCheckIn = table.Column<bool>(nullable: true, defaultValue: false),
                    IsSendEmail = table.Column<bool>(nullable: true, defaultValue: false),
                    IsModify = table.Column<bool>(nullable: true, defaultValue: true),
                    IsFianceRecord = table.Column<bool>(nullable: true, defaultValue: false),
                    IsInviteCode = table.Column<bool>(nullable: true, defaultValue: false),
                    IsCommitAbstract = table.Column<bool>(nullable: true, defaultValue: false),
                    IsPrint = table.Column<bool>(nullable: true, defaultValue: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    OtherOwner = table.Column<string>(maxLength: 3000, nullable: true),
                    OtherOwnerId = table.Column<string>(maxLength: 3000, nullable: true),
                    CompanyServicePackId = table.Column<Guid>(nullable: true),
                    ConferenceContractId = table.Column<Guid>(nullable: true),
                    PerPrice = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonContract", x => x.PersonContractId);
                    table.ForeignKey(
                        name: "FK_PersonContract_CompanyContract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "CompanyContract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContract_ComContractNumber",
                table: "CompanyContract",
                column: "ComContractNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContract_CompanyServicePackId",
                table: "CompanyContract",
                column: "CompanyServicePackId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContract_ConferenceContractId",
                table: "CompanyContract",
                column: "ConferenceContractId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServicePack_ContractTypeId",
                table: "CompanyServicePack",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServicePackMap_CompanyServicePackId",
                table: "CompanyServicePackMap",
                column: "CompanyServicePackId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServicePackMap_ServicePackId",
                table: "CompanyServicePackMap",
                column: "ServicePackId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractStatusDic_StatusCode",
                table: "ContractStatusDic",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractType_CTypeCode",
                table: "ContractType",
                column: "CTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DelegateServicePackDiscount_ContractId",
                table: "DelegateServicePackDiscount",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateServicePackDiscountForConferenceContract_Conference~",
                table: "DelegateServicePackDiscountForConferenceContract",
                column: "ConferenceContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraServicePackMap_ExtraServiceId",
                table: "ExtraServicePackMap",
                column: "ExtraServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraServicePackMap_ServicePackId",
                table: "ExtraServicePackMap",
                column: "ServicePackId");

            migrationBuilder.CreateIndex(
                name: "IX_InviteCodeRecord_InviteCodeId",
                table: "InviteCodeRecord",
                column: "InviteCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonContract_ContractId",
                table: "PersonContract",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonContract_PerContractNumber",
                table: "PersonContract",
                column: "PerContractNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RemarkDic_ContentCode",
                table: "RemarkDic",
                column: "ContentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackActivityMap_ServicePackId",
                table: "ServicePackActivityMap",
                column: "ServicePackId");

            migrationBuilder.CreateIndex(
                name: "IX_TagType_Code",
                table: "TagType",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyConference");

            migrationBuilder.DropTable(
                name: "CCNumberConfig");

            migrationBuilder.DropTable(
                name: "CompanyServicePackMap");

            migrationBuilder.DropTable(
                name: "ConferenceOnsite");

            migrationBuilder.DropTable(
                name: "ContractStatusDic");

            migrationBuilder.DropTable(
                name: "DelegateServicePackDiscount");

            migrationBuilder.DropTable(
                name: "DelegateServicePackDiscountForConferenceContract");

            migrationBuilder.DropTable(
                name: "ExtraServicePackMap");

            migrationBuilder.DropTable(
                name: "InviteCodeRecord");

            migrationBuilder.DropTable(
                name: "InviteLetter");

            migrationBuilder.DropTable(
                name: "OperateRecord");

            migrationBuilder.DropTable(
                name: "PersonContract");

            migrationBuilder.DropTable(
                name: "PersonContractActivityMap");

            migrationBuilder.DropTable(
                name: "RemarkDic");

            migrationBuilder.DropTable(
                name: "ServicePackActivityMap");

            migrationBuilder.DropTable(
                name: "TagType");

            migrationBuilder.DropTable(
                name: "YearConfig");

            migrationBuilder.DropTable(
                name: "ExtraService");

            migrationBuilder.DropTable(
                name: "InviteCode");

            migrationBuilder.DropTable(
                name: "CompanyContract");

            migrationBuilder.DropTable(
                name: "ServicePack");

            migrationBuilder.DropTable(
                name: "CompanyServicePack");

            migrationBuilder.DropTable(
                name: "ConferenceContract");

            migrationBuilder.DropTable(
                name: "ContractType");
        }
    }
}
