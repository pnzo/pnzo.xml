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
using System.Collections;
using xml.task.Data;

namespace xml.task.Forms
{

    /// <summary>
    /// Логика взаимодействия для GraphForm.xaml
    /// </summary>
    public partial class GraphForm : Window, INotifyPropertyChanged
    {
        private string _time = @"0";
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                double d;
                if (double.TryParse(value, out d) == false)
                    _time = @"0";
                else
                    _time = value;

            }
        }
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


        private void DeletePlotWithContextMenu(object sender, RoutedEventArgs e)
        {
            Plots.Remove((PlotData)PlotsListBox.SelectedItem);
        }

        private void EditPlotWithContextMenu(object sender, RoutedEventArgs e)
        {
            {

            }
            if (PlotsListBox.SelectedItem == null)
                return;
            var window = new PlotForm
            {
                Owner = this,
                plotData = (PlotData)PlotsListBox.SelectedItem
            };
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
