using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPorDescricaoTipoQueryHandler : IRequestHandler<ObterPendenciaPorDescricaoTipoQuery, long>
    {

        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciaPorDescricaoTipoQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<long> Handle(ObterPendenciaPorDescricaoTipoQuery request, CancellationToken cancellationToken)
        => await repositorioPendenciaAula.ObterPendenciaPorDescricaoTipo(request.Descricao, request.TipoPendencia);
    }
}
