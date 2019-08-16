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
                entity.Property(x => x.TimeStamp).IsRequired();
                entity.Property(x => x.message).IsRequired();
            });
        }
    }
}
