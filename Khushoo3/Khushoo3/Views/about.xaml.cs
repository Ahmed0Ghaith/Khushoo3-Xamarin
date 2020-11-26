using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Khushoo3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class about : ContentPage
    {
        public about()
        {
            InitializeComponent();
        }
        private void TapGestureRecognizerf(object sender, EventArgs e)
        {
            Launcher.TryOpenAsync("fb://profile/100005924471612");

        }
        private void TapGestureRecognizerw(object sender, EventArgs e)
        {
            Launcher.TryOpenAsync("whatsapp://send?phone=+2001159392938");

        }
        private void TapGestureRecognizert(object sender, EventArgs e)
        {
             Device.OpenUri(new Uri("https://twitter.com/AhmedGhaithTwit"));


        }
        private  void TapGestureRecognizery(object sender, EventArgs e)
        {
             Device.OpenUri(new Uri("https://www.youtube.com/channel/UCjOOQS143M0qxzZYQkBNHMg"));

        }


    }
}