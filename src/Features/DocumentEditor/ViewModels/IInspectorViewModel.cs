using DocumentEditor.Items;

namespace DocumentEditor.ViewModels;

public interface IInspectorViewModel
{
    EntityItem? SelectedEntity { get; set; }
}
