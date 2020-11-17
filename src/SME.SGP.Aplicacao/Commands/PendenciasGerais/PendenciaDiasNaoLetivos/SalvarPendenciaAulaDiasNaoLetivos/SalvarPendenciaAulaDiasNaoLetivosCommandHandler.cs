using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAulaDiasNaoLetivosCommandHandler : IRequestHandler<SalvarPendenciaAulaDiasNaoLetivosCommand, bool>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;
        public SalvarPendenciaAulaDiasNaoLetivosCommandHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<bool> Handle(SalvarPendenciaAulaDiasNaoLetivosCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaAula.Salvar(request.AulaId, request.Motivo, request.PendenciaId);
            return true;
        }

    }
}
