using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Geocoder
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called queue

        public static void PerformUpdate()
        {
            GeocoderGo job = new GeocoderGo();
            //myJob.GetListFromCSV();
        }


        [NoAutomaticTrigger]
        public static void ManualTrigger()
        {
            //log.WriteLine("Function is invoked with value={0}", value);
            //message = value.ToString();
            //log.WriteLine("Following message will be written on the Queue={0}", message);
            try {
                // Geocoder job = new Geocoder();
            } catch(System.InvalidOperationException e)
            {
                System.Console.WriteLine("Something went wromg: " + e);
            }
            
        }
    }
}
