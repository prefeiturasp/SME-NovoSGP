using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentosTurmaQueryHandler : IRequestHandler<ObterPorFechamentosTurmaQuery, IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioCache repositorioCache;

        public ObterPorFechamentosTurmaQueryHandler(IRepositorioFechamentoNota repositorioFechamentoNota,
            IRepositorioCache repositorioCache)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> Handle(ObterPorFechamentosTurmaQuery request, CancellationToken cancellationToken)
        {
            var nomeChaveCache = string.Format(NomeChaveCache.FECHAMENTO_NOTA_FINAL_COMPONENTE_TURMA, request.CodigoDisciplina, request.CodigoTurma);

            return repositorioCache.ObterAsync(nomeChaveCache,
                     async () => await repositorioFechamentoNota.ObterPorFechamentosTurma(request.Ids),
                     "Obter nota de fechamento turma");
        }
    }
}