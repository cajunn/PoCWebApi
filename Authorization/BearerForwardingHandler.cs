namespace PoCWebApi.Authorization
{
    public sealed class BearerForwardingHandler(IHttpContextAccessor ctx) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = ctx.HttpContext?.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrWhiteSpace(auth))
                request.Headers.TryAddWithoutValidation("Authorization", auth);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
