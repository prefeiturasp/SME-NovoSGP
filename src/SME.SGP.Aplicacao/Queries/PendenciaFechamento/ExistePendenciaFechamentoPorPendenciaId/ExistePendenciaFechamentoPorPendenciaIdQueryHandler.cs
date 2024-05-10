using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaFechamentoPorPendenciaIdQueryHandler : IRequestHandler<ExistePendenciaFechamentoPorPendenciaIdQuery, bool>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ExistePendenciaFechamentoPorPendenciaIdQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<bool> Handle(ExistePendenciaFechamentoPorPendenciaIdQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaFechamento.ExistePendenciaFechamentoPorPendenciaId(request.PendenciaId);
    }
}
