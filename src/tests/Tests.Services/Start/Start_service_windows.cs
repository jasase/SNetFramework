using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Framework.Abstraction.Plugins;
using Service;
using Tests.Services.Start;
using Xunit;

namespace Tests.Services
{
    public class Start_service_windows
    {
        private ServiceBootstrap _bootstrap;

        public Start_service_windows()
        {
            _bootstrap = new ServiceBootstrap(BootstrapInCodeConfiguration.Default());
        }

        [Fact]
        public void Should_start_and_stop_without_error()
        {
            _bootstrap.StartingService();

            DummyPlugin.StartCounter.Should().Be(1);

            _bootstrap.StopingService();
        }


    }
}
