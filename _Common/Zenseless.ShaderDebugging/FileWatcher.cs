﻿using System.ComponentModel;
using System.IO;

namespace Zenseless.ShaderDebugging
{
	public class FileWatcher
	{
		public FileWatcher(string filePath, ISynchronizeInvoke syncObject = null)
		{
			Dirty = true;
			FullPath = Path.GetFullPath(filePath);
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("File does not exist", filePath);
			}
			watcher = new FileSystemWatcher(Path.GetDirectoryName(FullPath), Path.GetFileName(FullPath));
			watcher.SynchronizingObject = syncObject;
			watcher.Changed += FileNotification;
			//visual studio does not change a file, but saves a copy and later deletes the original and renames
			watcher.Created += FileNotification;
			watcher.Renamed += FileNotification;
			watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.FileName;
			watcher.EnableRaisingEvents = true;
		}

		public event FileSystemEventHandler Changed;

		private void FileNotification(object sender, FileSystemEventArgs e)
		{
			Dirty = true;
			Changed?.Invoke(sender, e);
		}

		public bool Dirty { get; set; }
		public string FullPath { get; private set; }

		private FileSystemWatcher watcher;
	}
}
