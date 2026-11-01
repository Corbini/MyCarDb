using System.ComponentModel;
using DocumentEditor.Models;

namespace DocumentEditor.Services;

public interface IDocumentManager : INotifyPropertyChanged
{
    FileSession? ActiveDocument { get; set; }

    Task<ArchiveResult> OpenFromPathAsync(string path);
}
