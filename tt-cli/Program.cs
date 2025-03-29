using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TaskTracker
{
    public enum TaskStatus
    {
        NotDone,
        InProgress,
        Done
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.NotDone;

        public override string ToString()
        {
            return $"ID: {Id}, Description: {Description}, Status: {Status}";
        }
    }

    class Program
    {
        private const string taskspath = "tasks.json";

        static int Main(string[] args)
        {
            var tasks = LoadTasks();

            if (args.Length == 0)
            {
                ShowUsage();
                return 1;
            }

            string command = args[0].ToLower();

            try
            {
                switch (command)
                {
                    case "add":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Error: 'add' requires a task description.");
                            return 1;
                        }
                        AddTask(tasks, args[1]);
                        break;
                    case "update":
                        if (args.Length < 3)
                        {
                            Console.WriteLine("Error: 'update' requires a task ID and new description.");
                            return 1;
                        }
                        UpdateTask(tasks, args[1], args[2]);
                        break;

                    case "delete":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Error: 'delete' requires a task ID.");
                            return 1;
                        }
                        DeleteTask(tasks, args[1]);
                        break;

                    case "list":

                        ShowTasks(tasks, args);
                        break;

                    default:
                        Console.WriteLine("Unknown command, please try again");
                        ShowUsage();
                        return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception has occured: {ex}");
                return 1;
            }

            SaveTasks(tasks);
            return 0;
        }

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  add \"Task description\"");
            Console.WriteLine("  update <taskId> \"New description\"");
            Console.WriteLine("  delete <taskId>");
            Console.WriteLine("  list [--done|--inprogress]");
        }
        #region "CRUD"

        static void AddTask(List<TaskItem> tasks, string description)
        {
            int newId = tasks.Count > 0 ? tasks[^1].Id + 1 : 1;
            var newTask = new TaskItem
            {
                Id = newId,
                Description = description,
                Status = TaskStatus.NotDone
            };

            tasks.Add(newTask);
            Console.WriteLine("Added task: " + newTask);
        }

        static void UpdateTask(List<TaskItem> tasks, string idStr, string newDescription)
        {
            if (!int.TryParse(idStr, out int id))
            {
                Console.WriteLine("Invalid task ID.");
                return;
            }
            var task = tasks.Find(t => t.Id == id);

            if (task == null)
            {
                Console.WriteLine("Task with ID " + id + " not found.");
                return;
            }
            task.Description = newDescription;
            Console.WriteLine("Updated task: " + task);
        }
        static void DeleteTask(List<TaskItem> tasks, string idStr)
        {
            if (!int.TryParse(idStr, out int id))
            {
                Console.WriteLine("Invalid task ID.");
                return;
            }
            var task = tasks.Find(t => t.Id == id);

            if (task == null)
            {
                Console.WriteLine("Task with ID " + id + " not found.");
                return;
            }

            tasks.Remove(task);
            Console.WriteLine("Deleted task: " + task);
        }
        static void ShowTasks(List<TaskItem> tasks, string[] args)
        {
            bool filterDone = false;
            bool filterInProgress = false;

            //"list --done" or "list --inprogress"
            if (args.Length > 1)
            {
                if (args[1].Equals("--done", StringComparison.OrdinalIgnoreCase))
                    filterDone = true;
                else if (args[1].Equals("--inprogress", StringComparison.OrdinalIgnoreCase))
                    filterInProgress = true;
            }

            Console.WriteLine("Tasks:");
            foreach (var task in tasks)
            {
                if (filterDone && task.Status != TaskStatus.Done)
                    continue;
                if (filterInProgress && task.Status != TaskStatus.InProgress)
                    continue;

                Console.WriteLine(task);
            }
        }

        #endregion
        #region Load /Save
        static List<TaskItem> LoadTasks()
        {
            if (!File.Exists(taskspath))
            {
                File.WriteAllText(taskspath, "[]");
                return new List<TaskItem>();
            }

            string json = File.ReadAllText(taskspath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        static void SaveTasks(List<TaskItem> tasks)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(taskspath, json);
        }
        #endregion
    }
}