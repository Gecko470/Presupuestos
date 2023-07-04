using Dapper;
using Microsoft.Data.SqlClient;
using Presupuestos.Models;

namespace Presupuestos.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Create(Categoria categoria);
        Task Delete(int id);
        Task<IEnumerable<Categoria>> Get(int usuarioId);
        Task<IEnumerable<Categoria>> Get(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> GetById(int id, int usuarioId);
        Task Update(Categoria categoria);
    }


    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default");
        }

        public async Task Create(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);

            categoria.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO Categorias (Nombre, UsuarioId, TipoOperacionId) VALUES (@Nombre, @UsuarioId, @TipoOperacionId);
                                                                  SELECT SCOPE_IDENTITY();", categoria);
        }

        public async Task<IEnumerable<Categoria>> Get(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>($@"SELECT * FROM Categorias where UsuarioId = @usuarioId", new { usuarioId });
        }

        public async Task<IEnumerable<Categoria>> Get(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>($@"SELECT * FROM Categorias where UsuarioId = @usuarioId and TipoOperacionId = @tipoOperacionId", new { usuarioId, tipoOperacionId });
        }

        public async Task<Categoria> GetById(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>($@"SELECT * FROM Categorias where UsuarioId = @usuarioId AND Id = @id", new { id, usuarioId });
        }

        public async Task Update(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync($@"UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId WHERE UsuarioId = @usuarioId AND Id = @id", categoria);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync($@"DELETE CATEGORIAS WHERE Id = @id", new { id });
        }
    }
}
