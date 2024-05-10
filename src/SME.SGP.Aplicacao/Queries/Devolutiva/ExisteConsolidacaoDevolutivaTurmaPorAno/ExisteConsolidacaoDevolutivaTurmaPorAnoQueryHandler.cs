using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteConsolidacaoDevolutivaTurmaPorAnoQueryHandler : IRequestHandler<ExisteConsolidacaoDevolutivaTurmaPorAnoQuery, bool>
    {
        private readonly IRepositorioConsolidacaoDevolutivasConsulta repositorio;

        public ExisteConsolidacaoDevolutivaTurmaPorAnoQueryHandler(IRepositorioConsolidacaoDevolutivasConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(ExisteConsolidacaoDevolutivaTurmaPorAnoQuery request, CancellationToken cancellationToken)
            => await repositorio.ExisteConsolidacaoDevolutivaTurmaPorAno(request.Ano);
    }
}
