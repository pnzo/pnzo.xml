using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using xml.task.Model;
using xml.task.Model.Commands;
using xml.task.Model.Commands.SimpleCommands;
using xml.task.Windows;

namespace xml.task
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    /// 

    public class CommandTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string command;
    }

    public partial class NewCommandWindow
    {
        public ObservableCollection<CommandTemplate> Templates { get; set; }
        public NewCommandWindow()
        {

            Templates = new ObservableCollection<CommandTemplate>{
            new CommandTemplate{ Name = @"Расчет динамической устойчивости", Description = @"Простой расчет переходного процесса с проверкой на устойчивость", command = @"stability"},
            new CommandTemplate{ Name = @"Вывод графиков", Description = @"Графическое отображение переходного процесса в соответствии с заданными шаблонами графиков", command = @"graph"},
            };
            InitializeComponent();
            DataContext = this;
        }
    }
}
