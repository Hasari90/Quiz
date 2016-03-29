using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<string> Images { get; set; }

        public Gallery()
        {
            Images = new List<string>();
        }
    }
}