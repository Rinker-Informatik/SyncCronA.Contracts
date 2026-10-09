namespace SyncCronA.Contracts.Enrollment;

public record EnrollmentResponse(string ClientCertificatePem, string CaCertificatePem);