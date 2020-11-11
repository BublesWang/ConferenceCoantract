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

        private static string _connstr_unittest = ConfigurationManager.ConnectionStrings["connstr_debug_unittest"].ConnectionString;
        private static string _connstr_stable = ConfigurationManager.ConnectionStrings["connstr_debug_stable"].ConnectionString;

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

            var _sql = result.Equals("unittest") ? _connstr_unittest : _connstr_stable;
            return _sql;
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

        public static string EnvName()
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

            var _str = result.Equals("unittest") ? "unittest" : "stable";
            return _str;
        }
    }
}
