using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spotify_project.Model;
using Spotify_project.Options;
using System.Data.SqlClient;

namespace Spotify_project.Controllers
{
    [Route("api/playlist")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        [HttpPost]
        [Route("")]

        public IActionResult CreatePlaylist([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Playlist playlist)
        {

            bool playListExistente = VerificarPlaylistExistente(options, playlist.Nome);

            if (playListExistente == true)
            {
                return BadRequest("Uma playlist não pode ter o nome de outra playlist existente para um mesmo usuário.");
            }

            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"insert into Usuario (Nome,DataCriacao) values (@Nome,@DataCriacao)";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Nome", playlist.Nome));
                command.Parameters.Add(new SqlParameter("DataCriacao", playlist.DataCriacao));

                command.ExecuteNonQuery();
            }
            return Ok();
        }
        [HttpPut]
        [Route("")]

        public IActionResult UpdatePlaylist([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Playlist playlist)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"insert into Usuario (NomeMusic,DataCriacao) values (@NomeMusic,@DataCriacao)";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("NomeMusic", playlist.Nome));
                command.Parameters.Add(new SqlParameter("DataCriacao", playlist.DataCriacao));

                command.ExecuteNonQuery();
            }
            return Ok();
        }
        [HttpDelete]
        [Route("{IdPlayList}")]

        public IActionResult DeletePlaylist([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Playlist playlist, [FromRoute] int PlayList)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"delete from playlists where Id = @Id ";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("NomeMusic", playlist.Nome));
                command.Parameters.Add(new SqlParameter("DataCriacao", playlist.DataCriacao));

                command.ExecuteNonQuery();


            }
            return Ok();
        }

        [HttpGet]
        [Route("{idPlayList}")]
        public IActionResult GetPlaylist([FromServices] IOptions<ConnectionStringOptions> options, [FromQuery] int idPlayList)
        {
            Playlist playlists = null;

            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"select * from PlayList where Id = @Id";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Id", idPlayList));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        playlists = new Playlist()
                        {
                            DataCriacao = dr.GetDateTime(4),
                            Nome = dr.GetString(0)
                        };
                    }
                }

                return Ok(playlists);
            }
        }

        private bool VerificarPlaylistExistente(IOptions<ConnectionStringOptions> options, string nomePlayList)
        {

            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();
                SqlCommand command = new();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"select Nome from Playlist where IdUsuario = @IdUsuario";

                command.Parameters.Add(new SqlParameter("Nome", nomePlayList));

                int? id = (int?)command.ExecuteScalar();

                return id != null;

            }
        }

    }

}

