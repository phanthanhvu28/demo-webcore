namespace demo_webcore.Constants
{
    public class Common
    {
        public struct Service
        {
            public const string Name = "Service:Name";
        }
        public struct Serilog
        {
            public const string TraceId = "TraceId";
            public const string Environment = "Environment";
            public const string Service = "ServiceName";
        }
        public struct Grafana
        {
            public const string LokiUrl = "Grafana:Loki:Url";

            public const string TempoUrl = "Grafana:Tempo:Url";
            public const string TempoService = "Grafana:Tempo:ServiceName";
            public const string TempoSource = "Grafana:Tempo:SourceName";
        }
    }
}
