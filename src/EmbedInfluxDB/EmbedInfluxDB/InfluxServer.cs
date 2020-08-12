using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Presenters;
using StoneAge.Synchronous.Process.Runner;
using System;
using System.IO;

namespace EmbedInfluxDB
{
    public class InfluxServer : IDisposable
    {
        public int Port { get; internal set; }
        public string User { get; internal set; }
        public string Pass { get; internal set; }
        public bool UseSsl { get; internal set; }

        public string Url => Format_Url();

        private IBackgroundAction _action;

        public void Start()
        {
            // todo : detect OS and start correct version
            var arguments = Build_Arguments();
            var exePath = Get_Exe_Path();

            _action = new BackgroundAction(new InfluxDbServerTask(exePath, arguments));
            var presenter = new ErrorOnlyPropertyPresenter<ErrorOutput>();
            _action.Start(presenter);

            if (presenter.Output != null && presenter.Output.HasErrors)
            {
                throw new InfluxServerStartException($"Failed to start InfluxDb because of [{string.Join(",", presenter.Output.Errors)}]");
            }
        }

        private static string Get_Exe_Path()
        {
            var currentPath = Directory.GetCurrentDirectory();
            var exePath = $"{currentPath}{Path.DirectorySeparatorChar}InfluxDb{Path.DirectorySeparatorChar}influxd.exe";
            return exePath;
        }

        private string Build_Arguments()
        {
            // # bind-address = ":8086"
            
            
            var currentPath = Directory.GetCurrentDirectory();
            var configurationFilePath = $"{currentPath}{Path.DirectorySeparatorChar}InfluxDb{Path.DirectorySeparatorChar}influxdb.conf";
            var configSetting = $"-config {configurationFilePath}";

            var configuration = File.ReadAllText(configurationFilePath);
            configuration = configuration.Replace($"# bind-address = \":8086\"", "bind-address = \":"+Port+"\"");
            File.WriteAllText(configurationFilePath, configuration);

            return configSetting;
        }

        public void Stop()
        {
            _action?.Stop();
        }

        public void Dispose()
        {
            _action?.Stop();
        }

        private string Format_Url()
        {
            if (UseSsl)
            {
                return $"https://localhost:{Port}";
            }

            return $"http://localhost:{Port}";
        }
    }
}