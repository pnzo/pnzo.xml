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
        public List<CurveData> Curves;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class CurveData
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public string Selection { get; set; }
        public string Formula { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для GraphForm.xaml
    /// </summary>
    public partial class GraphForm : Window, INotifyPropertyChanged
    {
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
