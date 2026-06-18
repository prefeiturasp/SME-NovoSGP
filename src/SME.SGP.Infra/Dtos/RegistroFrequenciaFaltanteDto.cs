using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaFaltanteDto
    {
        public string DisciplinaId { get; set; }
        public string CodigoTurma { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public string NomeTurma { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string CodigoUe { get; set; }
        public string NomeUe { get; set; }
        public string CodigoDre { get; set; }
        public string NomeDre { get; set; }

        public IEnumerable<AulasPorTurmaDisciplinaDto> Aulas { get; set; }
    }
}
