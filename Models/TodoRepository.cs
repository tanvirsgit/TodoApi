using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch.Exceptions;

namespace TodoApiAssignment.Models
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        private readonly INoSqlDb _noSqlDb;
        public TodoRepository(TodoContext context,INoSqlDb noSqlDb)
        {
            _context = context;
            _noSqlDb = noSqlDb;
        }
        public async Task<int> AddTodo(Todo todo)
        {
            _context.Todos.Add(todo);
            var temp= await SaveChange();
            if (temp >= 0)
            {
                await _noSqlDb.AddTodo(todo);
            }
            return temp;

        }

        
        public async Task<Todo> DeleteTodo(int id)
        {
            var tempTodo = await _context.Todos.FindAsync(id);
            if (tempTodo == null)
                return new Todo { Id = 0 };

            _context.Todos.Remove(tempTodo);
            await _noSqlDb.DeleteTodo(tempTodo);
            await SaveChange();
            return tempTodo;
        }

        public async Task<IEnumerable<Todo>> GetAllTodos()
        {
           return await _context.Todos.ToListAsync();
        }

        public async Task<Todo> GetTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return new Todo { Id = 0 };
            return todo;
        }

        public async Task<Todo> UpdateTodo(int id, Todo todo)
        {
            var tempTodo = await _context.Todos.FindAsync(id);
            if (tempTodo == null)
                return new Todo { Id = 0 };
            await _noSqlDb.UpdateTodo(tempTodo, todo);
            tempTodo.DateTime = todo.DateTime;
            tempTodo.TaskTitle = todo.TaskTitle;
            await SaveChange();
            
            
            return tempTodo;
        }

        public async Task<Todo> PatchTodo(int id, Todo todo)
        {
            var tempTodo = await _context.Todos.FindAsync(id);
            if (tempTodo == null)
                return new Todo { Id = 0 };
            await _noSqlDb.PatchTodo(tempTodo, todo);
            tempTodo.DateTime = todo.DateTime;
            await SaveChange();


            return tempTodo;

        }

        private async Task<int> SaveChange()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException d)
            {
                return -1;
            }
        }

        public async Task<IEnumerable<Todo>> GetCassandraAll(DateTime dateTime)
        {
            var todos = await _noSqlDb.GetAll(dateTime);
            return todos;
        }
    }
}
