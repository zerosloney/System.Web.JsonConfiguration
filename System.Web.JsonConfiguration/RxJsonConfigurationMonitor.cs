﻿
using System.IO;
using System.Reactive.Linq;

namespace System.Web.JsonConfiguration
{
    class RxJsonConfigurationMonitor : IJsonConfigurationMonitor
    {
        public  void Start(JsonConfigurationPath conf)
        {
            var watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = false;
            watcher.Path = conf.Path;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.Size;
            watcher.Filter = conf.Name;
            watcher.EnableRaisingEvents = true;
            Observable.FromEventPattern<FileSystemEventArgs>(watcher, "Changed")
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(e =>
                {
                    if (!FileHelper.IsFileLocked(e.EventArgs.FullPath) && e.EventArgs.ChangeType == WatcherChangeTypes.Changed)
                    {
                        JsonConfigurationProvider.Set(e.EventArgs.Name, File.ReadAllText(e.EventArgs.FullPath));
                    }
                },
                ex => { /* 处理异常 */ },
                () => { /* 处理完成 */}
            );
        }
    }
}
