using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase : IObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Executar(string codigoTurma, int anoLetivo)
        {
            return await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(codigoTurma));
        }
    }
}
