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
      
         
        }
    }
}
