using Khushoo3.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Khushoo3.Helpers;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Windows.Input;

namespace Khushoo3.ViewModel
{
    public class AzkarVM : HomePageVM
    {
        public ObservableCollection<IGrouping<string, Azkar>> AzkarList { get; set; }
        public ObservableCollection<Azkar> Azkar { get; set; }



        public Command<IGrouping<string, Azkar>> Zekr { get; set; }

        public Command ClosePoP { get; set; }
       
     
        public AzkarVM()
        {
          
            AzkarList = new ObservableCollection<IGrouping<string, Azkar>>();
             Zekr = new Command<IGrouping<string, Azkar>>(Selected);
            ClosePoP = new Command(Close);
          
            ZekrFilter();
            Selected(AzkarList[0]);
        }

        private void Close(object obj)
        {
          PopUPZekrPage=false;
        }

        private void Selected(IGrouping<string, Azkar> obj)
        {

            Azkar = new ObservableCollection<Azkar>();


            foreach (var i in obj)
            {
                Azkar.Add(i);
            }
            PopUPZekrPage = true;
        }

        

     

        void ZekrFilter()
        {
       

               zekrinfo ZekrList = GetAzkar.GetJsonData();
            var list = ZekrList.Azkar.ToList();
            IEnumerable<IGrouping<string,Azkar>> Category = from p in list
                           group p by p.category
                           into G
                         
                           select G;
          
            foreach (var item in Category)
            {
                //  CatList.Add(item.Key);
                AzkarList.Add(item);
            }
        

        }


    }
}
