using System.ComponentModel.DataAnnotations;

namespace Presupuestos.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} debe tener un máximo de {1} caracteres..")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        [Display(Name = "Tipo Operación")]
        public TipoOperacion TipoOperacionId { get; set; }
    }
}
