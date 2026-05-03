# Third-Party Licenses

This product bundles third-party software components. Each component's full
license text lives in `licenses/` and **must ship with every distribution**
(installer, zip, etc.) alongside the binary.

| Component | License | Notice |
|---|---|---|
| Phosphor Icons | MIT | [licenses/phosphor-icons.LICENSE](licenses/phosphor-icons.LICENSE) |
| Inter (font) | SIL OFL 1.1 | [licenses/inter.LICENSE](licenses/inter.LICENSE) |
| Avalonia UI | MIT | [licenses/avalonia.LICENSE](licenses/avalonia.LICENSE) |
| .NET Community Toolkit | MIT | [licenses/communitytoolkit.LICENSE](licenses/communitytoolkit.LICENSE) |
| .NET Foundation libraries | MIT | [licenses/dotnet-foundation.LICENSE](licenses/dotnet-foundation.LICENSE) |

## Coverage details

- **Phosphor Icons** — selected SVGs in `src/AvaloniaStyles/Icons/`
- **Inter** — bundled via the `Avalonia.Fonts.Inter` NuGet package; reserved font name
- **Avalonia UI** — `Avalonia`, `Avalonia.Desktop`, `Avalonia.Themes.Fluent`, `Avalonia.Fonts.Inter`, `AvaloniaUI.DiagnosticsSupport`
- **.NET Community Toolkit** — `CommunityToolkit.Mvvm`
- **.NET Foundation libraries** — `Microsoft.Extensions.DependencyInjection`, `Microsoft.Extensions.Localization`

Build-time-only packages (`Microsoft.CodeAnalysis.CSharp` with `PrivateAssets="all"`,
test packages) do not ship with the application binary and are not listed here.
