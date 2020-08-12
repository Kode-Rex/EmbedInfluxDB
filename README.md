# EmbedInfluxDB
A C# library to spin up InfluxDB for testing. Only supports Windows right now.

# To Use

// instead of InfluxServerBuilder.FreePort you can specify the port number you would like to use
var server = new InfluxServerBuilder().With_Port(InfluxServerBuilder.FreePort).Build(); 
server.Start();

// your test code here

server.Stop();
