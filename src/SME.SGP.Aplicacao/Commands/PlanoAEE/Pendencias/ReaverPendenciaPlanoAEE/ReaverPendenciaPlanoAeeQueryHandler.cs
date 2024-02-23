using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReaverPendenciaPlanoAeeQueryHandler : IRequestHandler<ReaverPendenciaPlanoAeeQuery, bool>
    {
        private readonly IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE;
        private readonly IRepositorioPendencia repositorioPendencia;

        public ReaverPendenciaPlanoAeeQueryHandler(IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE,
                                                   IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioPendenciaPlanoAEE = repositorioPendenciaPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaPlanoAEE));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<bool> Handle(ReaverPendenciaPlanoAeeQuery request, CancellationToken cancellationToken)
        {
            var pendenciasPlanoAee = await repositorioPendenciaPlanoAEE
                .ObterPorPlanoId(request.PlanoAeeId);

            if (pendenciasPlanoAee == null || !pendenciasPlanoAee.Any())
                return true;

            var pendencias = (await repositorioPendencia
                .ObterPorIdsAsync(pendenciasPlanoAee.Select(p => p.PendenciaId).ToArray())).Where(p => p.Tipo == TipoPendencia.AEE);

            if (pendencias == null || !pendencias.Any() || pendencias.OrderBy(p => p.Id).Last().Situacao != SituacaoPendencia.Resolvida)
                return true;

            var pendenciaReaver = pendencias.OrderBy(p => p.Id).Last();

            pendenciaReaver.Situacao = SituacaoPendencia.Pendente;
            await repositorioPendencia.SalvarAsync(pendenciaReaver);

            return true;
        }
    }
}
