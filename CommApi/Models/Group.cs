using System.ComponentModel.DataAnnotations;

namespace CommApi.Models;

public class Group
{
    [Key]
    public string? Id { get; set; }

    [Required]
    public string GroupName { get; set; } = "";

    [Required]
    public string Scope { get; set; } = "";

    [Required]
    public int MessagesCount { get; set; }

    [Required]
    public string CreatedBy { get; set; } = "";

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}