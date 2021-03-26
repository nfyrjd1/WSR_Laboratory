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
    /// Логика взаимодействия для BloodPage.xaml
    /// </summary>
    public partial class BloodPage : Page
    {
        public BloodPage()
        {
            InitializeComponent();
            BloodDG.ItemsSource = Laboratory.GetContext().blood.ToList();
        }

        private void BloodDGRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGridRow) != null)
            {
                AddEditBlood blood = new AddEditBlood((sender as DataGridRow).DataContext as blood);
                Manager.MainFrame.Navigate(blood);
            }            
        }

        private void AddBloodBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditBlood blood = new AddEditBlood();
            Manager.MainFrame.Navigate(blood);
        }
    }
}
