using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQueryHandler : IRequestHandler<ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQuery, IEnumerable<AlunoSituacaoConselhoDto>>
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;

        public ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQueryHandler(IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado)
        {
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public async Task<IEnumerable<AlunoSituacaoConselhoDto>> Handle(ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQuery request, CancellationToken cancellationToken)
         => await repositorioConselhoClasseConsolidado.ObterStatusConsolidacaoConselhoClasseAlunoTurma(request.TurmaId, request.Bimestre);
    }
}
