using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Author)
            .WithMany(x => x.TaskComments)
            .HasForeignKey(x => x.AuthorId);
        
        builder.HasOne(x => x.Task)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.TaskId);
    }
}