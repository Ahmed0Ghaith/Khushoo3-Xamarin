using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Khushoo3.Models;
using Khushoo3.ViewModel;
using Newtonsoft.Json;
using Xamarin.Forms;


namespace Khushoo3.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {

            InitializeComponent();

          
        }
      

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
          

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //    search.IsVisible = true;
            //    Action<double> callback = input => SearchZekr.TranslationX = input;
            //    SearchZekr.Animate("anim", callback, 260, 0, 16, 300, Easing.CubicInOut);
            OpenMenu();
        }
        private void MenuTapped(object sender, EventArgs e)
        {
            OpenMenu();
            // OpenVedio();

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
    }
}
