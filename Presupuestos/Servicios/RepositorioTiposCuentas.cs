using Dapper;
using Microsoft.Data.SqlClient;
using Presupuestos.Models;

namespace Presupuestos.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> GetTiposCuenta(int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tiposCuentas);
        Task TipoCuentaDelete(int id);
        Task<TipoCuenta> TipoCuentaGet(int Id, int UsuarioId);
        Task TipoCuentaUpdate(TipoCuenta tipoCuenta);
    }

    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string ConnectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("Default");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(ConnectionString);

            var id = await connection.QuerySingleAsync<int>("TiposCuentaInsertar", new { Nombre= tipoCuenta.Nombre, UsuarioId = tipoCuenta.UsuarioId }, commandType: System.Data.CommandType.StoredProcedure);

            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(ConnectionString);

            var existe = await connection.QueryFirstOrDefaultAsync<int>($@"SELECT 1 FROM TipoCuenta where Nombre=@nombre AND UsuarioId=@usuarioId", new { nombre, usuarioId });

            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> GetTiposCuenta(int usuarioId)
        {
            using var connection = new SqlConnection(ConnectionString);

            IEnumerable<TipoCuenta> listaTiposCuentas = await connection.QueryAsync<TipoCuenta>($@"SELECT * FROM TipoCuenta WHERE UsuarioId = @usuarioId ORDER BY Orden", new { usuarioId });

            return listaTiposCuentas;
        }

        public async Task TipoCuentaUpdate(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(ConnectionString);

            await connection.ExecuteAsync($@"UPDATE TipoCuenta SET Nombre = @Nombre WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> TipoCuentaGet(int Id, int UsuarioId)
        {
            using var connection = new SqlConnection(ConnectionString);

            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@$"SELECT ID, Nombre, Orden FROM TipoCuenta WHERE UsuarioId = @UsuarioId AND Id = @Id", new { Id, UsuarioId });
        }

        public async Task TipoCuentaDelete(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync($@"DELETE TIPOCUENTA WHERE ID = @id", new { id });
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tiposCuentas)
        {
            string query = "UPDATE TipoCuenta SET Orden = @orden where Id = @id;";
            var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync(query, tiposCuentas);
        }
    }
}
