using Ocelot.Configuration;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ApiGateway
{
    public class OcelotJwtMiddleware
    {
        public static bool Authorize(HttpContext ctx)
        {
            try
            {
                DownstreamRoute route = (DownstreamRoute)ctx.Items["DownstreamRoute"];
                string key = route.AuthenticationOptions.AuthenticationProviderKey;

                if (key == null || key == "") return true;
                if (route.RouteClaimsRequirement.Count == 0) return true;
                else
                {
                    //flag for authorization
                    bool auth = false;

                    //where are stored the claims of the jwt token
                    Claim[] claims = ctx.User.Claims.ToArray<Claim>();

                    //where are stored the required claims for the route
                    Dictionary<string, string> required = route.RouteClaimsRequirement;

                    Regex reor = new Regex(@"[^,\s+$ ][^\,]*[^,\s+$ ]");
                    MatchCollection matches;

                    Regex reand = new Regex(@"[^&\s+$ ][^\&]*[^&\s+$ ]");
                    MatchCollection matchesand;
                    int cont = 0;
                    foreach (KeyValuePair<string, string> claim in required)
                    {
                        matches = reor.Matches(claim.Value);
                        foreach (Match match in matches)
                        {
                            matchesand = reand.Matches(match.Value);
                            cont = 0;
                            foreach (Match m in matchesand)
                            {
                                foreach (Claim cl in claims)
                                {
                                    if (cl.Type == claim.Key)
                                    {
                                        if (cl.Value == m.Value)
                                        {
                                            cont++;
                                        }
                                    }
                                }
                            }
                            if (cont == matchesand.Count)
                            {
                                return true;
                                // break;
                            }
                        }
                    }
                    return auth;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
