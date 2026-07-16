using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaApp.Data;
using TiendaApp.Models;

namespace TiendaApp.Controllers;

public class ProductosController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // LISTAR
    public async Task<IActionResult> Index()
    {
        return View(await _context.Productos.AsNoTracking().ToListAsync());
    }

    // CREAR (GET)
    public IActionResult Create()
    {
        return View();
    }

    // CREAR (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    // EDITAR (GET)
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        return View(producto);
    }

    // EDITAR (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Producto producto)
    {
        if (id != producto.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    // ELIMINAR (GET)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == id);

        if (producto == null)
            return NotFound();

        return View(producto);
    }

    // ELIMINAR (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto != null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // DETALLES
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var producto = await _context.Productos
            .FirstOrDefaultAsync(x => x.Id == id);

        if (producto == null)
            return NotFound();

        return View(producto);
    }

    [Route("promociones-del-mes/barrio-norte")]
public IActionResult OfertasEspeciales()
{
    ViewData["Message"] = "Ofertas exclusivas para los vecinos del barrio.";

    return View();
}
}