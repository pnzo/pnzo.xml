using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using xml.task.Model.RastrManager;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace xml.task.Model.Commands
{
    public abstract class Command : INotifyPropertyChanged
    {
        private List<string> files;
        private string status;
        private string filesString;
        private string errorMessage;

        public string Name { get; set; }
        public int Id { get; set; }
        public List<string> Files
        {
            get
            {
                return files;
            }
            set
            {
                files = value;
                OnPropertyChanged("FilesString");
            }
        }

        public string ResultMessage { get; set; }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
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
        public string FilesString
        {
            get
            {
                var s = @"";
                for (int i = 0; i < Files.Count; i++)
                {
                    s += i != Files.Count-1 ? $"{System.IO.Path.GetFileName(Files[i])}\r\n" : $"{System.IO.Path.GetFileName(Files[i])}"; 
                }
                return s;
            }
        }

        private readonly XElement _element;

        protected Command()
        {
        }

        protected Command(XElement xElement)
        {
            Status = @"В очереди";
            _element = xElement;
            Name = _element?.Attribute(@"name")?.Value;
        }

        public virtual void Perform()
        {
            Status = @"Выполняется";
        }

        public override string ToString()
        {
            return $@"{(Name ?? (Name = _element.ToString()))} [{Id}]";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
