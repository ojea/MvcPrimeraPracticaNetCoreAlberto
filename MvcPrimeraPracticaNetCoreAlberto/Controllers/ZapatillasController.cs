using Microsoft.AspNetCore.Mvc;
using MvcPrimeraPracticaNetCoreAlberto.Models;
using MvcPrimeraPracticaNetCoreAlberto.Respository;
using System.Runtime.InteropServices;

namespace MvcPrimeraPracticaNetCoreAlberto.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;

        public ZapatillasController(RepositoryZapatillas repo)

        {

            this.repo = repo;

        }

        public async Task<IActionResult> Index()
        {
            List<Zapatillas> zapatillas = await this.repo.GetZapatillasAsync();

            return View(zapatillas);
        }

        public async Task<IActionResult> Details(int idZapatilla)
        {
            PaginacionZapatillas pagZapas = await this.repo.GetPaginacionZapatillasAsync(1, idZapatilla);
            return View(pagZapas);
        }

        public async Task<IActionResult> PaginacionZapatillas(int? posicion, int idZapatilla)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            PaginacionZapatillas pagZapas = await this.repo.GetPaginacionZapatillasAsync(posicion.Value, idZapatilla);
            int numeroRegistros = pagZapas.numeroRegistros;
            int siguiente = posicion.Value + 1;
            if(siguiente > numeroRegistros)
                siguiente = numeroRegistros;
            int anterior = posicion.Value - 1;
            if (anterior < 1)
                anterior = 1;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["ULTIMO"] = pagZapas.numeroRegistros;
            ViewData["ZAPATILLA"] = pagZapas.Zapatillas;
            ViewData["POSICION"] = posicion;
            return PartialView("PaginacionZapatillas", pagZapas.ImagenZapas);
        }

        public async Task<IActionResult> InsertarImagen()
        {
            List<Zapatillas> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        [HttpPost]
        public async Task<IActionResult> InsertarImagen(List<string> imagenes, int idZapatailla)
        {
            await this.repo.InsertarImagen(imagenes, idZapatailla);
            return RedirectToAction("Index", "Home");
        }
    }
}
