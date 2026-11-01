using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contracts;
using DocumentEditor.Items;
using DocumentEditor.Models;
using DocumentEditor.Models.AppState;
using DocumentEditor.Models.Pickers;
using DocumentEditor.Services;
using DocumentEditor.Services.Pickers;
using DocumentEditor.ViewModels;
using GameWorld.Commands;
using GameWorld.Models;
using Microsoft.Extensions.Localization;
using MyCarDbView.Localizations;
using MyCarDbView.Models;
using MyCarDbView.Models.Alerts;
using MyCarDbView.Models.AppState;
using MyCarDbView.Services;
using MyCarDbView.Services.Pickers;

namespace MyCarDbView.ViewModels;

public sealed partial class EditorViewModel : ObservableObject, IViewModel
{
    private readonly IDocumentManager _docMgr;
    private readonly IWorldArchive _worldArchive;
    private readonly IFileOpenPickerService _filePicker;
    private readonly ISaveFolderResolver _folderResolver;
    private readonly IStringLocalizer<ViewLocalization> _viewLoc;
    private readonly IDialogService _dialogService;
    private readonly ILastOpenedService _lastOpenedService;
    private readonly ILocalizationSource _localization;
    private FileSession _session;

    public IEntityBrowserViewModel EntityBrowser { get; }
    public IInspectorViewModel Inspector { get; }
    public IMapViewModel Map { get; }

    [ObservableProperty]
    private IReadOnlyList<EntityItem> _displayEntities = [];

    [ObservableProperty]
    private EntityItem? _selectedEntity;

    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    private bool _isSidebarVisible = true;

    [ObservableProperty]
    private bool _isInspectorVisible = true;

    public EditorViewModel(
        IDocumentManager docMgr,
        IWorldArchive worldArchive,
        IFileOpenPickerService filePicker,
        ISaveFolderResolver folderResolver,
        IStringLocalizer<ViewLocalization> viewLoc,
        IDialogService dialogService,
        ILastOpenedService lastOpenedService,
        ILocalizationSource localization,
        IEntityBrowserViewModel entityBrowserViewModel,
        IInspectorViewModel inspectorViewModel,
        IMapViewModel mapViewModel)
    {
        _docMgr = docMgr;
        _worldArchive = worldArchive;
        _filePicker = filePicker;
        _folderResolver = folderResolver;
        _viewLoc = viewLoc;
        _dialogService = dialogService;
        _lastOpenedService = lastOpenedService;
        _localization = localization;
        EntityBrowser = entityBrowserViewModel;
        Inspector = inspectorViewModel;
        Map = mapViewModel;

        _session = docMgr.ActiveDocument
            ?? throw new InvalidOperationException("EditorViewModel requires an active document.");

        _docMgr.PropertyChanged += OnDocMgrChanged;
        EntityBrowser.SelectionChanged += OnSelectionChanged;
        Map.SelectionChanged += OnSelectionChanged;

        RefreshFromSession();
    }

    private void OnSelectionChanged(EntityItem? entity) => SelectedEntity = entity;

    partial void OnSelectedEntityChanged(EntityItem? value)
    {
        Inspector.SelectedEntity = value;
        EntityBrowser.SyncSelection(value);
        Map.SyncSelection(value);
    }

    private void OnDocMgrChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(IDocumentManager.ActiveDocument))
            return;
        _session = _docMgr.ActiveDocument
            ?? throw new InvalidOperationException("ActiveDocument went null while EditorViewModel is alive.");
        SelectedEntity = null;
        RefreshFromSession();
    }

    private void RefreshFromSession()
    {
        FileName = Path.GetFileName(_session.FilePath);
        DisplayEntities = _session.World.Entities.Select(CreateEntityItem).ToList();
        EntityBrowser.UpdateEntities(DisplayEntities);
        Map.UpdateEntities(DisplayEntities);
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

    [RelayCommand]
    private void ToggleSidebar() => IsSidebarVisible = !IsSidebarVisible;

    [RelayCommand]
    private void ToggleInspector() => IsInspectorVisible = !IsInspectorVisible;

    [RelayCommand]
    private void Undo() => _session.Stack.Undo();

    [RelayCommand]
    private void Redo() => _session.Stack.Redo();

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
        switch (await _docMgr.OpenFromPathAsync(path))
        {
            case ArchiveResult.Success:
                _lastOpenedService.Add(new LastOpenedFile(path, DateTime.Now));
                break;
            case ArchiveResult.Failed failed:
                await _dialogService.ShowAsync<AlertViewModel, AlertOptions>(
                    new AlertOptions
                    {
                        Severity = AlertSeverity.Critical,
                        Title = _viewLoc["Welcome_OpenSave_FailedTitle"].Value,
                        Message = _viewLoc[failed.DetailKey].Value,
                    });
                break;
        }
    }

    [RelayCommand]
    private async Task SaveFileAsync()
    {
        bool isSaved = await _worldArchive.SaveWorldAsync(_session.World, _session.FilePath);
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
