using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Jarvis.Utils.Controls
{
    /// <summary>
    /// Interaction logic for PasswordBox.xaml
    /// </summary>
    public partial class CustomPasswordBox : UserControl
    {
        private bool ignoreUpdate = false;

        public static DependencyProperty PasswordTextProperty = DependencyProperty.Register(
                    "PasswordText" ,
                    typeof( string ) ,
                    typeof( CustomPasswordBox ) ,
                    new FrameworkPropertyMetadata( "" , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );

        public CustomPasswordBox()
        {
            InitializeComponent();
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            passwordTextBox.TextChanged += PasswordTextBox_TextChanged;
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty( PasswordTextProperty , typeof( CustomPasswordBox ) );
            dpd.AddValueChanged( this , PasswordText_PropertyChanged );
        }

        private void PasswordText_PropertyChanged( object sender , EventArgs e )
        {
            if ( !ignoreUpdate )
            {
                ignoreUpdate = true;
                passwordBox.Password = PasswordText;
                passwordTextBox.Text = PasswordText;
                ignoreUpdate = false;
            }
        }

        private void PasswordBox_PasswordChanged( object sender , RoutedEventArgs e )
        {
            if ( !ignoreUpdate )
            {
                ignoreUpdate = true;
                PasswordText = passwordBox.Password;
                passwordTextBox.Text = passwordBox.Password;
                ignoreUpdate = false;
            }
        }

        private void PasswordTextBox_TextChanged( object sender , TextChangedEventArgs e )
        {
            if ( !ignoreUpdate )
            {
                ignoreUpdate = true;
                PasswordText = passwordTextBox.Text;
                passwordBox.Password = passwordTextBox.Text;
                ignoreUpdate = false;
            }
        }

        public bool displayPassword;

        private void ShowPasswordButton_Click( object sender , System.Windows.RoutedEventArgs e )
        {
            displayPassword = !displayPassword;
            if ( displayPassword )
            {
                passwordBox.Visibility = Visibility.Hidden;
                passwordTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                passwordBox.Visibility = Visibility.Visible;
                passwordTextBox.Visibility = Visibility.Hidden;
            }
        }

        public string PasswordText
        {
            get => ( string ) GetValue( PasswordTextProperty );
            set => SetValue( PasswordTextProperty , value );
        }
    }
}
