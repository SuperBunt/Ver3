using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace HelloWorld
{
    public class Functions
    {
        
        public static void PerformUpdate()
        {
            PPRFile myJob = new PPRFile();
            //myJob.GetListFromCSV();
        }


    }
}
