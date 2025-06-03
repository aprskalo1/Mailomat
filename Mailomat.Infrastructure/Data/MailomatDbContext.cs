using Microsoft.EntityFrameworkCore;

namespace Mailomat.Infrastructure.Data;

public class MailomatDbContext(DbContextOptions<MailomatDbContext> options) : DbContext(options)
{
}