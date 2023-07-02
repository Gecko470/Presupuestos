using Presupuestos.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        [Capitalize]
        public string Nombre { get; set; }
        [Display(Name = "Tipo Cuenta")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Descripcion { get; set; }
        public string TipoCuenta { get; set; }
    }
}
