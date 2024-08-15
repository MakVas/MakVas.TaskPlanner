using System.Globalization;
using MakVas.TaskPlanner.Domain.Models.Enums;

namespace MakVas.TaskPlanner.Domain.Models;

public class WorkItem
{
    private DateTime CreationDate;
    private DateTime DueDate;
    private Priority Priority;
    private Complexity Complexity;
    private string Title;
    private string Description;
    private bool IsCompleted;

    public override string ToString()
    {
        string priorityString = Priority.ToString().ToLower();
        string dueDateString = DueDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        return $"{Title}: due {dueDateString}, {priorityString} priority";
    }
}