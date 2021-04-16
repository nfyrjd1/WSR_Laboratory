using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSR_Laboratory_Mobile.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WSR_Laboratory_Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class News : ContentPage
    {
        public News()
        {
            InitializeComponent();
            NewsLV.ItemsSource = DB.GetContext().News.ToList();
        }
    }
}