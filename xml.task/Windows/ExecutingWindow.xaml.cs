using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using xml.task.Model;
using xml.task.Model.Commands;
using xml.task.Model.Commands.SimpleCommands;
using xml.task.Windows;

namespace xml.task
{
    public partial class ExecutingWindow
    {
        readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();
        public Calculation Calculation { get; set; }
        private Task _task;
        public ExecutingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _task = new Task(Run, _cancelToken.Token);
            _task.Start();
        }

        private void Run()
        {
            Dispatcher.BeginInvoke(new Action(delegate { ProgressBar.Maximum = Calculation.Commands.Count; }));

            foreach (var command in Calculation.Commands)
            {
                command.Perform();
                Dispatcher.BeginInvoke(new Action(delegate 
                {
                    ProgressBar.Value++;
                }));
            }
            Dispatcher.BeginInvoke(new Action(delegate { Title += @" - finished"; }));
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _cancelToken.Cancel();
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Command command;
            Window window = new Window();
            if (dataGrid.SelectedItem is PlotCommand)
            {
                command = dataGrid.SelectedItem as PlotCommand;
                window = new PlotWindow(command);
            }
            else
            {
                command = new ErrorCommand();
            }
            
            window.Owner = this;
            window.Title = $@"{command.Name} [{command.Id}]";
            window.Width = 600;
            window.Height = 800;
            if (!(command is ErrorCommand))
            {
                window.Show();
            }
        }
    }
}
