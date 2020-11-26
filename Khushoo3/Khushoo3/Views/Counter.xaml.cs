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
    public partial class Counter : ContentView
    {
        int Num = 0;
        int Numtens = 0;
        int TotalNum = 0;

        public Counter()
        {
            InitializeComponent();
        }

        private void CounterButton_Clicked(object sender, EventArgs e)
        {
            try
            {

               
              
               
                TotalNum++;
                Num++;

                CounterButton.Text = Num.ToString();
                if (Num == 33)
                {
                    Num = 0;
                    Numtens++;
                    var Sduration = TimeSpan.FromSeconds(0.5);
                    Vibration.Vibrate(Sduration);

                }

                tens.Text = Numtens.ToString();
                Total.Text = TotalNum.ToString();

            }
            catch 
            {

            }
           
        }
    }
}