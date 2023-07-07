using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQueryHandler : IRequestHandler<ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQuery, ConsolidadoConselhoClasseAlunoNotaCacheDto>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQueryHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<ConsolidadoConselhoClasseAlunoNotaCacheDto> Handle(ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQuery request, CancellationToken cancellationToken)
        {
            var nomeChaveCache = string.Format(NomeChaveCache.CHAVE_NOTA_CONSOLIDACAO_CONSELHO_CLASSE_TURMA_COMPONENTE_BIMESTRE_ALUNO,
               request.TurmaId, request.ComponenteCurricularId, request.Bimestre, request.AlunoCodigo);

            return (await repositorioCache.ObterObjetoAsync<ConsolidadoConselhoClasseAlunoNotaCacheDto>(nomeChaveCache,
                                                                                                        "Obter nota/conceito cache consolidação conselho classe turma/componente/bimestre/aluno"));
        }
    }
}
