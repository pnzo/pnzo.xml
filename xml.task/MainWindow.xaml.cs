using System;
using ICSharpCode.AvalonEdit.Folding;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using xml.task.Model.Commands;
using xml.task.Model.RastrManager;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Xml;
using xml.task.Model;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace xml.task
{
    public partial class MainWindow
    {
        private readonly FoldingManager _foldingManager;
        private readonly XmlFoldingStrategy _foldingStrategy;
        private string _filePath;

        public MainWindow()
        {
            InitializeComponent();
            TextEditor.Encoding = Encoding.UTF8;
            var foldingUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            foldingUpdateTimer.Tick += delegate { UpdateFoldings(); };
            foldingUpdateTimer.Start();
            TextEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();

            _foldingManager = FoldingManager.Install(TextEditor.TextArea);

            _foldingStrategy = new XmlFoldingStrategy
            {
                ShowAttributesWhenFolded = true
            };
            _foldingStrategy.UpdateFoldings(_foldingManager, TextEditor.Document);

            var task = new Task(DeleteExitFiles);
            task.Start();
        }

        private void UpdateFoldings()
        {
            _foldingStrategy?.UpdateFoldings(_foldingManager, TextEditor.Document);
        }

        private void LoadFileToTextEditor(string filename)
        {
            _filePath = @"";
            TextEditor.Text = File.ReadAllText(filename, Encoding.UTF8);
            _filePath = Path.GetFullPath(filename);
            FileLabel.Content = _filePath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_filePath != @"")
                TextEditor.Save(_filePath);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() != true) return;
            LoadFileToTextEditor(openFileDialog.FileName);
        }

        private void ExecutingButton_Click(object sender, RoutedEventArgs e)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Parse(TextEditor.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"Не удалось распознать задание: {exception.Message}", @"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var taskWindow = new ExecutingWindow
            {
                Owner = this,
                Calculation = new Calculation
                {
                    Commands = CommandClient.GetCommands(doc),
                    Status = @"Выполняется"
                },
                Title = CommandClient.GetHeader(doc),
            };
            taskWindow.DataContext = taskWindow.Calculation;
            taskWindow.Show();
        }

        private void TextEditor_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }

        private void TextEditor_PreviewDrop(object sender, DragEventArgs e)
        {
            var s = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            if (s == null) return;
            foreach (var f in s)
            {
                if (Path.GetExtension(f) != @".xml") continue;
                LoadFileToTextEditor(f);
                return;
            }

        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _filePath = @"";
            FileLabel.Content = _filePath;
            TextEditor.Text = @"<task name=""new task"">

</task>
";
        }

        private void ExampleButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFileToTextEditor(@"default\xmltext.xml");
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog { Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*" };
            if (saveFileDialog.ShowDialog() != true) return;
            TextEditor.Save(saveFileDialog.FileName);
            LoadFileToTextEditor(saveFileDialog.FileName);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string insertion = @"";
            var tag = (string)(sender as MenuItem)?.Tag;
            switch (tag)
            {
                case @"stability":
                    insertion = @"<stability name="">

</stability>";
                    break;
                case @"write":
                    insertion = @"<write name="""" table="""" column="""" selection="""" value="""">

</write>";
                    break;
                case @"graph":
                    insertion = @"<graph name="""">
	<plot name="""">
		<curve name="""" table="""" column="""" selection=""""/>
	</plot>
</graph>";
                    break;
                case @"file":
                    insertion = @"<file path=""""/>";
                    break;
                case @"files":
                    insertion = @"<files>
    <file path=""""/>
</files>";
                    break;
                case @"folder":
                    insertion = @"<folder path="""" var="""">
    <file path=""""/>
</folder>";
                    break;
            }
            TextEditor.Document.Insert(TextEditor.TextArea.Caret.Offset, insertion);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void DeleteExitFiles()
        {
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\exitfiles"))
                return;

            var files = Directory.GetFiles($@"{Environment.CurrentDirectory}\exitfiles");
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
