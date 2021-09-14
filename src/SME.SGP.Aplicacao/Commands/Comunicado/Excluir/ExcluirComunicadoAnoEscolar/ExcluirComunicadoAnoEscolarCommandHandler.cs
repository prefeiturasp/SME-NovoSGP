using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoAnoEscolarCommandHandler : IRequestHandler<ExcluirComunicadoAnoEscolarCommand, bool>
    {
        private readonly IRepositorioComunicadoAnoEscolar repositorioComunicadoAnoEscolar;

        public ExcluirComunicadoAnoEscolarCommandHandler(IRepositorioComunicadoAnoEscolar repositorioComunicadoAnoEscolar)
        {
            this.repositorioComunicadoAnoEscolar = repositorioComunicadoAnoEscolar ?? throw new ArgumentNullException(nameof(repositorioComunicadoAnoEscolar));
        }

        public async Task<bool> Handle(ExcluirComunicadoAnoEscolarCommand request, CancellationToken cancellationToken)
            => await repositorioComunicadoAnoEscolar.ExcluirPorIdComunicado(request.Id);        
    }
}
