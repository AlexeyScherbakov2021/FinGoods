using System;
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
using System.Windows.Shapes;

namespace FinGoods.View
{
    /// <summary>
    /// Логика взаимодействия для ModulWindow.xaml
    /// </summary>
    public partial class ModulWindow : Window
    {
        public ModulWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NumberModul.Focus();
        }
    }
}
