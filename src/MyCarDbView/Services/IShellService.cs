using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace MyCarDbView.Services;

public interface IShellService : INotifyPropertyChanged
{
    IViewModel? CurrentContent { get; }

    void NavigateTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>()
        where TViewModel : class, IViewModel;

    void NavigateTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameters>(TParameters parameters)
        where TViewModel : class, IViewModel
        where TParameters : notnull;
}
