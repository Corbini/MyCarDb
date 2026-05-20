using MyCarDbWorld.Commands;
using MyCarDbWorld.Models;

namespace MyCarDbView.Models;

public sealed record FileSession(World World, CommandStack Stack, string FilePath);
