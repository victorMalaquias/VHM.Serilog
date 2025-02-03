namespace VHM.Serilog.Nuget
{
    public class LogOptions
    {
        public bool EnableConsole { get; set; }
        public string SeqUrl { get; set; }
        public string SqlServerConnection { get; set; }
        public string MongoDbConnection { get; set; }
        public string MongoDbDatabase { get; set; }
    }
}
