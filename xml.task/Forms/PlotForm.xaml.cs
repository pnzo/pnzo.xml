using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
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
using xml.task.Data;
using xml.task.Model.RastrManager;
using System.Collections.ObjectModel;

namespace xml.task.Forms
{
    /// <summary>
    /// Логика взаимодействия для PlotForm.xaml
    /// </summary>
    public partial class PlotForm : Window, INotifyPropertyChanged
    {
        private CurveData _curveData;
        private List<RastrColumnTemplate> _columnTemplates = new List<RastrColumnTemplate>();

        public PlotData plotData { get; set; }
        public List<RastrTableTemplate> TableTemplates
        {
            get
            {
                return RastrOperations.tables;
            }
        }

        public List<RastrColumnTemplate> ColumnTemplates
        {
            get
            {
                return _columnTemplates;
            }
            set
            {
                _columnTemplates = value;
                OnPropertyChanged();
            }
        }

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

        private void Window_Closed(object sender, EventArgs e)
        {
            plotData.ContentCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tableTemplate = (RastrTableTemplate)TableComboBox.SelectedItem;

            if (tableTemplate!=null)
            {
                CurveData.Selection = tableTemplate.DefaultSelection;
                ColumnTemplates = RastrOperations.columns(tableTemplate.Name).Where(k => k.HasTransientGraph == true).ToList();
            }

        }

        private void ColumnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PlotForm_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine(@"!!!!");
            var formats = e.Data.GetFormats();
            var data = e.Data.GetData(formats[3]);

        }
    }
}
