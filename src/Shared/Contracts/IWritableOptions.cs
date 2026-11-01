using Microsoft.Extensions.Options;

namespace Contracts;

public interface IWritableOptions<T> : IOptions<T>
    where T : class, new()
{
    void Update(Action<T> applyChanges);
}
