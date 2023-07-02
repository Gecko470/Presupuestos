using Dapper;
using Microsoft.Data.SqlClient;
using Presupuestos.Models;
using System.Data.Common;

namespace Presupuestos.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Create(Cuenta cuenta);
        Task Delete(int id);
        Task<IEnumerable<Cuenta>> Get(int usuarioId);
        Task<Cuenta> GetById(int id, int usuarioId);
        Task Update(CuentaCreacionViewModel cuenta);
    }

    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default");
        }

        public async Task Create(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Cuentas (Nombre, TipoCuentaId, Balance, Descripcion) VALUES (@Nombre, @TipoCuentaId, @Balance, @Descripcion);
                                                            SELECT SCOPE_IDENTITY();", cuenta);

            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Get(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Cuenta>($@"SELECT C.Id, C.Nombre, C.Balance, TC.Nombre AS TipoCuenta FROM Cuentas C 
                                                            INNER JOIN TipoCuenta TC ON C.TipoCuentaId = TC.Id 
                                                            WHERE TC.UsuarioId = @UsuarioId ORDER BY Orden", new { usuarioId });
        }

        public async Task<Cuenta> GetById(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>($@"SELECT C.Id, C.Nombre, C.Balance, C.Descripcion, C.TipoCuentaId FROM Cuentas C 
                                                                    INNER JOIN TipoCuenta TC ON C.TipoCuentaId = TC.Id 
                                                                    WHERE C.Id = @id AND TC.UsuarioId = @UsuarioId", new { id, usuarioId });
        }

        public async Task Update(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync($@"UPDATE Cuentas SET Nombre = @Nombre, TipoCuentaId = @TipoCuentaId, Balance = @Balance, Descripcion = @Descripcion WHERE ID = @Id;", cuenta);
        }

        public async Task Delete(int id)
        {
            var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE FROM CUENTAS WHERE Id = @Id", new { id });
        }
    }
}
