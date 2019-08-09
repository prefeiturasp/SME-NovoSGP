using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class PlanoCicloDto
    {
        [Required]
        public string Descricao { get; set; }

        public long Id { get; set; }

        //[ListaTemElementos(ErrorMessage = "A matriz de saberes deve conter ao menos 1 elemento.")]
        public List<long> IdsMatrizesSaber { get; set; }

        //[ListaTemElementos(ErrorMessage = "Os objetivos de desenvolvimento sustentável devem conter ao menos 1 elemento.")]
        public List<long> IdsObjetivosDesenvolvimento { get; set; }
    }
}