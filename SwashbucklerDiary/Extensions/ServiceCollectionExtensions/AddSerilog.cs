﻿using SwashbucklerDiary.Config;
using Serilog;
using Serilog.Events;

namespace SwashbucklerDiary.Extend
{
    public static partial class ServiceCollectionExtend
    {
        public static IServiceCollection AddSerilogConfig(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                 .Enrich.FromLogContext()
#if DEBUG
                 .WriteTo.Debug()
#endif
                 .WriteTo.Async(c => c.SQLite(SQLiteConstants.DatabasePath, "LogModel"))
                 .CreateLogger();

            services.AddLogging(logging =>
            {
                logging.AddSerilog(dispose: true);
            });
            return services;
        }
    }
}
