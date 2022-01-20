using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAtividadeAvaliativaQueryHandler : IRequestHandler<ObterIndicativoPendenciasAtividadeAvaliativaQuery, bool>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterIndicativoPendenciasAtividadeAvaliativaQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<bool> Handle(ObterIndicativoPendenciasAtividadeAvaliativaQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.PossuiPendenciasAtividadeAvaliativa(request.DisciplinaId, request.TurmaId, request.AnoLetivo);
    }
}
