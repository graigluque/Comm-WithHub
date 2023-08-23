using Microsoft.EntityFrameworkCore;
using CommApi.Models;

namespace CommApi.Data;

public interface IDbContext {
    
    public DbSet<Group>? Groups { get; set; }
    public DbSet<Message>? Messages { get; set; }

    public bool EnsureCreated();

    public Task<int> SaveChangesAsync();

    public void SetModifyState(object obj);

    public void SetMessageId(Message message);
}