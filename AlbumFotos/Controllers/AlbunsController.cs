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
    public class AlbunsController : Controller
    {
        private readonly Context _context;
        private IWebHostEnvironment _env;

        public AlbunsController(Context context, IWebHostEnvironment env)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task<IActionResult> Index()
        {
            return _context.Albuns != null ?
                        View(await _context.Albuns.ToListAsync()) :
                        Problem("Entity set 'Context.Albums'  is null.");
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Albuns == null)
            {
                return NotFound();
            }

            var album = await _context.Albuns
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }
        // GET: Albuns/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId,Destino,FotoTopo,Inicio,Fim")] Album album, IFormFile arquivo)
        {
            var linkUpload = Path.Combine(_env.WebRootPath, "Imagens");

            if (album != null)
            {
                using (var fileStream = new FileStream(Path.Combine(linkUpload, arquivo.FileName), FileMode.Create))
                {
                    await arquivo.CopyToAsync(fileStream);
                    album.FotoTopo = "~/Imagens/" + arquivo.FileName;
                }

                album.AlbumId = Guid.NewGuid();

                _context.Add(album);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Albuns == null)
            {
                return NotFound();
            }

            var album = await _context.Albuns.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            TempData["FotoTopo"] = album.FotoTopo;

            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlbumId,Destino,FotoTopo,Inicio,Fim")] Album album, IFormFile arquivo)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (String.IsNullOrEmpty(album.FotoTopo))
                album.FotoTopo = TempData["FotoTopo"].ToString() != null ? TempData["FotoTopo"].ToString() : null;

            if (arquivo != null)
            {
                try
                {
                    var linkUpload = Path.Combine(_env.WebRootPath, "Imagens");
                    if (arquivo != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(linkUpload, arquivo.FileName), FileMode.Create))
                        {
                            await arquivo.CopyToAsync(fileStream);
                            album.FotoTopo = "~/Imagens/" + arquivo.FileName;
                        }
                    }

                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Guid AlbumId)
        {
            if (_context.Albuns == null)
            {
                return Json("Entity set 'Context.Albums'  is null.");
            }

            var album = await _context.Albuns.FindAsync(AlbumId);
            IEnumerable<string> links = _context.Imagens.Where(x => x.AlbumId == AlbumId).Select(i => i.Link);

            foreach (var link in links)
            {
                var linkImagem = link.Replace("~", "wwwroot");
                System.IO.File.Delete(linkImagem);

            }

            _context.Imagens.RemoveRange(_context.Imagens.Where(x => x.AlbumId == AlbumId));

            string linkFotoAlbum = album.FotoTopo;
            linkFotoAlbum = linkFotoAlbum.Replace("~", "wwwroot");
            System.IO.File.Delete(linkFotoAlbum);

            _context.Albuns.Remove(album);
            await _context.SaveChangesAsync();
            return Json("Excluido com sucesso!!!");
        }

        private bool AlbumExists(Guid id)
        {
            return (_context.Albuns?.Any(e => e.AlbumId == id)).GetValueOrDefault();
        }
    }
}
