using Moq;
using WPFStarter.ProgramLogic;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class DbConnectionSettingsTests
    {
        [Fact]
        public async Task SaveServerDatabase_CallsFileWriterWithCorrectArguments()
        {
            var mockFileWriter = new Mock<IFileWriter>();
            var dbSettings = new DbConnectionSettings(mockFileWriter.Object);
            string server = "TestServer";
            string database = "TestDatabase";

            await dbSettings.SaveServerDatabase(server, database);

            mockFileWriter.Verify(fw => fw.WriteAllTextAsync("db.txt", "TestServer TestDatabase"), Times.Once);
        }
    }
}
