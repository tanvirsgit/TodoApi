using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra.Mapping;

namespace TodoApiAssignment.Models
{
    public interface INoSqlDb
    {
        Task AddTodo( Todo todo);
        Task DeleteTodo(Todo todo);
        void initiateNoSql();
        Task PatchTodo(Todo tempTodo, Todo todo);
        Task UpdateTodo(Todo todo,Todo todo1);

        Task<IEnumerable<Todo>> GetAll(DateTime dateTime);
    }
}