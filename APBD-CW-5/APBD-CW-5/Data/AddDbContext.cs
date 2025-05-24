using Microsoft.EntityFrameworkCore;

namespace APBD_CW_5.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}