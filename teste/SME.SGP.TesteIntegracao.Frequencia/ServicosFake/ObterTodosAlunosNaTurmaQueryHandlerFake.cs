using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsolidacaoFrequenciaMensal.ServicosFakes
{

    public class ObterTodosAlunosNaTurmaQueryHandlerFake : IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";
        private readonly string ALUNO_CODIGO_6 = "6";

        private readonly string ATIVO = "Ativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";
        private readonly string NAO_COMPARECEU = "Não Compareceu";
        private readonly string DESISTENTE = "Desistente";

        public Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterTodosAlunosNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataRefencia = DateTimeExtension.HorarioBrasilia().AddMonths(-1);

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_1,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = request.CodigoTurma,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-5),
                    DataMatricula = dataRefencia.AddYears(-1),
                    NomeAluno = ALUNO_CODIGO_1,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_2,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = request.CodigoTurma,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-7),
                    DataMatricula = dataRefencia.AddYears(-1),
                    NomeAluno = ALUNO_CODIGO_2,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                },

                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_3,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = request.CodigoTurma,
                    DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                    DataSituacao = dataRefencia.AddDays(-25),
                    DataMatricula = dataRefencia.AddYears(-1),
                    NomeAluno = ALUNO_CODIGO_3,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel = CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                }
            };

            var dataReferenciaFixa = new DateTime(2023, 01, 01);

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_4,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = request.CodigoTurma,
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferenciaFixa,
                DataMatricula = dataReferenciaFixa.AddDays(-10),
                NomeAluno = ALUNO_CODIGO_4,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = ATIVO,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_5,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = request.CodigoTurma,
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataReferenciaFixa,
                DataMatricula = dataRefencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_5,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = NAO_COMPARECEU,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            alunos.Add(new AlunoPorTurmaResposta
            {
                Ano = 0,
                CodigoAluno = ALUNO_CODIGO_6,
                CodigoComponenteCurricular = 0,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                CodigoTurma = request.CodigoTurma,
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                DataSituacao = dataRefencia.AddDays(-8),
                DataMatricula = dataRefencia.AddDays(-130),
                NomeAluno = ALUNO_CODIGO_6,
                NumeroAlunoChamada = 0,
                SituacaoMatricula = DESISTENTE,
                NomeResponsavel = RESPONSAVEL,
                TipoResponsavel = TIPO_RESPONSAVEL_4,
                CelularResponsavel = CELULAR_RESPONSAVEL,
                DataAtualizacaoContato = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            return Task.FromResult(alunos.Where(x => x.CodigoTurma == request.CodigoTurma));
        }
    }
}
