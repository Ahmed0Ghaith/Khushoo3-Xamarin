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
    public partial class QiblaView : ContentView
    {
        double current_latitude = 0;
        double current_longitude = 0;
        double QiblaLatitude = 21.4224779;
        double QiblaLongitude = 39.8251832;
        double bearing_degree = 0;
        SensorSpeed speed = SensorSpeed.UI;
        public QiblaView()
        {
            InitializeComponent();
            GetLocation();
           
            Compass.ReadingChanged += Compass_ReadingChanged;
            PointToQibla();
        

            if (!Compass.IsMonitoring) Compass.Start(speed);
        }




        bool isVibrate = false;
        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
          
            int Value = Convert.ToInt32(e.Reading.HeadingMagneticNorth);

            circularGauge.RotateTo(360 - e.Reading.HeadingMagneticNorth);
            PointToQibla();

            if (Value > bearing_degree-2 && Value < bearing_degree+2)
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
                    catch (Exception ex)
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




    }
}
