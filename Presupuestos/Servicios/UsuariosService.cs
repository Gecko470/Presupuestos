namespace Presupuestos.Servicios
{
    public interface IUsuariosService
    {
        int GetUsuarioId();
    }
    public class UsuariosService : IUsuariosService
    {
        public int GetUsuarioId()
        {
            return 1;
        }
    }
}
