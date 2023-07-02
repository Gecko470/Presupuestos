using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Presupuestos.Models;
using Presupuestos.Servicios;

namespace Presupuestos.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IUsuariosService usuariosService;
        private readonly IMapper mapper;

        public CategoriasController(IRepositorioCategorias repositorioCategorias, IUsuariosService usuariosService, IMapper mapper)
        {
            this.repositorioCategorias = repositorioCategorias;
            this.usuariosService = usuariosService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var listaCategorias = await repositorioCategorias.Get(usuarioId);

            return View(listaCategorias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var usuarioId = usuariosService.GetUsuarioId();
            categoria.UsuarioId = usuarioId;

            await repositorioCategorias.Create(categoria);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var categoriaBD = await repositorioCategorias.GetById(id, usuarioId);

            if (categoriaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            return View(categoriaBD);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var usuarioId = usuariosService.GetUsuarioId();
            var categoriaBD = await repositorioCategorias.GetById(categoria.Id, usuarioId);

            if (categoriaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            categoria.UsuarioId = usuarioId;
            await repositorioCategorias.Update(categoria);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var categoriaBD = await repositorioCategorias.GetById(id, usuarioId);

            if (categoriaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            return View(categoriaBD);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var usuarioId = usuariosService.GetUsuarioId();
            var categoriaBD = await repositorioCategorias.GetById(id, usuarioId);

            if (categoriaBD == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            await repositorioCategorias.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
