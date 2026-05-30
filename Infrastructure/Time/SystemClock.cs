using [assembly-generic].Common.Time;

namespace [assembly-generic].Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
