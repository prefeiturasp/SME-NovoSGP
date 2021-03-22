using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmasComponentePeriodoQuery : IRequest<int>
    {
        public ObterAulasDadasPorTurmasComponentePeriodoQuery(string[] turmasCodigo, long componenteCurricularId, PeriodoEscolar periodoEscolar)
        {
            TurmasCodigo = turmasCodigo;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolar = periodoEscolar;
        }

        public string[] TurmasCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
    }
}
