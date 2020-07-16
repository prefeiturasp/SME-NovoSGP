using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class BimestrePlanoAnualDto
    {
        [Required(ErrorMessage = "O bimestre deve ser informado")]
        [Range(1, 4, ErrorMessage = "O bimestre deve ser entre 1 e 4")]
        public int? Bimestre { get; set; }

        [Required(ErrorMessage = "A descrição deve ser informada.")]
        public string Descricao { get; set; }

        public bool ObjetivosAprendizagemOpcionais { get; set; }

        public List<ObjetivoAprendizagemSimplificadoDto> ObjetivosAprendizagem { get; set; }
    }
}