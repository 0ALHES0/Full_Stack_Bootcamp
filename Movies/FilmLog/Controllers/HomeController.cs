using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FilmLog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FilmLog.Data; // Eklenen DataContext için
using Microsoft.EntityFrameworkCore;

namespace FilmLog.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context; // EF Core DbContext eklendi

    public HomeController(DataContext context) // Dependency Injection
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString, string category)
    {
        var films = await _context.Films.ToListAsync(); // Veritabanından çekildi
        
        if (!string.IsNullOrEmpty(searchString))
        {
            ViewBag.SearchString = searchString;
            films = films.Where(p => p.Name.ToLower().Contains(searchString)).ToList();
        }

        if (!string.IsNullOrEmpty(category) && category != "0")
        {
            films = films.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }
        
        return View(films);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Film model, IFormFile imageFile)
    {
        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
        
        if (imageFile != null)
        {
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
            }
            else
            {
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                try
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.Image = randomFileName;
                }
                catch
                {
                    ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu.");
                }
            }
        }
        else
        {
            ModelState.AddModelError("", "Bir resim dosyası seçiniz.");
        }

        if (ModelState.IsValid)
        {
            await _context.Films.AddAsync(model); // Veritabanına ekleme
            await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View(model);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = await _context.Films.FindAsync(id); // Veritabanından film çekildi
        if (entity == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
        return View(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Film model, IFormFile? imageFile)
    {
        if (id != model.MoviesId)
        {
            return NotFound();
        }

        var entity = await _context.Films.FindAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
        if (imageFile != null)
        {
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
            }
            else
            {
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                try
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    entity.Image = randomFileName;
                }
                catch
                {
                    ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu.");
                }
            }
        }

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
        if (id == null)
        {
            return NotFound();
        }

        var entity = await _context.Films.FindAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        _context.Films.Remove(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
