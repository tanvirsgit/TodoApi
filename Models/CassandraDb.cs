using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;

namespace TodoApiAssignment.Models
{
    public class CassandraDb : INoSqlDb
    {
        private Cluster cluster;
        private ISession session;
        private IMapper mapper;

        public CassandraDb()
        {
            initiateNoSql();
        }

        public void initiateNoSql()
        {
            cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();

            

            session = cluster.Connect();
            session.Execute("CREATE KEYSPACE IF NOT EXISTS todoapi WITH replication = { 'class': 'SimpleStrategy', 'replication_factor': '1' }");
            session.Execute("USE todoapi");
            session.Execute("CREATE TABLE IF NOT EXISTS todos(id int, tasktitle text, datetime timestamp, PRIMARY KEY(datetime,tasktitle,id))");


            MappingConfiguration.Global.Define(
               new Map<Todo>()
               .TableName("todos")
               .PartitionKey(u => u.Id)
               .Column(u => u.Id, cm => cm.WithName("id"))
            );

            mapper = new Mapper(session);

          
        }

        public async Task AddTodo(Todo todo)
        {
            await mapper.InsertAsync(todo);
        }

        public async Task UpdateTodo(Todo tempTodo,Todo todo)
        {
            todo.Id = tempTodo.Id;
            await DeleteTodo(tempTodo);
            await AddTodo(todo);
        }

        public async Task PatchTodo(Todo tempTodo, Todo todo)
        {
            todo.Id = tempTodo.Id;
            todo.TaskTitle = tempTodo.TaskTitle;
            await DeleteTodo(tempTodo);
            await AddTodo(todo);
        }

        public async Task DeleteTodo(Todo todo)
        {
            await mapper.DeleteAsync<Todo>("WHERE datetime=? AND tasktitle=? AND id=?", todo.DateTime, todo.TaskTitle, todo.Id);
        }

        public async Task<IEnumerable<Todo>> GetAll(DateTime dateTime)
        {
            var todos = await mapper.FetchAsync<Todo>("WHERE datetime=?", dateTime);

            return todos;
        }

    }
}
