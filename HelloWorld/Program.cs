using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace HelloWorld
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        
        static void Main()
        {
            Console.WriteLine("Begin: Executing Ppr update!");
            HelloWorld.Functions.PerformUpdate();
            Console.WriteLine("End: Executing Ppr update!");
        }
    }
}
