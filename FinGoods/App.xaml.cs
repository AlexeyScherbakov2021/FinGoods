using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FinGoods
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Window FocusedWindow => Current?.Windows?.Cast<Window>().FirstOrDefault(w => w.IsFocused);
        public static Window ActiveWindow => Current?.Windows?.Cast<Window>().FirstOrDefault(w => w.IsActive);

        //public static LogFile log = new LogFile();

    }
}
