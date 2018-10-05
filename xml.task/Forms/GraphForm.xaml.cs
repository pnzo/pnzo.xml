using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public class PlotData : INotifyPropertyChanged
    {
        private string _name = "Новая область";
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CurveData> Curves = new ObservableCollection<CurveData>();
        public string CurvesString
        {
            get
            {
                var curvesString = @"";
                foreach (var curve in Curves)
                {
                    curvesString += $@"{curve.Name}, ";
                }
                return curvesString;
            }
        }

        public PlotData()
        {
            Curves.CollectionChanged += ContentCollectionChanged;
        }

        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                OnPropertyChanged(@"CurvesString");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class CurveData : INotifyPropertyChanged
    {
        private string _name = @"Новая кривая";
        private string _table = @"Таблица";
        private string _column = @"Столбец";
        private string _selection = @"Выборка";
        private string _formula = @"Формула";

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                OnPropertyChanged();
            }
        }
        public string Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
                OnPropertyChanged();
            }
        }
        public string Selection
        {
            get
            {
                return _selection;
            }
            set
            {
                _selection = value;
                OnPropertyChanged();
            }
        }
        public string Formula
        {
            get
            {
                return _formula;
            }
            set
            {
                _formula = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isNumeric = double.TryParse(value.ToString(), out double n);
            if (!isNumeric)
                return 0.0;
            return n;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }
    }

    /// <summary>
    /// Логика взаимодействия для GraphForm.xaml
    /// </summary>
    public partial class GraphForm : Window, INotifyPropertyChanged
    {
        private double _time = 0.0;
        public double Time { get; set; }
        public ObservableCollection<object> Sets { get; set; }
        public ObservableCollection<PlotData> Plots { get; set; }


        public GraphForm()
        {
            InitializeComponent();
            Sets = new ObservableCollection<object>();
            Plots = new ObservableCollection<PlotData>();
            this.DataContext = this;
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var set = new FilesSet
            {
                Files = files.ToList()
            };
            Sets.Add(set);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void AddNewPlotWithContextMenu(object sender, RoutedEventArgs e)
        {
            Plots.Add(new PlotData());
        }

        private void AddNewCurveWithContextMenu(object sender, RoutedEventArgs e)
        {
            var window = new CurveForm();
            window.Curve = new CurveData();
            window.Plot = (PlotData)PlotsListBox.SelectedItem;
            window.DataContext = window;
            window.ShowDialog();
        }
    }

    public class FilesSet
    {
        public List<string> Files { get; set; }
    }

    public class FolderSet
    {
        public string FolderPath { get; set; }
        public List<string> VarExtensions { get; set; }
        public List<string> ConstantFiles { get; set; }
    }


}
