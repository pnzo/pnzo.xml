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
    /// Логика взаимодействия для NewCurveForm.xaml
    /// </summary>
    public partial class NewCurveForm : Window, INotifyPropertyChanged
    {

        private List<RastrTableTemplate> _tableTemplates = RastrOperations.tables;
        private List<RastrColumnTemplate> _columnTemplates;
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

        public PlotData Plot;
        public CurveData Curve { get; set; }

        public NewCurveForm()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {


        }

        private void TableContextMenuClick(object sender, RoutedEventArgs e)
        {
            var template = (RastrTableTemplate)((MenuItem)sender).Header;
            TableTextBlock.Text = template.Name;
            SelectionTextBlock.Text = template.DefaultSelection;
            ColumnTemplates = RastrOperations.columns(template.Name).Where(t => t.HasTransientGraph).ToList();
        }

        private void ColumnContextMenuClick(object sender, RoutedEventArgs e)
        {
            var template = (RastrColumnTemplate)((MenuItem)sender).Header;
            ColumnTextBlock.Text = template.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void CurveWindow_Closing(object sender, CancelEventArgs e)
        {
            var dialogResult = MessageBox.Show($@"Добавить кривую {Curve.Name} в область {Plot.Name}?", "pnzo", MessageBoxButton.YesNoCancel);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Plot.Curves.Add(Curve);
            }
            else if (dialogResult == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
