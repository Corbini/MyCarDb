using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Contracts;

namespace MyCarDbView.Services;

internal sealed class WritableOptions<T>(string filePath, JsonTypeInfo<T> typeInfo) : IWritableOptions<T>
    where T : class, new()
{
    private readonly T _current = LoadOrDefault(filePath, typeInfo);

    public T Value => _current;

    public void Update(Action<T> applyChanges)
    {
        applyChanges(_current);
        Persist();
    }

    private void Persist()
    {
        AppPaths.EnsureExists();
        string json = JsonSerializer.Serialize(_current, typeInfo);
        File.WriteAllText(filePath, json);
    }

    private static T LoadOrDefault(string path, JsonTypeInfo<T> info)
    {
        try
        {
            string content = File.ReadAllText(path);
            return JsonSerializer.Deserialize(content, info) ?? CreateAndPersistDefault(path, info);
        }
        catch
        {
            return CreateAndPersistDefault(path, info);
        }
    }

    private static T CreateAndPersistDefault(string path, JsonTypeInfo<T> info)
    {
        var defaults = new T();
        try
        {
            AppPaths.EnsureExists();
            File.WriteAllText(path, JsonSerializer.Serialize(defaults, info));
        }
        catch
        {
            // best effort — disk full, permissions, etc.
        }
        return defaults;
    }
}
