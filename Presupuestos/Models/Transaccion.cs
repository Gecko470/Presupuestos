using System.ComponentModel.DataAnnotations;

namespace Presupuestos.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.Date)]
        public DateTime FechaTransaccion { get; set; } = DateTime.Today;
        public Decimal Cantidad { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una Categoría..")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        [StringLength(maximumLength: 500, ErrorMessage = "El campo {0} debe tener un máximo de {1} caracteres")]
        public string Nota { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar una Cuenta..")]
        [Display(Name = "Cuenta")]
        public int CuentaId { get; set; }
        [Display(Name = "Tipo Operación")]
        public TipoOperacion TipoOperacionId { get; set; } = TipoOperacion.Ingreso;
    }
}
