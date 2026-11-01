using GameWorld.Commands;
using GameWorld.Models;

namespace MyCarDbView.Models;

public sealed record FileSession(World World, CommandStack Stack, string FilePath);
