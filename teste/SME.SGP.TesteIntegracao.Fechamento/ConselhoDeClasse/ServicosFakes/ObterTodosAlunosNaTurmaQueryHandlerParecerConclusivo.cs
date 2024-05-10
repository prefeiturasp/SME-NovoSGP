using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterTodosAlunosNaTurmaQueryHandlerParecerConclusivo : IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string ALUNO_CODIGO_1 = "1";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTodosAlunosNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            var alunos = new List<AlunoPorTurmaResposta>();

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_1,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = request.CodigoTurma,
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia,
                DataMatricula = dataReferencia.AddDays(-2),
                NomeAluno = ALUNO_CODIGO_1,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = "Ativo",
                NomeResponsavel = "RESPONSAVEL",
                TipoResponsavel = "4",
                CelularResponsavel = "11111111111",
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            });

            return await Task.FromResult(alunos);
        }
    }
}
