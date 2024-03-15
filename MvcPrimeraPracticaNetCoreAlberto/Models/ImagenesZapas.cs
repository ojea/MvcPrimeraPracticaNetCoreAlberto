using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcPrimeraPracticaNetCoreAlberto.Models
{
    [Table("IMAGENESZAPASPRACTICA")]
    public class ImagenesZapas
    {
        [Key]
        [Column("IDIMAGEN")]
        public int IdImagen { get; set; }

        [Column("IDPRODCUTO")]
        public int IdProducto { get; set; }

        [Column("IMAGEN")]
        public string Imagen { get; set; }
    }
}
