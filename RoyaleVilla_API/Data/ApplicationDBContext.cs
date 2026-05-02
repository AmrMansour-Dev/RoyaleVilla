using Microsoft.EntityFrameworkCore;

namespace RoyaleVilla_API.Data
{
    public class ApplicationDBContext(DbContextOptions options) : DbContext(options)
    {

    }
}
