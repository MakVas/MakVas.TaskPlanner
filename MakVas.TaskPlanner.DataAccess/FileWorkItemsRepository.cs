using MakVas.TaskPlanner.DataAccess.Abstractions;
using MakVas.TaskPlanner.Domain.Models;

namespace MakVas.TaskPlanner.DataAccess;

using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

public class FileWorkItemsRepository : IWorkItemsRepository
{
    public WorkItem Get(Guid id)
    {
        if (workItemsDictionary.TryGetValue(id, out var workItem))
        {
            return workItem;
        }
        throw new KeyNotFoundException($"WorkItem with ID {id} not found.");
    }

    public WorkItem[] GetAll()
    {
        return workItemsDictionary.Values.ToArray();
    }

    public bool Update(WorkItem workItem)
    {
        if (workItemsDictionary.ContainsKey(workItem.Id))
        {
            workItemsDictionary[workItem.Id] = workItem;
            return true;
        }
        return false;
    }

    public bool Remove(Guid id)
    {
        return workItemsDictionary.Remove(id);
    }

    private const string FileName = "work-items.json"; // Назва файлу
    private readonly Dictionary<Guid, WorkItem> workItemsDictionary; // Словник для зберігання об'єктів WorkItem

    public FileWorkItemsRepository()
    {
        workItemsDictionary = new Dictionary<Guid, WorkItem>();

        // Перевірка на існування файлу
        if (File.Exists(FileName))
        {
            // Читання вмісту файлу
            string fileContent = File.ReadAllText(FileName);

            if (!string.IsNullOrWhiteSpace(fileContent)) // Перевірка на непорожній вміст
            {
                // Десеріалізація в масив об'єктів WorkItem
                WorkItem[] workItemsArray = JsonConvert.DeserializeObject<WorkItem[]>(fileContent) ?? Array.Empty<WorkItem>();

                // Конвертація масиву в словник
                foreach (var item in workItemsArray)
                {
                    workItemsDictionary[item.Id] = item;
                }
            }
            else
            {
                Console.WriteLine($"Файл {FileName} порожній. Ініціалізовано порожній словник.");
            }
        }
        else
        {
            Console.WriteLine($"Файл {FileName} не знайдено. Ініціалізовано порожній словник.");
        }
    }

    public Guid Add(WorkItem workItem)
    {
        // Create a copy of the workItem
        WorkItem workItemCopy = workItem.Clone();

        // Generate a new Guid and assign it to the copy
        Guid newId = Guid.NewGuid();
        workItemCopy.Id = newId;

        // Add the copy to the dictionary
        workItemsDictionary[newId] = workItemCopy;

        // Return the generated ID
        return newId;
    }
    
    public void SaveChanges()
    {
        // Convert the dictionary values to an array
        WorkItem[] workItemsArray = workItemsDictionary.Values.ToArray();

        // Serialize the array to JSON
        string json = JsonConvert.SerializeObject(workItemsArray, Formatting.Indented);

        // Write the JSON to the file, overwriting the existing content
        File.WriteAllText(FileName, json);
    }
    
    // Метод для отримання словника WorkItem
    public Dictionary<Guid, WorkItem> GetWorkItemsDictionary()
    {
        return workItemsDictionary;
    }
}
