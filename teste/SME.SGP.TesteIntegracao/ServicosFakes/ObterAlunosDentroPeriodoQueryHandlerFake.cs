﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterAlunosDentroPeriodoQueryHandlerFake : IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        protected readonly IMediator mediator;
        private const long TIPO_CALENDARIO_1 = 1;
        private const string CODIGO_ALUNO_1 = "1";
        private const string CODIGO_ALUNO_2 = "2";
        private const string CODIGO_ALUNO_3 = "3";
        private const string CODIGO_ALUNO_4 = "4";
        private const string CODIGO_ALUNO_5 = "5";
        private const string CODIGO_ALUNO_6 = "6";
        private const string CODIGO_ALUNO_7 = "7";
        private const string CODIGO_ALUNO_8 = "8";
        private const string CODIGO_ALUNO_9 = "9";
        private const string CODIGO_ALUNO_10 = "10";
        private const string CODIGO_ALUNO_11 = "11";
        private const string SITUACAO_MATRICULA_1 = "1";
        private const string SITUACAO_MATRICULA_2 = "2";
        private const string SITUACAO_MATRICULA_4 = "4";
        private const string SITUACAO_MATRICULA_15 = "15";
        

        public ObterAlunosDentroPeriodoQueryHandlerFake(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosDentroPeriodoQuery request, CancellationToken cancellationToken)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(TIPO_CALENDARIO_1, request.Periodo.dataFim));
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(TIPO_CALENDARIO_1));
            var primeiroBimestre = periodosEscolares.FirstOrDefault(x => x.Bimestre == (int)Bimestre.Primeiro);

            var dataAtual = DateTime.Now;

            return new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 1",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = periodoEscolar.PeriodoInicio > dataAtual ? dataAtual : periodoEscolar.PeriodoFim,
                    CodigoAluno = CODIGO_ALUNO_1,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_1
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 2",
                    Ano = DateTime.Now.Year,
                    DataSituacao = DateTime.Now.Date,
                    CodigoAluno = CODIGO_ALUNO_4,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_1
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 3",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = periodoEscolar.PeriodoInicio,
                    CodigoAluno = CODIGO_ALUNO_2,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_15
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 4",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoInicio.AddDays(-1),
                    CodigoAluno = CODIGO_ALUNO_3,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_4
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 5",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoFim.AddDays(-1),
                    CodigoAluno = CODIGO_ALUNO_5,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 6",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoFim.AddDays(-2),
                    CodigoAluno = CODIGO_ALUNO_6,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 7",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoFim.AddDays(-3),
                    CodigoAluno = CODIGO_ALUNO_7,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 8",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoFim.AddDays(-4),
                    CodigoAluno = CODIGO_ALUNO_8,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 9",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoFim.AddDays(-5),
                    CodigoAluno = CODIGO_ALUNO_9,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 10",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoFim.AddDays(-6),
                    CodigoAluno = CODIGO_ALUNO_10,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 11",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = primeiroBimestre.PeriodoFim.AddDays(-7),
                    CodigoAluno = CODIGO_ALUNO_11,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
            };
        }
    }
}
