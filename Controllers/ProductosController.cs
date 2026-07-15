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
public async Task<IActionResult> Index()
{
// LINQ: AsNoTracking mejora rendimiento en solo lectura (Código Limpio)
var productos = await _context.Productos.AsNoTracking().ToListAsync();
return View(productos);
}
// CREATE: Vista
public IActionResult Create() => View();
// CREATE: Proceso POST
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Producto producto)
{
if (ModelState.IsValid)
{
_context.Add(producto);
await _context.SaveChangesAsync();
return RedirectToAction(nameof(Index));
}
return View(producto);
}
// NOTA: Implementar Edit y Delete siguiendo la misma lógica Async.
}
