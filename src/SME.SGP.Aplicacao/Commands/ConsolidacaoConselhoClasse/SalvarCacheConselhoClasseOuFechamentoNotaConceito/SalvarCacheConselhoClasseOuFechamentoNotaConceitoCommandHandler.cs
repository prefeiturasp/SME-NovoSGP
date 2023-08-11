using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class
        SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommandHandler : IRequestHandler<
            SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Handle(SalvarCacheConselhoClasseOuFechamentoNotaConceitoCommand request,
            CancellationToken cancellationToken)
        {
            var nomeChaveCache = string.Format(NomeChaveCache.NOTA_CONSOLIDACAO_CONSELHO_CLASSE_TURMA_COMPONENTE_BIMESTRE_ALUNO,
                request.TurmaId, request.ComponenteCurricularId, request.Bimestre, request.AlunoCodigo);

            var retornoCacheMapeado =
                await repositorioCache.ObterObjetoAsync<ConsolidadoConselhoClasseAlunoNotaCacheDto>(nomeChaveCache,
                    "Obter nota/conceito cache consolidação conselho classe turma/componente/bimestre/aluno");

            if (retornoCacheMapeado == null)
                retornoCacheMapeado = new ConsolidadoConselhoClasseAlunoNotaCacheDto() { AlunoCodigo = request.AlunoCodigo,
                                                                                    TurmaId = request.TurmaId,
                                                                                    ComponenteCurricularId = request.ComponenteCurricularId,
                                                                                    Bimestre = request.Bimestre
                };

            if (request.TipoAlteracao == TipoAlteracao.FechamentoNota)
            {
                retornoCacheMapeado.NotaFechamento = request.Nota;
                retornoCacheMapeado.ConceitoIdFechamento = request.ConceitoId;
            }
            else
            {
                retornoCacheMapeado.NotaConselhoClasse = request.Nota;
                retornoCacheMapeado.ConceitoIdConselhoClasse = request.ConceitoId;
            }

            await repositorioCache.SalvarAsync(nomeChaveCache, retornoCacheMapeado, 5);
            return true;
        }
    }
}