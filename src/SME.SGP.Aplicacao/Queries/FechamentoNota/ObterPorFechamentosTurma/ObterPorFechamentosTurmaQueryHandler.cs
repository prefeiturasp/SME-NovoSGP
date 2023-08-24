using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentosTurmaQueryHandler : IRequestHandler<ObterPorFechamentosTurmaQuery, IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IRepositorioCache repositorioCache;

        public ObterPorFechamentosTurmaQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
            IRepositorioCache repositorioCache)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> Handle(ObterPorFechamentosTurmaQuery request, CancellationToken cancellationToken)
        {
            var nomeChaveCache = string.Format(NomeChaveCache.FECHAMENTO_NOTA_FINAL_COMPONENTE_TURMA, request.CodigoDisciplina, request.CodigoTurma);

            var retornoCache = await repositorioCache.ObterObjetoAsync<List<FechamentoNotaAlunoAprovacaoDto>>(nomeChaveCache, "Obter fechamentos da turma");

            if (retornoCache != null)
                return retornoCache;

            var retornoDb = await repositorioFechamentoNota.ObterPorFechamentosTurma(request.Ids);
            await repositorioCache.SalvarAsync(nomeChaveCache, retornoDb);

            return retornoDb;
        }
    }
}