using MediatR;
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
    public class ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake : IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        protected readonly IMediator mediator;
        private const long TIPO_CALENDARIO_1 = 1;
        private const string CODIGO_ALUNO_1 = "1";
        private const string CODIGO_ALUNO_2 = "2";
        private const string CODIGO_ALUNO_3 = "3";
        private const string CODIGO_ALUNO_4 = "4";
        private const string SITUACAO_MATRICULA_1 = "1";
        private const string SITUACAO_MATRICULA_4 = "4";
        private const string SITUACAO_MATRICULA_15 = "15";

        public ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosAtivosPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(TIPO_CALENDARIO_1, request.DataAula));
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(TIPO_CALENDARIO_1));
            var primeiroBimestre = periodosEscolares.FirstOrDefault(x => x.Bimestre == (int) Bimestre.Primeiro);
            var dataAtual = DateTime.Now;
            var dataSituacao = periodoEscolar.EhNulo() ? dataAtual : periodoEscolar.PeriodoInicio;
            var dataSituacaoAluno1 = dataSituacao;

            if (periodoEscolar.NaoEhNulo())
                dataSituacaoAluno1 = periodoEscolar.PeriodoInicio > dataAtual ? dataAtual : periodoEscolar.PeriodoFim;
        
            return await Task.FromResult(new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 1",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = dataSituacaoAluno1,
                    CodigoAluno = CODIGO_ALUNO_1,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_1
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 2",
                    Ano = DateTime.Now.Year,
                    DataSituacao = dataSituacao.AddDays(1),
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
                    DataSituacao = dataSituacao,
                    CodigoAluno = CODIGO_ALUNO_2,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.ReclassificadoSaida,
                    SituacaoMatricula = SITUACAO_MATRICULA_15
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 4",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = dataSituacao.AddDays(-1),
                    CodigoAluno = CODIGO_ALUNO_3,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido,
                    SituacaoMatricula = SITUACAO_MATRICULA_4
                }
            });
        }
    }
}