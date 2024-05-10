using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesPorAlunoConselhoQueryHandler : IRequestHandler<ObterRecomendacoesPorAlunoConselhoQuery, IEnumerable<RecomendacoesAlunoFamiliaDto>>
    {
        private IRepositorioConselhoClasseAlunoRecomendacao repositorioConselhoClasseRecomendacao;
        public ObterRecomendacoesPorAlunoConselhoQueryHandler(IRepositorioConselhoClasseAlunoRecomendacao repositorioConselhoClasseRecomendacao)
        {
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
        }
        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> Handle(ObterRecomendacoesPorAlunoConselhoQuery request, CancellationToken cancellationToken)
         => await repositorioConselhoClasseRecomendacao.ObterRecomendacoesDoAlunoPorConselho(request.AlunoCodigo, request.Bimestre, request.FechamentoTurmaId, request.ConselhoClasseIds);
    }
}
