using StoneAge.Synchronous.Process.Runner.PipeLineTask;
using System;

namespace EmbedInfluxDB
{
    public class InfluxDbServerTask : GenericBackgroundTask
    {
        public InfluxDbServerTask(string applicationPath, string arguments) : base(applicationPath, arguments) { }

        public override int ProcessTimeout() => throw new NotImplementedException();
    }
}
