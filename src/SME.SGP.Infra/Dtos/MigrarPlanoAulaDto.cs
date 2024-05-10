using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class MigrarPlanoAulaDto
    {
        [ListaTemElementos(ErrorMessage = "A lista de turmas deve ser preenchida")]
        public IEnumerable<DataPlanoAulaTurmaDto> IdsPlanoTurmasDestino { get; set; }

        [Required(ErrorMessage = "O id do plano de aula deve ser informado")]
        public long PlanoAulaId { get; set; }

        [Required(ErrorMessage = "O componente curricular deve ser informado")]
        public string DisciplinaId { get; set; }

        public bool MigrarLicaoCasa { get; set; }

        public bool MigrarRecuperacaoAula { get; set; }

        public bool MigrarObjetivos { get; set; }
    }
}
