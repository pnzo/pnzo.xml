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
        private List<Command> commands;
        private string status;

        public List<Command> Commands
        {
            get
            {
                return commands;
            }
            set
            {
                commands = value;
                OnPropertyChanged("Commands");
            }
        }
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

}
