using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Browser.Models
{
    public class Element
    {
        public string Name { get; set; }
        public  string Extension { get; set; }
        public string Path { get; set; }
        public  IEnumerable<Element> Children { get; set; }
        public int Id { get; set; }

    }
}