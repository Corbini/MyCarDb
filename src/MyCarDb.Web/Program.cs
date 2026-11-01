using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyCarDb;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.Services.AddMyCarDb();

await builder.Build().RunAsync();
