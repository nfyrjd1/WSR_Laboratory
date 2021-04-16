using System;
using WSR_Laboratory_Mobile.Service;
using WSR_Laboratory_Mobile.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WSR_Laboratory_Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            Manager.MainPage = MainPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
