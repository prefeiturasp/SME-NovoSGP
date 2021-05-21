using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteConsolidacaoMatriculaTurmaPorAnoQueryHandler : IRequestHandler<ExisteConsolidacaoMatriculaTurmaPorAnoQuery, bool>
    {
        private readonly IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma;

        public ExisteConsolidacaoMatriculaTurmaPorAnoQueryHandler(IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma)
        {
            this.repositorioConsolidacaoMatriculaTurma = repositorioConsolidacaoMatriculaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoMatriculaTurma));
        }

        public async Task<bool> Handle(ExisteConsolidacaoMatriculaTurmaPorAnoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoMatriculaTurma.ExisteConsolidacaoMatriculaTurmaPorAno(request.AnoLetivo);
        }
    }
}
