using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidaSeTurmasPossuemPlanoAnualQueryHandler : IRequestHandler<ValidaSeTurmasPossuemPlanoAnualQuery, IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;
        public ValidaSeTurmasPossuemPlanoAnualQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }
        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Handle(ValidaSeTurmasPossuemPlanoAnualQuery request, CancellationToken cancellationToken)
        {
            var TurmaParaCopiaPlanoAnual = await repositorioPlanejamentoAnual.ValidaSeTurmasPossuemPlanoAnual(request.turmasId, request.ConsideraHistorico);

            return TurmaParaCopiaPlanoAnual;
        }
    }
}
