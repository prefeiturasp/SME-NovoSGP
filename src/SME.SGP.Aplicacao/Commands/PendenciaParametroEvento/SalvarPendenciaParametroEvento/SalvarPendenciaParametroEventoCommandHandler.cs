using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaParametroEventoCommandHandler : IRequestHandler<SalvarPendenciaParametroEventoCommand, long>
    {
        private readonly IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento;

        public SalvarPendenciaParametroEventoCommandHandler(IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento)
        {
            this.repositorioPendenciaParametroEvento = repositorioPendenciaParametroEvento ?? throw new ArgumentNullException(nameof(repositorioPendenciaParametroEvento));
        }

        public async Task<long> Handle(SalvarPendenciaParametroEventoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaParametroEvento.SalvarAsync(new Dominio.PendenciaParametroEvento()
            {
                PendenciaCalendarioUeId = request.PendenciaCalendarioUeId,
                ParametroSistemaId = request.ParametroSistemaId,
                QuantidadeEventos = request.QuantidadeEventos
            });
        }
    }
}
