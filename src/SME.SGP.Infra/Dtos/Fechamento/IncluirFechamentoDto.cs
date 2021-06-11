using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class IncluirFechamentoDto
    {
        public IncluirFechamentoDto(long turmaId, long? periodoId)
        {
            TurmaId = turmaId;
            PeriodoId = periodoId;
        }

        public long TurmaId { get; set; }        
        public long? PeriodoId { get; set; }
    }
}
