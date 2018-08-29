using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using xml.task.Model.Commands;

namespace xml.task
{
    public partial class ExecutingWindow
    {
        readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public List<Command> Commands;
        Task _task;
        public ExecutingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var command in Commands)
            {
                CommandListBox.Items.Add(command);
            }

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
                    CommandListBox_SelectionChanged(this, null);
                    ProgressBar.Value++;
                }));
            }

            Dispatcher.BeginInvoke(new Action(delegate { Title += @" - finished"; }));

        }

        private void CommandListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var command = (Command)CommandListBox.SelectedItem;
            if (command == null)
            {
                return;
            }
            ResultText.Text = command.ResultMessage;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _cancelToken.Cancel();
        }
    }
}
