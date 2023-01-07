 using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spotify_project.Model;
using Spotify_project.Options;
using System.Data.SqlClient;

namespace Spotify_project.Controllers
{
    [Route("api/albuns")]
    [ApiController]
    public class AlbumController : Controller
    {
        [HttpPost]
        [Route("")]

        public IActionResult PostAlbum([FromServices] IOptions<ConnectionStringOptions> options, [FromBody] Album album)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();
                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"insert into Album (Nome,Id,Email,CPF,DataNasc,DataCriacao) values (@Nome,@Id,@Email,@CPF,@DataNasc,@DataCriacao)";
                command.CommandType = System.Data.CommandType.Text;


                command.Parameters.Add(new SqlParameter("Nome", album.Nome));
                command.Parameters.Add(new SqlParameter("CPF", album.CPF));
                command.Parameters.Add(new SqlParameter("Email", album.Email));
                command.Parameters.Add(new SqlParameter("DataNasc", album.DataNasc));
                command.Parameters.Add(new SqlParameter("DataCriacao", album.DataCriacao));

                command.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpPut]
        [Route("{IdAlbum}")]

        public IActionResult PutAlbum([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int IdAlbum, [FromBody] Album album)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();
                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"UPDATE Album  SET CPF = @CPF,Nome = @Nome,Email = @Email,DataNasc = @DataNasc,DataCriacao = @DataCriacao where Id = @Id";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Nome", album.Nome));
                command.Parameters.Add(new SqlParameter("CPF", album.CPF));
                command.Parameters.Add(new SqlParameter("Email", album.Email));
                command.Parameters.Add(new SqlParameter("DataNasc", album.DataNasc));
                command.Parameters.Add(new SqlParameter("DataCriacao", album.DataCriacao));

                command.Parameters.Add(new SqlParameter("Id", IdAlbum));


                command.ExecuteNonQuery();


            }
            return Ok();

        }

        [HttpDelete]
        [Route("{IdAlbum}")]

        public IActionResult DeleteAlbum([FromServices] IOptions<ConnectionStringOptions> options, [FromRoute] int IdAlbum)
        {
            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"delete from Album where Id = @Id";

                command.Parameters.Add(new SqlParameter("Id", IdAlbum));

                command.ExecuteNonQuery();
            }

            return Ok();
        }


        [HttpGet]
        [Route("{IdAlbum}")]
        public IActionResult GetAlbum([FromServices] IOptions<ConnectionStringOptions> options, [FromQuery] int IdAlbum)
        {
            Album album = null;

            using (SqlConnection connection = new SqlConnection(options.Value.MyConnection))
            {
                connection.Open();

                SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = @"select * from Album where Id = @Id";
                command.CommandType = System.Data.CommandType.Text;

                command.Parameters.Add(new SqlParameter("Id", IdAlbum));

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        album = new Album()
                        {
                            CPF = dr.GetString(1),
                            DataCriacao = dr.GetDateTime(5),
                            DataNasc = dr.GetDateTime(2),
                            Email = dr.GetString(4),
                            Id = dr.GetInt32(3),
                            Nome = dr.GetString(0)
                        };
                    }
                }
            }
            return Ok(album);
        }

    }
}
