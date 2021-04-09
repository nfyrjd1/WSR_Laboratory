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
using WSR_Laboratory.Service;

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
        }

        private void QualityBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new QualityReportPage());
        }

        private void ServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ServiceReportPage());
        }
    }
}
