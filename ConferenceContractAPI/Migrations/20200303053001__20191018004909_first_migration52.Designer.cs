﻿// <auto-generated />
using System;
using ConferenceContractAPI.CCDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConferenceContractAPI.Migrations
{
    [DbContext(typeof(ConCDBContext))]
    [Migration("20200303053001__20191018004909_first_migration52")]
    partial class _20191018004909_first_migration52
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ApplyConference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(150)
                        .HasDefaultValue("");

                    b.Property<bool?>("IsConfirm");

                    b.Property<bool?>("IsParticularConf")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("MemberPK")
                        .HasMaxLength(150);

                    b.Property<string>("PersonContractId")
                        .HasMaxLength(150);

                    b.Property<string>("RemarkTranslation")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(40000)
                        .HasDefaultValue("");

                    b.Property<string>("SessionConferenceId")
                        .HasMaxLength(150);

                    b.Property<string>("TagTypeCodes")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(40000)
                        .HasDefaultValue("");

                    b.Property<string>("Year")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("ApplyConference");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.CCNumberConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNano")
                        .HasMaxLength(50);

                    b.Property<string>("ConferenceId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("ConferenceName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<int?>("Count");

                    b.Property<bool?>("IsDelete");

                    b.Property<string>("Prefix")
                        .HasMaxLength(50);

                    b.Property<int?>("Status");

                    b.Property<string>("Year")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("CCNumberConfig");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.CompanyContract", b =>
                {
                    b.Property<Guid>("ContractId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressTranslation")
                        .HasMaxLength(500);

                    b.Property<bool?>("CCIsdelete");

                    b.Property<string>("ComContractNumber")
                        .HasMaxLength(150);

                    b.Property<string>("ComNameTranslation")
                        .HasMaxLength(500);

                    b.Property<string>("ComPrice")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasDefaultValue("");

                    b.Property<Guid?>("CompanyId");

                    b.Property<Guid?>("CompanyServicePackId");

                    b.Property<Guid?>("ConferenceContractId");

                    b.Property<string>("ConferenceId")
                        .HasMaxLength(500);

                    b.Property<string>("ConferenceName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("ContractCode")
                        .HasMaxLength(50);

                    b.Property<string>("ContractStatusCode")
                        .HasMaxLength(50);

                    b.Property<string>("Country")
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<int?>("EnterpriseType");

                    b.Property<bool?>("IsCheckIn");

                    b.Property<bool?>("IsVerify");

                    b.Property<int?>("MaxContractNumber");

                    b.Property<int?>("MaxContractNumberSatUse")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("Ower")
                        .HasMaxLength(50);

                    b.Property<string>("Owerid")
                        .HasMaxLength(500);

                    b.Property<string>("PPTUrl")
                        .HasMaxLength(5000);

                    b.HasKey("ContractId");

                    b.HasIndex("ComContractNumber")
                        .IsUnique();

                    b.HasIndex("CompanyServicePackId");

                    b.HasIndex("ConferenceContractId");

                    b.ToTable("CompanyContract");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.CompanyServicePack", b =>
                {
                    b.Property<Guid>("CompanyServicePackId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CTypeCode")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasDefaultValue("");

                    b.Property<string>("ConferenceId")
                        .HasMaxLength(500);

                    b.Property<string>("ConferenceName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<Guid?>("ContractTypeId");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsDelete");

                    b.Property<bool>("IsGive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool?>("IsShownOnFront");

                    b.Property<bool>("IsSpeaker")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("PriceRMB")
                        .HasMaxLength(50);

                    b.Property<string>("PriceUSD")
                        .HasMaxLength(50);

                    b.Property<string>("RemarkTranslation")
                        .HasMaxLength(1000);

                    b.Property<int?>("Sort");

                    b.Property<string>("Translation")
                        .HasMaxLength(500);

                    b.Property<string>("Year")
                        .HasMaxLength(150);

                    b.HasKey("CompanyServicePackId");

                    b.HasIndex("ContractTypeId");

                    b.ToTable("CompanyServicePack");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.CompanyServicePackMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CompanyServicePackId");

                    b.Property<Guid?>("ServicePackId");

                    b.HasKey("MapId");

                    b.HasIndex("CompanyServicePackId");

                    b.HasIndex("ServicePackId");

                    b.ToTable("CompanyServicePackMap");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ConferenceContract", b =>
                {
                    b.Property<Guid>("ConferenceContractId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ComNameTranslation")
                        .HasMaxLength(500);

                    b.Property<string>("CompanyId")
                        .HasMaxLength(500);

                    b.Property<string>("ConferenceId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("ContractCode")
                        .HasMaxLength(50);

                    b.Property<string>("ContractNumber")
                        .HasMaxLength(150);

                    b.Property<string>("ContractStatusCode")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasDefaultValue("");

                    b.Property<string>("ContractYear")
                        .HasMaxLength(150);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsDelete");

                    b.Property<bool?>("IsModify")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsSendEmail")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("ModifyPermission")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(150)
                        .HasDefaultValue("0");

                    b.Property<string>("Ower")
                        .HasMaxLength(50);

                    b.Property<string>("Owerid")
                        .HasMaxLength(500);

                    b.Property<string>("PaymentStatusCode")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasDefaultValue("");

                    b.HasKey("ConferenceContractId");

                    b.ToTable("ConferenceContract");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ConferenceOnsite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiviType");

                    b.Property<string>("AddDate")
                        .HasMaxLength(150);

                    b.Property<string>("CompanyName")
                        .HasMaxLength(150);

                    b.Property<string>("ContractNumber")
                        .HasMaxLength(150);

                    b.Property<decimal>("Cost");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("Credited")
                        .HasMaxLength(150);

                    b.Property<string>("Currency")
                        .HasMaxLength(150);

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("PayType")
                        .HasMaxLength(150);

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ConferenceOnsite");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ContractStatusDic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("StatusCode")
                        .HasMaxLength(100);

                    b.Property<string>("StatusName")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("StatusCode")
                        .IsUnique();

                    b.ToTable("ContractStatusDic");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ContractType", b =>
                {
                    b.Property<Guid>("ContractTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CTypeCode")
                        .HasMaxLength(50);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool>("IsGive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsSpeaker")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<int?>("Sort");

                    b.Property<string>("Translation")
                        .HasMaxLength(500);

                    b.HasKey("ContractTypeId");

                    b.HasIndex("CTypeCode")
                        .IsUnique();

                    b.ToTable("ContractType");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.DelegateServicePackDiscount", b =>
                {
                    b.Property<Guid>("DiscountId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ContractId");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("PriceAfterDiscountRMB")
                        .HasMaxLength(50);

                    b.Property<string>("PriceAfterDiscountUSD")
                        .HasMaxLength(50);

                    b.Property<string>("Year")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasDefaultValue("2019");

                    b.HasKey("DiscountId");

                    b.HasIndex("ContractId");

                    b.ToTable("DelegateServicePackDiscount");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.DelegateServicePackDiscountForConferenceContract", b =>
                {
                    b.Property<Guid>("DiscountId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ConferenceContractId");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("PriceAfterDiscountRMB")
                        .HasMaxLength(50);

                    b.Property<string>("PriceAfterDiscountUSD")
                        .HasMaxLength(50);

                    b.HasKey("DiscountId");

                    b.HasIndex("ConferenceContractId");

                    b.ToTable("DelegateServicePackDiscountForConferenceContract");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ExtraService", b =>
                {
                    b.Property<Guid>("ExtraServiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("ExtraContractNumber")
                        .HasMaxLength(150);

                    b.Property<bool?>("IsDelete");

                    b.Property<string>("MemTranslation")
                        .HasMaxLength(500);

                    b.Property<Guid?>("MemberPK");

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("Ower")
                        .HasMaxLength(50);

                    b.Property<string>("Owerid")
                        .HasMaxLength(500);

                    b.HasKey("ExtraServiceId");

                    b.ToTable("ExtraService");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ExtraServicePackMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ExtraServiceId");

                    b.Property<Guid?>("ServicePackId");

                    b.HasKey("MapId");

                    b.HasIndex("ExtraServiceId");

                    b.HasIndex("ServicePackId");

                    b.ToTable("ExtraServicePackMap");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.InviteLetter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Company")
                        .HasMaxLength(500);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("EAcitvity")
                        .HasMaxLength(1000);

                    b.Property<string>("EHall")
                        .HasMaxLength(500);

                    b.Property<string>("ENo")
                        .HasMaxLength(500);

                    b.Property<string>("Language")
                        .HasMaxLength(50);

                    b.Property<string>("Profile")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.ToTable("InviteLetter");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.PersonContract", b =>
                {
                    b.Property<Guid>("PersonContractId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CTypeCode")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasDefaultValue("");

                    b.Property<string>("ConferenceId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<Guid?>("ContractId");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsCheckIn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool?>("IsFianceRecord")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool?>("IsModify")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool?>("IsSendEmail")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("MemTranslation")
                        .HasMaxLength(500);

                    b.Property<Guid?>("MemberPK");

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("Ower")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasDefaultValue("");

                    b.Property<string>("Owerid")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<bool?>("PCIsdelete");

                    b.Property<string>("PerContractNumber")
                        .HasMaxLength(150);

                    b.HasKey("PersonContractId");

                    b.HasIndex("ContractId");

                    b.HasIndex("PerContractNumber")
                        .IsUnique();

                    b.ToTable("PersonContract");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.PersonContractActivityMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityId")
                        .HasMaxLength(150);

                    b.Property<string>("ActivityName")
                        .HasMaxLength(1000);

                    b.Property<bool?>("IsCheck")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool?>("IsConfirm")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("MemberPK")
                        .HasMaxLength(150);

                    b.Property<string>("PersonContractId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(150)
                        .HasDefaultValue("");

                    b.Property<string>("Year")
                        .HasMaxLength(100);

                    b.HasKey("MapId");

                    b.ToTable("PersonContractActivityMap");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ServicePack", b =>
                {
                    b.Property<Guid>("ServicePackId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConferenceId")
                        .HasMaxLength(500);

                    b.Property<string>("ConferenceName")
                        .HasMaxLength(500);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<bool?>("IsDelete");

                    b.Property<string>("ModefieldBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModefieldOn");

                    b.Property<string>("PriceRMB")
                        .HasMaxLength(50);

                    b.Property<string>("PriceUSD")
                        .HasMaxLength(50);

                    b.Property<string>("SessionAddress")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("SessionConferenceId")
                        .HasMaxLength(500);

                    b.Property<string>("SessionConferenceName")
                        .HasMaxLength(500);

                    b.Property<string>("SessionDate")
                        .HasMaxLength(500);

                    b.Property<string>("SessionStartTime")
                        .HasMaxLength(500);

                    b.Property<string>("ThirdSessionConferenceId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("ThirdSessionConferenceName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("Translation")
                        .HasMaxLength(500);

                    b.Property<string>("Year")
                        .HasMaxLength(150);

                    b.HasKey("ServicePackId");

                    b.ToTable("ServicePack");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ServicePackActivityMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("ActivityName")
                        .HasMaxLength(1000);

                    b.Property<Guid?>("ServicePackId");

                    b.Property<string>("SessionConferenceID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(500)
                        .HasDefaultValue("");

                    b.Property<string>("SessionConferenceName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1000)
                        .HasDefaultValue("");

                    b.Property<string>("SessionIDs")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10000)
                        .HasDefaultValue("");

                    b.Property<int>("Sort");

                    b.HasKey("MapId");

                    b.HasIndex("ServicePackId");

                    b.ToTable("ServicePackActivityMap");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.TagType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(500);

                    b.Property<string>("NameTranslation")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("TagType");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.YearConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsUse");

                    b.Property<string>("Year")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("YearConfig");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.CompanyContract", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.CompanyServicePack", "companyServicePack")
                        .WithMany()
                        .HasForeignKey("CompanyServicePackId");

                    b.HasOne("ConferenceContractAPI.DBModels.ConferenceContract", "conferenceContract")
                        .WithMany("companyContract")
                        .HasForeignKey("ConferenceContractId");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.CompanyServicePack", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.ContractType", "contractType")
                        .WithMany("companyServicePack")
                        .HasForeignKey("ContractTypeId");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.CompanyServicePackMap", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.CompanyServicePack", "companyServicePack")
                        .WithMany("companyServicePackMap")
                        .HasForeignKey("CompanyServicePackId");

                    b.HasOne("ConferenceContractAPI.DBModels.ServicePack", "servicePack")
                        .WithMany("companyServicePackMap")
                        .HasForeignKey("ServicePackId");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.DelegateServicePackDiscount", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.CompanyContract", "companyContract")
                        .WithMany("delegateServicePackDiscount")
                        .HasForeignKey("ContractId");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.DelegateServicePackDiscountForConferenceContract", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.ConferenceContract", "conferenceContract")
                        .WithMany("delegateServicePackDiscountForConferenceContract")
                        .HasForeignKey("ConferenceContractId");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ExtraServicePackMap", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.ExtraService", "extraService")
                        .WithMany("extraServicePackMap")
                        .HasForeignKey("ExtraServiceId");

                    b.HasOne("ConferenceContractAPI.DBModels.ServicePack", "servicePack")
                        .WithMany("extraServicePackMap")
                        .HasForeignKey("ServicePackId");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.PersonContract", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.CompanyContract", "companyContract")
                        .WithMany("personContract")
                        .HasForeignKey("ContractId");
                });

            modelBuilder.Entity("ConferenceContractAPI.DBModels.ServicePackActivityMap", b =>
                {
                    b.HasOne("ConferenceContractAPI.DBModels.ServicePack", "servicePack")
                        .WithMany("servicePackActivityMap")
                        .HasForeignKey("ServicePackId");
                });
#pragma warning restore 612, 618
        }
    }
}
