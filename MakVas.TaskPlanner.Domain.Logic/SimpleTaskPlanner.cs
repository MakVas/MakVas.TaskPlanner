using MakVas.TaskPlanner.DataAccess.Abstractions;
using MakVas.TaskPlanner.Domain.Models;

namespace MakVas.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository;
        }

        public WorkItem[] CreatePlan()
        {
            // Get the list of tasks from the repository
            var items = _repository.GetAll();
            
            // Filter out completed tasks
            var incompleteItems = items.Where(item => !item.IsCompleted).ToList();
            
            return SortWorkItems(incompleteItems.ToArray());
        }

        public WorkItem[] SortWorkItems(WorkItem[] workItems)
        {
            return workItems.OrderBy(w => w.DueDate).ToArray();
        }
    }
}