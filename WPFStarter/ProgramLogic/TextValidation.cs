using System.Diagnostics;


namespace WPFStarter.ProgramLogic
{
    public class TextValidation
    {
        private readonly Interfaces.IMessageBox _messageBox;
        public TextValidation(Interfaces.IMessageBox messageBox)
        {
            _messageBox = messageBox;
        }
        ///<summary>
        /// E.A.T. 7-February-2025
        /// Checking the entered word.
        ///</summary>
        public void CheckingWord(string? word, out bool outWord)
        {
            Debug.WriteLine("### Start of method CheckingWord ###");
            outWord = false;

            if (!string.IsNullOrEmpty(word))
            {
                string capitalLetters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                string lowercaseLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя-";

                if (!capitalLetters.Contains(word[0]))
                {
                    _messageBox.Show($"{word} нужно ввести с большой буквы!");
                    return;
                }

                for (int i = 1; i < word.Length; i++)
                {
                    if (!lowercaseLetters.Contains(word[i]))
                    {
                        _messageBox.Show($"После первой заглавной буквы Вы ввели недопустимый символ или пробел! Вы ввели: {word}");
                        return;
                    }
                }

                outWord = true;
            }

            Debug.WriteLine("### End of method CheckingWord ###");
        }
    }
}
