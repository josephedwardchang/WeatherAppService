using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WindowsAppService
{
    public class CustomLogger : ICustomLogger
    {
        EventLogger m_EventLogger;
        FileLogger m_FileLogger;
        public CustomLogger() 
        {
            m_EventLogger = new EventLogger();
            m_FileLogger = new FileLogger();
        }
        public void Debug(string message, Exception exception)
        {
            m_EventLogger.Debug(message, exception);
            m_FileLogger.Debug(message, exception);
        }

        public void Error(string message, Exception exception)
        {
            m_EventLogger.Error(message, exception);
            m_FileLogger.Error(message, exception);
        }

        public void Fatal(string message, Exception exception)
        {
            m_EventLogger.Fatal(message, exception);
            m_FileLogger.Fatal(message, exception);
        }

        public void Info(string message, Exception exception)
        {
            m_EventLogger.Info(message, exception);
            m_FileLogger.Info(message, exception);
        }

        public void Warn(string message, Exception exception)
        {
            m_EventLogger.Warn(message, exception);
            m_FileLogger.Warn(message, exception);
        }
    }
}
