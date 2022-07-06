using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake : IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        protected readonly IMediator mediator;
        const long TIPO_CALENDARIO_1 = 1;

        public ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosAtivosPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(TIPO_CALENDARIO_1, request.DataAula));
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(TIPO_CALENDARIO_1));
            var primeiroBimestre = periodosEscolares.FirstOrDefault(x => x.Bimestre == (int)Bimestre.Primeiro);

            var dataAtual = DateTime.Now;

            return new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta() {
                    Ano = DateTime.Now.Year ,
                    DataSituacao = periodoEscolar.PeriodoInicio > dataAtual ? dataAtual : periodoEscolar.PeriodoFim,
                    CodigoAluno= "1",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "1"
                },
                    new AlunoPorTurmaResposta() {
                    Ano = DateTime.Now.Year ,
                    DataSituacao = periodoEscolar.PeriodoInicio.AddDays(1),
                    CodigoAluno= "4",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "1"

                },
                    new AlunoPorTurmaResposta() {
                    Ano = DateTime.Now.Year ,
                    DataSituacao = periodoEscolar.PeriodoInicio,
                    CodigoAluno= "2",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.ReclassificadoSaida,
                    SituacaoMatricula = "15"
                },
                    new AlunoPorTurmaResposta() {
                    Ano = DateTime.Now.Year ,
                    DataSituacao = primeiroBimestre.PeriodoInicio.AddDays(-1),
                    CodigoAluno= "3",
                    CodigoComponenteCurricular= 138,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.VinculoIndevido,
                    SituacaoMatricula = "4"
                }

            };
        }
    }
}
