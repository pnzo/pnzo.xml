using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xml.task.Data
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
        public ObservableCollection<CurveData> Curves { get; set; }
        public string CurvesString
        {
            get
            {
                if (Curves.Count == 0)
                    return @"";
                var curvesString = @"";
                for (int i = 0; i < Curves.Count-1; i++)
                {
                    curvesString += $@"{Curves[i].Name}, ";
                }
                curvesString += Curves.Last().Name;
                return curvesString;
            }
        }

        public PlotData()
        {
            Curves = new ObservableCollection<CurveData>();
            Curves.CollectionChanged += ContentCollectionChanged;
        }

        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                OnPropertyChanged(@"CurvesString");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
