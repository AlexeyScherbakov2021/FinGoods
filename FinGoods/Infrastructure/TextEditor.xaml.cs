using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinGoods.Infrastructure
{
    /// <summary>
    /// Логика взаимодействия для TextEditor.xaml
    /// </summary>
    public partial class TextEditor : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextEditor),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTextChanged)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextEditor ted)
            {
                ted.text.Text = e.NewValue.ToString();
                ted.textBox.Text = ted.text.Text;
            }
        }



        public TextEditor()
        {
            InitializeComponent();
        }

        private void text_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void text_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
