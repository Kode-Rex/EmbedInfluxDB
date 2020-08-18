using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Presenters;
using StoneAge.Synchronous.Process.Runner;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

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

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var linuxExePath = $"{currentPath}{Path.DirectorySeparatorChar}InfluxDb{Path.DirectorySeparatorChar}influxd";
                return linuxExePath;
            }

            var windowsExePath = $"{currentPath}{Path.DirectorySeparatorChar}InfluxDb{Path.DirectorySeparatorChar}influxd.exe";
            return windowsExePath;
        }

        private string Build_Arguments()
        {            
            var currentPath = Directory.GetCurrentDirectory();
            var configurationFilePath = $"{currentPath}{Path.DirectorySeparatorChar}InfluxDb{Path.DirectorySeparatorChar}influxdb.conf";
            var configSetting = $"-config {configurationFilePath}";

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "EmbedInfluxDB.InfluxDb.influxdb.conf";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                var configuration = reader.ReadToEnd();
                configuration = configuration.Replace($"# bind-address = \":8086\"", "bind-address = \":" + Port + "\"");
                File.WriteAllText(configurationFilePath, configuration);
            }

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