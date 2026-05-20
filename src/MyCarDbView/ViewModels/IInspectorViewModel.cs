using MyCarDbView.Items;

namespace MyCarDbView.ViewModels;

public interface IInspectorViewModel
{
    EntityItem? SelectedEntity { get; set; }
}
