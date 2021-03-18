using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoAlunoUseCase : AbstractUseCase, IObterAcompanhamentoAlunoUseCase
    {
        public ObterAcompanhamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AcompanhamentoAlunoDto>> Executar(FiltroAcompanhamentoTurmaAlunoSemestreDto filtro)
        {
            return await mediator.Send(new ObterAcompanhamentoPorAlunoTurmaESemestreQuery(filtro.TurmaId, filtro.AlunoId, filtro.Semestre));
        }
    }
}
