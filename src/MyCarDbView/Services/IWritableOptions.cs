using Microsoft.Extensions.Options;

namespace MyCarDbView.Services;

public interface IWritableOptions<T> : IOptions<T>
    where T : class, new()
{
    void Update(Action<T> applyChanges);
}
