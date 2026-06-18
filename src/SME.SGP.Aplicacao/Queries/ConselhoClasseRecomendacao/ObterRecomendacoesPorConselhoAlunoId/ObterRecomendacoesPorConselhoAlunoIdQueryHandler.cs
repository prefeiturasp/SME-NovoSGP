using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesPorConselhoAlunoIdQueryHandler : IRequestHandler<ObterRecomendacoesPorConselhoAlunoIdQuery, IEnumerable<long>>
    {
        private IRepositorioConselhoClasseAlunoRecomendacao repositorioConselhoClasseRecomendacao;
        public ObterRecomendacoesPorConselhoAlunoIdQueryHandler(IRepositorioConselhoClasseAlunoRecomendacao repositorioConselhoClasseRecomendacao)
        {
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
        }

        public async Task<IEnumerable<long>> Handle(ObterRecomendacoesPorConselhoAlunoIdQuery request, CancellationToken cancellationToken)
            => await repositorioConselhoClasseRecomendacao.ObterRecomendacoesDoAlunoPorConselhoAlunoId(request.ConselhoClasseAlunoId);
    }
}
