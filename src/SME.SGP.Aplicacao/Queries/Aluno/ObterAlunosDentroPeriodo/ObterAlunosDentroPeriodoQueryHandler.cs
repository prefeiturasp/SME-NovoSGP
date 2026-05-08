using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosDentroPeriodoQueryHandler : IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IMediator mediator;

        public ObterAlunosDentroPeriodoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosDentroPeriodoQuery request, CancellationToken cancellationToken)
        {
            var alunosEol = await mediator
                .Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(request.CodigoTurma)));

            if (alunosEol.EhNulo() || !alunosEol.Any())
                throw new NegocioException("Não foram localizados alunos para a turma e data informados.");

            if (request.ConsideraSomenteAtivos)
                return alunosEol.Where(a => !a.Inativo && a.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido);

            if (request.ConsideraSomenteAtivosPeriodoFechamento)
                return alunosEol.Where(a => (new[] {
                                    SituacaoMatriculaAluno.Ativo,
                                    SituacaoMatriculaAluno.PendenteRematricula,
                                    SituacaoMatriculaAluno.Rematriculado,
                                    SituacaoMatriculaAluno.SemContinuidade,
                                    SituacaoMatriculaAluno.Concluido }).Contains(a.CodigoSituacaoMatricula));

            return alunosEol
                .Where(a => ((!a.Inativo && a.DataMatricula.Date < request.Periodo.dataFim.Date) ||
                             (a.Inativo && a.DataMatricula.Date < request.Periodo.dataFim.Date && a.DataSituacao.Date >= request.Periodo.dataInicio.Date)) &&
                             a.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido);
        }
    }
}
