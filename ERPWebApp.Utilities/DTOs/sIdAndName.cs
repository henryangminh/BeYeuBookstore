using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Utilities.DTOs
{
   public class sIdAndName
    {
        public sIdAndName()
        {
        }

        public sIdAndName(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
