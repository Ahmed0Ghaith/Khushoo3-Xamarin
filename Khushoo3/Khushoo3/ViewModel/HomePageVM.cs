using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private  bool _PopUPZekrPage = false;
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
            set {
                _SearchZekr = value;
                Search();
            }
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
  
    
         public  HomePageVM()
        {

            
                SalatTime = new ObservableCollection<TodayST>();
            Azkar = new ObservableCollection<Azkar>();

            AzkarList = new ObservableCollection<IGrouping<string, Azkar>>();
                Zekr = new Command<IGrouping<string, Azkar>>(Selected);
                Con = new SQLiteConnection(App.DataBaseLocation);
                ClosePoP = new Command(Close);
                ZekrCountr = new Command<PanCardView.EventArgs.ItemAppearedEventArgs>(ZekrTimes);
               
                CounterTapped = new Command(CTapped);
            ZekrList = new zekrinfo();
                ZekrFilter();
                _ = LoadSalatData();
          

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
              
            } else if (count == 0)

            {
                try
                {

                    var duration = TimeSpan.FromSeconds(0.2);
                    Vibration.Vibrate(duration);


                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
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

            

            Azkar.Clear();
            PopUPZekrPage = true;
            foreach (var i in obj)
            {
                Azkar.Add(i);
            }
          
          

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



                var fd = DateTime.Parse(Table[0].Date).ToString("MMM");

                    if (DateTime.Parse(Table[0].Date).ToString("MMM") == DateTime.Now.ToString("MMM"))
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



            string t = Item.Date;
            string yn = DateTime.Now.ToString("dd MMM yyyy");
            if (Item.Date == DateTime.Now.ToString("dd MMM yyyy"))
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
            //    var dates = DateTime.Now.ToString("MMddyyyy");
            Con.DeleteAll<DBST>();
            string Month=   DateTime.Now.ToString("MM");
              
            var info = await RestHelper.GetAsync<Root>($"https://api.aladhan.com/v1/calendar?latitude={Latitude}&longitude={Longitude}&method=5&month={Month}&year=2020");

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
          
            LoadSalatData();


        }
    }
}
