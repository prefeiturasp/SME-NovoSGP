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
        private readonly IServicoEol _servicoEol;

        public ObterAlunosPorTurmaEAnoLetivoQueryHandler(IServicoEol servicoEol)
        {
            this._servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var alunos = await _servicoEol.ObterAlunosPorTurma(request.CodigoTurma);

            if (alunos == null || !alunos.Any())
                throw new NegocioException($"Não foi encontrado alunos para a turma {request.CodigoTurma}");

            return alunos.OrderBy(x => x.NumeroAlunoChamada);
        }
    }
}
