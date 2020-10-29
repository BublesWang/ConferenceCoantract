CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190305084530_first_migration35') THEN
    ALTER TABLE "ConferenceContract" ADD "IsSendEmail" boolean NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190305084530_first_migration35') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190305084530_first_migration35', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190322071338_first_migration36') THEN
    ALTER TABLE "PersonContract" ADD "IsCheckIn" boolean NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190322071338_first_migration36') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190322071338_first_migration36', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190329023852_first_migration37') THEN
    ALTER TABLE "PersonContract" ADD "IsSendEmail" boolean NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190329023852_first_migration37') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190329023852_first_migration37', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190417025427_first_migration38') THEN
    ALTER TABLE "ContractType" ADD "IsGive" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190417025427_first_migration38') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190417025427_first_migration38', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190417091003_first_migration39') THEN
    ALTER TABLE "CompanyServicePack" ADD "CTypeCode" character varying(50) NULL DEFAULT '';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190417091003_first_migration39') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190417091003_first_migration39', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190506023444_first_migration40') THEN
    ALTER TABLE "CompanyServicePack" ADD "IsGive" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190506023444_first_migration40') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190506023444_first_migration40', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190517005532_first_migration41') THEN
    CREATE TABLE "InviteLetter" (
        "Id" uuid NOT NULL,
        "Company" character varying(500) NULL,
        "EHall" character varying(500) NULL,
        "ENo" character varying(500) NULL,
        "Profile" character varying(1000) NULL,
        "EAcitvity" character varying(1000) NULL,
        "CreatedOn" timestamp without time zone NULL,
        CONSTRAINT "PK_InviteLetter" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190517005532_first_migration41') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190517005532_first_migration41', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190520024144_first_migration42') THEN
    ALTER TABLE "InviteLetter" ADD "Language" character varying(50) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190520024144_first_migration42') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190520024144_first_migration42', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    ALTER TABLE "ServicePack" ADD "Year" character varying(150) NULL DEFAULT '2019';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    ALTER TABLE "CompanyServicePack" ADD "Year" character varying(150) NULL DEFAULT '2019';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    ALTER TABLE "ApplyConference" ADD "RemarkTranslation" character varying(40000) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    ALTER TABLE "ApplyConference" ADD "TagTypeCodes" character varying(40000) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    ALTER TABLE "ApplyConference" ADD "Year" character varying(150) NULL DEFAULT '2019';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    CREATE TABLE "TagType" (
        "Id" uuid NOT NULL,
        "NameTranslation" character varying(500) NULL,
        "Code" character varying(500) NULL,
        CONSTRAINT "PK_TagType" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    CREATE UNIQUE INDEX "IX_TagType_Code" ON "TagType" ("Code");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619055123_first_migration43') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190619055123_first_migration43', '2.1.8-servicing-32085');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619060032_first_migration44') THEN
    ALTER TABLE "ApplyConference" ALTER COLUMN "TagTypeCodes" TYPE character varying(40000);
    ALTER TABLE "ApplyConference" ALTER COLUMN "TagTypeCodes" DROP NOT NULL;
    ALTER TABLE "ApplyConference" ALTER COLUMN "TagTypeCodes" SET DEFAULT '';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619060032_first_migration44') THEN
    ALTER TABLE "ApplyConference" ALTER COLUMN "RemarkTranslation" TYPE character varying(40000);
    ALTER TABLE "ApplyConference" ALTER COLUMN "RemarkTranslation" DROP NOT NULL;
    ALTER TABLE "ApplyConference" ALTER COLUMN "RemarkTranslation" SET DEFAULT '';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619060032_first_migration44') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190619060032_first_migration44', '2.1.8-servicing-32085');
    END IF;
END $$;
