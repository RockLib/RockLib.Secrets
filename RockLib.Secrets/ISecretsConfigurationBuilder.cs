namespace RockLib.Secrets
{
    public interface ISecretsConfigurationBuilder
    {
        ISecretsConfigurationBuilder AddSecret(ISecret secret);
    }
}
