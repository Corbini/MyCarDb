using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using MyCarDbView.Items;
using MyCarDbView.Localizations;
using MyCarDbView.Models;
using MyCarDbView.Models.Alerts;
using MyCarDbView.Models.AppState;
using MyCarDbView.Models.Pickers;
using MyCarDbView.Services;
using MyCarDbView.Services.Pickers;
using MyCarDbWorld.Commands;
using MyCarDbWorld.Models;

namespace MyCarDbView.ViewModels;

public record EditorViewModelParameters(
    World World,
    string FilePath
);

public sealed partial class EditorViewModel : ObservableObject, IViewModel
{
    private readonly IWorldArchive _worldArchive;
    private readonly IFileOpenPickerService _filePicker;
    private readonly ISaveFolderResolver _folderResolver;
    private readonly IStringLocalizer<ViewLocalization> _viewLoc;
    private readonly IDialogService _dialogService;
    private readonly ILastOpenedService _lastOpenedService;
    private readonly ILocalizationSource _localization;
    public EntityBrowserViewModel EntityBrowser { get; }

    public InspectorViewModel Inspector { get; }

    public MapViewModel Map { get; }

    public CommandStack Stack { get; }

    [ObservableProperty]
    private World _world;

    [ObservableProperty]
    private IReadOnlyList<EntityItem> _displayEntities = [];

    [ObservableProperty]
    private EntityItem? _selectedEntity;

    partial void OnSelectedEntityChanged(EntityItem? value)
        => Inspector.SelectedEntity = value;

    partial void OnWorldChanged(World value)
    {
        DisplayEntities = value.Entities.Select(CreateEntityItem).ToList();
        EntityBrowser.UpdateEntities(DisplayEntities);
        Stack.ChangeWorld(value);
    }

    private EntityItem CreateEntityItem(IEntity entity)
    {
        var (key, serial) = EntityItem.ParseId(entity.Id);
        return new EntityItem
        {
            Value = entity,
            Title = _localization.GetDb(key),
            Serial = serial,
        };
    }

    private string _filePath;

    [ObservableProperty]
    private string _fileName;

    public EditorViewModel(
        IWorldArchive worldArchive,
        IFileOpenPickerService filePicker,
        ISaveFolderResolver folderResolver,
        IStringLocalizer<ViewLocalization> viewLoc,
        IDialogService dialogService,
        ILastOpenedService lastOpenedService,
        ILocalizationSource localization,
        EditorViewModelParameters parameters,
        EntityBrowserViewModel entityBrowserViewModel,
        MapViewModel mapViewModel)
    {
        _worldArchive = worldArchive;
        _filePicker = filePicker;
        _folderResolver = folderResolver;
        _viewLoc = viewLoc;
        _dialogService = dialogService;
        _lastOpenedService = lastOpenedService;
        _localization = localization;
        Stack = new CommandStack(parameters.World);
        EntityBrowser = entityBrowserViewModel;
        Inspector = new InspectorViewModel(Stack);
        Map = mapViewModel;

        EntityBrowser.SelectionChanged += entity => SelectedEntity = entity;

        World = parameters.World;
        _filePath = parameters.FilePath;
        FileName = Path.GetFileName(parameters.FilePath);
    }

    [ObservableProperty]
    private bool _isSidebarVisible = true;

    [RelayCommand]
    private void ToggleSidebar()
        => IsSidebarVisible = !IsSidebarVisible;

    [ObservableProperty]
    private bool _isInspectorVisible = true;

    [RelayCommand]
    private void ToggleInspector()
        => IsInspectorVisible = !IsInspectorVisible;

    [RelayCommand]
    private void Undo() => Stack.Undo();

    [RelayCommand]
    private void Redo() => Stack.Redo();

    [RelayCommand]
    private Task OpenSettingsAsync(CancellationToken ct) =>
        _dialogService.ShowAsync<SettingsViewModel>(ct);

    [RelayCommand]
    private async Task OpenFileDialogAsync()
    {
        string? path = await _filePicker.PickAsync(new FileOpenPickerOptions
        {
            Title = _viewLoc["Welcome_OpenSave_Label"].Value,
            SuggestedFolder = _folderResolver.SuggestedFolder,
            FileTypes = [new FileTypeFilter { Name = _viewLoc["Welcome_OpenSave_FileType"].Value, Patterns = ["*.txt"] }],
        });

        if (path is null)
            return;

        await OpenFileFromPath(path);
    }

    [RelayCommand]
    private async Task OpenFileFromPath(string path)
    {
        switch (await _worldArchive.LoadWorldAsync(path))
        {
            case ArchiveResult.Success success:
                _filePath = path;
                FileName = Path.GetFileName(path);
                World = success.Content;
                break;
            case ArchiveResult.Failed failed:
                await _dialogService.ShowAsync<AlertViewModel, AlertOptions>(
                    new AlertOptions
                    {
                        Severity = AlertSeverity.Critical,
                        Title = _viewLoc["Welcome_OpenSave_FailedTitle"].Value,
                        Message = _viewLoc[failed.DetailKey].Value,
                    });
                return;
        }

        _lastOpenedService.Add(new LastOpenedFile(path, DateTime.Now));
    }

    [RelayCommand]
    private async Task SaveFileAsync()
    {
        bool isSaved = await _worldArchive.SaveWorldAsync(World, _filePath);
        if (!isSaved)
        {
            await _dialogService.ShowAsync<AlertViewModel, AlertOptions>(
                    new AlertOptions
                    {
                        Severity = AlertSeverity.Critical,
                        Title = _viewLoc["Editor_Save_FailedTitle"].Value,
                        Message = _viewLoc["Editor_Save_FailedMessage"].Value,
                    });
        }
    }
}
