using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ConselhoClasse.ObterImpressaoPorTurmaAluno
{
    public class ObterImpressaoPorTurmaAlunoQueryHandler : IRequestHandler<ObterImpressaoPorTurmaAlunoQuery>
    {
        Task<Unit> IRequestHandler<ObterImpressaoPorTurmaAlunoQuery, Unit>.Handle(ObterImpressaoPorTurmaAlunoQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
