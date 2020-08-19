using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Input;
using Khushoo3.Helpers;
using Khushoo3.Models;
using PanCardView.EventArgs;
using PanCardView.Extensions;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Khushoo3.ViewModel
{
    public class HomePageVM : ViewModelBase
    {
        private ObservableCollection<TodayST> _SalatTime;
        public ObservableCollection<TodayST> SalatTime
        {
            get => _SalatTime;
            set => SetProperty(ref _SalatTime, value);
        }
        // List<string> ssst;
        //private ObservableCollection<DBST> _SalatTime;
        //public ObservableCollection<DBST> SalatTime
        //{
        //    get => _SalatTime;
        //    set => SetProperty(ref _SalatTime, value);
        //}

        private List<Datum> _salattime;
        public List<Datum> salattime
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

        SQLiteConnection Con;
     //   public ICommand LoadData { get; set; }

         public  HomePageVM()
        {
            
                SalatTime = new ObservableCollection<TodayST>();
            Con = new SQLiteConnection(App.DataBaseLocation);
           
          
                _ = LoadSalatData();

               
            
        }
        List<string> imgs;
        int ImageNO = 0;
        public async Task LoadSalatData()
        {
            int ImageNO = 0;
            IsBusy = true;
            DataLoaded = false;
            IsErrorState = false;
            LoadingText = "تحميل مواقيت الصلاة";
            try
            {

               
            Con.CreateTable<DBST>();
            var Table = Con.Table<DBST>().ToList();
                if (Table.Count == 0)
                { RequstData(); }



                var fd = DateTime.Parse(Table[0].Date).ToString("dd");

                    if (DateTime.Parse(Table[0].Date).ToString("dd") == DateTime.Now.ToString("MM"))
                    {
                     imgs = new List<string>();
                    imgs.Add("FR");
                    imgs.Add("SR");
                    imgs.Add("ZH");
                    imgs.Add("AS");
                    imgs.Add("MA");
                    imgs.Add("IS");
                    foreach (var t in Table)
                        {


                            RequstDB(t);
                            if (SalatTime.Count() == 6)
                            { break; }

                        }



                    }
                    else
                    {
                        LoadingText = "تحديث مواقيت الصلاة";
                        RequstData();
                        //   RequstDB(t);
                    }
                

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
       
        //requst from DB
        public async void RequstDB(DBST Item)
        {
           

            


            if (Item.Date == DateTime.Now.ToString("dd-MM-yyyy"))
                        {
                            if (string.IsNullOrEmpty(Hdate))
                            {
                                Hdate = Item.HDate;
                                Hmonth = Item.Month;
                                Hday = day;

                            }
                            TodayST TS = new TodayST()

                            {
                                Salat = Item.Salat,
                                Time = Item.Time,
                                IMGURI = imgs[ImageNO]
                            };

                            SalatTime.Add(TS);
                            ImageNO++;

                        }
                      
        }

       // RequstData And Store it 
        private async void RequstData()
        {
            var locator = await Geolocation.GetLocationAsync();

            Timestamp = locator.Timestamp;
            Latitude = locator.Latitude;
            Longitude = locator.Longitude;
            var dates = DateTime.Now.ToString("MMddyyyy");



            var info = await RestHelper.GetAsync<Root>($"https://api.aladhan.com/v1/calendar?latitude={Latitude}&longitude={Longitude}&method=5&month=8&year=2020");

            salattime = info.data;

            foreach (var Item in salattime)
            {

                char[] separator = { '(', ')', ' ' };
                string Fajr = DateTime.Parse(Item.timings.Fajr.Split(separator)[0]).ToString("hh:mm");
                string Sunrise = DateTime.Parse(Item.timings.Sunrise.Split(separator)[0]).ToString("hh:mm");
                string Dhuhr = DateTime.Parse(Item.timings.Dhuhr.Split(separator)[0]).ToString("hh:mm");
                string Asr = DateTime.Parse(Item.timings.Asr.Split(separator)[0]).ToString("hh:mm");
                string Maghrib = DateTime.Parse(Item.timings.Maghrib.Split(separator)[0]).ToString("hh:mm");
                string Isha = DateTime.Parse(Item.timings.Isha.Split(separator)[0]).ToString("hh:mm");

                List<DBST> TS = new List<DBST>()

                {
                    new DBST{Salat ="صلاة الفجر" , Time= Fajr ,Date = Item.date.gregorian.date ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="شروق الشمس" , Time= Sunrise,Date = Item.date.gregorian.date ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة الظهر " , Time= Dhuhr,Date = Item.date.gregorian.date ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة العصر" , Time= Asr,Date = Item.date.gregorian.date ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة المغرب" , Time= Maghrib,Date = Item.date.gregorian.date ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة العشاء" , Time= Isha,Date = Item.date.gregorian.date ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                };

                foreach (var item in TS)
                {
                    Con.Insert(item);


                }
            }
            isAlreadyLoaded = true;
            LoadSalatData();


        }
    }
}
