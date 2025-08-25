
using System.Windows;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarter.ProgramLogic.Services
{
    ///<summary>
    /// E.A.T. 24-August-2025
    /// Implementation of the IMessageBox interface for displaying messages and confirmation dialogs.
    ///</summary>
    internal class MessageBoxService:IMessageBox
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public MessageBoxResult ShowConfirmation(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

    }
}
