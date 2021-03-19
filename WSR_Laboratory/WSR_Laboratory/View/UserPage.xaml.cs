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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            if (Manager.CurrentUser.user_type.name == "Пациент")
            {
                FullName.Text = Manager.CurrentUser.patient.FirstOrDefault().full_name;
                Role.Text = Manager.CurrentUser.user_type.name;
            }
            else if (Manager.CurrentUser.user_type.name == "Администратор")
            {
                FullName.Visibility = Visibility.Collapsed;
                Role.Text = Manager.CurrentUser.user_type.name;
            }
            else
            {
                FullName.Text = Manager.CurrentUser.employee.FirstOrDefault().full_name;
                Role.Text = Manager.CurrentUser.user_type.name;
            }

            string image = "";
            switch (Manager.CurrentUser.user_type.name)
            {
                case "Лаборант":
                    {
                        image = @"/Resources/laborant_1.jpeg";
                        break;
                    }
                case "Лаборант-исследователь":
                    {
                        image = @"/Resources/laborant_2.png";
                        break;
                    }
                case "Бухгалтер":
                    {
                        image = @"/Resources/Бухгалтер.jpeg";
                        break;
                    }
                case "Администратор":
                    {
                        image = @"/Resources/Администратор.png";
                        break;
                    }
                default:
                    {
                        UserImage.Visibility = Visibility.Collapsed;
                        break;
                    }
            }

            if (image.Length > 0)
            {
                UserImage.Source = new BitmapImage(new Uri(image, UriKind.Relative));
            }            
        }
    }
}
