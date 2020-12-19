using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceContractAPI.Common
{
    public class ContextConnect
    {

        //private static string _connstr_unittest = ConfigurationManager.ConnectionStrings["connstr_unittest"].ConnectionString;
        //private static string _connstr_stable = ConfigurationManager.ConnectionStrings["connstr_stable"].ConnectionString;

        private static string _connectionString { get; set; }
        private static string _connstr_unittest = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private static string _connstr_stable = ConfigurationManager.ConnectionStrings["connstr_stable"].ConnectionString;
        private static string _connstr_local_unittest = ConfigurationManager.ConnectionStrings["local_connstr"].ConnectionString;


        public static string ReadConnstrContent()
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
                sql = _connstr_local_unittest;
            }
            else if (result.Equals("unittest"))
            {
                sql = _connstr_unittest;
            }
            else
            {
                sql = _connstr_stable;
            }
            return sql;
        }


        public static string GrpcChannelConnstrContent(string debug_channel, string server_channel)
        {
            string result = string.Empty;
            string path = @"./envgrpc";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                result = line;
                break;
            }

            var _sql = result.Equals("local_debug_unittest") ? debug_channel : server_channel;
            return _sql;
        }


    }
}
