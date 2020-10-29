using ConferenceContractAPI.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.CCDBContext
{
    public class ConCDBContext : DbContext
    {
        public ConCDBContext(DbContextOptions<ConCDBContext> options) : base(options)
        {

        }

        public DbSet<ConferenceContract> ConferenceContract { get; set; }
        public DbSet<CompanyContract> CompanyContract { get; set; }

        public DbSet<DelegateServicePackDiscount> DelegateServicePackDiscount { get; set; }

        public DbSet<PersonContract> PersonContract { get; set; }

        public DbSet<ExtraService> ExtraService { get; set; }

        public DbSet<ExtraServicePackMap> ExtraServicePackMap { get; set; }

        public DbSet<ServicePackActivityMap> ServicePackActivityMap { get; set; }

        public DbSet<ServicePack> ServicePack { get; set; }

        public DbSet<CompanyServicePackMap> CompanyServicePackMap { get; set; }

        public DbSet<CompanyServicePack> CompanyServicePack { get; set; }

        public DbSet<ContractType> ContractType { get; set; }

        public DbSet<CCNumberConfig> CCNumberConfig { get; set; }

        public DbSet<ContractStatusDic> ContractStatusDic { get; set; }

        public DbSet<PersonContractActivityMap> PersonContractActivityMap { get; set; }

        public DbSet<DelegateServicePackDiscountForConferenceContract> DelegateServicePackDiscountForConferenceContract { get; set; }

        public DbSet<ApplyConference> ApplyConference { get; set; }

        public DbSet<InviteLetter> InviteLetter { get; set; }

        public DbSet<TagType> TagType { get; set; }

        public DbSet<YearConfig> YearConfig { get; set; }

        public DbSet<ConferenceOnsite> ConferenceOnsite { get; set; }

        public DbSet<InviteCode> InviteCode { get; set; }

        public DbSet<InviteCodeRecord> InviteCodeRecord { get; set; }

        public DbSet<OperateRecord> OperateRecord { get; set; }
        public DbSet<RemarkDic> RemarkDic { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConferenceContract>(b =>
            {
                b.Property(l => l.ConferenceId).HasDefaultValue(string.Empty);
                b.Property(l => l.ContractStatusCode).HasDefaultValue(string.Empty);
                b.Property(l => l.PaymentStatusCode).HasDefaultValue(string.Empty);
                b.Property(l => l.IsSendEmail).HasDefaultValue(false);
                b.Property(l => l.IsModify).HasDefaultValue(true);
                b.Property(l => l.ModifyPermission).HasDefaultValue("0");
            });

            modelBuilder.Entity<ApplyConference>(b =>
            {
                b.Property(l => l.CompanyId).HasDefaultValue(string.Empty);
                b.Property(l => l.TagTypeCodes).HasDefaultValue(string.Empty);
                b.Property(l => l.RemarkTranslation).HasDefaultValue(string.Empty);
                b.Property(l => l.IsParticularConf).HasDefaultValue(false);
                b.Property(l => l.Owerid).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<CompanyContract>(b =>
            {
                b.HasIndex(d => d.ComContractNumber).IsUnique();
                b.Property(l => l.ConferenceName).HasDefaultValue(string.Empty);
                b.Property(l => l.ComPrice).HasDefaultValue(string.Empty);
                b.Property(l => l.MaxContractNumberSatUse).HasDefaultValue(0);
            });

            modelBuilder.Entity<CompanyServicePack>(b =>
            {
                b.Property(l => l.ConferenceName).HasDefaultValue(string.Empty);
                b.Property(l => l.IsSpeaker).HasDefaultValue(false);
                b.Property(l => l.CTypeCode).HasDefaultValue(string.Empty);
                b.Property(l => l.IsGive).HasDefaultValue(false);
                b.Property(l => l.RemarkCode).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<DelegateServicePackDiscount>(b =>
            {
                //b.Property(l => l.Year).HasDefaultValue("2019");
            });

            modelBuilder.Entity<YearConfig>(b =>
            {
                b.Property(l => l.Date).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<ServicePack>(b =>
            {
                b.Property(l => l.ThirdSessionConferenceId).HasDefaultValue(string.Empty);
                b.Property(l => l.ThirdSessionConferenceName).HasDefaultValue(string.Empty);
                b.Property(l => l.SessionAddress).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<PersonContract>(b =>
            {
                b.HasIndex(d => d.PerContractNumber).IsUnique();
                b.Property(l => l.Owerid).HasDefaultValue(string.Empty);
                b.Property(l => l.Ower).HasDefaultValue(string.Empty);
                b.Property(l => l.ConferenceId).HasDefaultValue(string.Empty);
                b.Property(l => l.CTypeCode).HasDefaultValue(string.Empty);
                b.Property(l => l.IsCheckIn).HasDefaultValue(false);
                b.Property(l => l.IsSendEmail).HasDefaultValue(false);
                b.Property(l => l.IsModify).HasDefaultValue(true);
                b.Property(l => l.IsFianceRecord).HasDefaultValue(false);
                b.Property(l => l.IsInviteCode).HasDefaultValue(false);
                b.Property(l => l.InviteCodeId).HasDefaultValue(string.Empty);
                b.Property(l => l.IsCommitAbstract).HasDefaultValue(false);
                b.Property(l => l.IsPrint).HasDefaultValue(false);
                b.Property(l => l.PaidAmount).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<ContractType>(b =>
            {
                b.HasIndex(d => d.CTypeCode).IsUnique();
                b.Property(l => l.IsSpeaker).HasDefaultValue(false);
                b.Property(l => l.IsGive).HasDefaultValue(false);
            });

            modelBuilder.Entity<ContractStatusDic>(b =>
            {
                b.HasIndex(d => d.StatusCode).IsUnique();
            });

            modelBuilder.Entity<PersonContractActivityMap>(b =>
            {
                b.Property(l => l.IsConfirm).HasDefaultValue(false);
                b.Property(l => l.IsCheck).HasDefaultValue(false);
                b.Property(l => l.PersonContractId).HasDefaultValue(string.Empty);
                b.Property(l => l.SessionConferenceID).HasDefaultValue(string.Empty);
                b.Property(l => l.SessionConferenceName).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<CCNumberConfig>(b =>
            {
                b.Property(l => l.ConferenceId).HasDefaultValue(string.Empty);
                b.Property(l => l.ConferenceName).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<ServicePackActivityMap>(b =>
            {
                b.Property(l => l.SessionConferenceID).HasDefaultValue(string.Empty);
                b.Property(l => l.SessionConferenceName).HasDefaultValue(string.Empty);
                b.Property(l => l.ActivityId).HasDefaultValue(string.Empty);
                b.Property(l => l.SessionIDs).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<TagType>(b =>
            {
                b.HasIndex(d => d.Code).IsUnique();
            });

            modelBuilder.Entity<ConferenceOnsite>(b =>
            {
                b.Property(l => l.Remark).HasColumnType("text");
            });

            modelBuilder.Entity<OperateRecord>(b =>
            {
                b.Property(l => l.OperateContent).HasColumnType("text");
                b.Property(l => l.OperateContent).HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<RemarkDic>(b =>
            {
                b.Property(l => l.ContentCn).HasColumnType("text");
                b.Property(l => l.ContentEn).HasColumnType("text");
                b.Property(l => l.ContentJp).HasColumnType("text");
                b.HasIndex(d => d.ContentCode).IsUnique();
            });
        }
    }
}
