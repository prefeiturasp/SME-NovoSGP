using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
   public class MigrarPlanoAulaDto
    {
        [ListaTemElementos(ErrorMessage = "A lista de turmas deve ser preenchida")]
        public IEnumerable<DataPlanoAulaTurmaDto> IdsPlanoTurmasDestino { get; set; }

        public PlanoAulaDto PlanoAula { get; set; }

        [Required(ErrorMessage = "O RF do professor deve ser informado")]
        public string RFProfessor { get; set; }

        public bool EhProfessorCJ { get; set; }

        [Required(ErrorMessage = "A disciplina deve ser informada")]
        public string DisciplinaId { get; set; }

        public bool MigrarLicaoCasa { get; set; }

        public bool MigrarRecuperacaoAula { get; set; }

        public bool MigrarObjetivos { get; set; }
    }
}
