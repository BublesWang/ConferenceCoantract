/*
 Navicat Premium Data Transfer

 Source Server         : conferencecontract_snec_latest.data
 Source Server Type    : PostgreSQL
 Source Server Version : 100006
 Source Host           : 47.104.249.194:3434
 Source Catalog        : conferencecontract
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 100006
 File Encoding         : 65001

 Date: 11/02/2019 13:26:22
*/


-- ----------------------------
-- Table structure for ApplyConference
-- ----------------------------
DROP TABLE IF EXISTS "public"."ApplyConference";
CREATE TABLE "public"."ApplyConference" (
  "Id" uuid NOT NULL,
  "PersonContractId" varchar(150) COLLATE "pg_catalog"."default",
  "MemberPK" varchar(150) COLLATE "pg_catalog"."default",
  "SessionConferenceId" varchar(150) COLLATE "pg_catalog"."default",
  "IsConfirm" bool,
  "CompanyId" varchar(150) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for CCNumberConfig
-- ----------------------------
DROP TABLE IF EXISTS "public"."CCNumberConfig";
CREATE TABLE "public"."CCNumberConfig" (
  "Id" uuid NOT NULL,
  "Prefix" varchar(50) COLLATE "pg_catalog"."default",
  "Year" varchar(50) COLLATE "pg_catalog"."default",
  "CNano" varchar(50) COLLATE "pg_catalog"."default",
  "Count" int4,
  "Status" int4,
  "IsDelete" bool,
  "ConferenceId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ConferenceName" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for CompanyContract
-- ----------------------------
DROP TABLE IF EXISTS "public"."CompanyContract";
CREATE TABLE "public"."CompanyContract" (
  "ContractId" uuid NOT NULL,
  "CompanyServicePackId" uuid,
  "CompanyId" uuid,
  "ComNameTranslation" varchar(500) COLLATE "pg_catalog"."default",
  "ComContractNumber" varchar(150) COLLATE "pg_catalog"."default",
  "Country" varchar(50) COLLATE "pg_catalog"."default",
  "AddressTranslation" varchar(500) COLLATE "pg_catalog"."default",
  "MaxContractNumber" int4,
  "CCIsdelete" bool,
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "EnterpriseType" int4,
  "IsVerify" bool,
  "Ower" varchar(50) COLLATE "pg_catalog"."default",
  "Owerid" varchar(500) COLLATE "pg_catalog"."default",
  "ContractCode" varchar(50) COLLATE "pg_catalog"."default",
  "ConferenceId" varchar(500) COLLATE "pg_catalog"."default",
  "ConferenceName" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ContractStatusCode" varchar(50) COLLATE "pg_catalog"."default",
  "IsCheckIn" bool,
  "PPTUrl" varchar(5000) COLLATE "pg_catalog"."default",
  "ConferenceContractId" uuid,
  "ComPrice" varchar(50) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for CompanyServicePack
-- ----------------------------
DROP TABLE IF EXISTS "public"."CompanyServicePack";
CREATE TABLE "public"."CompanyServicePack" (
  "CompanyServicePackId" uuid NOT NULL,
  "ContractTypeId" uuid,
  "Sort" int4,
  "Translation" varchar(500) COLLATE "pg_catalog"."default",
  "PriceRMB" varchar(50) COLLATE "pg_catalog"."default",
  "PriceUSD" varchar(50) COLLATE "pg_catalog"."default",
  "IsShownOnFront" bool,
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "RemarkTranslation" varchar(1000) COLLATE "pg_catalog"."default",
  "ConferenceId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ConferenceName" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "IsDelete" bool DEFAULT false,
  "IsSpeaker" bool NOT NULL DEFAULT false
)
;

-- ----------------------------
-- Table structure for CompanyServicePackMap
-- ----------------------------
DROP TABLE IF EXISTS "public"."CompanyServicePackMap";
CREATE TABLE "public"."CompanyServicePackMap" (
  "MapId" uuid NOT NULL,
  "CompanyServicePackId" uuid,
  "ServicePackId" uuid
)
;

-- ----------------------------
-- Table structure for ConferenceContract
-- ----------------------------
DROP TABLE IF EXISTS "public"."ConferenceContract";
CREATE TABLE "public"."ConferenceContract" (
  "ConferenceContractId" uuid NOT NULL,
  "CompanyId" varchar(500) COLLATE "pg_catalog"."default",
  "ComNameTranslation" varchar(500) COLLATE "pg_catalog"."default",
  "ContractNumber" varchar(150) COLLATE "pg_catalog"."default",
  "ContractCode" varchar(50) COLLATE "pg_catalog"."default",
  "IsDelete" bool,
  "Owerid" varchar(500) COLLATE "pg_catalog"."default",
  "Ower" varchar(50) COLLATE "pg_catalog"."default",
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "ContractYear" varchar(150) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ConferenceId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ContractStatusCode" varchar(50) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "PaymentStatusCode" varchar(50) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for ContractStatusDic
-- ----------------------------
DROP TABLE IF EXISTS "public"."ContractStatusDic";
CREATE TABLE "public"."ContractStatusDic" (
  "Id" uuid NOT NULL,
  "StatusName" varchar(50) COLLATE "pg_catalog"."default",
  "StatusCode" varchar(100) COLLATE "pg_catalog"."default",
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Table structure for ContractType
-- ----------------------------
DROP TABLE IF EXISTS "public"."ContractType";
CREATE TABLE "public"."ContractType" (
  "ContractTypeId" uuid NOT NULL,
  "Sort" int4,
  "Translation" varchar(500) COLLATE "pg_catalog"."default",
  "CTypeCode" varchar(50) COLLATE "pg_catalog"."default",
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "IsSpeaker" bool NOT NULL DEFAULT false
)
;

-- ----------------------------
-- Table structure for DelegateServicePackDiscount
-- ----------------------------
DROP TABLE IF EXISTS "public"."DelegateServicePackDiscount";
CREATE TABLE "public"."DelegateServicePackDiscount" (
  "DiscountId" uuid NOT NULL,
  "ContractId" uuid,
  "PriceAfterDiscountUSD" varchar(50) COLLATE "pg_catalog"."default",
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "PriceAfterDiscountRMB" varchar(50) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Table structure for DelegateServicePackDiscountForConferenceContract
-- ----------------------------
DROP TABLE IF EXISTS "public"."DelegateServicePackDiscountForConferenceContract";
CREATE TABLE "public"."DelegateServicePackDiscountForConferenceContract" (
  "DiscountId" uuid NOT NULL,
  "ConferenceContractId" uuid,
  "PriceAfterDiscountRMB" varchar(50) COLLATE "pg_catalog"."default",
  "PriceAfterDiscountUSD" varchar(50) COLLATE "pg_catalog"."default",
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Table structure for ExtraService
-- ----------------------------
DROP TABLE IF EXISTS "public"."ExtraService";
CREATE TABLE "public"."ExtraService" (
  "ExtraServiceId" uuid NOT NULL,
  "MemberPK" uuid,
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "ExtraContractNumber" varchar(150) COLLATE "pg_catalog"."default",
  "IsDelete" bool DEFAULT false,
  "MemTranslation" varchar(500) COLLATE "pg_catalog"."default",
  "Ower" varchar(50) COLLATE "pg_catalog"."default",
  "Owerid" varchar(500) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Table structure for ExtraServicePackMap
-- ----------------------------
DROP TABLE IF EXISTS "public"."ExtraServicePackMap";
CREATE TABLE "public"."ExtraServicePackMap" (
  "MapId" uuid NOT NULL,
  "ExtraServiceId" uuid,
  "ServicePackId" uuid
)
;

-- ----------------------------
-- Table structure for PersonContract
-- ----------------------------
DROP TABLE IF EXISTS "public"."PersonContract";
CREATE TABLE "public"."PersonContract" (
  "PersonContractId" uuid NOT NULL,
  "ContractId" uuid,
  "PerContractNumber" varchar(150) COLLATE "pg_catalog"."default",
  "MemberPK" uuid,
  "MemTranslation" varchar(500) COLLATE "pg_catalog"."default",
  "PCIsdelete" bool,
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "Ower" varchar(50) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "Owerid" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "CTypeCode" varchar(50) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ConferenceId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for PersonContractActivityMap
-- ----------------------------
DROP TABLE IF EXISTS "public"."PersonContractActivityMap";
CREATE TABLE "public"."PersonContractActivityMap" (
  "MapId" uuid NOT NULL,
  "MemberPK" varchar(150) COLLATE "pg_catalog"."default",
  "ActivityId" varchar(150) COLLATE "pg_catalog"."default",
  "ActivityName" varchar(1000) COLLATE "pg_catalog"."default",
  "Year" varchar(100) COLLATE "pg_catalog"."default",
  "IsCheck" bool DEFAULT false,
  "IsConfirm" bool DEFAULT false,
  "PersonContractId" varchar(150) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for ServicePack
-- ----------------------------
DROP TABLE IF EXISTS "public"."ServicePack";
CREATE TABLE "public"."ServicePack" (
  "ServicePackId" uuid NOT NULL,
  "Translation" varchar(500) COLLATE "pg_catalog"."default",
  "PriceRMB" varchar(50) COLLATE "pg_catalog"."default",
  "PriceUSD" varchar(50) COLLATE "pg_catalog"."default",
  "CreatedOn" timestamp(6),
  "CreatedBy" varchar(50) COLLATE "pg_catalog"."default",
  "ModefieldOn" timestamp(6),
  "ModefieldBy" varchar(50) COLLATE "pg_catalog"."default",
  "ConferenceId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ConferenceName" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "SessionConferenceId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "SessionConferenceName" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "IsDelete" bool DEFAULT false,
  "SessionDate" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "SessionStartTime" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ThirdSessionConferenceId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ThirdSessionConferenceName" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "SessionAddress" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for ServicePackActivityMap
-- ----------------------------
DROP TABLE IF EXISTS "public"."ServicePackActivityMap";
CREATE TABLE "public"."ServicePackActivityMap" (
  "MapId" uuid NOT NULL,
  "ServicePackId" uuid,
  "ActivityId" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "ActivityName" varchar(1000) COLLATE "pg_catalog"."default",
  "Sort" int4,
  "SessionConferenceID" varchar(500) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "SessionConferenceName" varchar(1000) COLLATE "pg_catalog"."default" DEFAULT ''::character varying,
  "SessionIDs" varchar(10000) COLLATE "pg_catalog"."default" DEFAULT ''::character varying
)
;

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS "public"."__EFMigrationsHistory";
CREATE TABLE "public"."__EFMigrationsHistory" (
  "MigrationId" varchar(150) COLLATE "pg_catalog"."default" NOT NULL,
  "ProductVersion" varchar(32) COLLATE "pg_catalog"."default" NOT NULL
)
;

-- ----------------------------
-- Primary Key structure for table ApplyConference
-- ----------------------------
ALTER TABLE "public"."ApplyConference" ADD CONSTRAINT "PK_ApplyConference" PRIMARY KEY ("Id");

-- ----------------------------
-- Primary Key structure for table CCNumberConfig
-- ----------------------------
ALTER TABLE "public"."CCNumberConfig" ADD CONSTRAINT "PK_CCNumberConfig" PRIMARY KEY ("Id");

-- ----------------------------
-- Indexes structure for table CompanyContract
-- ----------------------------
CREATE UNIQUE INDEX "IX_CompanyContract_ComContractNumber" ON "public"."CompanyContract" USING btree (
  "ComContractNumber" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IX_CompanyContract_CompanyServicePackId" ON "public"."CompanyContract" USING btree (
  "CompanyServicePackId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);
CREATE INDEX "IX_CompanyContract_ConferenceContractId" ON "public"."CompanyContract" USING btree (
  "ConferenceContractId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table CompanyContract
-- ----------------------------
ALTER TABLE "public"."CompanyContract" ADD CONSTRAINT "PK_CompanyContract" PRIMARY KEY ("ContractId");

-- ----------------------------
-- Indexes structure for table CompanyServicePack
-- ----------------------------
CREATE INDEX "IX_CompanyServicePack_ContractTypeId" ON "public"."CompanyServicePack" USING btree (
  "ContractTypeId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table CompanyServicePack
-- ----------------------------
ALTER TABLE "public"."CompanyServicePack" ADD CONSTRAINT "PK_CompanyServicePack" PRIMARY KEY ("CompanyServicePackId");

-- ----------------------------
-- Indexes structure for table CompanyServicePackMap
-- ----------------------------
CREATE INDEX "IX_CompanyServicePackMap_CompanyServicePackId" ON "public"."CompanyServicePackMap" USING btree (
  "CompanyServicePackId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);
CREATE INDEX "IX_CompanyServicePackMap_ServicePackId" ON "public"."CompanyServicePackMap" USING btree (
  "ServicePackId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table CompanyServicePackMap
-- ----------------------------
ALTER TABLE "public"."CompanyServicePackMap" ADD CONSTRAINT "PK_CompanyServicePackMap" PRIMARY KEY ("MapId");

-- ----------------------------
-- Primary Key structure for table ConferenceContract
-- ----------------------------
ALTER TABLE "public"."ConferenceContract" ADD CONSTRAINT "PK_ConferenceContract" PRIMARY KEY ("ConferenceContractId");

-- ----------------------------
-- Indexes structure for table ContractStatusDic
-- ----------------------------
CREATE UNIQUE INDEX "IX_ContractStatusDic_StatusCode" ON "public"."ContractStatusDic" USING btree (
  "StatusCode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table ContractStatusDic
-- ----------------------------
ALTER TABLE "public"."ContractStatusDic" ADD CONSTRAINT "PK_ContractStatusDic" PRIMARY KEY ("Id");

-- ----------------------------
-- Indexes structure for table ContractType
-- ----------------------------
CREATE UNIQUE INDEX "IX_ContractType_CTypeCode" ON "public"."ContractType" USING btree (
  "CTypeCode" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table ContractType
-- ----------------------------
ALTER TABLE "public"."ContractType" ADD CONSTRAINT "PK_ContractType" PRIMARY KEY ("ContractTypeId");

-- ----------------------------
-- Indexes structure for table DelegateServicePackDiscount
-- ----------------------------
CREATE INDEX "IX_DelegateServicePackDiscount_ContractId" ON "public"."DelegateServicePackDiscount" USING btree (
  "ContractId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table DelegateServicePackDiscount
-- ----------------------------
ALTER TABLE "public"."DelegateServicePackDiscount" ADD CONSTRAINT "PK_DelegateServicePackDiscount" PRIMARY KEY ("DiscountId");

-- ----------------------------
-- Indexes structure for table DelegateServicePackDiscountForConferenceContract
-- ----------------------------
CREATE INDEX "IX_DelegateServicePackDiscountForConferenceContract_Conference~" ON "public"."DelegateServicePackDiscountForConferenceContract" USING btree (
  "ConferenceContractId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table DelegateServicePackDiscountForConferenceContract
-- ----------------------------
ALTER TABLE "public"."DelegateServicePackDiscountForConferenceContract" ADD CONSTRAINT "PK_DelegateServicePackDiscountForConferenceContract" PRIMARY KEY ("DiscountId");

-- ----------------------------
-- Primary Key structure for table ExtraService
-- ----------------------------
ALTER TABLE "public"."ExtraService" ADD CONSTRAINT "PK_ExtraService" PRIMARY KEY ("ExtraServiceId");

-- ----------------------------
-- Indexes structure for table ExtraServicePackMap
-- ----------------------------
CREATE INDEX "IX_ExtraServicePackMap_ExtraServiceId" ON "public"."ExtraServicePackMap" USING btree (
  "ExtraServiceId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);
CREATE INDEX "IX_ExtraServicePackMap_ServicePackId" ON "public"."ExtraServicePackMap" USING btree (
  "ServicePackId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table ExtraServicePackMap
-- ----------------------------
ALTER TABLE "public"."ExtraServicePackMap" ADD CONSTRAINT "PK_ExtraServicePackMap" PRIMARY KEY ("MapId");

-- ----------------------------
-- Indexes structure for table PersonContract
-- ----------------------------
CREATE INDEX "IX_PersonContract_ContractId" ON "public"."PersonContract" USING btree (
  "ContractId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "IX_PersonContract_PerContractNumber" ON "public"."PersonContract" USING btree (
  "PerContractNumber" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table PersonContract
-- ----------------------------
ALTER TABLE "public"."PersonContract" ADD CONSTRAINT "PK_PersonContract" PRIMARY KEY ("PersonContractId");

-- ----------------------------
-- Primary Key structure for table PersonContractActivityMap
-- ----------------------------
ALTER TABLE "public"."PersonContractActivityMap" ADD CONSTRAINT "PK_PersonContractActivityMap" PRIMARY KEY ("MapId");

-- ----------------------------
-- Primary Key structure for table ServicePack
-- ----------------------------
ALTER TABLE "public"."ServicePack" ADD CONSTRAINT "PK_ServicePack" PRIMARY KEY ("ServicePackId");

-- ----------------------------
-- Indexes structure for table ServicePackActivityMap
-- ----------------------------
CREATE INDEX "IX_ServicePackActivityMap_ServicePackId" ON "public"."ServicePackActivityMap" USING btree (
  "ServicePackId" "pg_catalog"."uuid_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table ServicePackActivityMap
-- ----------------------------
ALTER TABLE "public"."ServicePackActivityMap" ADD CONSTRAINT "PK_ServicePackActivityMap" PRIMARY KEY ("MapId");

-- ----------------------------
-- Primary Key structure for table __EFMigrationsHistory
-- ----------------------------
ALTER TABLE "public"."__EFMigrationsHistory" ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");

-- ----------------------------
-- Foreign Keys structure for table CompanyContract
-- ----------------------------
ALTER TABLE "public"."CompanyContract" ADD CONSTRAINT "FK_CompanyContract_CompanyServicePack_CompanyServicePackId" FOREIGN KEY ("CompanyServicePackId") REFERENCES "public"."CompanyServicePack" ("CompanyServicePackId") ON DELETE RESTRICT ON UPDATE NO ACTION;
ALTER TABLE "public"."CompanyContract" ADD CONSTRAINT "FK_CompanyContract_ConferenceContract_ConferenceContractId" FOREIGN KEY ("ConferenceContractId") REFERENCES "public"."ConferenceContract" ("ConferenceContractId") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table CompanyServicePack
-- ----------------------------
ALTER TABLE "public"."CompanyServicePack" ADD CONSTRAINT "FK_CompanyServicePack_ContractType_ContractTypeId" FOREIGN KEY ("ContractTypeId") REFERENCES "public"."ContractType" ("ContractTypeId") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table CompanyServicePackMap
-- ----------------------------
ALTER TABLE "public"."CompanyServicePackMap" ADD CONSTRAINT "FK_CompanyServicePackMap_CompanyServicePack_CompanyServicePack~" FOREIGN KEY ("CompanyServicePackId") REFERENCES "public"."CompanyServicePack" ("CompanyServicePackId") ON DELETE RESTRICT ON UPDATE NO ACTION;
ALTER TABLE "public"."CompanyServicePackMap" ADD CONSTRAINT "FK_CompanyServicePackMap_ServicePack_ServicePackId" FOREIGN KEY ("ServicePackId") REFERENCES "public"."ServicePack" ("ServicePackId") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table DelegateServicePackDiscount
-- ----------------------------
ALTER TABLE "public"."DelegateServicePackDiscount" ADD CONSTRAINT "FK_DelegateServicePackDiscount_CompanyContract_ContractId" FOREIGN KEY ("ContractId") REFERENCES "public"."CompanyContract" ("ContractId") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table DelegateServicePackDiscountForConferenceContract
-- ----------------------------
ALTER TABLE "public"."DelegateServicePackDiscountForConferenceContract" ADD CONSTRAINT "FK_DelegateServicePackDiscountForConferenceContract_Conference~" FOREIGN KEY ("ConferenceContractId") REFERENCES "public"."ConferenceContract" ("ConferenceContractId") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table ExtraServicePackMap
-- ----------------------------
ALTER TABLE "public"."ExtraServicePackMap" ADD CONSTRAINT "FK_ExtraServicePackMap_ExtraService_ExtraServiceId" FOREIGN KEY ("ExtraServiceId") REFERENCES "public"."ExtraService" ("ExtraServiceId") ON DELETE RESTRICT ON UPDATE NO ACTION;
ALTER TABLE "public"."ExtraServicePackMap" ADD CONSTRAINT "FK_ExtraServicePackMap_ServicePack_ServicePackId" FOREIGN KEY ("ServicePackId") REFERENCES "public"."ServicePack" ("ServicePackId") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table PersonContract
-- ----------------------------
ALTER TABLE "public"."PersonContract" ADD CONSTRAINT "FK_PersonContract_CompanyContract_ContractId" FOREIGN KEY ("ContractId") REFERENCES "public"."CompanyContract" ("ContractId") ON DELETE RESTRICT ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table ServicePackActivityMap
-- ----------------------------
ALTER TABLE "public"."ServicePackActivityMap" ADD CONSTRAINT "FK_ServicePackActivityMap_ServicePack_ServicePackId" FOREIGN KEY ("ServicePackId") REFERENCES "public"."ServicePack" ("ServicePackId") ON DELETE RESTRICT ON UPDATE NO ACTION;
