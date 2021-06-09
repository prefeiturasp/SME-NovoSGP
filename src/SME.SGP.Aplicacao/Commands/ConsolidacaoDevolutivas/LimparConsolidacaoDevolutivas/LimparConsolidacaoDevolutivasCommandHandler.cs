using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoDevolutivasCommandHandler : IRequestHandler<LimparConsolidacaoDevolutivasCommand, bool>
    {
        private readonly IRepositorioConsolidacaoDevolutivas repositorio;

        public LimparConsolidacaoDevolutivasCommandHandler(IRepositorioConsolidacaoDevolutivas repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(LimparConsolidacaoDevolutivasCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorio.LimparConsolidacaoDevolutivasPorAno(request.AnoLetivo);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            
            return true;
        }
    }
}
