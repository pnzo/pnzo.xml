using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xml.task.Data
{
    public class CurveData : INotifyPropertyChanged
    {
        private string _name = @"Новая кривая";
        private string _table;
        private string _column;
        private string _selection;
        private string _formula;
        private bool _printable = true;

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

        public bool Printable
        {
            get
            {
                return _printable;
            }
            set
            {
                _printable = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
