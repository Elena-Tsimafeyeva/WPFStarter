using Moq;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class TestConnectionTests
    {
        [Fact]
        public async Task TestConnectionAsync_ReturnsTrue_WhenConnectionSucceeds()
        {
            var messageBox = new Mock<IMessageBox>();
            var connectionWrapper = new Mock<ISqlConnectionWrapper>();
            var connectionFactory = new Mock<ISqlConnection>();

            connectionWrapper.Setup(c => c.OpenAsync()).Returns(Task.CompletedTask);
            connectionFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(connectionWrapper.Object);

            var service = new TestConnection(messageBox.Object, connectionFactory.Object);
            var result = await service.TestConnectionAsync("valid-connection");

            Assert.True(result);
            messageBox.Verify(m => m.Show(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task TestConnectionAsync_ReturnsFalse_WhenSqlExceptionOccurs()
        {
            var messageBox = new Mock<IMessageBox>();
            var connectionWrapper = new Mock<ISqlConnectionWrapper>();
            var connectionFactory = new Mock<ISqlConnection>();

            connectionWrapper.Setup(c => c.OpenAsync()).ThrowsAsync(new Exception("Ошибка подключения"));
            connectionFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(connectionWrapper.Object);

            var service = new TestConnection(messageBox.Object, connectionFactory.Object);
            var result = await service.TestConnectionAsync("invalid-connection");

            Assert.False(result);
            messageBox.Verify(m => m.Show(It.Is<string>(s => s.Contains("Ошибка подключения"))), Times.Once);
        }

        [Fact]
        public async Task TestConnectionAsync_ReturnsFalse_WhenGenericExceptionOccurs()
        {
            var messageBox = new Mock<IMessageBox>();
            var connectionWrapper = new Mock<ISqlConnectionWrapper>();
            var connectionFactory = new Mock<ISqlConnection>();

            connectionWrapper.Setup(c => c.OpenAsync()).ThrowsAsync(new Exception("Неизвестная ошибка"));
            connectionFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(connectionWrapper.Object);

            var service = new TestConnection(messageBox.Object, connectionFactory.Object);
            var result = await service.TestConnectionAsync("broken-connection");

            Assert.False(result);
            messageBox.Verify(m => m.Show(It.Is<string>(s => s.Contains("Неизвестная ошибка"))), Times.Once);

        }
}
}
