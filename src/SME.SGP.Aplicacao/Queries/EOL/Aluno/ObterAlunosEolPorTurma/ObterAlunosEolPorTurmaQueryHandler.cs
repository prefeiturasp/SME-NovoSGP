using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEolPorTurmaQueryHandler :IRequestHandler<ObterAlunosEolPorTurmaQuery,IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IServicoEol servicoEaol;

        public ObterAlunosEolPorTurmaQueryHandler(IServicoEol servicoEaol)
        {
            this.servicoEaol = servicoEaol ?? throw new ArgumentNullException(nameof(servicoEaol));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosEolPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await servicoEaol.ObterAlunosPorTurma(request.TurmaId,request.ConsideraInativos);
        }
    }
}