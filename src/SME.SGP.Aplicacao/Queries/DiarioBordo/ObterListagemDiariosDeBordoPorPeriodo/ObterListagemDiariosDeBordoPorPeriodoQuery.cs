using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterListagemDiariosDeBordoPorPeriodoQuery : IRequest<PaginacaoResultadoDto<DiarioBordoTituloDto>>
    {
        public ObterListagemDiariosDeBordoPorPeriodoQuery(long turmaId, string componenteCurricularPaiId, long componenteCurricularFilhoId, DateTime? dataInicio, DateTime? dataFim)
        {
            TurmaId = turmaId;
            ComponenteCurricularPaiId = componenteCurricularPaiId;
            ComponenteCurricularFilhoId = componenteCurricularFilhoId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long TurmaId { get; set; }

        public string ComponenteCurricularPaiId { get; set; }
        public long ComponenteCurricularFilhoId { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
