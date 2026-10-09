namespace SyncCronA.Contracts.Enrollment;

public sealed record BootstrapResponse(string Status, Guid? ChallengeId, string? Challenge, Guid? AgentId = null);