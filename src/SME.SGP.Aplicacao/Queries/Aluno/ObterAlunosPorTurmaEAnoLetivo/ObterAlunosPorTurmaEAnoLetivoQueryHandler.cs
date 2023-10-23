using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEAnoLetivoQueryHandler : IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IMediator mediator;

        public ObterAlunosPorTurmaEAnoLetivoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(request.CodigoTurma, DateTimeExtension.HorarioBrasilia()));

            if (alunos.EhNulo() || !alunos.Any())
                throw new NegocioException($"Não foi encontrado alunos para a turma {request.CodigoTurma}");

            return alunos.OrderBy(x => x.NumeroAlunoChamada);
        }
    }
}
