using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.XPath;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ViewerXMLDocuments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSelectFile(object sender, RoutedEventArgs e)
        {
            ListBoxResults.Items.Clear();

            var openFileDialog = new OpenFileDialog();

            //openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = $"C:\\";

            if (openFileDialog.ShowDialog() != true) return;

            TextBoxPathFile.Text = openFileDialog.FileName;
            TextBoxPreViewFile.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_SearchInfo(object sender, RoutedEventArgs e)
        {
            TextBoxXPath.Background = Brushes.White;

            if (TextBoxPathFile.Text == "Файл не выбран")
            {
                ListBoxResults.Items.Add($"Не выбран файл для загрузки.");
                return;
            };

            if (String.IsNullOrEmpty(TextBoxXPath.Text))
            {
                ListBoxResults.Items.Clear();
                //MessageBox.Show($"Необходимо ввести строку поиска.", $"{this.Title}", MessageBoxButton.OK, MessageBoxImage.Information);
                ListBoxResults.Items.Add($"Необходимо ввести строку поиска.");
                TextBoxXPath.Background = Brushes.Chartreuse;
                return;
            }
            
            var docXml = new XmlDocument();
            docXml.PreserveWhitespace = false;

            try
            {
                docXml.Load(TextBoxPathFile.Text);
            }

            catch (Exception ex)
            {

                //MessageBox.Show($"Возникли ошибки при загрузке файла.", $"{this.Title}", MessageBoxButton.OK, MessageBoxImage.Error);
                ListBoxResults.Items.Add($"Возникли ошибки при загрузке файла.");
            }

            try
            {
                ListBoxResults.Items.Clear();

                XPathNavigator navigator = docXml.CreateNavigator();

                XPathNodeIterator valueNode = null;

                valueNode = navigator.Select(TextBoxXPath.Text);

                if (valueNode.Count == 0)
                {
                    ListBoxResults.Items.Add($"Ничего не найдено.");
                }

                while (valueNode.MoveNext())
                {
                    ListBoxResults.Items.Add($"{valueNode.Current.Value}");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Возникли ошибки при поиске информации в документе.", $"{this.Title}", MessageBoxButton.OK, MessageBoxImage.Error);
                ListBoxResults.Items.Add($"Возникли ошибки при поиске информации в файле.");
            }
        }
    }
}
