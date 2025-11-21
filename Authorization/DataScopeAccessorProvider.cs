namespace PoCWebApi.Authorization
{
    public sealed class DataScopeProvider(IDataScopeAccessor acc) : IDataScope
    {
        public bool HasScope => acc.Current.HasScope;
        public IReadOnlyDictionary<string, string[]> Items => acc.Current.Items;
    }
}
