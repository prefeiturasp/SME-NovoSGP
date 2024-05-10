using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoReaberturaPorDataTurmaQueryHandler : IRequestHandler<ObterFechamentoReaberturaPorDataTurmaQuery, FechamentoReabertura>
    {
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public ObterFechamentoReaberturaPorDataTurmaQueryHandler(IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }
        public async Task<FechamentoReabertura> Handle(ObterFechamentoReaberturaPorDataTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoReabertura.ObterPorDataTurmaCalendarioAsync(request.UeId, request.DataParaVerificar, request.TipoCalendarioId);            
        }
    }
}
