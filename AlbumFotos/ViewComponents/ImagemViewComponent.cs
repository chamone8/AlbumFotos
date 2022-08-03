using AlbumFotos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlbumFotos.ViewComponents
{
    public class ImagemViewComponent : ViewComponent
    {
        private readonly Context _context;

        public ImagemViewComponent(Context contexto)
        {
            _context = contexto;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid id)
        {
            return View(await _context.Imagens.Where(x => x.AlbumId == id).ToListAsync());
        }
    }
}
