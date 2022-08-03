using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlbumFotos.Models;

namespace AlbumFotos.Controllers
{
    public class ImagensController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;

        public ImagensController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Create(Guid id)
        {
            ViewBag.Destinos = _context.Albuns.FirstOrDefault(x => x.AlbumId == id);   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImagemId,Link,AlbumId")] Imagem imagem, IFormFile arquivo)
        {
            if (imagem != null)
            {
                var linkUpload = Path.Combine(_env.WebRootPath, "Imagens");
                if(arquivo != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(linkUpload, arquivo.FileName), FileMode.Create))
                    {
                        await arquivo.CopyToAsync(fileStream);
                        imagem.Link = "~/Imagens/" + arquivo.FileName;
                    }
                }
                imagem.ImagemId = Guid.NewGuid();
                _context.Add(imagem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Albuns", new {id = imagem.AlbumId});
            }
            ViewData["AlbumId"] = new SelectList(_context.Albuns, "AlbumId", "Destino", imagem.AlbumId);
            return View(imagem);
        }
    }
}
