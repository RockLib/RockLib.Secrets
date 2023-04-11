using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Secrets.Aws.Tests;

// Defined purely for the appsettings.json file
// for testing.
#pragma warning disable CA1065
public sealed class FakeSecretsManager
    : IAmazonSecretsManager
{
    public ISecretsManagerPaginatorFactory Paginators => throw new NotImplementedException();

    public IClientConfig Config => throw new NotImplementedException();

    public Task<CancelRotateSecretResponse> CancelRotateSecretAsync(CancelRotateSecretRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public CancelRotateSecretResponse CancelRotateSecret(CancelRotateSecretRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CreateSecretResponse> CreateSecretAsync(CreateSecretRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public CreateSecretResponse CreateSecret(CreateSecretRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DeleteResourcePolicyResponse> DeleteResourcePolicyAsync(DeleteResourcePolicyRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public DeleteResourcePolicyResponse DeleteResourcePolicy(DeleteResourcePolicyRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DeleteSecretResponse> DeleteSecretAsync(DeleteSecretRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public DeleteSecretResponse DeleteSecret(DeleteSecretRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DescribeSecretResponse> DescribeSecretAsync(DescribeSecretRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public DescribeSecretResponse DescribeSecret(DescribeSecretRequest request)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<GetRandomPasswordResponse> GetRandomPasswordAsync(GetRandomPasswordRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public GetRandomPasswordResponse GetRandomPassword(GetRandomPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<GetResourcePolicyResponse> GetResourcePolicyAsync(GetResourcePolicyRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public GetResourcePolicyResponse GetResourcePolicy(GetResourcePolicyRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<GetSecretValueResponse> GetSecretValueAsync(GetSecretValueRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public GetSecretValueResponse GetSecretValue(GetSecretValueRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ListSecretsResponse> ListSecretsAsync(ListSecretsRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ListSecretsResponse ListSecrets(ListSecretsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ListSecretVersionIdsResponse> ListSecretVersionIdsAsync(ListSecretVersionIdsRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ListSecretVersionIdsResponse ListSecretVersionIds(ListSecretVersionIdsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PutResourcePolicyResponse> PutResourcePolicyAsync(PutResourcePolicyRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public PutResourcePolicyResponse PutResourcePolicy(PutResourcePolicyRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PutSecretValueResponse> PutSecretValueAsync(PutSecretValueRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public PutSecretValueResponse PutSecretValue(PutSecretValueRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<RemoveRegionsFromReplicationResponse> RemoveRegionsFromReplicationAsync(RemoveRegionsFromReplicationRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public RemoveRegionsFromReplicationResponse RemoveRegionsFromReplication(RemoveRegionsFromReplicationRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ReplicateSecretToRegionsResponse> ReplicateSecretToRegionsAsync(ReplicateSecretToRegionsRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ReplicateSecretToRegionsResponse ReplicateSecretToRegions(ReplicateSecretToRegionsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<RestoreSecretResponse> RestoreSecretAsync(RestoreSecretRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public RestoreSecretResponse RestoreSecret(RestoreSecretRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<RotateSecretResponse> RotateSecretAsync(RotateSecretRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public RotateSecretResponse RotateSecret(RotateSecretRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<StopReplicationToReplicaResponse> StopReplicationToReplicaAsync(StopReplicationToReplicaRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public StopReplicationToReplicaResponse StopReplicationToReplica(StopReplicationToReplicaRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<TagResourceResponse> TagResourceAsync(TagResourceRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public TagResourceResponse TagResource(TagResourceRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UntagResourceResponse> UntagResourceAsync(UntagResourceRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public UntagResourceResponse UntagResource(UntagResourceRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateSecretResponse> UpdateSecretAsync(UpdateSecretRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public UpdateSecretResponse UpdateSecret(UpdateSecretRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateSecretVersionStageResponse> UpdateSecretVersionStageAsync(UpdateSecretVersionStageRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public UpdateSecretVersionStageResponse UpdateSecretVersionStage(UpdateSecretVersionStageRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ValidateResourcePolicyResponse> ValidateResourcePolicyAsync(ValidateResourcePolicyRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValidateResourcePolicyResponse ValidateResourcePolicy(ValidateResourcePolicyRequest request)
    {
        throw new NotImplementedException();
    }
}
#pragma warning restore CA1065