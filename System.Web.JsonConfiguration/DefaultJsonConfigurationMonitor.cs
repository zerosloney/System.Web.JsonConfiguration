using System.IO;

namespace System.Web.JsonConfiguration
{
    /// <summary>
    /// 文件监听
    /// </summary>
    class DefaultJsonConfigurationMonitor : IJsonConfigurationMonitor
    {
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="json"></param>
        public  void Start(JsonConfigurationPath json)
        {
            var watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = false;
            watcher.Path = json.Path;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.Size;
            watcher.Filter = json.Name;
            watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            watcher.EnableRaisingEvents = true;
        }
        /// <summary>
        /// 文件改变
        /// </summary>
        /// <param name="render"></param>
        /// <param name="e"></param>
        private  void OnFileChanged(object render, FileSystemEventArgs e)
        {
            if (!FileHelper.IsFileLocked(e.FullPath))
            {
                var watcher = render as FileSystemWatcher;
                watcher.EnableRaisingEvents = false;
                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    JsonConfigurationProvider.Set(e.Name, File.ReadAllText(e.FullPath));
                }
                watcher.EnableRaisingEvents = true;
            }
        }

    }

}
