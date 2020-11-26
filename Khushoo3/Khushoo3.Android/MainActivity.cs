using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using PanCardView.Droid;
using Environment = System.Environment;
using System.IO;
using Xamarin.Forms;

namespace Khushoo3.Droid
{
    [Activity(Label = "Khushoo3", Theme = "@style/MainTheme",   ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            DependencyService.Register<Toast>();
      
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CardsViewRenderer.Preserve();
            string DataBaseName = "Timings_DB.sqlite";
            string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string FullPath = Path.Combine(FolderPath, DataBaseName);
            LoadApplication(new App(FullPath));
        
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}