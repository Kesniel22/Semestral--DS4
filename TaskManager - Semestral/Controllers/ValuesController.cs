using System.Collections.Generic;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;

namespace TaskManager___Semestral.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["TaskManagerConnect"].ConnectionString;

        // Obtener todas las tareas
        [HttpGet]
        [Route("api/tasks")]
        public IHttpActionResult GetAllTasks()
        {
            List<object> tasks = new List<object>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetAllTasks", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new
                    {
                        Id = reader["TaskId"],
                        Name = reader["TaskName"],
                        Description = reader["Description"],
                        IsCompleted = reader["IsCompleted"]
                    });
                }
            }
            return Ok(tasks);
        }

        // Crear una nueva tarea
        [HttpPost]
        [Route("api/tasks")]
        public IHttpActionResult CreateTask([FromBody] dynamic task)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("InsertTask", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@TaskName", (string)task.TaskName);
                command.Parameters.AddWithValue("@Description", (string)task.Description);
                command.Parameters.AddWithValue("@IsCompleted", (bool)task.IsCompleted);
                connection.Open();
                command.ExecuteNonQuery();
            }
            return Ok("Tarea creada correctamente");
        }

        // Actualizar una tarea
        [HttpPut]
        [Route("api/tasks/{id}")]
        public IHttpActionResult UpdateTask([FromBody] dynamic task)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UpdateTask", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@TaskName", (string)task.TaskName);
                command.Parameters.AddWithValue("@Description", (string)task.Description);
                command.Parameters.AddWithValue("@IsCompleted", (bool)task.IsCompleted);
                connection.Open();
                command.ExecuteNonQuery();
            }
            return Ok("Tarea actualizada correctamente");
        }

        // Eliminar una tarea
        [HttpDelete]
        [Route("api/tasks/{id}")]
        public IHttpActionResult DeleteTask(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DeleteTask", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@TaskId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
            return Ok("Tarea eliminada correctamente");
        }
    }
}
