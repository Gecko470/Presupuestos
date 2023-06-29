using Microsoft.AspNetCore.Mvc;
using Presupuestos.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.Models
{
    public class TipoCuenta /*: IValidatableObject implementamos esta interfaz para hacer validaciones a nivel del modelo, a todos los campos o a alguno en particular*/
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(maximumLength: 50, MinimumLength = 5,  ErrorMessage = "El campo {0} debe tener una longitud entre {2} y {1} caracteres")]
        [Capitalize] //Validación por atributo, creamos una validación por atributo en carpeta Validaciones para un campo en particular y colocamos el atributo de validación al campo que queramos
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TiposCuentas")]
        [Display(Name = "Nombre Tipo Cuenta")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Nombre != null && Nombre.Length > 0)
        //    {
        //        string primeraLetra = Nombre.ToString()[0].ToString();

        //        if(primeraLetra != primeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayúscula", new[] {nameof(Nombre)}); 
        //        }
        //    }
        //}

        //[Required]
        //[EmailAddress(ErrorMessage = "El campo {0} debe ser un correo electrónico válido")]
        //public string Email { get; set; }
        //[Required]
        //[Range(minimum:18, maximum:110, ErrorMessage = "Debes tener una {0} entre {1} y {2}")]
        //public int Edad { get; set; }
        //[Required]
        //[Url(ErrorMessage = "El campo url debe ser una url válida")]
        //public string Url { get; set; }
        //[Required]
        //[CreditCard(ErrorMessage = "El campo {0} debe ser una tarjeta de crédito válida")]
        //[Display(Name = "Tarjeta de crédito")]
        //public string TarjetaCredito { get; set; }
    }
}
