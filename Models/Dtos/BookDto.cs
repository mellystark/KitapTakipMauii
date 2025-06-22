﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitapTakipMauii.Models.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Description { get; set; }
        public int? PageCount { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsReading { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

