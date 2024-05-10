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
    public class ObterRecomendacoesAlunoFamiliaQueryHandler : IRequestHandler<ObterRecomendacoesAlunoFamiliaQuery, IEnumerable<RecomendacoesAlunoFamiliaDto>>
    {
        private IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao;
        private readonly IRepositorioCache repositorioCache;

        public ObterRecomendacoesAlunoFamiliaQueryHandler(IRepositorioConselhoClasseRecomendacao repositorioConselhoClasseRecomendacao, IRepositorioCache repositorioCache) 
        {
            this.repositorioConselhoClasseRecomendacao = repositorioConselhoClasseRecomendacao ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseRecomendacao));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> Handle(ObterRecomendacoesAlunoFamiliaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(NomeChaveCache.LISTA_DE_RECOMENDACOES_ALUNO_FAMILIA,
                async () => await repositorioConselhoClasseRecomendacao.ObterIdRecomendacoesETipoAsync(),
                "Obter recomendações do aluno para a família");
        }
    }
}
