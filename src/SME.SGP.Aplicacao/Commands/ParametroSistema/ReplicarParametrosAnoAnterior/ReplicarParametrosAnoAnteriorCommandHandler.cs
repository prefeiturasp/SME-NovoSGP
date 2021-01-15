using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReplicarParametrosAnoAnteriorCommandHandler : IRequestHandler<ReplicarParametrosAnoAnteriorCommand, bool>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ReplicarParametrosAnoAnteriorCommandHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<bool> Handle(ReplicarParametrosAnoAnteriorCommand request, CancellationToken cancellationToken)
        {
            var anoLetivoAnterior = DateTime.Today.AddYears(-1);
            
            await repositorioParametrosSistema.ReplicarParametrosAnoAnteriorAsync(request.AnoLetivo, anoLetivoAnterior.Year);

            return true;
        }
    }
}
