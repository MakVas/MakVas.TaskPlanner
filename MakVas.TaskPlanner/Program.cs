using MakVas.TaskPlanner.Domain.Logic;
using MakVas.TaskPlanner.Domain.Models;
using MakVas.TaskPlanner.Domain.Models.Enums;

internal static class Program
{
    public static void Main(string[] args)
    {
        List<WorkItem> workItems = new List<WorkItem>();

        while (true)
        {
            Console.WriteLine("Enter WorkItem details (or type 'exit' to finish):");

            Console.Write("Title: ");
            string title = Console.ReadLine();
            if (title.ToLower() == "exit") break;

            Console.Write("Description: ");
            string description = Console.ReadLine();

            Console.Write("Creation Date (dd.MM.yyyy): ");
            DateTime creationDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Due Date (dd.MM.yyyy): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Priority (Low, Medium, High): ");
            Priority priority = Enum.Parse<Priority>(Console.ReadLine(), true);

            Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
            Complexity complexity = Enum.Parse<Complexity>(Console.ReadLine(), true);

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

            workItems.Add(workItem);
        }

        SimpleTaskPlanner planner = new SimpleTaskPlanner();
        WorkItem[] sortedWorkItems = planner.SortWorkItems(workItems.ToArray());

        Console.WriteLine("Sorted WorkItems:");
        foreach (var item in sortedWorkItems)
        {
            Console.WriteLine(item);
        }
    }
}