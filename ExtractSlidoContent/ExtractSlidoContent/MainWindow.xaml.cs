using HtmlAgilityPack;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace ExtractSlidoContent
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

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "HTML Files (*.html)|*.html";

            if (openFileDialog.ShowDialog() == true)
            {
                fileContent.Text = openFileDialog.FileName;
            }
        }

        private void Questions_Click(object sender, RoutedEventArgs e)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load(fileContent.Text);

            var results = htmlDoc.DocumentNode
                .Descendants("span")
                .Where(n => n.GetClasses().Count() == 1
                         && n.HasClass("ng-star-inserted"))
                .Select(x => x.InnerText).ToArray();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TXT Files (*.txt)|*.txt";

            questionsCount.Content = $"{results.Length} foram encontradas!";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false))
                {
                    for (int index = 0; index < results.Length; index++)
                        sw.WriteLine($"{index + 1} - {results[index]}");

                    sw.Flush();
                }
            }

            Process.Start("notepad.exe", saveFileDialog.FileName);
        }
    }
}
