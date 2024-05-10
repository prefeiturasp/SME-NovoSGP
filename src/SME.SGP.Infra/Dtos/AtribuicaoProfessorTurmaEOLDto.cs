using System;

namespace SME.SGP.Infra.Dtos
{
    public class AtribuicaoProfessorTurmaEOLDto
    {
        public int AnoAtribuicao { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataDisponibilizacao { get; set; }
        public DateTime? DataFimTurma { get; set; }
        public int? CodigoMotivoDisponibilizacao { get; set; }

        public bool SemAtribuicaoNaData(DateTime data)
            => (DataCancelamento.HasValue && DataCancelamento < data)
            || (DataDisponibilizacao.HasValue && DataDisponibilizacao < data);
    }
}
