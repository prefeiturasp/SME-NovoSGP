using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PeriodoFechamentoUseCase : AbstractUseCase, IPeriodoFechamentoUseCase
    {
        public PeriodoFechamentoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(string turmaCodigo, DateTime dataReferencia, int bimestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            return await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, dataReferencia, bimestre));
        }
    }
}
