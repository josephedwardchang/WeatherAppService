using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WindowsAppService
{
    public class FileLogger : ICustomLogger
    {
        protected static readonly ILog m_logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FileLogger() 
        { 
        }
        public void Debug(string message, Exception exception)
        {
            m_logger.Debug(message, exception); 
        }

        public void Error(string message, Exception exception)
        {
            m_logger.Error(message, exception); 
        }

        public void Fatal(string message, Exception exception)
        {
            m_logger.Fatal(message, exception);
        }

        public void Info(string message, Exception exception)
        {
            m_logger.Info(message, exception);
        }

        public void Warn(string message, Exception exception)
        {
            m_logger.Warn(message, exception);
        }
    }
}
