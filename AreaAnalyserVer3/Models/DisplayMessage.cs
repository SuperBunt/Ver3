﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class DisplayMessage
    {
        public string Subject { get; set; }
        public DateTimeOffset ReceivedDateTime { get; set; }
        public string From { get; set; }

        public DisplayMessage(string subject, DateTimeOffset? dateTimeReceived,
            Microsoft.Office365.OutlookServices.Recipient from)
        {
            this.Subject = subject;
            this.ReceivedDateTime = (DateTimeOffset)dateTimeReceived;
            this.From = from != null ? string.Format("{0} ({1})", from.EmailAddress.Name,
                            from.EmailAddress.Address) : "EMPTY";
        }
    }
}