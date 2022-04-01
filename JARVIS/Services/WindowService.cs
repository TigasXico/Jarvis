using System;
using System.Collections.Generic;
using System.Windows;

namespace Jarvis.Services
{
    public static class WindowService
    {
        private static readonly Dictionary<object , Window> viewModelToWindowMapping = new Dictionary<object , Window>();

        public static bool ShowWindowForController( object viewModel , string windowTitle )
        {
            if ( string.IsNullOrWhiteSpace( windowTitle ) )
            {
                windowTitle = "Nova Janela";
            }

            Window window = new Window
            {
                Title = windowTitle ,
                Content = viewModel ,
                DataContext = viewModel ,
                ShowInTaskbar = true ,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            viewModelToWindowMapping[viewModel] = window;

            window.SizeToContent = SizeToContent.WidthAndHeight;

            bool? result = window.ShowDialog();

            return result.HasValue && result.Value;
        }

        public static void CloseWindowOfViewModel( object viewModel , bool? dialogResult = null )
        {
            if ( viewModelToWindowMapping.ContainsKey( viewModel ) )
            {
                Window targetWindow = viewModelToWindowMapping[viewModel];
                targetWindow.DialogResult = dialogResult;
                targetWindow.Close();
            }
        }

        public static bool? DisplayMessage( MessageType messageType , string message , string title = null )
        {
            bool? result = null;

            switch ( messageType )
            {
                case MessageType.Error:
                    MessageBox.Show( message , title , MessageBoxButton.OK , MessageBoxImage.Error );
                    break;
                case MessageType.Warning:
                    MessageBox.Show( message , title , MessageBoxButton.OK , MessageBoxImage.Warning );
                    break;
                case MessageType.Information:
                    MessageBox.Show( message , title , MessageBoxButton.OK , MessageBoxImage.Information );
                    break;
                case MessageType.Confirmation:
                    MessageBoxResult dialogResult = MessageBox.Show( message , title , MessageBoxButton.YesNo , MessageBoxImage.Question );
                    result = dialogResult == MessageBoxResult.Yes;
                    break;
                case MessageType.SuccessfullResult:
                    MessageBox.Show( message , "Operação bem sucedida" , MessageBoxButton.OK , MessageBoxImage.Information );
                    break;
                case MessageType.UnsuccessfullResult:
                    MessageBox.Show( message , "Operação não sucedida" , MessageBoxButton.OK , MessageBoxImage.Error );
                    break;
                case MessageType.None:
                default:
                    MessageBox.Show( message );
                    break;
            }

            return result;
        }

        public static void ShowException( Exception exception )
        {
            DisplayMessage( MessageType.Error , exception.Message + " " + exception.StackTrace , exception.GetType().FullName );
        }
    }

    public enum MessageType
    {
        None,
        Error,
        Warning,
        Information,
        Confirmation,
        SuccessfullResult,
        UnsuccessfullResult
    }
}
