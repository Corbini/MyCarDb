using GameWorld.Commands;
using GameWorld.Models;

namespace DocumentEditor.Models;

public sealed record FileSession(World World, CommandStack Stack, string FilePath);
