using System.ComponentModel.DataAnnotations;

namespace AlbumFotos.Models
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [StringLength(50,ErrorMessage = "Maximo 50 Caractere")]
        public string Destino { get; set; }
        public string FotoTopo { get; set; }    
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public DateTime Inicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public DateTime Fim { get; set; }

        public ICollection<Imagem> Imagens { get; set; }
    }
}
