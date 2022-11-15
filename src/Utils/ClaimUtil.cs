using System.Security.Claims;

namespace IWantAPP.Utils;

public class ClaimUtil
{
    public static string GetClaim(IList<Claim> claims, string type)
    {
        var claimInfo = claims.FirstOrDefault(c => c.Type == type);
        return claimInfo == null ? string.Empty : claimInfo.Value;
    }
}
