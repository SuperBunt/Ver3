using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Geocoder
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            //var host = new JobHost();
            // The following code will invoke a function called ManualTrigger and 
            // pass in data (value in this case) to the function
            try
            {
                //host.Call(typeof(Functions).GetMethod("ManualTrigger"), new { value = 20 });
                Console.WriteLine("Begin: Executing business update!");
                Geocoder.Functions.PerformUpdate();           
                Console.WriteLine("End: Executing business update!");
            }
            catch (System.InvalidOperationException e)
            {
                System.Console.WriteLine("Something went wromg: " + e);
            }
            
        }
    }
}
