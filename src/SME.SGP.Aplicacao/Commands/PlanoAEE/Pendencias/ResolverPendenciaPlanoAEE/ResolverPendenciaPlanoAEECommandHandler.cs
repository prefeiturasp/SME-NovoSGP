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
    public class ResolverPendenciaPlanoAEECommandHandler : IRequestHandler<ResolverPendenciaPlanoAEECommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPendencia repositorioPendencia;

        public ResolverPendenciaPlanoAEECommandHandler(IUnitOfWork unitOfWork, IRepositorioPendencia repositorioPendencia)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<bool> Handle(ResolverPendenciaPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var pendenciasPlano = await repositorioPendencia.ObterIdsPendenciasPorPlanoAEEId(request.PlanoAEEId);
            if (pendenciasPlano != null)
                await ResolverPendencias(pendenciasPlano);
            return true;
        }

        private async Task ResolverPendencias(long[] ids)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await repositorioPendencia.AtualizarStatusPendenciasPorIds(ids, SituacaoPendencia.Resolvida);
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

    }
}
