using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase : AbstractUseCase, IObterAtividadesNotasAlunoPorTurmaPeriodoUseCase
    {
        public ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<IEnumerable<AvaliacaoNotaAlunoDto>> Executar(FiltroTurmaAlunoPeriodoEscolarDto param)
            => mediator.Send(new ObterAtividadesNotasAlunoPorTurmaPeriodoQuery(param.TurmaId, param.PeriodoEscolarId, param.AlunoCodigo, param.ComponenteCurricular));
    }
}
