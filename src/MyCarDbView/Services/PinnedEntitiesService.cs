using MyCarDbView.Dto;

namespace MyCarDbView.Services;

internal sealed class PinnedEntitiesService(IWritableOptions<PinnedEntities> settings) : IPinnedEntitiesService
{
    public IReadOnlyList<string> Entities => settings.Value.Entities;


    public void Add(string entity) =>
        settings.Update(s => s.Entities.Add(entity));

    public void Remove(string entity) =>
        settings.Update(s => s.Entities.Remove(entity));
}
