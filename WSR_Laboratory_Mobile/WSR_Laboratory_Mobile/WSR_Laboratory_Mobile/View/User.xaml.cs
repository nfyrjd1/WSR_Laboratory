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
    public partial class User : ContentPage
    {
        Model.User CurrentUser;
        public User()
        {
            InitializeComponent();
        }

        private async void OnLoad()
        {
            BindingContext = null;
            Model.User user = DB.GetContext().Users.ToList().Where(p => p.IsCurrentUser == true).LastOrDefault();
            if (user == null)
            {
                await Navigation.PopAsync();
                return;
            }

            CurrentUser = user;
            BindingContext = user;
        }

        private void UserBtn_Clicked(object sender, EventArgs e)
        {
            Manager.MainPage.Navigation.PushAsync(new NavigationPage(new EditUser(CurrentUser)) { Title="Редактирование" });
        }

        private void ExitBtn_Clicked(object sender, EventArgs e)
        {
            CurrentUser.IsCurrentUser = false;
            DB.GetContext().SaveChanges();
            Manager.MainPage.Navigation.PopToRootAsync();
        }

        private void User_Appearing(object sender, EventArgs e)
        {
            OnLoad();
        }
    }
}