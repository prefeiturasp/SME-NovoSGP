using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQueryHandler : IRequestHandler<ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }
        public async Task<IEnumerable<long>> Handle(ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ObterPendenciasAulaPorDreUeTipoModalidade(request.DreId, request.UeId, request.TipoPendencia, request.Modalidade);
    }
}
