using System.Security.Claims;

namespace PoCWebApi.Authorization
{
    public record EvaluateRequest(string Action, string? Resource = null);
    public record EvaluateResult(bool Allow, IDictionary<string, string[]>? Scope = null);

    public interface IPdpClient
    {
        Task<EvaluateResult> EvaluateAsync(ClaimsPrincipal user, string action, string? resource = null, CancellationToken ct = default);
        Task<string[]> GetFeatureFlagsAsync(ClaimsPrincipal user, CancellationToken ct = default);
    }

    public interface IDataScope
    {
        bool HasScope { get; }
        IReadOnlyDictionary<string, string[]> Items { get; }
    }

    public sealed class DataScope : IDataScope
    {
        public bool HasScope => Items.Count > 0;
        public IReadOnlyDictionary<string, string[]> Items { get; init; } = new Dictionary<string, string[]>();
    }

    public interface IDataScopeAccessor
    {
        DataScope Current { get; set; }
    }
    public sealed class DataScopeAccessor : IDataScopeAccessor
    {
        public DataScope Current { get; set; } = new();
    }
}
