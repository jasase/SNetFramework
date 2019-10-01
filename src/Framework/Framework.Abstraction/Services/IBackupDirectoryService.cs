using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.Abstraction.Services
{
    public interface IBackupDirectoryService
    {
        string BackupDirectory { get; }

        string CreateOrGetPathFor(DateTime date);
        void SaveBackup(DateTime date, Stream data);

        IEnumerable<FileInfo> BackupsOn(DateTime date);

        void DeleteBackupsFor(DateTime forDate);
    }
}
