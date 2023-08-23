using Microsoft.EntityFrameworkCore;
using CommApi.Models;

namespace CommApi.Data;

public class SqlServerDbContext : DbContext, IDbContext
{

    protected readonly IConfiguration Configuration;

    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options, IConfiguration configuration)
        : base(options)
    {
        Configuration = configuration;
    }

    public DbSet<Group>? Groups { get; set; }
    public DbSet<Message>? Messages { get; set; }

    public bool EnsureCreated()
    {
        return base.Database.EnsureCreated();
    }

    public void SetModifyState(object obj)
    {
        base.Entry(obj).State = EntityState.Modified;
    }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration["Database:SQLServerDB:ConnectionString"]);
    }

    public void SetMessageId(Message message)
    {
        message.Id = Guid.NewGuid().ToString();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>().HasData(new Group
        {
            Id = "1001",
            GroupName = "Group 1001",
            CreatedAt = DateTime.Now,
            UpdatedAt = null,
            Scope = "demo-chat",
            MessagesCount = 0,
            CreatedBy = "user1"
        }, new Group
        {
            Id = "1002",
            GroupName = "Group 1002",
            CreatedAt = DateTime.Now,
            Scope = "demo-chat",
            MessagesCount = 0,
            CreatedBy = "user2"
        });

        // var messageBuilder = modelBuilder.Entity<Message>();
        // messageBuilder.Property(e => e.Id)
        //         .ValueGeneratedOnAdd()
        //         .UseIdentityColumn();

        modelBuilder.Entity<Message>().HasData(new Message
        {
            Id = Guid.NewGuid().ToString(),
            Content = "Hello! This is a sample message.",
            CreatedAt = DateTime.Now,
            Scope = "demo-chat",
            GroupId = "1001",
            Sender = "user1",
            SenderEmail = "user1@deloitte.com",
            Origin = "portal1"
        });


    }

}