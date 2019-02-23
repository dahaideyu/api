using System;
using System.Collections.Generic;
using System.Configuration;

namespace Koowoo.Services
{
    public class Constant
    {
        public static string SUPER_ADMIN = ConfigurationManager.AppSettings["superAdmin"];
        public static bool Sysc = ConfigurationManager.AppSettings["Synchronization"] == "true" ? true : false;
        public static string LkbAccount = ConfigurationManager.AppSettings["LkbAccount"];
        public static string LkbPassword = ConfigurationManager.AppSettings["LkbPassword"];
        public static string LkbCompanyId = ConfigurationManager.AppSettings["LkbCompanyId"];

        public static byte[] DescKeyBytes = {0x11, 0x22, 0x4F, 0x58,
            (byte)0x88, 0x10, 0x40, 0x38, 0x28, 0x25, 0x79, 0x51,
            (byte)0xCB,
            (byte)0xDD, 0x55, 0x66, 0x77, 0x29, 0x74,
            (byte)0x98, 0x30, 0x40, 0x36,
            (byte)0xE2
            };

        public static string FormatDateTime(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToString("yyyyMMddHHmmss") : "";
        }

        public static List<int> ConvertTypeList = new List<int>{254,294};
    }
}
