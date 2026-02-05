using TaskTrecker;

try
{
    // пользователь ничего не ввел 
    if (args.Length == 0)
    {
        Console.WriteLine("Используй: dotnet run list (all/done/todo/in-progress) or add \"описание задачи\" or update/delete Id or mark-in-progress/mark-done Id");
        return;
    }

    var tasks = TaskManager.LoadTasks(); // загружаем список задач
    string command = args[0].ToLower(); // берет первую команду (т.е. 1, что идет после dotnet run) и приводит к нижнему регистру 

    switch (command)
    {
        case "list":

            // проверяем, есть ли после list условие
            string filter = args.Length > 1 ? args[1].ToLower() : "all";
            if (filter != "all" && filter != "done" && filter != "todo" && filter != "in-progress")
            {
                Console.WriteLine("Используй: list done/in-progress/todo.");
                return;
            }
            
                // Объявляем переменную, содержащую коллекция типа TaskItem
                IEnumerable<TaskItem> filtered = filter switch // свич-выражение 
                {
                    "done" => tasks.Where(t => t.Status == "done"),
                    "in-progress" => tasks.Where(t => t.Status == "in-progress"),
                    "todo" => tasks.Where(t => t.Status == "todo"),
                    "all" => tasks,
                };
            
          
            if (!filtered.Any())
            {
                Console.WriteLine("Задач не найдено.");
                return;
            }
            else
            {
                foreach (var foundtask in filtered)
                {
                    Console.WriteLine($"{foundtask.Id} {foundtask.Description} {foundtask.Status}");
                }                
            }

        break;


            
           


        case "add":
            if (args.Length < 2)
            {
                Console.WriteLine("Используй: add \"описание задачи\".");
                return;
            }

            var newTask = new TaskItem(args[1]); // в конструтор передаем текст задачи 
            newTask.Id = TaskManager.nextId++;
            tasks.Add(newTask);
            TaskManager.SaveTasks(tasks);
            Console.WriteLine($"Задача добавлена успешно. Id = {newTask.Id}.");
            break;

        case "update":
            if (args.Length < 3)
            {
                Console.WriteLine("Используй: update Id \"описание задачи\".");
                return;
            }
            if (!int.TryParse(args[1], out int id))
            {
                Console.WriteLine("Id введен некорректно.");
                return;
            }
            // поиск задачи по id
            var taskToUpd = tasks.FirstOrDefault(t => t.Id == id); // для каждой задачи t найти t.Id и сравнить с id из запроса 
            if (taskToUpd == null)
            {
                Console.WriteLine("Задача не найдена.");
                return;
            }
            taskToUpd.Description = args[2];
            taskToUpd.UpdatedAt = DateTime.Now;
            TaskManager.SaveTasks(tasks);
            break;

        case "delete":
            if (args.Length < 2)
            {
                Console.WriteLine("Используй: delete Id.");
                return;
            }
            if (!int.TryParse(args[1], out id))
            {
                Console.WriteLine("Id введен некорректно.");
                return;
            }
            var task = tasks.FirstOrDefault(task => task.Id == id);
            if (task == null )
            {
                Console.WriteLine("Задача с таким Id не найдена.");
                return;
            }
            else
                tasks.Remove(task);
            TaskManager.SaveTasks(tasks);
            Console.WriteLine("Задача успешно удалена.");
            break;

        case "mark-in-progress":
        case "mark-done":
            if (args.Length < 2)
            {
                Console.WriteLine("Используй: mark-in-progress/mark-done Id.");
                return;
            }
            if (!int.TryParse(args[1],out id))
            {
                Console.WriteLine("Id введен некорректно.");
                return;
            }
            var taskToMark = tasks.FirstOrDefault(task=> task.Id == id);
            if (taskToMark == null)
            {
                Console.WriteLine("Задача с таким Id не найдена.");
                return;
            }
            else
                taskToMark.Status = command == "mark-in-progress" ? "in-progress" : "done";
            TaskManager.SaveTasks(tasks);
            Console.WriteLine("Статус задачи успешно изменен.");
            break;

        default:
            Console.WriteLine("Неизвестная команда.");
            break;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}



