using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JaspersoftRESTClient
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Organization org = new Organization("organisation_142");
            Console.WriteLine(org.Exists());
            org.Create("organizarion_100");
        }
    }
}
