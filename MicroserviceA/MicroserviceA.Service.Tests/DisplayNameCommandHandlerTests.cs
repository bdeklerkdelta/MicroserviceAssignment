using MicroserviceA.Messaging.Send;
using MicroserviceA.Service.Command;
using System;
using Xunit;
using Moq;
using MicroserviceA.Domain;

namespace MicroserviceA.Service.Tests
{
    public class DisplayNameCommandHandlerTests
    {
        private DisplayNameCommandHandler _displayNameCommandHandler;

        [Fact]
        public void Handle_ShouldCallDisplayNamePublisher()
        {
            var displayNamePublisher = new Mock<IDisplayNamePublisher>();
            _displayNameCommandHandler = new DisplayNameCommandHandler(displayNamePublisher.Object);

            _displayNameCommandHandler.Handle(new DisplayNameCommand(), default);

            displayNamePublisher.Verify(x => x.SendDisplayName(It.IsAny<DisplayName>()), Times.Once());
        }
    }
}
