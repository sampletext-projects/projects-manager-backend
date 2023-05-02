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

        builder.HasOne(x => x.Creator)
            .WithMany(x => x.CreatedProjects)
            .HasForeignKey(x => x.CreatorId);
    }
}