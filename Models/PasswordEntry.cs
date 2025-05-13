using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.Models
{
    public class PasswordEntry
    {
        public int Id { get; set; }

        [Required]
        public string ServiceName { get; set; } = string.Empty;  // ex: Gmail

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string EncryptedPassword { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Association à l'utilisateur connecté
        public string UserId { get; set; } = string.Empty;
    }
}
