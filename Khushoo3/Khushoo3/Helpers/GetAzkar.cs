using Khushoo3.Models;
using Khushoo3.ViewModel;
using Khushoo3.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Khushoo3.Helpers
{
  public  class GetAzkar 
    {
        
      
      

        public static zekrinfo GetJsonData()
        {
            string jsonfile = "Resources.azkar.json";
            zekrinfo Result = new zekrinfo();
            var assembly = typeof(HomePage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonfile}");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var JsonString = reader.ReadToEnd();
                Result = JsonConvert.DeserializeObject<zekrinfo>(JsonString);
            }
            return Result;

        }
    }
}
