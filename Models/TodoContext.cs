using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApiAssignment.Models
{
    public class TodoContext:DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> option) : base(option)
        {

        }

        public DbSet<Todo> Todos { get; set; }
    }
}
