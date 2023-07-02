using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presupuestos.Models
{
    public class CuentaCreacionViewModel: Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
