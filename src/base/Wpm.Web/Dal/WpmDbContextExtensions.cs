namespace Wpm.Web.Dal;

public static class WpmDbContextExtensions
{
    public static void AddWpmDb(this IServiceCollection services)
    {
        services.AddDbContext<WpmDbContext>();
    }
    public static void EnsureWpmDbIsCreated(this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetService<WpmDbContext>()!;
        db.Database.EnsureCreated();
        db.Dispose();
    }
}