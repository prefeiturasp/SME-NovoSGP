using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterListagemDiariosDeBordoPorPeriodoQuery : IRequest<PaginacaoResultadoDto<DiarioBordoTituloDto>>
    {
        public ObterListagemDiariosDeBordoPorPeriodoQuery(long turmaId, long componenteCurricularId, DateTime? dataInicio, DateTime? dataFim)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long TurmaId { get; set; }

        public long ComponenteCurricularId { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
