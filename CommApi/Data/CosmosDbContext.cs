using Microsoft.EntityFrameworkCore;
using CommApi.Models;

namespace CommApi.Data;

public class CosmosDbContext : DbContext, IDbContext
{

    protected readonly IConfiguration Configuration;

    public CosmosDbContext(DbContextOptions<CosmosDbContext> options, IConfiguration configuration)
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
        string Uri = Configuration["Database:CosmosDB:Uri"] ?? "";
        string Key = Configuration["Database:CosmosDB:Key"] ?? "";
        string databaseName = Configuration["Database:CosmosDB:DatabaseName"] ?? "";
        optionsBuilder.UseCosmos(Uri, Key, databaseName: databaseName);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // configuring Groups
        modelBuilder.Entity<Group>()
            .ToContainer("groups") // ToContainer
            .HasNoDiscriminator() // No other entity type will ever be stored in this container
            .UseETagConcurrency()
            .HasPartitionKey(c => c.Scope)
            .Property(e => e.Id).ToJsonProperty("id")
            .ValueGeneratedOnAdd(); // Partition Key

        // configuring Messages
        var messageBuilder = modelBuilder.Entity<Message>();
        messageBuilder.ToContainer("messages"); // ToContainer
        messageBuilder.HasNoDiscriminator(); // No other entity type will ever be stored in this container
        messageBuilder.UseETagConcurrency();
        messageBuilder.HasPartitionKey(c => c.GroupId); // Partition Key

        messageBuilder.Property(e => e.Id).ToJsonProperty("id").ValueGeneratedOnAdd();

        // modelBuilder.Entity<Message>().OwnsMany(m => m.Attachments);

        // messageBuilder.Property(e => e.Scope).ToJsonProperty("scope");
        // messageBuilder.Property(e => e.GroupId).ToJsonProperty("groupId");
        // messageBuilder.Property(e => e.Content).ToJsonProperty("content");
        // messageBuilder.Property(e => e.CreatedAt).ToJsonProperty("createdAt");
        // messageBuilder.Property(e => e.Origin).ToJsonProperty("origin");
        // messageBuilder.Property(e => e.Sender).ToJsonProperty("sender");
        // messageBuilder.Property(e => e.SenderEmail).ToJsonProperty("senderEmail");
    }

    public void SetMessageId(Message message)
    {
        message.Id = Guid.NewGuid().ToString();
    }
}