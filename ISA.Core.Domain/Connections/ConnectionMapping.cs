namespace ISA.Core.Domain.Connections;

public static class ConnectionMapping
{
    private static readonly Dictionary<Guid, string> _connections = new Dictionary<Guid, string>();

    public static void Add(Guid userId, string connectionId)
    {
        lock (_connections)
        {
            _connections[userId] = connectionId;
        }
    }

    public static string GetConnectionId(Guid userId)
    {
        lock (_connections)
        {
            return _connections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }
    }

    public static void Remove(Guid userId)
    {
        lock (_connections)
        {
            _connections.Remove(userId);
        }
    }
}