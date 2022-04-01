using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Jarvis.Utils.Controls
{
    /// <summary>
    /// Interaction logic for LabelledTextBox.xaml
    /// </summary>
    public partial class LabelledTextBox : UserControl
    {
        public static DependencyProperty LabelProperty = DependencyProperty.Register(
                    "Label" ,
                    typeof( string ) ,
                    typeof( LabelledTextBox ) ,
                    new FrameworkPropertyMetadata( "Unnamed Label" ) );

        public static DependencyProperty TextProperty = DependencyProperty.Register(
                    "Text" ,
                    typeof( string ) ,
                    typeof( LabelledTextBox ) ,
                    new FrameworkPropertyMetadata( "" , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );

        public static DependencyProperty ReadOnlyProperty = DependencyProperty.Register(
                    "ReadOnly" ,
                    typeof( bool ) ,
                    typeof( LabelledTextBox ) ,
                    new FrameworkPropertyMetadata( true ) );

        public string Label
        {
            get => ( string ) GetValue( LabelProperty );
            set => SetValue( LabelProperty , value );
        }

        public string Text
        {
            get => ( string ) GetValue( TextProperty );
            set => SetValue( TextProperty , value );
        }

        public bool ReadOnly
        {
            get => ( bool ) GetValue( ReadOnlyProperty );
            set => SetValue( ReadOnlyProperty , value );
        }


        public LabelledTextBox()
        {
            InitializeComponent();
            Root.DataContext = this;
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty( ReadOnlyProperty , typeof( LabelledTextBox ) );
            dpd.AddValueChanged( this , ReadOnly_PropertyChanged );
        }

        private void ReadOnly_PropertyChanged( object sender , EventArgs e )
        {
            contentTextBox.IsReadOnly = ReadOnly;
        }

    }
}
