using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presupuestos.Models;
using Presupuestos.Servicios;
using System.Reflection;

namespace Presupuestos.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IUsuariosService usuariosService;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IMapper mapper;

        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IUsuariosService usuariosService, IRepositorioCuentas repositorioCuentas, IMapper mapper)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.usuariosService = usuariosService;
            this.repositorioCuentas = repositorioCuentas;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var listaCuentas = await repositorioCuentas.Get(usuarioId);
            var modelo = listaCuentas.GroupBy(x => x.TipoCuenta).Select(grupo => new CuentaIndexViewModel
            {
                TipoCuenta = grupo.Key,
                Cuentas = grupo.AsEnumerable()
            }).ToList();

            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var usuarioId = usuariosService.GetUsuarioId();

            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = await TiposCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.TipoCuentaGet(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await TiposCuentas(usuarioId);
                return View(cuenta);
            }

            await repositorioCuentas.Create(cuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var cuenta = await repositorioCuentas.GetById(id, usuarioId);

            if (cuenta == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var modelo = mapper.Map<CuentaCreacionViewModel>(cuenta);

            modelo.TiposCuentas = await TiposCuentas(usuarioId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var cuentaBD = await repositorioCuentas.GetById(cuenta.Id, usuarioId);

            if (cuentaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var tipoCuenta = await repositorioTiposCuentas.TipoCuentaGet(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            await repositorioCuentas.Update(cuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var cuenta = await repositorioCuentas.GetById(id, usuarioId);

            if (cuenta == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var cuenta = await repositorioCuentas.GetById(id, usuarioId);

            if (cuenta == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            await repositorioCuentas.Delete(id);

            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> TiposCuentas(int usuarioId)
        {
            var tiposCuentas = await repositorioTiposCuentas.GetTiposCuenta(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
