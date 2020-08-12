using EmbedInfluxDB;
using FluentAssertions;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmbedInfluxDBTests
{
    [TestFixture]
    public class InfluxServerBuilderTests
    {
        [TestFixture]
        class Build_With
        {
            [Test]
            public void When_No_Port_Set_Expect_Random_Free_Port()
            {
                // arrange
                var server = new InfluxServerBuilder()
                                 .Build();
                // act
                var actual = server.Port;
                // assert
                actual.Should().NotBe(0);
            }

            [Test]
            public void When_Port_Set_To_Free_Port_Expect_Random_Free_Port()
            {
                // arrange
                var server = new InfluxServerBuilder()
                                 .With_Port(InfluxServerBuilder.FreePort)
                                 .Build();
                // act
                var actual = server.Port;
                // assert
                actual.Should().NotBe(0);
            }

            [Test]
            public void When_Port_Set_Expect_Port()
            {
                // arrange
                var server = new InfluxServerBuilder()
                                 .With_Port(9999)
                                 .Build();
                // act
                var actual = server.Port;
                // assert
                actual.Should().Be(9999);
            }

            //[Test]
            //public void When_Auth_Set_Expect_Auth()
            //{
            //    // arrange
            //    var server = new InfluxServerBuilder()
            //                     .With_Auth("user", "pass")
            //                     .Build();
            //    // act
            //    var actualUser = server.User;
            //    var actualPass = server.Pass;
            //    // assert
            //    actualUser.Should().Be("user");
            //    actualPass.Should().Be("pass");
            //}

            //[Test]
            //public void When_SSL_Set_Expect_UseSsl_True()
            //{
            //    // arrange
            //    var server = new InfluxServerBuilder()
            //                     .With_Ssl()
            //                     .Build();
            //    // act
            //    var actual = server.UseSsl;
            //    // assert
            //    actual.Should().BeTrue();
            //}
        }
        
        [TestFixture]
        class Start
        {
            [Test]
            public async Task Should_Start_Server()
            {
                // arrange
                var server = new InfluxServerBuilder()
                                 .Build();
                // act
                server.Start();
                using var client = new HttpClient();

                var result = await client.GetAsync($"{server.Url}/ping");
                server.Stop();

                // assert
                result.StatusCode.Should().Be(204);
            }
        }
    }
}
