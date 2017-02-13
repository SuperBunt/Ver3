using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class WikiImage
    {
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        // Simple Constructor
        public WikiImage(string url, int height, int width)
        {
            Url = url;
            Height = height;
            Width = width;
        }
    }
}