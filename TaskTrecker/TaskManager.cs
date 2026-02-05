using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TaskTrecker
{
    public class TaskManager
    {
        private static string filePath = "tasks.json"; // имя файла, где будут храниться задачи
        public static int nextId = 1;

        public static List<TaskItem> LoadTasks()
        {
            if (!File.Exists(filePath)) // если не существует 
                return new List<TaskItem>();

            try
            {
                var json = File.ReadAllText(filePath); // читает как строку 
                var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new(); // преобразование в объект 

                if (tasks.Any())
                    nextId = tasks.Max(t => t.Id) + 1;

                return tasks;

            }
            catch (Exception ex)
            {
                
                    Console.WriteLine(ex.Message);
                    return new List<TaskItem>();
                


            }
        }

        public static void SaveTasks(List<TaskItem> tasks)
        {
            try
            {
                // преобразование в строку json (текст для записи в файл). добавляем красивый вид
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }


        }
    }
}
