namespace Presupuestos.Models
{
    public class TransaccionEditViewModel : TransaccionCreacionViewModel
    {
        public int CuentaAnteriorId { get; set; }
        public decimal CantidadAnterior { get; set; }
    }
}
