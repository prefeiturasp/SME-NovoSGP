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
        private readonly IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta;

        public ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQueryHandler(IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta)
        {
            this.repositorioConselhoClasseConsolidadoConsulta = repositorioConselhoClasseConsolidadoConsulta ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoConsulta));
        }

        public async Task<IEnumerable<AlunoSituacaoConselhoDto>> Handle(ObterAlunoEStatusConselhoClasseConsolidadoPorTurmaEBimestreQuery request, CancellationToken cancellationToken)
         => await repositorioConselhoClasseConsolidadoConsulta.ObterStatusConsolidacaoConselhoClasseAlunoTurma(request.TurmaId, request.Bimestre);
    }
}
