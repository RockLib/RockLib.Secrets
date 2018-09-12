namespace RockLib.Secrets
{
    public interface ISecret
    {
        string Key { get; }
        string GetValue();
    }
}
