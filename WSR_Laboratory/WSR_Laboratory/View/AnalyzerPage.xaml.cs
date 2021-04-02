using System;
using System.Collections.Generic;
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
using WSR_Laboratory.Model;
using WSR_Laboratory.Service;

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для AnalyzerPage.xaml
    /// </summary>
    public partial class AnalyzerPage : Page
    {
        public AnalyzerPage()
        {
            InitializeComponent();
            AnalyzerDG.ItemsSource = Laboratory.GetContext().analyzer.ToList();
            AnalyzerManager.StartTimer();
        }

        private void AnalyzerDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            analyzer Analyzer = (sender as DataGridRow).DataContext as analyzer;
            if (Analyzer != null)
            {
                AnalyzerBloodServicePage analyzerBloodServicePage = new AnalyzerBloodServicePage(Analyzer);
                Manager.MainFrame.Navigate(analyzerBloodServicePage);
            }
        }
    }
}
