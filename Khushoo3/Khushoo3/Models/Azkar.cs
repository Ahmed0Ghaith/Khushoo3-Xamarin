using System;
using System.Collections.Generic;
using System.Text;

namespace Khushoo3.Models
{
 
   
   
    public class Azkar
    {
        public string category { get; set; }
        public string count { get; set; }
        public string description { get; set; }
        public string reference { get; set; }
        public string zekr { get; set; }
    }

    public class zekrinfo
    {
        public IList<Azkar> Azkar { get; set; }
    }





}
