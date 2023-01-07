using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spotify_project.Model;
using Spotify_project.Options;
using System.Data.SqlClient;

namespace Spotify_project.Controllers
{
    [Route("api/musicas")]
    [ApiController]
    public class MusicasController : Controller
    {
        [HttpPost]
        [Route("")]

        public IActionResult PostMusicas([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Musicas musicas)
        {
            using (SqlConnection Connection = new SqlConnection(options.Value.MyConnection))
            {
                Connection.Open();

                SqlCommand command = new();
                command.Connection = Connection;
                command.CommandText = @"insert into Musicas (Nome,TempoDeDuracao,DataDeLancamento) values (@Nome,@TempoDeDuracao,@DataDeLancamento) ";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Nome", musicas.Nome));
                command.Parameters.Add(new SqlParameter("TempoDeDuracao", musicas.TempoDeDuracao));
                command.Parameters.Add(new SqlParameter("DataDeLancamento", musicas.DataDeLancamento));


                command.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpPut]
        [Route("{IdMusica}")]

        public IActionResult PutMusicas([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Musicas musicas, [FromRoute] int IdMusica)
        {
            using (SqlConnection Connection = new SqlConnection(options.Value.MyConnection))
            {
                Connection.Open();

                SqlCommand command = new();
                command.Connection = Connection;
                command.CommandText = @"UPDATE Musicas SET Nome = @Nome,TempoDeDuracao = @TempoDeDuracao, DataDeLancamento = @DataDeLancamento where Id = @Id";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Nome", musicas.Nome));
                command.Parameters.Add(new SqlParameter("TempoDeDuracao", musicas.TempoDeDuracao));
                command.Parameters.Add(new SqlParameter("DataDeLancamento", musicas.DataDeLancamento));

                command.Parameters.Add(new SqlParameter("Id", IdMusica));

                command.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpGet]
        [Route("{idMusica}")]
        public IActionResult GetPlaylist([FromServices] IOptions<ConnectionStringOptions> options, [FromQuery] int idMusica)
        {
            Musicas musicas = null;

            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"select * from Musica where Id = @Id";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Id", idMusica));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        musicas = new Musicas()
                        {
                            DataDeLancamento = dr.GetDateTime(1),
                            Nome = dr.GetString(0),
                            TempoDeDuracao = dr.GetTimeSpan(4)
                        };
                    }
                }

                return Ok(musicas);
            }
        }
    }
}
