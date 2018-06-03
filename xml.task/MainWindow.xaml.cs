using System;
using ICSharpCode.AvalonEdit.Folding;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using xml.task.Model.Commands;

namespace xml.task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FoldingManager foldingManager;
        XmlFoldingStrategy foldingStrategy;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(0.5);
            foldingUpdateTimer.Tick += delegate { UpdateFoldings(); };
            foldingUpdateTimer.Start();
            textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();

            foldingManager = FoldingManager.Install(textEditor.TextArea);

            foldingStrategy = new XmlFoldingStrategy();
            foldingStrategy.ShowAttributesWhenFolded = true;
            foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);


            textEditor.Load(@"D:\xmltext.xml");
        }

        void UpdateFoldings()
        {
            if (foldingStrategy is XmlFoldingStrategy)
            {
                ((XmlFoldingStrategy)foldingStrategy).UpdateFoldings(foldingManager, textEditor.Document);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            const string fileName = @"D:\xmltext.xml";
            var doc = XDocument.Parse(System.IO.File.ReadAllText(fileName, Encoding.Default));
            foreach (var element in doc.Root.Elements())
            {
                var command = new TestCommand(element);
                command.Perform();
            }
        }
    }
}
