using System;
using System.Collections.Generic;
using Khushoo3.Models;

namespace Khushoo3.Models
{
    public class Timings
    {
        public string Fajr { get; set; }
        public string Sunrise { get; set; }
        public string Dhuhr { get; set; }
        public string Asr { get; set; }
        public string Sunset { get; set; }
        public string Maghrib { get; set; }
        public string Isha { get; set; }
        public string Imsak { get; set; }
        public string Midnight { get; set; }
    }

   

    public class Month
    {
        public int number { get; set; }
        public string en { get; set; }
        public string ar { get; set; }
    }

   

    public class Hijri
    {
        public string date { get; set; }
       
        public string day { get; set; }
      
        public Month month { get; set; }
        public string year { get; set; }
      
       
    }

    

    public class Date
{
   
    public Hijri hijri { get; set; }
    
}





    public class Data
    {
        public Timings timings { get; set; }
        public Date date { get; set; }

    }

    public class salatifo
    {

        public Data data { get; set; }

    }



}

