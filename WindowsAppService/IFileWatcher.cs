using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAppService
{
    public interface IFileWatcher
    {
        void OnChanged(object sender, FileSystemEventArgs e);

        void OnCreated(object sender, FileSystemEventArgs e);

        void OnDeleted(object sender, FileSystemEventArgs e);

        void OnRenamed(object sender, RenamedEventArgs e);

        void OnError(object sender, ErrorEventArgs e);

        void SendException(Exception ex);
    }
}
