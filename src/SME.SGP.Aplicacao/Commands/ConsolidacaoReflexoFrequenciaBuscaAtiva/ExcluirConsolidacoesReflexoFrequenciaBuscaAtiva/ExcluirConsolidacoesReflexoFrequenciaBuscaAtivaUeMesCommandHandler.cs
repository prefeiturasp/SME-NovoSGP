using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommandHandler : IRequestHandler<ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand, bool>
    {
        private readonly IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva repositorioConsolidacaoReflexoFrequencia;

        public ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommandHandler (IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva repositorioConsolidacaoReflexoFrequencia)
        {
            this.repositorioConsolidacaoReflexoFrequencia = repositorioConsolidacaoReflexoFrequencia ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoReflexoFrequencia));
        }

        public async Task<bool> Handle(ExcluirConsolidacoesReflexoFrequenciaBuscaAtivaUeMesCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoReflexoFrequencia.ExcluirConsolidacoes(request.UeCodigo, request.Mes, request.AnoLetivo);
            return true;
        }
    }
}
