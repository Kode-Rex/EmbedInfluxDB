# EmbedInfluxDB
A C# library to spin up InfluxDB for testing. Only supports Windows right now.

# To Use
var server = new InfluxServerBuilder().With_Port(InfluxServerBuilder.FreePort).Build(); 

server.Start();

// your test code here

server.Stop();

# Port Number
Instead of InfluxServerBuilder.FreePort you can specify the port number you would like to use
Or just use new InfluxServerBuilder().Build(); it will find a free port

# URL
server.Url will give you the url to use in your test.
