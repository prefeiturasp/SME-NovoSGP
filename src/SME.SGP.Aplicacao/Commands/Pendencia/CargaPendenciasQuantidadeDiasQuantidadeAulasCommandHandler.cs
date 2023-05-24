using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class CargaPendenciasQuantidadeDiasQuantidadeAulasCommandHandler : IRequestHandler<CargaPendenciasQuantidadeDiasQuantidadeAulasCommand,bool>
    {
        private readonly IRepositorioPendencia repositorioPendencia;

        public CargaPendenciasQuantidadeDiasQuantidadeAulasCommandHandler(IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<bool> Handle(CargaPendenciasQuantidadeDiasQuantidadeAulasCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendencia.AtualizarQuantidadeDiasAulas(request.Carga.PendenciaId,request.Carga.QuantidadeAulas,request.Carga.QuantidadeDias);
            return true;
        }
    }
}