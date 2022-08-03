using System.ComponentModel.DataAnnotations;

namespace AlbumFotos.Models
{
    public class Imagem
    {
        public Guid ImagemId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Link { get; set; }
        public Guid AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
