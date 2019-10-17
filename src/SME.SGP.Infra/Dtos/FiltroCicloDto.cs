using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroCicloDto
    {
        [Required(ErrorMessage = "Os anos são obrigatórios.")]
        public IList<string> Anos { get; set; }

        [Required(ErrorMessage = "O ano selecionado é obrigatório")]
        public string AnoSelecionado { get; set; }

        [Required(ErrorMessage = "A modalidade é obrigatória")]
        public int Modalidade { get; set; }
    }
}