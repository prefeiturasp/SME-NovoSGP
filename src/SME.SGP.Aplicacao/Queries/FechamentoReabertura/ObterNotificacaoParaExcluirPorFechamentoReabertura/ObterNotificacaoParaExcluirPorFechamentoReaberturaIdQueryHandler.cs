using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQueryHandler : IRequestHandler<ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQuery,long>
    {
        private readonly IRepositorioFechamentoReabertura repositorioReabertura;

        public ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQueryHandler(IRepositorioFechamentoReabertura reabertura)
        {
            repositorioReabertura = reabertura ?? throw new ArgumentNullException(nameof(reabertura));
        }

        public async Task<long> Handle(ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioReabertura.ObterNotificacaoParaExcluirPorFechamentoReaberturaId(request.FechamentoReaberturaId);
        }
    }
}