using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spotify_project.Model;
using Spotify_project.Options;
using System.Data.SqlClient;

namespace Spotify_project.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : Controller
    {
        [HttpPost]
        [Route("")]

        public IActionResult PostUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Usuario usuarios)
        {
            using (SqlConnection Connection = new SqlConnection(options.Value.MyConnection))
            {
                Connection.Open();

                SqlCommand command = new();
                command.Connection = Connection;
                command.CommandText = @"insert into Usuario (CPF,Nome,Email,DataNasc,DataCriacao) values (@CPF,@Nome,@Email,@DataNasc,@DataCriacao)";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Nome", usuarios.NomeUsuario));
                command.Parameters.Add(new SqlParameter("CPF", usuarios.CPF));
                command.Parameters.Add(new SqlParameter("Email", usuarios.Email));
                command.Parameters.Add(new SqlParameter("DataNasc", usuarios.DataNasc));
                command.Parameters.Add(new SqlParameter("DataCriacao", usuarios.DataCriacao));

                int IdadeAtual = Convert.ToInt32(DateTime.Today.Subtract(usuarios.DataNasc).TotalDays / 365);

                command.ExecuteNonQuery();

            }

            return Ok();
        }

        [HttpPut]
        [Route("{IdUsuario}")]

        public IActionResult PutUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int IdUsuario, [FromBody] Usuario usuarios)
        {
            using (SqlConnection Connection = new SqlConnection(options.Value.MyConnection))
            {
                Connection.Open();

                SqlCommand command = new();
                command.Connection = Connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"UPDATE Usuario SET CPF = @CPF,Nome = @Nome,Email = @Email,DataNasc = @DataNasc,DataCriacao = @DataCriacao where Id = @Id";

                command.Parameters.Add(new SqlParameter("Nome", usuarios.NomeUsuario));
                command.Parameters.Add(new SqlParameter("CPF", usuarios.CPF));
                command.Parameters.Add(new SqlParameter("Email", usuarios.Email));
                command.Parameters.Add(new SqlParameter("DataNasc", usuarios.DataNasc));
                command.Parameters.Add(new SqlParameter("DataCriacao", usuarios.DataCriacao));

                command.Parameters.Add(new SqlParameter("Id", IdUsuario));


                command.ExecuteNonQuery();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{IdUsuario}")]

        public IActionResult DeleteUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int IdUsuario)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"delete from Usuario where Id = @Id";

                command.Parameters.Add(new SqlParameter("Id", IdUsuario));

                command.ExecuteNonQuery();
            }

            return Ok();
        }


        [HttpGet]
        [Route("{idUsuario}")]
        public IActionResult GetUsuario([FromServices] IOptions<ConnectionStringOptions> options, [FromQuery] int idUsuario)
        {
            Usuario usuario = null;

            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"select * from Usuario where Id = @Id";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Id", idUsuario));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usuario = new Usuario()
                        {
                            CPF = dr.GetString(1),
                            DataCriacao = dr.GetDateTime(5),
                            DataNasc = dr.GetDateTime(2),
                            Email = dr.GetString(4),
                            Id = dr.GetInt32(3),
                            NomeUsuario = dr.GetString(0)
                        };
                    }
                }
            }
            return Ok(usuario);
        }


        private bool VerificarUsuarioExistente(IOptions<ConnectionStringOptions> options, string CPF, string Email)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                Usuario usuario = new Usuario();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"select Id from Usuario where CPF = @CPF and Email = @Email";

                command.Parameters.Add(new SqlParameter("Email", Email));
                command.Parameters.Add(new SqlParameter("CPF", CPF));

                int? id = (int?)command.ExecuteScalar();


                return id != null;

            }
        }
    }
}

