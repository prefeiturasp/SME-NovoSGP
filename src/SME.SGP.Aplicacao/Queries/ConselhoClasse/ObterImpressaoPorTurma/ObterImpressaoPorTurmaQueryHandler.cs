using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ConselhoClasse.ObterImpressaoPorTurma
{
    public class ObterImpressaoPorTurmaQueryHandler : IRequestHandler<ObterImpressaoPorTurmaQuery>
    {
        public Task<Unit> Handle(ObterImpressaoPorTurmaQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
