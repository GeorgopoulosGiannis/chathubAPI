using chathubAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.DATA
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ChatMessage> Μessages { get; set; }

        public DbSet<Relationship> Relationships { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<ImageComment> ImageComments { get; set; }
        public DbSet<LikedImage> LikedImages { get; set; }
        public DbSet<FcmToken> FcmTokens { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("Messages");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.from).IsRequired();
                entity.Property(x => x.to).IsRequired();
                entity.Property(x => x.timeStamp).IsRequired();
                entity.Property(x => x.message).IsRequired();
            });

            builder.Entity<Relationship>(entity =>
            {
                entity.HasKey(x => new { x.User_OneId, x.User_TwoId });

                entity.HasOne(x => x.User_One)
                .WithMany()
                .HasForeignKey(x => x.User_OneId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.User_Two)
                .WithMany()
                .HasForeignKey(x => x.User_TwoId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Image>(entity =>
            {
                entity.ToTable("Images");
                entity.HasOne(x => x.Profile)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProfileId)
                .IsRequired();

                entity.Property(x => x.Path)
                .IsRequired();
            });

            builder.Entity<LikedImage>(entity =>
            {
                entity.HasKey(x => new { x.ImageId, x.LikedById });

                entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.LikedById)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(x => x.ImageId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<FcmToken>(entity =>
            {
                entity.HasKey(x => x.Token);
                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.TokenOwner)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<ImageComment>(entity =>
            {
                entity.HasKey(x => new { x.CommentId, x.ImageId });

                entity.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(x => x.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Comment)
                .WithMany()
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
