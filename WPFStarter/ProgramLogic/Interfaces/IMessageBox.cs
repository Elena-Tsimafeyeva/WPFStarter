
using System.Windows;

namespace WPFStarter.ProgramLogic.Interfaces
{
    ///<summary>
    /// E.A.T. 24-August-2025
    /// Interface for displaying messages and confirmation dialogs to the user.
    ///</summary>
    internal interface IMessageBox
    {
        void Show(string message);
        MessageBoxResult ShowConfirmation(string message, string caption);
    }
}
