using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWServer
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            ITW.Config.ConfigManager.Initialize("config.ini");
            ITWServer server = new ITWServer();
            server.Start(args);
        }
    }
}