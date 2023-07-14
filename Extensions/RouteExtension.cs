namespace virtual_web_socket.Extensions
{
    public static class RouteExtension
    {
        public static IEndpointConventionBuilder CustomRoute<T>(
            this IEndpointRouteBuilder endpoints, string pattern)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<T>()
                .Build();

            return endpoints.Map(pattern, pipeline);
        }
    }
}
