using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AulaPrevistaDto
    {
        [Required(ErrorMessage = "O componente curricular deve ser informado")]
        public string DisciplinaId { get; set; }

        [EnumeradoRequirido(ErrorMessage = "A modalidade deve ser informada")]
        public Modalidade Modalidade { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public string TurmaId { get; set; }

        public long Id { get; set; }

        public IEnumerable<AulaPrevistaBimestreQuantidadeDto> BimestresQuantidade { get; set; }
    }
}
