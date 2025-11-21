using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

namespace PoCWebApi.Authorization
{
    public sealed class PdpClient(HttpClient http, ILogger<PdpClient> log) : IPdpClient
    {
        static readonly JsonSerializerOptions J = new(JsonSerializerDefaults.Web);

        public async Task<EvaluateResult> EvaluateAsync(ClaimsPrincipal user, string action, string? resource = null, CancellationToken ct = default)
        {
            var token = user.GetBearerToken(); // helper below
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var req = new EvaluateRequest(action, resource);
            var res = await http.PostAsync("/evaluate",
                new StringContent(JsonSerializer.Serialize(req, J), Encoding.UTF8, "application/json"), ct);

            res.EnsureSuccessStatusCode();
            var payload = await res.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<EvaluateResult>(payload, J)!;
        }

        public async Task<string[]> GetFeatureFlagsAsync(ClaimsPrincipal user, CancellationToken ct = default)
        {
            var token = user.GetBearerToken();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await http.GetAsync("/me/feature-flags", ct);
            res.EnsureSuccessStatusCode();
            var payload = await res.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<string[]>(payload, J) ?? Array.Empty<string>();
        }
    }

    public static class ClaimsPrincipalExt
    {
        public static string GetBearerToken(this ClaimsPrincipal user)
        {
            // If you're using JwtBearer auth, ASP.NET stores it in HttpContext. We’ll pick it up via IHttpContextAccessor:
            var accessor = user.Identity as IIdentity; // placeholder
                                                       // Simpler approach: we’ll bind it in a delegating handler from HttpContext (see registration below).
            throw new NotImplementedException("This is filled by DelegatingHandler below; do not call directly.");
        }
    }
}
