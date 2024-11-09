using System.Globalization;
using MakVas.TaskPlanner.Domain.Models.Enums;

namespace MakVas.TaskPlanner.Domain.Models;

public class WorkItem
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public Complexity Complexity { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        string priorityString = Priority.ToString().ToLower();
        string dueDateString = DueDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        return $"ID: {Id}, name {Title}: due {dueDateString}, {priorityString} priority";
    }
    
    public WorkItem Clone()
    {
        return new WorkItem
        {
            Id = this.Id,
            CreationDate = this.CreationDate,
            DueDate = this.DueDate,
            Priority = this.Priority,
            Complexity = this.Complexity,
            Title = this.Title,
            Description = this.Description,
            IsCompleted = this.IsCompleted
        };
    }
}