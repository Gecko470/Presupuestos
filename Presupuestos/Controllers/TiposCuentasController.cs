using Microsoft.AspNetCore.Mvc;
using Presupuestos.Models;
using Presupuestos.Servicios;
using System.Reflection.Metadata.Ecma335;

namespace Presupuestos.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IUsuariosService usuariosService;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IUsuariosService usuariosService)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.usuariosService = usuariosService;
        }

        public async Task<ActionResult> Index()
        {
            int usuarioId = usuariosService.GetUsuarioId();
            IEnumerable<TipoCuenta> listaTiposCuentas = await repositorioTiposCuentas.GetTiposCuenta(usuarioId);
            return View(listaTiposCuentas);
        }
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = usuariosService.GetUsuarioId();

            var yaExiste = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExiste)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe en la BD..");
                return View(tipoCuenta);
            }
            else
            {
                await repositorioTiposCuentas.Crear(tipoCuenta);
                return RedirectToAction("Index");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            TipoCuenta tipoCuenta = await repositorioTiposCuentas.TipoCuentaGet(id, usuarioId);

            if(tipoCuenta is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            tipoCuenta.UsuarioId = usuariosService.GetUsuarioId();
            var existeTipoCuenta = await repositorioTiposCuentas.TipoCuentaGet(tipoCuenta.Id, tipoCuenta.UsuarioId);

            if(existeTipoCuenta is null)
            {
                return RedirectToAction("Index", "Home");
            }
            await repositorioTiposCuentas.TipoCuentaUpdate(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            TipoCuenta tipoCuenta = await repositorioTiposCuentas.TipoCuentaGet(id, usuarioId);

            if(tipoCuenta is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            TipoCuenta tipoCuenta = await repositorioTiposCuentas.TipoCuentaGet(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            await repositorioTiposCuentas.TipoCuentaDelete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            int usuarioId = usuariosService.GetUsuarioId();

            var existe = await repositorioTiposCuentas.Existe(nombre, usuarioId);

            if (existe)
            {
                return Json($"El nombre {nombre} ya existe en la tabla TipoCuenta..");
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> OrdenarTabla([FromBody] int[] ids)
        {
            return Ok();
        }
    }
}
