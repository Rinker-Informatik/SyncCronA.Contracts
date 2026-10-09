using System.Buffers.Binary;
using System.Text;
using SyncCronA.Contracts.Enrollment;

namespace SyncCronA.Contracts.Unittests.Enrollment;

[TestFixture]
public class EnrollmentProofTests
{
    [Test]
    public void Canonicalize_UsesTheExpectedWireFormat()
    {
        var agentId = Guid.Parse("00112233-4455-6677-8899-aabbccddeeff");
        var secondIdentifier = Guid.Parse("10213243-5465-7687-98a9-bacbdcedfe0f");

        var result = EnrollmentProof.Canonicalize(
            agentId,
            secondIdentifier,
            [0x00, 0xff, 0x80],
            [0xde, 0xad, 0xbe, 0xef]);

        // Version, decimal agent ID, little-endian .NET Guid bytes, and two binary fields.
        var expected = Convert.FromHexString(
            "010000002430303131323233332d343435352d363637372d383839392d616162626363646465656666" +
            "00000010433221106554877698a9bacbdcedfe0f" +
            "0000000300ff80" +
            "00000004deadbeef");

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Canonicalize_WithEmptyBinaryFields_WritesZeroLengthsAndNoPayload()
    {
        var result = EnrollmentProof.Canonicalize(Guid.Empty, Guid.Empty, [], []);

        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo(1));
            Assert.That(result.Length, Is.EqualTo(1 + 4 + 36 + 4 + 16 + 4 + 4));
            Assert.That(Encoding.ASCII.GetString(result, 5, 36),
                Is.EqualTo("00000000-0000-0000-0000-000000000000"));
            Assert.That(result.AsSpan(45, 16).ToArray(), Is.All.Zero);
            Assert.That(result.AsSpan(61, 8).ToArray(), Is.All.Zero);
        });
    }

    [TestCase(1)]
    [TestCase(255)]
    [TestCase(256)]
    [TestCase(65536)]
    public void Canonicalize_EncodesChallengeLengthInBigEndian(int challengeLength)
    {
        var challenge = Enumerable.Range(0, challengeLength).Select(i => (byte)i).ToArray();
        byte[] csrHash = [0x11, 0x22];

        var result = EnrollmentProof.Canonicalize(Guid.Empty, Guid.Empty, challenge, csrHash);

        const int challengeLengthOffset = 1 + 4 + 36 + 4 + 16;
        var hashLengthOffset = challengeLengthOffset + 4 + challengeLength;
        Assert.Multiple(() =>
        {
            Assert.That(BinaryPrimitives.ReadInt32BigEndian(result.AsSpan(challengeLengthOffset, 4)),
                Is.EqualTo(challengeLength));
            Assert.That(result.AsSpan(challengeLengthOffset + 4, challengeLength).ToArray(),
                Is.EqualTo(challenge));
            Assert.That(BinaryPrimitives.ReadInt32BigEndian(result.AsSpan(hashLengthOffset, 4)),
                Is.EqualTo(csrHash.Length));
            Assert.That(result.AsSpan(hashLengthOffset + 4).ToArray(), Is.EqualTo(csrHash));
        });
    }

    [Test]
    public void Canonicalize_DifferentFieldBoundariesProduceDifferentResults()
    {
        var first = EnrollmentProof.Canonicalize(Guid.Empty, Guid.Empty, [0x01], [0x02, 0x03]);
        var second = EnrollmentProof.Canonicalize(Guid.Empty, Guid.Empty, [0x01, 0x02], [0x03]);

        Assert.That(first, Is.Not.EqualTo(second));
    }

    [Test]
    public void Canonicalize_ChangingEitherIdentifierChangesTheResult()
    {
        var id = Guid.Parse("00112233-4455-6677-8899-aabbccddeeff");
        var baseline = EnrollmentProof.Canonicalize(Guid.Empty, Guid.Empty, [], []);

        Assert.Multiple(() =>
        {
            Assert.That(EnrollmentProof.Canonicalize(id, Guid.Empty, [], []), Is.Not.EqualTo(baseline));
            Assert.That(EnrollmentProof.Canonicalize(Guid.Empty, id, [], []), Is.Not.EqualTo(baseline));
        });
    }

    [Test]
    public void Canonicalize_WithNullChallenge_Throws()
    {
        Assert.Throws<NullReferenceException>(() =>
            EnrollmentProof.Canonicalize(Guid.Empty, Guid.Empty, null!, []));
    }

    [Test]
    public void Canonicalize_WithNullCsrHash_Throws()
    {
        Assert.Throws<NullReferenceException>(() =>
            EnrollmentProof.Canonicalize(Guid.Empty, Guid.Empty, [], null!));
    }
}
