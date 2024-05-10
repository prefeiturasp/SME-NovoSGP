using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteConsolidacaoFrequenciaTurmaPorAnoQueryHandler : IRequestHandler<ExisteConsolidacaoFrequenciaTurmaPorAnoQuery, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorio;

        public ExisteConsolidacaoFrequenciaTurmaPorAnoQueryHandler(IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(ExisteConsolidacaoFrequenciaTurmaPorAnoQuery request, CancellationToken cancellationToken)
            => await repositorio.ExisteConsolidacaoFrequenciaTurmaPorAno(request.Ano);
    }
}
