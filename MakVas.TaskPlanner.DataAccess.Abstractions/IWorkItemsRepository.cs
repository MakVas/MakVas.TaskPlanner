
using MakVas.TaskPlanner.Domain.Models;

namespace MakVas.TaskPlanner.DataAccess.Abstractions;

public interface IWorkItemsRepository
{
    Guid Add(WorkItem workItem);
    WorkItem Get(Guid id);
    WorkItem[] GetAll();
    bool Update(WorkItem workItem);
    bool Remove(Guid id);
    void SaveChanges();
}