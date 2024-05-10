using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaCommandHandler : IRequestHandler<SalvarCompensacaoAusenciaCommand, long>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public SalvarCompensacaoAusenciaCommandHandler(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public Task<long> Handle(SalvarCompensacaoAusenciaCommand request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusencia.SalvarAsync(request.CompensacaoAusencia);
        }
    }
}
