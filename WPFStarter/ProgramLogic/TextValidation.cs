using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFStarter.ProgramLogic
{
    internal class TextValidation
    {
        ///<summary>
        /// E.A.T. 7-February-2025
        /// Checking the entered word.
        ///</summary>
        public static void CheckingWord(string? word, out bool outWord)
        {
            Debug.WriteLine("### Start of method CheckingWord ###");
            outWord = false;
            if (!string.IsNullOrEmpty(word))
            {
                int lengthWord = word.Length;
                string capitalLetters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                string lowercaseLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя-";
                int counter = 0;
                for (int i = 0; i < capitalLetters.Length; i++)
                {
                    if (word[0] == capitalLetters[i])
                    {
                        for (int j = 0; j < lowercaseLetters.Length; j++)
                        {
                            for (int k = 0; k < lengthWord; k++)
                            {
                                if (lowercaseLetters[j] == word[k])
                                {
                                    counter++;
                                }
                            }
                        }
                        for (int j = 0; j < capitalLetters.Length; j++)
                        {
                            for (int k = 0; k < lengthWord; k++)
                            {
                                if (capitalLetters[j] == word[k])
                                {
                                    counter++;
                                }
                            }
                        }
                    }
                }
                if (counter == 0)
                {
                    MessageBox.Show($"{word} нужно ввести с большой буквы!");
                }
                else if (counter != lengthWord)
                {
                    MessageBox.Show($"После первой заглавной буквы Вы ввели недопустимый симол или ввели пробел! Вы ввели: {word}");
                }
                else
                {
                    outWord = true;
                }

            }
            Debug.WriteLine("### End of method CheckingWord ###");
        }
    }
}
