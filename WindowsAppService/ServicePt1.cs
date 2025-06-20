using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using System.Configuration;

namespace WindowsAppService
{
    public partial class Service: ServiceBase, IDisposable
    {
        CustomLogger m_logger;
        FileSystemWatcher m_filewatcher = null;
        string m_fileDest = string.Empty;
        bool m_mustExit = false;
        string m_appdir = string.Empty;
        public Service()
        {
            //InitializeComponent();

            m_logger = new CustomLogger();
            var IsX86FileExist = false;

            try
            {
                m_fileDest = ConfigurationManager.AppSettings["FolderDest"];
                m_filewatcher = new FileSystemWatcher(ConfigurationManager.AppSettings["FolderSourceMonitor"]);
                IsX86FileExist = File.Exists("%ProgramFiles(x86)%\\installService_x86.bat");
                m_appdir = IsX86FileExist ? ConfigurationManager.AppSettings["InstallDir_x86"] : ConfigurationManager.AppSettings["InstallDir"];
            }
            catch
            {
                m_logger.Fatal("m_filewatcher or m_fileDest is null. Check configuration file and check source/dest folders", null);
            }

            m_filewatcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            m_filewatcher.Changed += OnChanged;
            m_filewatcher.Created += OnCreated;
            m_filewatcher.Deleted += OnDeleted;
            m_filewatcher.Renamed += OnRenamed;
            m_filewatcher.Error += OnError;

            //m_filewatcher.Filter = "*.*";
            m_filewatcher.IncludeSubdirectories = true;
            m_filewatcher.EnableRaisingEvents = true;

            try
            {
                // Create the source, if it does not already exist.
                if (!EventLog.SourceExists(ConfigurationManager.AppSettings["FolderMonitorSourceEvent"]))
                {
                    //An event log source should not be created and immediately used.
                    //There is a latency time to enable the source, it should be created
                    //prior to executing the application that uses the source.
                    //Execute this sample a second time to use the new source.
                    EventLog.CreateEventSource(ConfigurationManager.AppSettings["FolderMonitorSourceEvent"], ConfigurationManager.AppSettings["FolderMonitorEvent"]);
                    Console.WriteLine("CreatedEventSource");
                    Console.WriteLine("Exiting, execute the application a second time to use the event source.");
                    m_mustExit = true;
                }
            }
            catch (Exception ex)
            {
                m_logger.Fatal("Can't create event log", ex);
            }
        }

        private void CheckExit()
        {
            if (m_mustExit)
            {
                this.Stop();
                m_logger.Info("On initialization, service must exit the first time.", null);
            }
        }

        protected override void OnStart(string[] args)
        {
            CheckExit();
            m_logger.Info($"OnStart called", null);
        }
 
        protected override void OnStop()
        {
            m_logger.Info($"OnStop called", null);
            m_filewatcher.EnableRaisingEvents = false;
            this.Dispose();
        }

        ~Service()
        {
            m_filewatcher.Changed -= OnChanged;
            m_filewatcher.Created -= OnCreated;
            m_filewatcher.Deleted -= OnDeleted;
            m_filewatcher.Renamed -= OnRenamed;
            m_filewatcher.Error -= OnError;
            m_logger.Info("service destruction", null);
        }
    }
}
