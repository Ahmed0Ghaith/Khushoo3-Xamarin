using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Khushoo3.Helpers;
using Khushoo3.Models;

using Xamarin.Essentials;

namespace Khushoo3.ViewModel
{
    public class HomePageVM : ViewModelBase
    {

       // List<string> ssst;
        private List<string> _ssst;
        public List<string> ssst
        {
            get => _ssst;
            set => SetProperty(ref _ssst, value);
        }

        private Timings _salattime;
        public Timings salattime
        {
            get => _salattime;
            set => SetProperty(ref _salattime, value);
        }
        private Hijri _Hijri;
        public Hijri hijri
        {
            get => _Hijri;
            set => SetProperty(ref _Hijri, value);
        }

        private string _Hday;
        public string Hday
        {
            get => _Hday;
            set => SetProperty(ref _Hday, value);
        }
        private string _Hmonth;
        public string Hmonth
        {
            get => _Hmonth;
            set => SetProperty(ref _Hmonth, value);
        }
        private string _Hdate;
        public string Hdate
        {
            get => _Hdate;
            set => SetProperty(ref _Hdate, value);
        }

        bool isAlreadyLoaded = false;


        public HomePageVM()
        {
            if (!isAlreadyLoaded)
            {

                LoadSalatData();

                isAlreadyLoaded = true;
            }
        }


        public async Task LoadSalatData()
        {
            IsBusy = true;
            DataLoaded = false;
            IsErrorState = false;
            LoadingText = "تحميل مواقيت الصلاة";
            try
            {
                var locator = await Geolocation.GetLocationAsync();

                Timestamp = locator.Timestamp;
                Latitude = locator.Latitude;
                Longitude = locator.Longitude;
                var dates = DateTime.Now.ToString("MMddyyyy");
                //Get salat&time Info


                var info = await RestHelper.GetAsync<salatifo>($"https://api.aladhan.com/v1/timings/{dates}?latitude={Latitude}&longitude={Longitude}&method=5");
                salattime = info.data.timings;
                hijri = info.data.date.hijri;
                DTHV();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Cancel");

            }
            finally
            {
                IsBusy = false;
                DataLoaded = true;
                IsErrorState = true;
            }

        }

        // Date Time Hijri View
        public void DTHV()
            {
            Hday = day;
            Hmonth = hijri.month.ar;
            Hdate = hijri.date.ToString();
            ssst = new List<string>();
            //order salat by the next
            ssst.Add(salattime.Fajr);
            ssst.Add( salattime.Dhuhr);
            ssst.Add( salattime.Asr);
            ssst.Add( salattime.Maghrib);
            ssst.Add( salattime.Isha);



            //DateTime date1 = DateTime.Now.AddHours(-2).AddMinutes(-25);
            //DateTime date2 = DateTime.Now;
            ////TimeSpan interval = date2.Subtract(date1);

            //int hoursDiff = interval.Hours;
            //int minutesDiff = interval.Minutes;
            //double minutesTotal = interval.TotalMinutes;




        }
    }
}