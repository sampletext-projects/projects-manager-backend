using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .IsRequired(true);

        builder.Property(x => x.Description)
            .IsRequired(false);

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Projects)
            .UsingEntity<Participation>(
                arg => arg.HasOne(r => r.User)
                    .WithMany(item => item.Participations),
                arg => arg.HasOne(r => r.Project)
                    .WithMany(r => r.Participations),
                obj => obj.HasKey(r => new {r.UserId, r.ProjectId})
            );
    }
}