using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTrecker
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; } = ""; // ставим значения по умолчанию, чтобы при незаполнении этих строк не было null

        public string Status { get; set; } = "todo";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public TaskItem(string description)
        {
            Description = description;
            CreatedAt = DateTime.Now;
            UpdatedAt = CreatedAt;
        }
    }
}
