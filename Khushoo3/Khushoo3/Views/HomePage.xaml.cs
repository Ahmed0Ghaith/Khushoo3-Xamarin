using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Khushoo3.Models;
using Khushoo3.ViewModel;
using Newtonsoft.Json;

using Xamarin.Essentials;
using Xamarin.Forms;


namespace Khushoo3.Views
{
    public partial class HomePage : ContentPage
    {
        HomePageVM VM;

        double current_latitude = 0;
        double current_longitude = 0;
        double QiblaLatitude = 21.4224779;
        double QiblaLongitude = 39.8251832;
        double bearing_degree = 0;
        SensorSpeed speed = SensorSpeed.UI;
        public HomePage()
        {

            InitializeComponent();
          this.BindingContext=  VM  = new HomePageVM();
            Compass.ReadingChanged += Compass_ReadingChanged;
            

        }
      
        protected override bool OnBackButtonPressed()
        {


            if (VM.PopUPZekrPage)
            {
                VM.PopUPZekrPage = false;

                return true;
            }
            else
            { 
              return  base.OnBackButtonPressed();

            }



        }
      

      

        private void SearchIconTapped(object sender, EventArgs e)
        {
            
            OpenMenu();
        }
        private void MenuTapped(object sender, EventArgs e)
        {
            OpenMenu();
           

        }
        private void OverlayTapped(object sender, EventArgs e)
        {
            CloseMenu();
        }

        private void OpenMenu()
        {
            search.IsVisible = true;

            Action<double> callback = input => SearchZekr.TranslationX = input;
            SearchZekr.Animate("anim", callback, 280, 0, 16, 300, Easing.CubicInOut);
        }

        private void CloseMenu()
        {
          

            search.IsVisible = false;
        }

        


        public void ChangeColor(object obj)
        {
            Zekr.TextColor = Color.FromHex("#FFD500");
            Qibla.TextColor = Color.FromHex("#FFD500");
            Counter.TextColor = Color.FromHex("#FFD500");

            var lable = obj as Label;
            lable.TextColor = Color.FloralWhite;


        }

        private void ZekrTabbed(object sender, EventArgs e)
        {
            ChangeColor(sender);
            SearchIcon.IsVisible = true;

            AzkarView.IsVisible = true;
            QiblaView.IsVisible = false;
            CounterView.IsVisible = false;
            Action<double> callback = input => AzkarView.TranslationX = input;
            AzkarView.Animate("anim", callback, 300, 0, 16, 300, Easing.CubicInOut);
        }

        private void CounterTapped(object sender, EventArgs e)
        {
            ChangeColor(sender);
            SearchIcon.IsVisible = false;
            CounterView.IsVisible = true;
            AzkarView.IsVisible = false;
            QiblaView.IsVisible = false;
            Action<double> callback = input => CounterView.TranslationX = input;
            CounterView.Animate("anim", callback, 300, 0, 16, 300, Easing.CubicInOut);
        }




        private void QiblaTabbed(object sender, EventArgs e)
        {
            ChangeColor(sender);
            QiblaIsAppear();
            SearchIcon.IsVisible = false;

            QiblaView.IsVisible = true;
           
            AzkarView.IsVisible = false;
            CounterView.IsVisible = false;
            Action<double> callback = input => QiblaView.TranslationX = input;
            QiblaView.Animate("anim", callback, 300, 0, 16, 300, Easing.CubicInOut);
        }
        private void QiblaIsAppear()
        {
           


            if (!Compass.IsMonitoring) Compass.Start(speed);
            GetLocation();
            PointToQibla();
            if (!Compass.IsMonitoring) Compass.Start(speed);

        }
        bool isVibrate = false;
        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            if (QiblaView.IsVisible)
            {
                int Value = Convert.ToInt32(e.Reading.HeadingMagneticNorth);

                circularGauge.RotateTo(360 - e.Reading.HeadingMagneticNorth);
                PointToQibla();

                if (Value > bearing_degree - 2 && Value < bearing_degree + 2)
                {

                    pointer1.KnobColor = Color.Gold;
                    scale.RimColor = Color.Gold;
                    if (!isVibrate)
                    {
                        try
                        {

                            isVibrate = true;
                            var duration = TimeSpan.FromSeconds(0.5);
                            Vibration.Vibrate(duration);


                        }
                        catch 
                        {

                        }
                    }
                }
                else
                {
                    isVibrate = false;
                    pointer1.KnobColor = Color.White;
                    circularGauge.BackgroundColor = Color.Transparent;
                    scale.RimColor = Color.LightGray;

                }
            }
        }


        private async void GetLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
            current_latitude = location.Latitude;
            current_longitude = location.Longitude;
        }

        private double Mod(double a, double b)
        {
            return a - b * Math.Floor(a / b);
        }

        void PointToQibla()
        {
            double latt_from_radians = current_latitude * Math.PI / 180;
            double long_from_radians = current_longitude * Math.PI / 180;
            double latt_to_radians = QiblaLatitude * Math.PI / 180;
            double lang_to_radians = QiblaLongitude * Math.PI / 180;
            double bearing = Math.Atan2(Math.Sin(lang_to_radians - long_from_radians) * Math.Cos(latt_to_radians), (Math.Cos(latt_from_radians) * Math.Sin(latt_to_radians)) - (Math.Sin(latt_from_radians) * Math.Cos(latt_to_radians) * Math.Cos(lang_to_radians - long_from_radians)));
            bearing = Mod(bearing, 2 * Math.PI);
            bearing_degree = bearing * 180 / Math.PI;
            pointer1.Value = bearing_degree;
        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new about());
        }
    }
}
