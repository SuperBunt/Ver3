using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class WikiContent
    {
        public WikiContent()
        {
        }

        public string PageTitle { get; set; }
        public string PageContent { get; set; }

        public WikiContent(string t, string con)
        {
            PageTitle = t;
            PageContent = con;
        }

        
    }
}