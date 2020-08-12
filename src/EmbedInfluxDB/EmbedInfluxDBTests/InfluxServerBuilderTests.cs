using EmbedInfluxDB;
using FluentAssertions;
using NUnit.Framework;
using System;

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
            public void foo()
            {
                // arrange
                var server = new InfluxServerBuilder()
                                 .Build();
                // act
                Console.WriteLine(server.Url);
                server.Start();
                // todo : try and query the DB
                // assert
                server.Stop();
            }
        }
    }
}
