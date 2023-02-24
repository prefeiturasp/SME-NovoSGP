using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class FechamentoTurmaDisciplinaPendenciaDto
    {
        public long Id { get; set; }
        public long DisciplinaId { get; set; }
        public SituacaoFechamento SituacaoFechamento { get; set; }
        public string CodigoTurma { get; set; }
        public long TurmaId { get; set; }
        public string NomeTurma { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public int Bimestre { get; set; }
        public string Justificativa { get; set; }
        public long UsuarioId { get; set; }
        public TipoTurma TipoTurma { get; set; }
        public string CriadoRF { get; set; }
        public string AlteradoRF { get; set; }

    }
}
