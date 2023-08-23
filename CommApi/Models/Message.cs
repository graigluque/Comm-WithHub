using System.ComponentModel.DataAnnotations;

namespace CommApi.Models;

public class Message
{
    [Key]
    public string? Id { get; set; }

    [Required]
    public string GroupId { get; set; } = "";

    [Required]
    public string Scope { get; set; } = "";

    [Required]
    public string Origin { get; set; } = "";

    [Required, MaxLength(400)]
    public string Content { get; set; } = "";

    [Required]
    public string Sender { get; set; } = "";

    [Required]
    public string SenderEmail { get; set; } = "";

    public DateTime? CreatedAt { get; set; }
}

// [DatabaseGenerated(DatabaseGeneratedOption.Identity)]