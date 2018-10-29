using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class PlotForm : Window, INotifyPropertyChanged
    {
        private CurveData _curveData;

        public PlotData plotData { get; set; }
        public CurveData CurveData
        {
            get
            {
                return _curveData;
            }
            set
            {
                _curveData = value;
                OnPropertyChanged();
            }
        }
        public PlotForm()
        {
            InitializeComponent();
            Console.WriteLine();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            plotData.Curves.Add(new CurveData());
        }

        private void CurvesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurveData = (CurveData)CurvesListBox.SelectedItem;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
