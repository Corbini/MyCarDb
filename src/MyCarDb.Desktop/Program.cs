using Microsoft.Extensions.DependencyInjection;
using MyCarDb;
using Photino.Blazor;

var builder = PhotinoBlazorApp.CreateBuilder(args);
builder.RootComponents.Add<Routes>("#app");
builder.Services.AddMyCarDb();
builder.ConfigureMainWindow(window => window.SetTitle("MyCarDb").SetSize(1280, 800));

await using var app = builder.Build();
return app.Run();
