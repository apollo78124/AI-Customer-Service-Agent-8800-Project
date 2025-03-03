
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AI_Customer_Service_Lee_8900.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AICustomerServiceLee8900;Trusted_Connection=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversations> Conversations { get; set; }
        public DbSet<Credentials> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure model relationships (optional but recommended)
            modelBuilder.Entity<Conversations>()
                .HasOne(c => c.User)
                .WithMany(u => u.Conversations)
                .HasForeignKey(c => c.UserId);
        }

    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Navigation property
        public ICollection<Conversations> Conversations { get; set; }
    }

    public class Credentials
    {
        [Key]
        public int CredentialId { get; set; }
        public int UserId { get; set; }
        public string? Password { get; set; }
    }

    // Conversation model
    public class Conversations
    {
        [Key]
        public int ConversationId { get; set; }
        public string? Name { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        // Foreign key to User
        public int UserId { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
