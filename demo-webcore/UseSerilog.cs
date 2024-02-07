using Serilog;
using Serilog.Sinks.Grafana.Loki;
using System.Diagnostics;

namespace demo_webcore
{
    public static class UseSerilog
    {
        private const string OutputTemplate =
       "[{Timestamp:dd-MM-yyyy HH:mm:ss}] [{Level:u3}] [{Environment}] [{ServiceName}] [{TraceId}] {Message}{NewLine}{Exception}";

        public static readonly string DefaultTraceId = ActivityTraceId.CreateRandom().ToString();

        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder,
            IConfiguration configuration)
        {
            string serviceName = builder.GetServiceName();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty(Constants.Common.Serilog.Environment, builder.Environment.EnvironmentName)
                .Enrich.WithProperty(Constants.Common.Serilog.TraceId, DefaultTraceId)
                .Enrich.WithProperty(Constants.Common.Serilog.Service, serviceName)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                // TODO: disable write log to Loki. Devops guy said because this make memory leak
                .WriteTo.GrafanaLoki(
                    configuration[Constants.Common.Grafana.LokiUrl]!,
                    GetLogLabel(builder.Environment.EnvironmentName, serviceName),
                    credentials: null)
                .WriteTo.Console(outputTemplate: OutputTemplate)
                .CreateLogger();

            builder.Host.UseSerilog();
            return builder;
        }
        private static string GetServiceName(this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            return configuration[Constants.Common.Service.Name] ?? builder.Environment.ApplicationName;
        }
        private static List<LokiLabel> GetLogLabel(string environmentName, string serviceName)
        {
            List<LokiLabel> list = new()
        {
            new LokiLabel
            {
                Key = "env",
                Value = environmentName
            },
            new LokiLabel
            {
                Key = "service",
                Value = serviceName
            }
        };
            return list;
        }
    }
}
