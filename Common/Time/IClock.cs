namespace [assembly-generic].Common.Time;

public interface IClock
{
    DateTime UtcNow { get; }
}
