using System;
using System.Collections.Generic;
using Khushoo3.Models;
using SQLite;
namespace Khushoo3.Models
{
    public class Timings
    {
        
        public string Fajr    { get; set; }
        public string Sunrise { get; set; }
        public string Dhuhr   { get; set; }
        public string Asr     { get; set; }
        public string Sunset  { get; set; }
        public string Maghrib { get; set; }
        public string Isha    { get; set; }
        public string Imsak    { get; set; }
        public string Midnight { get; set; }
    }

   
    public class Month
    {
        public int number { get; set; }
        public string en { get; set; }
        public string ar { get; set; }

    }

  

    public class Gregorian
    {
        public string date { get; set; }
        public string format { get; set; }
        public string day { get; set; }        
        public Month month { get; set; }
        public string year { get; set; }
     
    }

    public class Weekday2
    {
        public string en { get; set; }
        public string ar { get; set; }
    }

    public class Month2
    {
        public int number { get; set; }
        public string en { get; set; }
        public string ar { get; set; }
    }

   

    public class Hijri
    {
        public string date { get; set; }
        public string format { get; set; }
        public string day { get; set; }
        public Weekday2 weekday { get; set; }
        public Month2 month { get; set; }
        public string year { get; set; }
     
        public List<string> holidays { get; set; }
    }

    public class Date
    {
        public string readable { get; set; }
       
        public Gregorian gregorian { get; set; }
        public Hijri hijri { get; set; }
    }

    




public class Datum
{
    public Timings timings { get; set; }
    public Date date { get; set; }
    
}

public class Root
{
   
    public List<Datum> data { get; set; }
}


}

