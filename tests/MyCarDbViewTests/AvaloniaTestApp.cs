using Avalonia;
using Avalonia.Headless;
using Xunit;

namespace MyCarDbViewTests;

internal sealed class AvaloniaTestApp : Application;

public sealed class AvaloniaAppFixture
{
    public AvaloniaAppFixture()
    {
        AppBuilder.Configure<AvaloniaTestApp>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions())
            .SetupWithoutStarting();
    }
}

[CollectionDefinition(nameof(AvaloniaCollection))]
public sealed class AvaloniaCollection : ICollectionFixture<AvaloniaAppFixture>;
