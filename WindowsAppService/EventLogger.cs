using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WindowsAppService
{
    public class EventLogger : ICustomLogger, IDisposable
    {
        protected static readonly ILog m_logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EventLog m_folderEvent = null;
        private bool disposedValue;

        public EventLogger()
        {
            try
            {
                // Create an EventLog instance and assign its source.
                m_folderEvent = new EventLog();
                m_folderEvent.Source = ConfigurationManager.AppSettings["FolderMonitorSourceEvent"];

            }
            catch (Exception ex) 
            { 
                m_logger.Fatal("Can't initialize eventlog. " + ex.Message + "; " + 
                    ex.InnerException != null ? ex.InnerException.Message : null, ex);
            }
        }

        public void Debug(string message, Exception exception)
        {
            try
            {
                m_folderEvent.WriteEntry(message + ", Debug", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                m_logger.Fatal("Debug: " + message + " Can't add eventlog. " + ex.Message + "; " +
                    ex.InnerException != null ? ex.InnerException.Message : null, ex);
            }
        }

        public void Error(string message, Exception exception)
        {
            try
            {
                m_folderEvent.WriteEntry(message, EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                m_logger.Fatal("Error: " + message + " Can't add eventlog. " + ex.Message + "; " +
                    ex.InnerException != null ? ex.InnerException.Message : null, ex);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            try
            {
                m_folderEvent.WriteEntry(message + ", Fatal", EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                m_logger.Fatal("Fatal: " + message + " Can't add eventlog. " + ex.Message + "; " +
                    ex.InnerException != null ? ex.InnerException.Message : null, ex);
            }
        }

        public void Info(string message, Exception exception)
        {
            try
            {
                // Write an informational entry to the event log.
                m_folderEvent.WriteEntry(message, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                m_logger.Fatal("Info: " + message + " Can't add eventlog. " + ex.Message + "; " +
                    ex.InnerException != null ? ex.InnerException.Message : null, ex);
            }
        }

        public void Warn(string message, Exception exception)
        {
            try
            {
                m_folderEvent.WriteEntry(message, EventLogEntryType.Warning);
            }
            catch (Exception ex)
            {
                m_logger.Fatal("Warn: " + message + " Can't add eventlog. " + ex.Message + "; " +
                    ex.InnerException != null ? ex.InnerException.Message : null, ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    m_folderEvent.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                m_folderEvent = null;

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~EventLogger()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
