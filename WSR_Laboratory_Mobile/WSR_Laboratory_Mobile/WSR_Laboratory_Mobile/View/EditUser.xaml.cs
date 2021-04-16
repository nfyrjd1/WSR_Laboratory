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
    public partial class EditUser : ContentPage
    {
        Model.User CurrentUser;
        public EditUser(Model.User CurrentUser)
        {
            InitializeComponent();
            this.CurrentUser = CurrentUser;
            BindingContext = this.CurrentUser;
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PopAsync();
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            StringBuilder Errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(CurrentUser.Phone))
            {
                Errors.AppendLine("Вы не ввели номер телефона");
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Email))
            {
                Errors.AppendLine("Вы не ввели email");
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

            DB.GetContext().SaveChanges();
            await Manager.MainPage.Navigation.PopAsync();
        }
    }
}