using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEmPeriodoFechamentoReaberturaQueryHandler : IRequestHandler<ObterTurmaEmPeriodoFechamentoReaberturaQuery, FechamentoReabertura>
    {
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public ObterTurmaEmPeriodoFechamentoReaberturaQueryHandler(IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<FechamentoReabertura> Handle(ObterTurmaEmPeriodoFechamentoReaberturaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(
                                                            request.Bimestre,
                                                            request.DataReferencia,
                                                            request.TipoCalendarioId,
                                                            request.DreCodigo,
                                                            request.UeCodigo);
        }
    }
}
