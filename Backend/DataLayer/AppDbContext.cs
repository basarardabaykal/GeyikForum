using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CoreLayer.Entities;

namespace DataLayer;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostVote> PostVotes { get; set; }
    public DbSet<PreviousNickname> PreviousNicknames { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure AppUser
        builder.Entity<AppUser>(entity =>
        {
            entity.ToTable("AspNetUsers");
            entity.Property(e => e.Nickname).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Nickname).IsUnique();
            entity.Property(e => e.Karma).HasDefaultValue(0);
        });
        
        // Configure Post
        builder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(300);
            entity.Property(e => e.VoteScore).HasDefaultValue(0);
            entity.Property(e => e.CommentCount).HasDefaultValue(0);
            entity.Property(e => e.Depth).HasDefaultValue(0);
            
            // Self-referencing relationship for comments
            entity.HasOne(p => p.Parent)
                  .WithMany(p => p.Children)
                  .HasForeignKey(p => p.ParentId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            // Relationship with User
            entity.HasOne(p => p.User)
                  .WithMany(u => u.Posts)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            // Indexes for performance
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ParentId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.VoteScore);
        });
        
        // Configure PostVote
        builder.Entity<PostVote>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Composite unique constraint - one vote per user per post
            entity.HasIndex(e => new { e.UserId, e.PostId }).IsUnique();
            
            // Relationships
            entity.HasOne(pv => pv.User)
                  .WithMany(u => u.Votes)
                  .HasForeignKey(pv => pv.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(pv => pv.Post)
                  .WithMany(p => p.Votes)
                  .HasForeignKey(pv => pv.PostId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configure PreviousNickname
        builder.Entity<PreviousNickname>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nickname).IsRequired().HasMaxLength(50);
            
            entity.HasOne(pnn => pnn.User)
                  .WithMany(u => u.PreviousNicknames)
                  .HasForeignKey(pnn => pnn.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasIndex(e => e.UserId);
        });
        
        // Configure automatic timestamps
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(t => typeof(IBaseEntity).IsAssignableFrom(t.ClrType)))
        {
            builder.Entity(entityType.ClrType)
                   .Property<DateTime>("CreatedAt")
                   .HasDefaultValueSql("GETUTCDATE()");
                   
            builder.Entity(entityType.ClrType)
                   .Property<DateTime>("UpdatedAt")
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is IBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            
        foreach (var entity in entities)
        {
            var baseEntity = (IBaseEntity)entity.Entity;
            
            if (entity.State == EntityState.Added)
            {
                baseEntity.CreatedAt = DateTime.UtcNow;
            }
            
            baseEntity.UpdatedAt = DateTime.UtcNow;
        }
    }
}