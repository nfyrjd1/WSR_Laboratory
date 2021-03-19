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
using System.Windows.Shapes;
using WSR_Laboratory.Service;

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            MainFrame.Navigate(new UserPage());

            switch (Manager.CurrentUser.user_type.name) {
                case "Администратор":
                    {
                        HistoryBtn.Visibility = Visibility.Visible;
                        ReportBtn.Visibility = Visibility.Visible;
                        ResourcesBtn.Visibility = Visibility.Visible;
                        break;
                    }
                case "Лаборант": {
                        ReportBtn.Visibility = Visibility.Visible;
                        BloodBtn.Visibility = Visibility.Visible;
                        break;
                    }
                case "Лаборант-исследователь":
                    {
                        AnalyzerBtn.Visibility = Visibility.Visible;
                        break;
                    }
                case "Бухгалтер":
                    {
                        InsuranceBtn.Visibility = Visibility.Visible;
                        ReportBtn.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }

        private void UserBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserPage());
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Manager.Auth.Show();
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HistoryPage());
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
