using System.Buffers.Binary;
using System.Text;

namespace SyncCronA.Contracts.Enrollment;

public static class EnrollmentProof
{
    public static byte[] Canonicalize(Guid agentId, Guid secondIdentifier, byte[] challenge, byte[] csrHash)
    {
        using var stream = new MemoryStream();
        stream.WriteByte(1);
        Write(stream, Encoding.UTF8.GetBytes(agentId.ToString()));
        Write(stream, secondIdentifier.ToByteArray());
        Write(stream, challenge);
        Write(stream, csrHash);
        return stream.ToArray();
    }

    private static void Write(Stream stream, byte[] value)
    {
        Span<byte> size = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(size, value.Length);
        stream.Write(size);
        stream.Write(value);
    }
}