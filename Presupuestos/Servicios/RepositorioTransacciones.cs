using Dapper;
using Microsoft.Data.SqlClient;
using Presupuestos.Models;
using System.Transactions;

namespace Presupuestos.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Create(Transaccion transaccion);
        Task Delete(int id);
        Task<Transaccion> GetById(int id, int usuarioId);
        Task Update(Transaccion transaccion, decimal cantidadAnterior, int cuentaAnteriorId);
    }


    public class RepositorioTransacciones : IRepositorioTransacciones
    {
        private readonly String connectionString;
        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default");
        }

        public async Task Create(Transaccion transaccion)
        {
            using var connection = new SqlConnection(connectionString);
            transaccion.Id = await connection.QuerySingleAsync<int>($@"Transaccion_Insert", new
            {
                transaccion.UsuarioId,
                transaccion.FechaTransaccion,
                transaccion.Cantidad,
                transaccion.CategoriaId,
                transaccion.Nota,
                transaccion.CuentaId

            }, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task Update(Transaccion transaccion, decimal cantidadAnterior, int cuentaAnteriorId)
        {
            using var connection = new SqlConnection(connectionString);
            transaccion.Id = await connection.ExecuteAsync($@"Transaccion_Update", new
            {
                transaccion.Id,
                transaccion.FechaTransaccion,
                transaccion.Cantidad,
                cantidadAnterior,
                transaccion.CategoriaId,
                transaccion.Nota,
                transaccion.CuentaId,
                cuentaAnteriorId

            }, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<Transaccion> GetById(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaccion>($@"SELECT T.*, C.TipoOperacionId 
                                                                          FROM Transacciones T
                                                                          INNER JOIN Categorias C 
                                                                          ON C.Id = T.CategoriaId
                                                                          WHERE T.Id = @id AND T.UsuarioId = @usuarioId", new { id, usuarioId });
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("Transaccion_Delete", new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
