using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Khushoo3.ViewModel
{
   public class AzkarViewModel
    {
      public  Command SelectedZekr { get; set; }
        public AzkarViewModel()
        {
            SelectedZekr = new Command(Zekr);
        }

        private void Zekr(object obj)
        {
           
        }
    }
}
