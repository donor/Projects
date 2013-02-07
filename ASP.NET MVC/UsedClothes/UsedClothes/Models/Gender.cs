using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedClothes.Models
{
    public class Gender
    {
        public Gender(int id,string name)
        {
            GenderId = id;
            GenderName = name; }

        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}