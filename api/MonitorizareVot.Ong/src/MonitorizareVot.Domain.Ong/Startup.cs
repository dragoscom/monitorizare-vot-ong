﻿using System.IO;
using Microsoft.Extensions.Configuration;

namespace MonitorizareVot.Domain.Ong
{
    public class Startup
    {
        public static IConfigurationRoot RegisterConfiguration()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile("appsettings.development.json", optional: true);

            return builder.Build();
        }
    }
}
