using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        Timer WorkTimer;
        DateTime WorkTime;
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
                        PatientsBtn.Visibility = Visibility.Visible;
                        ServiceBtn.Visibility = Visibility.Visible;
                        SetWorkTimer();
                        break;
                    }
                case "Лаборант-исследователь":
                    {
                        AnalyzerBtn.Visibility = Visibility.Visible;
                        SetWorkTimer();
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

        private void SetWorkTimer()
        {
            WorkTime = new DateTime();
            Time.Text = WorkTime.ToShortTimeString();

            WorkTimer = new Timer();
            WorkTimer.Interval = 60000;
            WorkTimer.Start();
            WorkTimer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            WorkTime = WorkTime.AddMinutes(1);
            Dispatcher.Invoke(() => Time.Text = WorkTime.ToShortTimeString());
            //2:15
            if (WorkTime.ToShortTimeString() == "0:05")
            {
                Dispatcher.Invoke(() => MessageBox.Show("Ваш сеанс работы закончится через 15 минут", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning));
            }

            //2:30
            if (WorkTime.ToShortTimeString() == "0:10")
            {
                WorkTimer.Stop();
                Dispatcher.Invoke(() => {
                    MessageBox.Show("Кварцевание помещения, работа приложения заблокирована на 30 минут", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Manager.Auth.Show();
                    Hide();
                    Manager.Auth.BlockAuth(60);
                    Close();
                });
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

        private void BloodBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BloodPage());
        }

        private void PatientsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PatientPage());
        }

        private void ServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ServicePage());
        }
    }
}
