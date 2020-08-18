using System;
using Khushoo3.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont ("ArbFONTS-Droid-Arabic-Kufi.ttf", Alias ="KufiFont" ) ]
[assembly: ExportFont("ArbFONTS-Droid-Arabic-Kufi-Bold.ttf", Alias = "KufiFontBold")]
[assembly: ExportFont("ArbFONTS-GaliModern-Regular.ttf", Alias = "GaliModernFont")]
[assembly: ExportFont("Tajawal-Bold.ttf", Alias = "TajawalFontBold")]
[assembly: ExportFont("Tajawal-Regular.ttf", Alias = "TajawalFont")]


namespace Khushoo3
{
    public partial class App : Application   
    {
        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
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
