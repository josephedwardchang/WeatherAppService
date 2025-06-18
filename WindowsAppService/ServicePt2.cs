using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAppService
{
    public partial class Service : ServiceBase, IFileWatcher, IDisposable
    {
        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            CheckExit();
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}, {e.ChangeType}");
            m_logger.Info($"Changed: {e.FullPath}, {e.ChangeType}", null);
        }

        public void OnCreated(object sender, FileSystemEventArgs e)
        {
            CheckExit();
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            m_logger.Info($"Created: {e.FullPath}", null);

            // perform file move
            try
            {
                File.Move(e.FullPath, m_fileDest);
                m_logger.Info($"Moved file {e.FullPath} to {m_fileDest}", null);
            }
            catch (Exception ex)
            {
                m_logger.Fatal($"Can't move file {e.FullPath}", ex);
            }
        }

        public void OnDeleted(object sender, FileSystemEventArgs e)
        {
            CheckExit();
            Console.WriteLine($"Deleted: {e.FullPath}");
            m_logger.Info($"Deleted: {e.FullPath}", null);
        }

        public void OnRenamed(object sender, RenamedEventArgs e)
        {
            CheckExit();
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
            m_logger.Info($"Renamed: {e.OldFullPath} To: {e.FullPath}", null);
        }

        public void OnError(object sender, ErrorEventArgs e) =>
            SendException(e.GetException());

        public void SendException(Exception ex)
        {
            CheckExit();
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                //PrintException(ex.InnerException);
                m_logger.Error($"Exception: {ex.Message}", ex);
            }
        }
    }
}
