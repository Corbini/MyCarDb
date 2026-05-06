using System;
using Avalonia;
using Avalonia.Controls;

namespace AvaloniaStyles.Controls;

public class ActionButton : Button
{
    public static readonly StyledProperty<ActionButtonVariant> VariantProperty =
        AvaloniaProperty.Register<ActionButton, ActionButtonVariant>(nameof(Variant), ActionButtonVariant.Bordered);

    static ActionButton()
    {
        VariantProperty.Changed.AddClassHandler<ActionButton>((b, _) => b.SyncVariantClasses());
    }

    public ActionButton() => SyncVariantClasses();

    protected override Type StyleKeyOverride => typeof(Button);

    public ActionButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    private void SyncVariantClasses()
    {
        Classes.Set("filled", Variant == ActionButtonVariant.Filled);
        Classes.Set("bordered", Variant == ActionButtonVariant.Bordered);
        Classes.Set("plain", Variant == ActionButtonVariant.Plain);
        Classes.Set("borderless", Variant == ActionButtonVariant.Borderless);
    }
}

public enum ActionButtonVariant
{
    Filled,
    Bordered,
    Plain,
    Borderless,
}
