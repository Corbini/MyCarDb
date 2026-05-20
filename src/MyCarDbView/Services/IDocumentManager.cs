using System.ComponentModel;
using MyCarDbView.Models;

namespace MyCarDbView.Services;

public interface IDocumentManager : INotifyPropertyChanged
{
    FileSession? ActiveDocument { get; set; }

    Task<ArchiveResult> OpenFromPathAsync(string path);
}
