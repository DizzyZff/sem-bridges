namespace SemBridges.Sample;

public interface IGreetingService
{
    string Greet(string name);
}

public sealed class GreetingService : IGreetingService
{
    public string Greet(string name)
    {
        return $"Hello, {name}.";
    }
}
