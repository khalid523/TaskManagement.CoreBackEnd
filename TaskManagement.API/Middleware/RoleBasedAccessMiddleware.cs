namespace TaskManagement.API.Middleware;

public class RoleBasedAccessMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RoleBasedAccessMiddleware> _logger;

    public RoleBasedAccessMiddleware(RequestDelegate next, ILogger<RoleBasedAccessMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract user role from header or token
        var role = context.Request.Headers["X-User-Role"].ToString();
        var userId = context.Request.Headers["X-User-Id"].ToString();

        if (!string.IsNullOrEmpty(role))
        {
            context.Items["UserRole"] = role;
            context.Items["UserId"] = int.TryParse(userId, out var id) ? id : 0;
        }

        await _next(context);
    }
}