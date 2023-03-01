using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarPendenciaCommandHandler : IRequestHandler<AlterarPendenciaCommand, long>
    {
        private readonly IRepositorioPendencia repositorioPendencia;

        public AlterarPendenciaCommandHandler(IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<long> Handle(AlterarPendenciaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioPendencia.SalvarAsync(request.Pendencia);
        }
    }
}
