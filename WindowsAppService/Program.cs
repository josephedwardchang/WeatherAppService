﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;


namespace WindowsAppService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            string strFolder1 = ConfigurationManager.AppSettings["FolderSourceMonitor"];
            string strFolder2 = ConfigurationManager.AppSettings["FolderDest"]; 
            try
            {
                if (!Directory.Exists(strFolder1))
                {
                    Directory.CreateDirectory(strFolder1);
                }
                if (!Directory.Exists(strFolder2))
                {
                    Directory.CreateDirectory(strFolder2);
                }
            }
            catch 
            { 
                
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
