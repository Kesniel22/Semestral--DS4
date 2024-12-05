using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Http;
using System;
using TaskManager___Semestral.Models;

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
            try
            {
                List<TaskManager> tasks = new List<TaskManager>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "SELECT * FROM Tasks ORDER BY DueDate ASC";
                    SqlCommand command = new SqlCommand(sql, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tasks.Add(new TaskManager
                        {
                            TaskID = (int)reader["TaskID"],
                            Title = (string)reader["Title"],
                            Description = (string)reader["Description"],
                            DueDate = (DateTime)reader["DueDate"],
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            Status = (string)reader["Status"]
                        });
                    }
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Crear una nueva tarea
        [HttpPost]
        [Route("api/tasks")]
        public IHttpActionResult CreateTask([FromBody] TaskManager task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (task.Status != "Completado" && task.Status != "En Proceso" && task.Status != "Pendiente")
            {
                return BadRequest("Invalid status value.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO Tasks (Title, Description, DueDate, Status) VALUES (@Title, @Description, @DueDate, @Status)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.Parameters.AddWithValue("@DueDate", task.DueDate);
                    command.Parameters.AddWithValue("@Status", task.Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return Ok("Tarea creada correctamente");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Actualizar una tarea
        [HttpPut]
        [Route("api/tasks/{id}")]
        public IHttpActionResult UpdateTask(int id, [FromBody] TaskManager task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (task.Status != null && task.Status != "Completado" && task.Status != "En Proceso" && task.Status != "Pendiente")
            {
                return BadRequest("Valor de estado no válido.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "UPDATE Tasks SET ";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    bool hasUpdates = false;

                    if (!string.IsNullOrEmpty(task.Title))
                    {
                        sql += "Title = @Title, ";
                        parameters.Add(new SqlParameter("@Title", task.Title));
                        hasUpdates = true;
                    }
                    if (!string.IsNullOrEmpty(task.Description))
                    {
                        sql += "Description = @Description, ";
                        parameters.Add(new SqlParameter("@Description", task.Description));
                        hasUpdates = true;
                    }
                    if (task.DueDate != default(DateTime))
                    {
                        sql += "DueDate = @DueDate, ";
                        parameters.Add(new SqlParameter("@DueDate", task.DueDate));
                        hasUpdates = true;
                    }
                    if (!string.IsNullOrEmpty(task.Status))
                    {
                        sql += "Status = @Status, ";
                        parameters.Add(new SqlParameter("@Status", task.Status));
                        hasUpdates = true;
                    }

                    if (hasUpdates)
                    {
                        // Remove the trailing comma
                        sql = sql.TrimEnd(',', ' ');
                        sql += " WHERE TaskID = @TaskID";
                        parameters.Add(new SqlParameter("@TaskID", id));

                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddRange(parameters.ToArray());
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        // No fields to update
                        return Ok("No hay cambios para actualizar.");
                    }
                }
                return Ok("Tarea actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Eliminar una tarea
        [HttpDelete]
        [Route("api/tasks/{id}")]
        public IHttpActionResult DeleteTask(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Check if the task exists
                    string checkSql = "SELECT COUNT(*) FROM Tasks WHERE TaskID = @TaskID";
                    SqlCommand checkCommand = new SqlCommand(checkSql, connection);
                    checkCommand.Parameters.AddWithValue("@TaskID", id);
                    connection.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count == 0)
                    {
                        return NotFound();
                    }

                    // Proceed to delete
                    string deleteSql = "DELETE FROM Tasks WHERE TaskID = @TaskID";
                    SqlCommand deleteCommand = new SqlCommand(deleteSql, connection);
                    deleteCommand.Parameters.AddWithValue("@TaskID", id);
                    deleteCommand.ExecuteNonQuery();
                }
                return Ok("Tarea eliminada correctamente");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("api/tasks/{id}")]
        public IHttpActionResult GetTask(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Tasks WHERE TaskID = @TaskID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@TaskID", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TaskManager task = new TaskManager
                    {
                        TaskID = (int)reader["TaskID"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        DueDate = (DateTime)reader["DueDate"],
                        Status = (string)reader["Status"]
                    };
                    return Ok(task);
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}