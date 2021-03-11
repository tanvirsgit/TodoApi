using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApiAssignment.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace TodoApiAssignment.Controllers
{
    [ApiController]
    [Route("api/todoes")]
    public class TodoesController : ControllerBase
    {
        private readonly ITodoRepository _demorep;

        public TodoesController(ITodoRepository _repo)
        {
            _demorep = _repo;
        }

        // Get API api/todoes

        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodo()
        {
            var temp = await _demorep.GetAllTodos();
            if (temp == null)
                return NotFound();
            return Ok(temp);
        }

        // Get API /api/todoes?datetime=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAll([FromQuery]string datetime)
        {
            if (datetime == null)
                return await GetAllTodo();
            DateTime date;
            try
            {
                date = DateTime.Parse(datetime);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            var tempTodo = await _demorep.GetCassandraAll(date);
            if (tempTodo == null)
                return NotFound();
            return Ok(tempTodo);
        }

        //Get Api api/todoes/id

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var tempTodo = await _demorep.GetTodo(id);
            if (tempTodo.Id == 0)
                return NotFound("No Todo with id " + id + " found");
            return Ok(tempTodo);
        }

        //POST API api/todoes

        [HttpPost]
        public async Task<ActionResult<Todo>> AddTodo(Todo todo)
        {
            if (!ModelState.IsValid)
                return ValidationProblem();
            var createdTodo = await _demorep.AddTodo(todo);
            if (createdTodo < 0)
                return BadRequest("Input formate wrong\nTry without ID field");
            return Ok(todo);
        }

        //DELETE API api/todoes/id

        [HttpDelete("{id}")]
        public async Task<ActionResult<Todo>> DeleteTodo(int id)
        {
            var todoToDelete = await _demorep.DeleteTodo(id);
            if (todoToDelete.Id == 0)
                return NotFound("NO todo with id " + id + " found");
            return Ok(todoToDelete);
        }

        // UPDATE API api/todoes/id

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, Todo todo)
        {
            if (!ModelState.IsValid)
                return ValidationProblem();
            var tempTodo = await _demorep.UpdateTodo(id, todo);
            if (tempTodo.Id == 0)
                return NotFound("No todo with id " + id);
            return tempTodo;
        }

        //PATCH API api/todoes/id

        [HttpPatch("{id}")]
        public async Task<ActionResult<Todo>> PatchTodo(int id, Todo patchTodo)
        {
            var tempTodo = await _demorep.PatchTodo(id, patchTodo);
            if (tempTodo.Id == 0)
                return NotFound();

            if (!TryValidateModel(tempTodo))
                return ValidationProblem();

            return Ok(tempTodo);
        }

        
    }
}
