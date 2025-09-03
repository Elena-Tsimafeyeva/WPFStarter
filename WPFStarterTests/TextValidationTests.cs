using Moq;
using WPFStarter.ProgramLogic;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class TextValidationTests
    {
        [Fact]
        public void CheckingWord_ValidData()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            string? word = "Елена";
            bool outWord;

            var textValidation = new TextValidation(mockMessageBox.Object);
            textValidation.CheckingWord(word, out outWord);

            Assert.True(outWord);
        }
        [Fact]
        public void CheckingWord_InvalidData_WordNotCapitalized_ShowsErrorMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            string? word = "елена";
            bool outWord;

            var textValidation = new TextValidation(mockMessageBox.Object);
            textValidation.CheckingWord(word, out outWord);

            mockMessageBox.Verify(m => m.Show($"{word} нужно ввести с большой буквы!"), Times.Once);
            Assert.False(outWord);
        }
        [Fact]
        public void CheckingWord_InvalidData_EnteredASymbol_ShowsErrorMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            string? word = "Елена@";
            bool outWord;

            var textValidation = new TextValidation(mockMessageBox.Object);
            textValidation.CheckingWord(word, out outWord);

            mockMessageBox.Verify(m => m.Show($"После первой заглавной буквы Вы ввели недопустимый символ или пробел! Вы ввели: {word}"), Times.Once);
            Assert.False(outWord);
        }
        [Fact]
        public void CheckingWord_InvalidData_EnteredASpace_ShowsErrorMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            string? word = "Еле на";
            bool outWord;

            var textValidation = new TextValidation(mockMessageBox.Object);
            textValidation.CheckingWord(word, out outWord);

            mockMessageBox.Verify(m => m.Show($"После первой заглавной буквы Вы ввели недопустимый символ или пробел! Вы ввели: {word}"), Times.Once);
            Assert.False(outWord);
        }
    }
}
