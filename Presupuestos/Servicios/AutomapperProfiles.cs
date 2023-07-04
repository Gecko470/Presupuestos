using AutoMapper;
using Presupuestos.Models;

namespace Presupuestos.Servicios
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
            CreateMap<TransaccionEditViewModel, Transaccion>().ReverseMap();
        }
    }
}
