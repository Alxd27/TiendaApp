using Microsoft.EntityFrameworkCore;
using TiendaApp.Data;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// PRINCIPIO SOLID (SRP)
// Este middleware tiene una única responsabilidad:
// medir el tiempo que tarda cada petición.

app.Use(async (context, next) =>
{
    var timer = System.Diagnostics.Stopwatch.StartNew();

    context.Response.Headers.Append("X-Server-Performance", "Tracking");

    await next();

    timer.Stop();

    var elapsedMs = timer.ElapsedMilliseconds;

    Console.WriteLine($"[MONITOR] {context.Request.Method} {context.Request.Path} - {elapsedMs} ms");
});

app.Map("/sitemap.xml", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/xml";

        var sitemapContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>

<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">

<url>
<loc>https://laeconomica.com/</loc>
<priority>1.0</priority>
</url>

<url>
<loc>https://laeconomica.com/Productos</loc>
<priority>0.8</priority>
</url>

<url>
<loc>https://laeconomica.com/Ofertas</loc>
<priority>0.9</priority>
</url>

</urlset>";

        await context.Response.WriteAsync(sitemapContent);
    });
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();