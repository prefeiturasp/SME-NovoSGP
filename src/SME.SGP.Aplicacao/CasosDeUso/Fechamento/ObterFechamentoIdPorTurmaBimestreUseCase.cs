using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoIdPorTurmaBimestreUseCase : AbstractUseCase, IObterFechamentoIdPorTurmaBimestreUseCase
    {
        public ObterFechamentoIdPorTurmaBimestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<FechamentoTurmaPeriodoEscolarDto> Executar(TurmaBimestreDto param)
        {
            return mediator.Send(new ObterFechamentoTurmaComPeriodoEscolarQuery(param.TurmaId, param.Bimestre));
        }
    }
}
