using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSR_Laboratory_Mobile.Model;
using WSR_Laboratory_Mobile.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WSR_Laboratory_Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Authorization : ContentPage
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            StringBuilder Errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(LoginTB.Text))
            {
                Errors.AppendLine("Вы не ввели логин");
            }

            if (string.IsNullOrWhiteSpace(PasswordTB.Text))
            {
                Errors.AppendLine("Вы не ввели пароль");
            }

            if (Errors.Length > 0)
            {
                await DisplayAlert("Ошибка", Errors.ToString(), "ОК");
                return;
            }

            Model.User user = DB.GetContext().Users.ToList().Where(p => p.Login == LoginTB.Text && p.Password == PasswordTB.Text).LastOrDefault();
            if (user == null)
            {
                await DisplayAlert("Ошибка", "Неверный логин или пароль", "ОК");
                return;
            }

            user.IsCurrentUser = true;
            DB.GetContext().SaveChanges();

            await Manager.MainPage.Navigation.PopToRootAsync();
            await Manager.MainPage.Navigation.PushAsync(new NavigationPage(new User()) { Title = "Профиль" });
        }

        private void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PushAsync(new NavigationPage(new Register()) { Title="Регистрация" });
        }
    }
}