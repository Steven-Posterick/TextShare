using Microsoft.EntityFrameworkCore;
using TextShare.API.Models.Entities;

namespace TextShare.API.Contexts;

public class TextShareContext : DbContext
{
    public TextShareContext(DbContextOptions<TextShareContext> options)
        : base(options)
    { }

    public DbSet<SharedText> SharedTexts { get; set; }
}