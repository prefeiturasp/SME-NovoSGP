using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTurmasSeriadasNaUeQueryHandler : IRequestHandler<ObterQuantidadeTurmasSeriadasNaUeQuery, int>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterQuantidadeTurmasSeriadasNaUeQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<int> Handle(ObterQuantidadeTurmasSeriadasNaUeQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterQuantidadeTurmasSeriadas(request.UeId, request.Ano);
    }
}
