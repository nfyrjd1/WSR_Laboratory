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

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
            UpdateHistory();
        }

        private void UpdateHistory()
        {
            List<history> history = Laboratory.GetContext().history.OrderByDescending(p => p.date_time).ToList();
            if (!string.IsNullOrWhiteSpace(LoginTB.Text))
            {
                history = history.Where(p => p.login.ToLower().Contains(LoginTB.Text.ToLower())).ToList();
            }
            HistoryDT.ItemsSource = history;
        }

        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHistory();
        }
    }
}
