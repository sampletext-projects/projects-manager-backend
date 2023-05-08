using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class ProjectCommentConfiguration : IEntityTypeConfiguration<ProjectComment>
{
    public void Configure(EntityTypeBuilder<ProjectComment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Author)
            .WithMany(x => x.ProjectComments)
            .HasForeignKey(x => x.AuthorId);
        
        builder.HasOne(x => x.Project)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ProjectId);
    }
}