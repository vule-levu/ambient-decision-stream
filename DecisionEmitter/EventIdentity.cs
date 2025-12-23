using System.Security.Cryptography;
using System.Text;

public static class EventIdentity
{
    public static string From(DecisionChanged evt)
    {
        var raw = $"{evt.DecisionId}|{evt.ActorId}|{evt.Dimension}|{evt.Sequence}|{evt.OccurredAt:O}";
        return Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(raw))
        );
    }
}
