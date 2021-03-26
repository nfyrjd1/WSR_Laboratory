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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            UpdateService();
        }

        private void UpdateService()
        {
            List<service> Services = Laboratory.GetContext().service.ToList();
            if (!string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                string Search = SearchTB.Text.ToLower();
                Services = Services.Where(p =>
                    Levenshtein.Distance(p.name.ToLower(), Search) <= 3 ||
                    p.code.ToString().Contains(Search) ||
                    p.price.ToString().Contains(Search)
                ).ToList();
            }

            ServiceDG.ItemsSource = Services;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateService();
        }
    }
}
