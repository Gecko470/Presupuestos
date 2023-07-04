using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presupuestos.Models;
using Presupuestos.Servicios;

namespace Presupuestos.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly IRepositorioTransacciones repositorioTransacciones;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IUsuariosService usuarioService;
        private readonly IMapper mapper;

        public TransaccionesController(IRepositorioTransacciones repositorioTransacciones, IRepositorioCuentas repositorioCuentas, IRepositorioCategorias repositorioCategorias, IUsuariosService usuarioService, IMapper mapper)
        {
            this.repositorioTransacciones = repositorioTransacciones;
            this.repositorioCuentas = repositorioCuentas;
            this.repositorioCategorias = repositorioCategorias;
            this.usuarioService = usuarioService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var usuarioId = usuarioService.GetUsuarioId();
            var modelo = new TransaccionCreacionViewModel();
            modelo.Cuentas = await Cuentas(usuarioId);
            modelo.Categorias = await Categorias(usuarioId, modelo.TipoOperacionId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransaccionCreacionViewModel modelo)
        {
            var usuarioId = usuarioService.GetUsuarioId();

            if (!ModelState.IsValid)
            {
                modelo.Cuentas = await Cuentas(usuarioId);
                modelo.Categorias = await Categorias(usuarioId, modelo.TipoOperacionId);

                return View(modelo);
            }

            var cuentaBD = await repositorioCuentas.GetById(modelo.CuentaId, usuarioId);

            if (cuentaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var categoriaBD = await repositorioCategorias.GetById(modelo.CategoriaId, usuarioId);

            if (categoriaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            modelo.UsuarioId = usuarioId;

            if (modelo.TipoOperacionId == TipoOperacion.Gasto)
            {
                modelo.Cantidad *= -1;
            }

            await repositorioTransacciones.Create(modelo);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuarioId = usuarioService.GetUsuarioId();
            var transaccionBD = await repositorioTransacciones.GetById(id, usuarioId);

            if (transaccionBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var modelo = mapper.Map<TransaccionEditViewModel>(transaccionBD);

            modelo.CantidadAnterior = modelo.Cantidad;

            if (modelo.TipoOperacionId == TipoOperacion.Gasto)
            {
                modelo.CantidadAnterior = modelo.Cantidad * -1;
            }

            modelo.CuentaAnteriorId = transaccionBD.CuentaId;
            modelo.Cuentas = await Cuentas(usuarioId);
            modelo.Categorias = await Categorias(usuarioId, modelo.TipoOperacionId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TransaccionEditViewModel modelo)
        {
            var usuarioId = usuarioService.GetUsuarioId();

            if (!ModelState.IsValid)
            {
                modelo.Cuentas = await Cuentas(usuarioId);
                modelo.Categorias = await Categorias(usuarioId, modelo.TipoOperacionId);
                return View(modelo);
            }

            var cuentaBD = await repositorioCuentas.GetById(modelo.CuentaId, usuarioId);

            if (cuentaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var categoriaBD = await repositorioCategorias.GetById(modelo.CategoriaId, usuarioId);

            if (categoriaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var transaccion = mapper.Map<Transaccion>(modelo);

            if (modelo.TipoOperacionId == TipoOperacion.Gasto)
            {
                transaccion.Cantidad *= -1;
            }

            await repositorioTransacciones.Update(transaccion, modelo.CantidadAnterior, modelo.CuentaAnteriorId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = usuarioService.GetUsuarioId();

            var transaccionBD = await repositorioTransacciones.GetById(id, usuarioId);

            if (transaccionBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            await repositorioTransacciones.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetCategorias([FromBody] TipoOperacion tipoOperacionId)
        {
            var usuarioId = usuarioService.GetUsuarioId();
            var categorias = await Categorias(usuarioId, tipoOperacionId);

            return Ok(categorias);
        }

        private async Task<IEnumerable<SelectListItem>> Cuentas(int usuarioId)
        {
            var cuentas = await repositorioCuentas.Get(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> Categorias(int usuarioId, TipoOperacion tipoOperacionId)
        {
            var categorias = await repositorioCategorias.Get(usuarioId, tipoOperacionId);
            return categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
