using ConferenceContractAPI.CCDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ConferenceContractAPI.Config
{
    public class DatabaseConfig
    {
        private static DbContextOptionsBuilder<MemberDBContext> _memberOptions;
        private static DbContextOptionsBuilder<RoleDBContext> _roleOptions;

        #region 会员连接字符串
        private static string _member_connectionString { get; set; }
        private static string _member_connstr_unittest = ConfigurationManager.ConnectionStrings["member_connstr"].ConnectionString;
        private static string _member_connstr_stable = ConfigurationManager.ConnectionStrings["member_connstr_stable"].ConnectionString;
        private static string _member_connstr_local_unittest = ConfigurationManager.ConnectionStrings["member_local_connstr"].ConnectionString;
        #endregion

        #region Role连接字符串
        private static string _role_connectionString { get; set; }
        private static string _role_connstr_unittest = ConfigurationManager.ConnectionStrings["role_connstr"].ConnectionString;
        private static string _role_connstr_stable = ConfigurationManager.ConnectionStrings["role_debug_stable"].ConnectionString;
        private static string _role_connstr_local_unittest = ConfigurationManager.ConnectionStrings["role_debug_unittest"].ConnectionString;
        #endregion

        public static string ReadMemberConnstrContent()
        {
            string result = string.Empty;
            string path = @"./env";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                result = line;
                break;
            }
            string sql = string.Empty;

            if (result.Equals("localtest"))
            {
                sql = _member_connstr_local_unittest;
            }
            else if (result.Equals("unittest"))
            {
                sql = _member_connstr_unittest;
            }
            else
            {
                sql = _member_connstr_stable;
            }

            return sql;
        }

        public static string ReadRoleConnstrContent()
        {
            string result = string.Empty;
            string path = @"./env";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                result = line;
                break;
            }
            string sql = string.Empty;

            if (result.Equals("localtest"))
            {
                sql = _role_connstr_local_unittest;
            }
            else if (result.Equals("unittest"))
            {
                sql = _role_connstr_unittest;
            }
            else
            {
                sql = _role_connstr_stable;
            }

            return sql;
        }


        public static DbContextOptions<MemberDBContext> GetMemberDbOptions()
        {
            if (_memberOptions == null)
            {
                _memberOptions = new DbContextOptionsBuilder<MemberDBContext>();
                _member_connectionString = ReadMemberConnstrContent();
                _memberOptions.UseNpgsql(_member_connectionString);
            }
            return _memberOptions.Options;
        }

        public static DbContextOptions<RoleDBContext> GetRoleDbOptions()
        {
            if (_roleOptions == null)
            {
                _roleOptions = new DbContextOptionsBuilder<RoleDBContext>();
                _role_connectionString = ReadRoleConnstrContent();
                _roleOptions.UseNpgsql(_role_connectionString);
            }
            return _roleOptions.Options;
        }
    }
}