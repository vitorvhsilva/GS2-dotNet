using API.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data.ConnectionPool;

public class PooledDbContextFactory : IDbContextFactory<ApplicationDbContext>
{
    private readonly IServiceProvider _serviceProvider;

    public PooledDbContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
