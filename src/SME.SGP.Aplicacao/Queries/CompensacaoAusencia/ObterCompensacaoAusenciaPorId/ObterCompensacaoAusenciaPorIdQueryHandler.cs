using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaPorIdQueryHandler : IRequestHandler<ObterCompensacaoAusenciaPorIdQuery, Dominio.CompensacaoAusencia>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ObterCompensacaoAusenciaPorIdQueryHandler(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public Task<Dominio.CompensacaoAusencia> Handle(ObterCompensacaoAusenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusencia.ObterPorIdAsync(request.Id);
        }
    }
}
