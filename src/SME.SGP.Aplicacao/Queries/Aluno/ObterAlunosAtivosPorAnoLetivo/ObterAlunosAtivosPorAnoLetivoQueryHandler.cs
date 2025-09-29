using MediatR;
using SME.SGP.Dados.ElasticSearch;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosAtivosPorAnoLetivo
{
    public class ObterAlunosAtivosPorAnoLetivoQueryHandler : IRequestHandler<ObterAlunosAtivosPorAnoLetivoQuery, IList<DadosMatriculaAlunoTipoPapDto>>
    {
        private readonly IRepositorioElasticTurma repositorioElasticTurma;
        public ObterAlunosAtivosPorAnoLetivoQueryHandler(IRepositorioElasticTurma repositorioElasticTurma)
        {
            this.repositorioElasticTurma = repositorioElasticTurma ?? throw new ArgumentNullException(nameof(repositorioElasticTurma));
        }
        public async Task<IList<DadosMatriculaAlunoTipoPapDto>> Handle(ObterAlunosAtivosPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var alunosTurma = await repositorioElasticTurma.ObterDadosAlunosDisciplinaPapPeloAnoLetivo(request.AnoLetivo);
            var grupoAlunos = alunosTurma?.GroupBy(aluno => aluno.CodigoMatricula)
                                        .Select(agrupado => agrupado.OrderByDescending(aluno => aluno.DataSituacao)
                                                                    .ThenByDescending(aluno => aluno.NumeroAlunoChamada)
                                                                    .First())
                                     .Where(aluno => aluno.CodigoSituacaoMatricula != (int)SituacaoMatriculaAluno.VinculoIndevido);
            grupoAlunos = grupoAlunos?.GroupBy(aluno => aluno.CodigoAluno)
                                    .Select(agrupado => agrupado.OrderByDescending(aluno => aluno.DataSituacao)
                                                                .ThenByDescending(aluno => aluno.NumeroAlunoChamada)
                                                                .First());

            return grupoAlunos.Select(dadosMatriculaAlunoElastic => new DadosMatriculaAlunoTipoPapDto
            {
                CodigoAluno = dadosMatriculaAlunoElastic.CodigoAluno,
                CodigoDre = dadosMatriculaAlunoElastic.CodigoDre,
                CodigoUe = dadosMatriculaAlunoElastic.CodigoEscola,
                CodigoTurma = dadosMatriculaAlunoElastic.CodigoTurma,
                CodigoMatricula = dadosMatriculaAlunoElastic.CodigoMatricula,
                AnoLetivo = dadosMatriculaAlunoElastic.Ano
            }).ToList();
        }
    }
}
