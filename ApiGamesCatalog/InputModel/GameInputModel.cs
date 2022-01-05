using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGamesCatalog.InputModel
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Name must contain between 3 and 100 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must contain between 3 and 100 characters")]
        public string Publisher { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
        public double Price { get; set; }
    }
}
