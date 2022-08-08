using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesAlunoFamiliaQueryHandler : CacheQuery<IEnumerable<RecomendacoesAlunoFamiliaDto>>, IRequestHandler<ObterRecomendacoesAlunoFamiliaQuery, IEnumerable<RecomendacoesAlunoFamiliaDto>>
    {
        private IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao;
        public ObterRecomendacoesAlunoFamiliaQueryHandler(IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
        }
        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> Handle(ObterRecomendacoesAlunoFamiliaQuery request, CancellationToken cancellationToken)
         => await Obter();

        protected override string ObterChave()
        {
            return NomeChaveCache.CHAVE_LISTA_DE_RECOMENDACOES;
        }

        protected override async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterObjetoRepositorio()
        {
            return await repositorioConselhoClasseRecomendacao.ObterIdRecomendacoesETipoAsync();
        }
    }
}
