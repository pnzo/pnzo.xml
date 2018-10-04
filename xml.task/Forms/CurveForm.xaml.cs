using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using xml.task.Model.RastrManager;

namespace xml.task.Forms
{
    /// <summary>
    /// Логика взаимодействия для CurveForm.xaml
    /// </summary>
    public partial class CurveForm : Window, INotifyPropertyChanged
    {

        private List<RastrTableTemplate> _tableTemplates = RastrOperations.tables;
        public List<RastrTableTemplate> TableTemplates
        {
            get
            {
                return _tableTemplates;
            }
            set
            {
                _tableTemplates = value;
                OnPropertyChanged();
            }
        }
        public PlotData Plot;
        public CurveData Curve { get; set; }

        public CurveForm()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Plot.Curves.Add(Curve);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
