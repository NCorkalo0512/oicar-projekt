using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FriendFinderAPI.Model;

public partial class FriendFinderDbContext : DbContext
{
    public FriendFinderDbContext()
    {
    }

    public FriendFinderDbContext(DbContextOptions<FriendFinderDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DeletedMessage> DeletedMessages { get; set; }

    public virtual DbSet<Matching> Matchings { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageReport> MessageReports { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeletedMessage>(entity =>
        {
            entity.HasKey(e => e.IddeletedMessage).HasName("PK__DeletedM__5D8807BACE377880");

            entity.ToTable("DeletedMessage");

            entity.Property(e => e.IddeletedMessage).HasColumnName("IDDeletedMessage");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.MessageId).HasColumnName("MessageID");

            entity.HasOne(d => d.Message).WithMany(p => p.DeletedMessages)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__DeletedMe__Messa__70DDC3D8");
        });

        modelBuilder.Entity<Matching>(entity =>
        {
            entity.HasKey(e => e.Idmatching).HasName("PK__Matching__049DB3257ECAF63B");

            entity.ToTable("Matching");

            entity.HasIndex(e => new { e.UserId1, e.UserId2 }, "UniqueConnection").IsUnique();

            entity.Property(e => e.Idmatching).HasColumnName("IDMatching");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.UserId1).HasColumnName("UserID1");
            entity.Property(e => e.UserId2).HasColumnName("UserID2");

            entity.HasOne(d => d.UserId1Navigation).WithMany(p => p.MatchingUserId1Navigations)
                .HasForeignKey(d => d.UserId1)
                .HasConstraintName("FK__Matching__UserID__656C112C");

            entity.HasOne(d => d.UserId2Navigation).WithMany(p => p.MatchingUserId2Navigations)
                .HasForeignKey(d => d.UserId2)
                .HasConstraintName("FK__Matching__UserID__66603565");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Idmessage).HasName("PK__Message__195595EC430F9EEF");

            entity.ToTable("Message");

            entity.Property(e => e.Idmessage).HasColumnName("IDMessage");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.SeenDateTime).HasColumnType("datetime");
            entity.Property(e => e.Text)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserReceiverId).HasColumnName("UserReceiverID");
            entity.Property(e => e.UserSenderId).HasColumnName("UserSenderID");

            entity.HasOne(d => d.UserReceiver).WithMany(p => p.MessageUserReceivers)
                .HasForeignKey(d => d.UserReceiverId)
                .HasConstraintName("FK__Message__UserRec__6E01572D");

            entity.HasOne(d => d.UserSender).WithMany(p => p.MessageUserSenders)
                .HasForeignKey(d => d.UserSenderId)
                .HasConstraintName("FK__Message__UserSen__6D0D32F4");
        });

        modelBuilder.Entity<MessageReport>(entity =>
        {
            entity.HasKey(e => e.IdmessageReport).HasName("PK__MessageR__3D9BEF92F11798D7");

            entity.ToTable("MessageReport");

            entity.Property(e => e.IdmessageReport).HasColumnName("IDMessageReport");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Explanation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserSenderId).HasColumnName("UserSenderID");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageReports)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__MessageRe__Messa__73BA3083");

            entity.HasOne(d => d.UserSender).WithMany(p => p.MessageReports)
                .HasForeignKey(d => d.UserSenderId)
                .HasConstraintName("FK__MessageRe__UserS__74AE54BC");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Idnotifications).HasName("PK__Notifica__03667DBED7E1310F");

            entity.Property(e => e.Idnotifications).HasColumnName("IDNotifications");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.SeenDateTime).HasColumnType("datetime");
            entity.Property(e => e.UserReciverId).HasColumnName("UserReciverID");
            entity.Property(e => e.UserSenderId).HasColumnName("UserSenderID");

            entity.HasOne(d => d.UserReciver).WithMany(p => p.NotificationUserRecivers)
                .HasForeignKey(d => d.UserReciverId)
                .HasConstraintName("FK__Notificat__UserR__693CA210");

            entity.HasOne(d => d.UserSender).WithMany(p => p.NotificationUserSenders)
                .HasForeignKey(d => d.UserSenderId)
                .HasConstraintName("FK__Notificat__UserS__6A30C649");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Idprofile).HasName("PK__Profile__0968DE382B7D88A8");

            entity.ToTable("Profile");

            entity.Property(e => e.Idprofile).HasColumnName("IDProfile");
            entity.Property(e => e.ProjectDescription).HasColumnType("text");
            entity.Property(e => e.Technology).HasColumnType("text");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Profile__UserID__05D8E0BE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DFF7DD656E");

            entity.ToTable("User");

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.CreateDateTime).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .HasConstraintName("FK__User__UserTypeID__5EBF139D");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.IduserType).HasName("PK__UserType__EA4074F260A36DBF");

            entity.ToTable("UserType");

            entity.Property(e => e.IduserType).HasColumnName("IDUserType");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
