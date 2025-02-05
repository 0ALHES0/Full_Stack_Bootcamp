using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FilmLog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FilmLog.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmLog.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;

    // Constructor: Bağımlılık enjeksiyonu ile DataContext alınır
    public HomeController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString, string category)
    {
        // Veritabanından tüm filmleri getirir
        var films = await _context.Films.ToListAsync();

        // Arama metni varsa, filmleri filtreler
        if (!string.IsNullOrEmpty(searchString))
        {
            ViewBag.SearchString = searchString;
            films = films.Where(p => p.Name.ToLower().Contains(searchString.ToLower())).ToList();
        }

        // Kategori seçildiyse, filmleri filtreler
        if (!string.IsNullOrEmpty(category) && category != "0")
        {
            films = films.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }
        
        return View(films);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // Kategori listesini ViewBag ile View'e gönderir
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Film model, IFormFile imageFile)
    {
        // Resim dosyası seçilmemişse hata ekler
        if (imageFile == null)
        {
            ModelState.AddModelError("", "Bir resim dosyası seçiniz.");
        }
        else
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            // Geçerli bir dosya uzantısı değilse hata ekler
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
            }
            else
            {
                // Rastgele dosya adı oluşturup resmi kaydeder
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = randomFileName;
            }
        }

        // Model geçerliyse filmi veritabanına ekler ve kaydeder
        if (ModelState.IsValid)
        {
            await _context.Films.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Form tekrar yüklendiğinde kategori listesini ViewBag ile gönderir
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        // ID'ye göre filmi veritabanından bulur
        var entity = await _context.Films.FindAsync(id);
        if (entity == null) return NotFound();

        // Kategori listesini ViewBag ile View'e gönderir
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Film model, IFormFile? imageFile)
    {
        if (id != model.MoviesId) return NotFound();

        // ID'ye göre filmi veritabanından bulur
        var entity = await _context.Films.FindAsync(id);
        if (entity == null) return NotFound();

        // Yeni bir resim seçildiyse işlemi gerçekleştirir
        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
            }
            else
            {
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                entity.Image = randomFileName;
            }
        }

        // Model geçerliyse güncellemeyi kaydeder
        if (ModelState.IsValid)
        {
            entity.Name = model.Name;
            entity.CategoryId = model.CategoryId;
            _context.Films.Update(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View(model);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        // ID'ye göre filmi bulur ve siler
        var entity = await _context.Films.FindAsync(id);
        if (entity == null) return NotFound();

        _context.Films.Remove(entity);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
