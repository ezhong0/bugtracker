using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using MySql.Data.EntityFrameworkCore;
//using System.Data.Entity;


namespace BugTracker.Models
{
    public partial class Database : DbContext
    {
        public Database() { }
        public Database(DbContextOptions<Database> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<History> Historys { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Media> Medias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=bugtrackerdb;user=root;password=Bl@ckpink");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.UserId).HasColumnName("userid");
                entity.Property(e => e.RoleId).HasColumnName("roleid");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(80)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(80)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasMaxLength(80)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("ticket");

                entity.Property(e => e.TicketId).HasColumnName("ticketid");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.PriorityId).HasColumnName("priorityid");
                entity.Property(e => e.StatusId).HasColumnName("statusid");
                entity.Property(e => e.TypeId).HasColumnName("typeid");

                entity.Property(e => e.DateModified)
                    .HasColumnName("datemodified")
                    .HasColumnType("date");
                entity.Property(e => e.DateCreated)
                    .HasColumnName("datemodified")
                    .HasColumnType("date");

                entity.HasOne(d => d.UserCreated)
                    .WithMany(p => p.CreateTicket)
                    .HasForeignKey(d => d.UserCreatedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ticket_userCreated");

                entity.HasOne(d => d.UserAssigned)
                    .WithMany(p => p.AssignTicket)
                    .HasForeignKey(d => d.UserAssignedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ticket_userAssigned");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ticket_project");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project");

                entity.Property(e => e.ProjectId).HasColumnName("projectid");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DateModified)
                    .HasColumnName("datemodified")
                    .HasColumnType("date");

                entity.HasOne(d => d.UserCreated)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_project");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("history");

                entity.Property(e => e.HistoryId).HasColumnName("historyid");
                entity.Property(e => e.Attribute)
                   .HasColumnName("attribute")
                   .HasMaxLength(80)
                   .IsUnicode(false);
                entity.Property(e => e.OldValue)
                   .HasColumnName("oldvalue")
                   .HasMaxLength(80)
                   .IsUnicode(false);
                entity.Property(e => e.NewValue)
                   .HasColumnName("newvalue")
                   .HasMaxLength(80)
                   .IsUnicode(false);
                entity.Property(e => e.UserId).HasColumnName("userid");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.History)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ticket_history");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.CommentId).HasColumnName("commentid");
                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("text");
                entity.Property(e => e.DateCreated)
                    .HasColumnName("datecreated")
                    .HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("userid");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ticket_comment");
            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.ToTable("media");

                entity.Property(e => e.MediaId).HasColumnName("mediaid");
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(80)
                    .IsUnicode(false);
                entity.Property(e => e.Data)
                    .HasColumnName("data"); //review

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Media)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ticket_media");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public partial class User
    {
        public User()
        {
            AssignTicket = new HashSet<Ticket>();
            CreateTicket = new HashSet<Ticket>();
            Project = new HashSet<Project>();
        }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; } // will need to change

        public virtual ICollection<Ticket> AssignTicket { get; set; }
        public virtual ICollection<Ticket> CreateTicket { get; set; }
        public virtual ICollection<Project> Project { get; set; }

    }

    public partial class Ticket
    {
        public Ticket()
        {
            History = new HashSet<History>();
            Comment = new HashSet<Comment>();
            Media = new HashSet<Media>();
        }

        public int TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public int UserCreatedId { get; set; }
        public int UserAssignedId { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual User UserCreated { get; set; }
        public virtual User UserAssigned { get; set; }

        public virtual ICollection<History> History { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Media> Media { get; set; }

    }

    public partial class Project
    {
        public Project()
        {
            Ticket = new HashSet<Ticket>();
        }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateModified { get; set; }

        public int UserId { get; set; }

        public virtual User UserCreated { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
    }

    public partial class History
    {
        public int HistoryId { get; set; }
        public string Attribute { get; set; }
        public string OldValue { get; set; } //incl. uid
        public string NewValue { get; set; }
        public DateTime DateModified { get; set; }

        public int UserId { get; set; }
        public int TicketId { get; set; }
        //public virtual User User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }

    public partial class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }

        public int UserId { get; set; }
        public int TicketId { get; set; }
        //public virtual User User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }

    public partial class Media
    {
        public int MediaId { get; set; }
        public string Title { get; set; }
        public Byte[] Data { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
