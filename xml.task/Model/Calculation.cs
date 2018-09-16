using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using xml.task.Model.Commands;

namespace xml.task.Model
{
   public class Calculation : INotifyPropertyChanged
    {
        private List<Command> _commands;
        private string _status;

        public List<Command> Commands
        {
            get => _commands;
            set
            {
                _commands = value;
                OnPropertyChanged();
            }
        }
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

}
