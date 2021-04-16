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
    public partial class Register : ContentPage
    {
        Model.User CurrentUser;
        public Register()
        {
            InitializeComponent();
            CurrentUser = new Model.User();
            CurrentUser.Birthday = DateTime.Now;
            BindingContext = CurrentUser;
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PopAsync();
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            StringBuilder Errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(CurrentUser.Name))
            {
                Errors.AppendLine("Вы не ввели ФИО");
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Passport))
            {
                Errors.AppendLine("Вы не ввели паспортные данные");
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Insurance))
            {
                Errors.AppendLine("Вы не ввели номер страхового полиса");
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Phone))
            {
                Errors.AppendLine("Вы не ввели номер телефона");
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Email))
            {
                Errors.AppendLine("Вы не ввели Email");
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Login))
            {
                Errors.AppendLine("Вы не ввели логин");
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Password))
            {
                Errors.AppendLine("Вы не ввели пароль");
            }

            if (Errors.Length > 0)
            {
                await DisplayAlert("Ошибка", Errors.ToString(), "ОК");
                return;
            }

            CurrentUser.IsCurrentUser = true;

            DB.GetContext().Users.Add(CurrentUser);
            DB.GetContext().SaveChanges();

            await Manager.MainPage.Navigation.PopToRootAsync();
            await Manager.MainPage.Navigation.PushAsync(new NavigationPage(new User()) { Title="Профиль" });
        }
    }
}