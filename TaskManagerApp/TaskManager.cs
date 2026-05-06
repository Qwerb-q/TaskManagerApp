using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class TaskManager
{
    public List<Task> Tasks { get; private set; }
    private const string FileName = "tasks.txt";

    public TaskManager()
    {
        Tasks = new List<Task>();
        LoadTasks();
    }

    public void AddTask(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Описание задачи не может быть пустым.");
        }
        Tasks.Add(new Task(description));
        SaveTasks();
    }

    public void RemoveTask(int index)
    {
        if (index < 0 || index >= Tasks.Count)
        {
            throw new IndexOutOfRangeException("Некорректный индекс задачи.");
        }
        Tasks.RemoveAt(index);
        SaveTasks();
    }

    public void ToggleTaskCompletion(int index)
    {
        if (index < 0 || index >= Tasks.Count)
        {
            throw new IndexOutOfRangeException("Некорректный индекс задачи.");
        }
        Tasks[index].IsCompleted = !Tasks[index].IsCompleted;
        SaveTasks();
    }

    private void SaveTasks()
    {
        var lines = Tasks.Select(t => $"{t.IsCompleted}\t{t.Description}");
        File.WriteAllLines(FileName, lines);
    }

    private void LoadTasks()
    {
        if (File.Exists(FileName))
        {
            try
            {
                var lines = File.ReadAllLines(FileName);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(new[] { '\t' }, 2, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        if (bool.TryParse(parts[0], out bool isCompleted))
                        {
                            Tasks.Add(new Task(parts[1]) { IsCompleted = isCompleted });
                        }
                    }
                }
            }
            catch (Exception)
            {
                Tasks = new List<Task>();
            }
        }
    }
}