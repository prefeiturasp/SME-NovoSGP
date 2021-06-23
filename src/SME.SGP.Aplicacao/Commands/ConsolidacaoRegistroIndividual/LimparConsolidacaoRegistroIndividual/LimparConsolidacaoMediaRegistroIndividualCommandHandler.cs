using MediatR;
using Sentry;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoMediaRegistroIndividualCommandHandler : IRequestHandler<LimparConsolidacaoMediaRegistroIndividualCommand, bool>
    {
        private readonly IRepositorioConsolidacaoRegistroIndividualMedia repositorio;

        public LimparConsolidacaoMediaRegistroIndividualCommandHandler(IRepositorioConsolidacaoRegistroIndividualMedia repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(LimparConsolidacaoMediaRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorio.LimparConsolidacaoMediaRegistrosIndividuaisPorAno(request.AnoLetivo);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return true;
        }
    }
}
