using System;
using System.Configuration;
using System.ServiceModel;

namespace battleship_server
{
    class Server
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri(ConfigurationManager.AppSettings["addr"]);
            ServiceHost host = new ServiceHost(typeof(BattleshipService), uri);
            host.Open();
            Console.WriteLine("Chat service listen on endpoint {0}", uri.ToString());
            Console.WriteLine("Press ENTER to stop chat service...");
            Console.ReadLine();
            host.Abort();
            host.Close(); 
        }
    }
}
