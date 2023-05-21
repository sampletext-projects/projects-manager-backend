namespace DataAccess.Models;

public enum TaskStatus : uint
{
    Unknown = 0,
    Created = 1,
    InProgress = 2,
    Testing = 3,
    Done = 4
}