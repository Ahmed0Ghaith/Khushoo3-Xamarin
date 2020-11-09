using Khushoo3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Khushoo3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AzkarPoPUP : ContentView
    {
        public AzkarPoPUP()
        {
            InitializeComponent();
          
        }

        private void CarouselView_ItemAppeared(PanCardView.CardsView view, PanCardView.EventArgs.ItemAppearedEventArgs args)
        {
          
        }

        private void CarouselView_ItemAppearing(PanCardView.CardsView view, PanCardView.EventArgs.ItemAppearingEventArgs args)
        {

        }
    }
}