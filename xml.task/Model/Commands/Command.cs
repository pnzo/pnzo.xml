using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using xml.task.Model.RastrManager;
using System.Diagnostics;

namespace xml.task.Model.Commands
{
    public abstract class Command
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<string> Files { get; set; }
        public string Status { get; set; }
        public string ResultMessage { get; set; }
        public string ErrorMessage { get; set; }

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

        }

        public override string ToString()
        {
            return Name ?? (Name = _element.ToString());
        }
    }
}
