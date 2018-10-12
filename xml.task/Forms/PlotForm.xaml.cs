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

namespace xml.task.Forms
{
    /// <summary>
    /// Логика взаимодействия для PlotForm.xaml
    /// </summary>
    public partial class PlotForm : Window
    {
        public PlotData plotData;
        public PlotForm()
        {
            InitializeComponent();
            Console.WriteLine();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = plotData;
        }
    }
}
