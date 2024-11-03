using MakVas.TaskPlanner.Domain.Models;

namespace MakVas.TaskPlanner.Domain.Logic;

public class SimpleTaskPlanner
{
    public WorkItem[] CreatePlan(WorkItem[] items)
    {
        var itemsAsList = items.ToList();
        SortWorkItems(items);
        return itemsAsList.ToArray();
    }
    public WorkItem[] SortWorkItems(WorkItem[] workItems)
    {
        return workItems.OrderBy(w => w.DueDate).ToArray();
    }
}