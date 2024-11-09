using System.Globalization;
using MakVas.TaskPlanner.DataAccess;
using MakVas.TaskPlanner.Domain.Logic;
using MakVas.TaskPlanner.Domain.Models;
using MakVas.TaskPlanner.Domain.Models.Enums;

internal static class Program
{
    public static void Main(string[] args)
    {
        FileWorkItemsRepository repository = new FileWorkItemsRepository();
        SimpleTaskPlanner planner = new SimpleTaskPlanner(repository);

        while (true)
        {
            Console.WriteLine("Choose an operation: [A]dd, [B]uild a plan, [M]ark as completed, [R]emove, [Q]uit");
            string operation = Console.ReadLine().ToUpper();

            switch (operation)
            {
                case "A":
                    AddWorkItem(repository);
                    break;
                case "B":
                    BuildPlan(planner);
                    break;
                case "M":
                    MarkAsCompleted(repository);
                    break;
                case "R":
                    RemoveWorkItem(repository);
                    break;
                case "Q":
                    repository.SaveChanges();
                    return;
                default:
                    Console.WriteLine("Invalid operation. Please try again.");
                    break;
            }
        }
    }

    private static void AddWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Description: ");
        string description = Console.ReadLine();

        DateTime creationDate;
        while (true)
        {
            Console.Write("Creation Date (dd.MM.yyyy): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out creationDate))
            {
                break;
            }

            Console.WriteLine("Invalid date format. Please try again.");
        }

        DateTime dueDate;
        while (true)
        {
            Console.Write("Due Date (dd.MM.yyyy): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dueDate))
            {
                break;
            }

            Console.WriteLine("Invalid date format. Please try again.");
        }

        Priority priority;
        while (true)
        {
            Console.Write("Priority (Low, Medium, High): ");
            if (Enum.TryParse(Console.ReadLine(), true, out priority) && Enum.IsDefined(typeof(Priority), priority))
            {
                break;
            }

            Console.WriteLine("Invalid priority. Please enter Low, Medium, or High.");
        }

        Complexity complexity;
        while (true)
        {
            Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
            if (Enum.TryParse(Console.ReadLine(), true, out complexity) &&
                Enum.IsDefined(typeof(Complexity), complexity))
            {
                break;
            }

            Console.WriteLine("Invalid complexity. Please enter None, Minutes, Hours, Days, or Weeks.");
        }

        WorkItem workItem = new WorkItem
        {
            Title = title,
            Description = description,
            CreationDate = creationDate,
            DueDate = dueDate,
            Priority = priority,
            Complexity = complexity,
            IsCompleted = false
        };

        repository.Add(workItem);
        repository.SaveChanges();
        Console.WriteLine("Work item added successfully.");
    }

    private static void BuildPlan(SimpleTaskPlanner planner)
    {
        WorkItem[] sortedWorkItems = planner.CreatePlan();

        Console.WriteLine("Sorted WorkItems:");
        foreach (var item in sortedWorkItems)
        {
            Console.WriteLine(item);
        }
    }

    private static void MarkAsCompleted(FileWorkItemsRepository repository)
    {
        Console.Write("Enter the ID of the work item to mark as completed: ");
        Guid id = Guid.Parse(Console.ReadLine());

        WorkItem workItem = repository.Get(id);
        if (workItem != null)
        {
            workItem.IsCompleted = true;
            repository.Update(workItem);
            repository.SaveChanges();
            Console.WriteLine("Work item marked as completed.");
        }
        else
        {
            Console.WriteLine("Work item not found.");
        }
    }

    private static void RemoveWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Enter the ID of the work item to remove: ");
        Guid id = Guid.Parse(Console.ReadLine());

        if (repository.Remove(id))
        {
            repository.SaveChanges();
            Console.WriteLine("Work item removed successfully.");
        }
        else
        {
            Console.WriteLine("Work item not found.");
        }
    }
}