using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSR_Laboratory_Mobile.Model;
using WSR_Laboratory_Mobile.Service;
using Xamarin.Forms;

namespace WSR_Laboratory_Mobile.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void NewsBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PushAsync(new NavigationPage(new News()) { Title="Новости" });            
        }

        private void ServiceBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PushAsync(new NavigationPage(new Services()) { Title="Услуги" });
        }

        private void AuthBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PushAsync(new NavigationPage(new Authorization()) { Title = "Авторизация" });
        }

        private void UserBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PushAsync(new NavigationPage(new User()) { Title="Профиль" });
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            Model.User user = DB.GetContext().Users.ToList().Where(p => p.IsCurrentUser == true).LastOrDefault();
            if (user == null)
            {
                AuthBtn.IsVisible = true;
                UserBtn.IsVisible = false;
            }
            else
            {
                UserBtn.IsVisible = true;
                AuthBtn.IsVisible = false;
            }
        }
    }
}
