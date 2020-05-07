using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
   public class BimestrePlanoAnualTerritorioSaberDto
    {
        [Required(ErrorMessage = "O bimestre deve ser informado")]
        [Range(1, 4, ErrorMessage = "O bimestre deve ser entre 1 e 4")]
        public int? Bimestre { get; set; }

        [Required(ErrorMessage = "O desenvolvimento descrição deve ser informada.")]
        public string Desenvolvimento { get; set; }

        [Required(ErrorMessage = "A descrição deve ser informada.")]
        public string Reflexao { get; set; }
    }
}
