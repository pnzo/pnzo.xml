using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using xml.task.Model.Commands;

namespace xml.task
{
    public partial class ExecutingWindow
    {
        readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();
        public List<Command> Commands;
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
            Dispatcher.BeginInvoke(new Action(delegate { ProgressBar.Maximum = Commands.Count; }));

            foreach (var command in Commands)
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
    }
}
