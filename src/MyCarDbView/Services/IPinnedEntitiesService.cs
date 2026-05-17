namespace MyCarDbView.Services;

public interface IPinnedEntitiesService
{
    IReadOnlyList<string> Entities { get; }
    void Add(string entity);
    void Remove(string entity);
}
