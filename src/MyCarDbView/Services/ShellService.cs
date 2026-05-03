using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace MyCarDbView.Services;

internal sealed partial class ShellService(IServiceProvider services) : ObservableObject, IShellService
{
    [ObservableProperty]
    private IViewModel? _currentContent;


    public void NavigateTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>()
        where TViewModel : class, IViewModel
    {
        CurrentContent = ActivatorUtilities.CreateInstance<TViewModel>(services);
    }

    public void NavigateTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameters>(TParameters parameters)
        where TViewModel : class, IViewModel
        where TParameters : notnull
    {
        CurrentContent = ActivatorUtilities.CreateInstance<TViewModel>(services, parameters);
    }
}
