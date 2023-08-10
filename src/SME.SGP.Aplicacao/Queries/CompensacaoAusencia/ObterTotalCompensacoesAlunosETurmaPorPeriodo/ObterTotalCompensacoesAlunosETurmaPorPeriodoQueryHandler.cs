using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesAlunosETurmaPorPeriodoQueryHandler : IRequestHandler<ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>>
    {
        private const int MINUTOS_EXPIRACAO_CACHE = 5;
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorio;
        private readonly IRepositorioCache repositorioCache;

        public ObterTotalCompensacoesAlunosETurmaPorPeriodoQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorio,
                                                                        IRepositorioCache repositorioCache)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>> Handle(ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery request, CancellationToken cancellationToken)
        {
            var valorNomeChave = string.Format(NomeChaveCache.NOME_CHAVE_COMPENSACAO_TURMA_BIMESTRE, request.TurmaCodigo, request.Bimestre);

            var compensacoesTurma = await repositorioCache
                .ObterAsync(valorNomeChave,
                    async () => await repositorio.ObterTotalCompensacoesPorAlunosETurmaAsync(request.Bimestre, request.Alunos, request.TurmaCodigo, request.Professor), MINUTOS_EXPIRACAO_CACHE);

            var alunosNaoConstamListaCompensacoes = request.Alunos
                .Except(compensacoesTurma.Select(ct => ct.AlunoCodigo));

            if (alunosNaoConstamListaCompensacoes.Any())
            {
                compensacoesTurma = compensacoesTurma
                    .Concat(await repositorio.ObterTotalCompensacoesPorAlunosETurmaAsync(request.Bimestre, alunosNaoConstamListaCompensacoes.ToList(), request.TurmaCodigo, request.Professor));
            }

            if (request.Alunos != null && request.Alunos.Any())
            {
                return from c in compensacoesTurma
                       join a in request.Alunos
                       on c.AlunoCodigo equals a
                       where string.IsNullOrWhiteSpace(request.Professor) || (!string.IsNullOrWhiteSpace(request.Professor) && c.Professor == request.Professor)
                       select c;
            }

            if (string.IsNullOrWhiteSpace(request.Professor))
                return compensacoesTurma;

            return compensacoesTurma
                .Where(c => c.Professor == request.Professor);
        }
    }
}
