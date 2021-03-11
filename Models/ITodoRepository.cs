using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace TodoApiAssignment.Models
{
    public interface ITodoRepository
    {
        Task<Todo> GetTodo(int id);
        Task<IEnumerable<Todo>> GetAllTodos();
        Task<int> AddTodo(Todo todo);
        Task<Todo> UpdateTodo(int id,Todo todo);
        Task<Todo> DeleteTodo(int id);
        Task<Todo> PatchTodo(int id, Todo patchTodo);
        Task<IEnumerable<Todo>> GetCassandraAll(DateTime dateTime);
    }
}
