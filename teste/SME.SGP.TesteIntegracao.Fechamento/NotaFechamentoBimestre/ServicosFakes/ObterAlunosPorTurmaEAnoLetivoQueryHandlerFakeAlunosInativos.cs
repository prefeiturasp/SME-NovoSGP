using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes
{
    /// <summary>
    /// Devolve somente alunos inativos com data de situação anterior ao início do último bimestre.
    /// Eles passam pelo guard de "turma sem alunos" do início do use case, mas são descartados por
    /// DeveMostrarNaChamada, produzindo uma listagem de alunos válidos vazia no fechamento final.
    /// </summary>
    public class ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeAlunosInativos : IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string ALUNO_CODIGO_12 = "12";
        private const string ALUNO_CODIGO_13 = "13";

        private const string DESISTENTE = "Desistente";
        private const string RESPONSAVEL = "RESPONSAVEL";
        private const string TIPO_RESPONSAVEL_4 = "4";
        private const string CELULAR_RESPONSAVEL = "11111111111";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            var alunos = new List<AlunoPorTurmaResposta>
            {
                CriarAlunoInativo(ALUNO_CODIGO_12, request.CodigoTurma, dataReferencia),
                CriarAlunoInativo(ALUNO_CODIGO_13, request.CodigoTurma, dataReferencia)
            };

            return await Task.FromResult(alunos.Where(x => x.CodigoTurma.ToString() == request.CodigoTurma));
        }

        private static AlunoPorTurmaResposta CriarAlunoInativo(string codigoAluno, string codigoTurma, DateTime dataReferencia)
        {
            return new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = codigoAluno,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                CodigoTurma = int.Parse(codigoTurma),
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferencia.AddDays(-50),
                DataMatricula = dataReferencia.AddDays(-130),
                NomeAluno = codigoAluno,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = DESISTENTE,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            };
        }
    }
}
