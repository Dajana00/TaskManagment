using Microsoft.EntityFrameworkCore;
using Trello.Model;

namespace Trello.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }    
        public DbSet<Board> Boards { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
       // public DbSet<Column> Columns { get; set; }  
        public DbSet<Card> Cards { get; set; }
        public DbSet<Comment> Comments { get; set; }    
        public DbSet<Backlog> Backlogs { get; set; }
        public DbSet<UserStory> UserStories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProject>()
          .HasKey(up => new { up.UserId, up.ProjectId });



            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Kada se User obriše, briše se i veza

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId)
                .OnDelete(DeleteBehavior.NoAction); // Kada se Project obriše, briše se i veza

            modelBuilder.Entity<Project>()
               .HasOne(p => p.Board)  // Project ima jednu tablu
               .WithOne(b => b.Project) // Board pripada tačno jednom projektu
               .HasForeignKey<Board>(b => b.ProjectId) // Ključ se čuva u Board tabeli
               .OnDelete(DeleteBehavior.Cascade); // Ako se Project obriše, briše se i Board
            modelBuilder.Entity<Project>()
               .HasOne(p => p.Backlog)  // Project ima jednu tablu
               .WithOne(b => b.Project) // Backlog pripada tačno jednom projektu
               .HasForeignKey<Backlog>(b => b.ProjectId) // Ključ se čuva u backlog tabeli
               .OnDelete(DeleteBehavior.Cascade); // Ako se Project obriše, briše se i Board

            modelBuilder.Entity<Sprint>()
                .HasOne(s => s.Project)       
                .WithMany(p => p.Sprints)       
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserStory>()
                .HasOne(s => s.Backlog)
                .WithMany(p => p.UserStories)          // nadam se da je ok provejritiiti
                .HasForeignKey(s => s.BacklogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Board>()
                .HasOne(b => b.ActiveSprint) 
                .WithOne()                     
                .HasForeignKey<Board>(b => b.ActiveSprintId)
                .OnDelete(DeleteBehavior.NoAction); // ako se sprint obrise onda board ostaje bez aktivnog sprinta

         

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Sprint)   // pitanjee***********************************
                .WithMany()
                .HasForeignKey(c => c.SprintId)
                .OnDelete(DeleteBehavior.Cascade);  // DA LI JE OVO OKEJ

            modelBuilder.Entity<Card>()
                .HasMany(c => c.Comments)  
                .WithOne(com => com.Card)
                .HasForeignKey(com => com.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserCard>()
                .HasKey(uc => new { uc.UserId, uc.CardId });  // Composite ključ

            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCards)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);  //  brisanje korisnika -> brisanje vezze

            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.Card)
                .WithMany(c => c.UserCards)
                .HasForeignKey(uc => uc.CardId)
                .OnDelete(DeleteBehavior.Restrict);  // brisanje card -> brisanje veze

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.NoAction); // Postavlja AuthorId na NULL umesto da briše ceo komentar

        }
    }
}
