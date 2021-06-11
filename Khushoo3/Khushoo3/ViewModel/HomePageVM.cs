using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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
        zekrinfo ZekrList;
        public ObservableCollection<IGrouping<string, Azkar>> AzkarList { get; set; }


        public ObservableCollection<TodayST> SalatTime { get; set; }
        public ObservableCollection<Azkar> Azkar { get; set; }

        public Command<IGrouping<string, Azkar>> Zekr { get; set; }

        public Command<PanCardView.EventArgs.ItemAppearedEventArgs> ZekrCountr { get; set; }


        public Command CounterTapped { get; set; }



        public Command ClosePoP { get; set; }

        private bool _isBusy = true;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _isrefresh = false;
        public bool isrefresh
        {
            get => _isrefresh;
            set => SetProperty(ref _isrefresh, value);
        }



        private string _loadingText = string.Empty;
        public string LoadingText
        {
            get => _loadingText;
            set => SetProperty(ref _loadingText, value);
        }

        private bool _dataLoaded = false;
        public bool DataLoaded
        {
            get => _dataLoaded;
            set => SetProperty(ref _dataLoaded, value);
        }

        private bool _PopUPZekrPage = false;
        public bool PopUPZekrPage
        {
            get => _PopUPZekrPage;
            set => SetProperty(ref _PopUPZekrPage, value);
        }



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

        private string _SearchZekr;

        public string SearchZekr
        {
            get { return _SearchZekr; }
            set
            {
                _SearchZekr = value;
                Search();
            }
        }



        private bool _Isupdated = true;
        public bool Isupdated
        {
            get => _Isupdated;
            set => SetProperty(ref _Isupdated, value);
        }

        private string _Hday;
        public string Hday
        {
            get => _Hday;
            set => SetProperty(ref _Hday, value);
        }
        private string _Counter;
        public string Counter
        {
            get => _Counter;
            set => SetProperty(ref _Counter, value);
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

        readonly SQLiteConnection Con;


        public HomePageVM()
        {


            SalatTime = new ObservableCollection<TodayST>();
            Azkar = new ObservableCollection<Azkar>();
            AzkarList = new ObservableCollection<IGrouping<string, Azkar>>();
            Zekr = new Command<IGrouping<string, Azkar>>(Selected);
            Con = new SQLiteConnection(App.DataBaseLocation);
            ClosePoP = new Command(Close);

            ZekrCountr = new Command<ItemAppearedEventArgs>(ZekrTimes);

            CounterTapped = new Command(CTapped);
            ZekrList = new zekrinfo();

            LoadSalatData();


        }



        private async void CTapped(object obj)
        {

            var Button = obj as Button;


            int count = int.Parse(Button.Text);
            if (count > 0)
            {
                Button.BorderColor = Color.FromHex("#FFD500");
                Button.BorderWidth = 2;
                await Task.Delay(100);
                Counter = (count - 1).ToString();

            }
            else if (count == 0)

            {
                try
                {

                    var duration = TimeSpan.FromSeconds(0.2);
                    Vibration.Vibrate(duration);


                }

                catch
                {

                }
            }
            Button.BorderColor = Color.FromHex("#ffed94");


        }

        private void ZekrTimes(PanCardView.EventArgs.ItemAppearedEventArgs obj)
        {
            string Count = "1";
            var Item = obj.Item as Azkar;
            if (!string.IsNullOrEmpty(Item.count))
            {
                Count = Item.count;

            }
            Counter = Count;

        }

        private void Search()
        {
            AzkarList.Clear();
            IEnumerable<IGrouping<string, Azkar>> Category;
            if (!string.IsNullOrEmpty(SearchZekr))
            {


                var list = ZekrList.Azkar.ToList();
                Category = from p in list
                           group p by p.category
                                                                     into G
                           where G.Key.Contains(SearchZekr)
                           select G;
            }
            else
            {
                var list = ZekrList.Azkar.ToList();
                Category = from p in list
                           group p by p.category
                                                                 into G

                           select G;



            }

            foreach (var item in Category)
            {

                AzkarList.Add(item);
            }


        }
        void ZekrFilter()
        {



            ZekrList = GetAzkar.GetJsonData();
            var list = ZekrList.Azkar.ToList();
            IEnumerable<IGrouping<string, Azkar>> Category = from p in list
                                                             group p by p.category
                                                             into G

                                                             select G;

            foreach (var item in Category)
            {

                AzkarList.Add(item);
            }


        }

        private void Close(object obj)
        {

            PopUPZekrPage = false;

        }
        private async void Selected(IGrouping<string, Azkar> obj)
        {
            LoadingText = "تحميل الذكر";
            IsBusy = true;

            //  await Task.Delay(100);


            PopUPZekrPage = true;


            Azkar.Clear();

            foreach (var i in obj)
            {
                Azkar.Add(i);
            }

            IsBusy = false;




        }




        List<string> imgs;
        int ImageNO = 0;
        public async void LoadSalatData()
        {
            DataLoaded = false;
            IsBusy = true;
            LoadingText = "تحميل مواقيت الصلاة";

            try
            {
                var current = Connectivity.NetworkAccess;



                Con.CreateTable<DBST>();
                var Table = Con.Table<DBST>().ToList();


                if (Table.Count == 0)
                {

                    await RequstData();
                }

                else
                {
                    if (DateTime.Parse(Table[0].Date).ToString("MM") != DateTime.Now.ToString("MM"))
                    {
                        await RequstData();
                    }
                }

                Con.CreateTable<DBST>();
                var table = Con.Table<DBST>().ToList();




                imgs = new List<string>();
                imgs.Add("FR");
                imgs.Add("SR");
                imgs.Add("ZH");
                imgs.Add("AS");
                imgs.Add("MA");
                imgs.Add("IS");
                foreach (var t in table)
                {


                    RequstDB(t);
                    if (SalatTime.Count() == 6)
                    { break; }

                }





                ZekrFilter();


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Cancel");

            }
            finally
            {
                DataLoaded = true;
                IsBusy = false;

            }

        }

        //requst from DB
        public void RequstDB(DBST Item)
        {


            string ItemTime = Convert.ToDateTime(Item.Date).ToString("dd MM yyyy");

            if (ItemTime == DateTime.Now.ToString("dd MM yyyy"))
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
        private async Task RequstData()
        {
            try
            {

                LoadingText = "تحديث مواقيت الصلاة";
                var locator = await Geolocation.GetLocationAsync();
                if (locator == null)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "قم بالسماح للتطبيق بالوصول للموقع لتعيين القبله و مواقيت الصلاه", "غلق");

                    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                    if (status != PermissionStatus.Granted || status == PermissionStatus.Denied)
                    {
                        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                        await App.Current.MainPage.DisplayAlert("تنبيه", "قم بالسماح للتطبيق بالوصول للموقع لتعيين القبله و مواقيت الصلاه", "غلق");

                    }
                }
                Timestamp = locator.Timestamp;
                Latitude = locator.Latitude;
                Longitude = locator.Longitude;


                string Month = DateTime.Now.ToString("MM");

                var info = await RestHelper.GetAsync<Root>($"https://api.aladhan.com/v1/calendar?latitude={Latitude}&longitude={Longitude}&method=5&month={Month}&year={DateTime.Now.Year}&adjustment=1");
                Con.DeleteAll<DBST>();
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
                    new DBST{Salat ="صلاة الفجر" , Time= Fajr ,Date =Item.date.readable ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="شروق الشمس" , Time= Sunrise,Date = Item.date.readable ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة الظهر " , Time= Dhuhr,Date =Item.date.readable ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة العصر" , Time= Asr,Date = Item.date.readable ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة المغرب" , Time= Maghrib,Date = Item.date.readable ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                    new DBST{Salat ="صلاة العشاء" , Time= Isha,Date =Item.date.readable ,Day=Item.date.hijri.day ,Month = Item.date.hijri.month.ar,HDate=Item.date.hijri.date},
                };

                    foreach (var item in TS)
                    {
                        Con.Insert(item);


                    }
                }


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("خطأ", ex.Message, "غلق");

            }



        }
    }
}
