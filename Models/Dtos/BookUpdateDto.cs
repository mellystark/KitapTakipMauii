using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitapTakipMauii.Models.Dtos
{
    public class BookUpdateDto
    {
        [Required(ErrorMessage = "Kitap ID'si zorunludur.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap başlığı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık 100 karakterden uzun olamaz.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Yazar adı 50 karakterden uzun olamaz.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tür zorunludur.")]
        [StringLength(50, ErrorMessage = "Tür 50 karakterden uzun olamaz.")]
        public string Genre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notlar 500 karakterden uzun olamaz.")]
        public string? Notes { get; set; }
        public string? Description { get; set; }
        public string CoverImage { get; set; }
        public int? PageCount { get; internal set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsReading { get; set; }
    }
}
