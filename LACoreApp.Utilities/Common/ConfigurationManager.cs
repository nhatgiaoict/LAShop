using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LACoreApp.Utilities.Common
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSetting { get; }
        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        }

        public static string GetUrlRoot()
        {
            return AppSetting["SystemSettings:Domain"];
        }
    }
}
